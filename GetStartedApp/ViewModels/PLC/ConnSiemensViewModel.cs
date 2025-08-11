using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting;
using Avalonia.Threading;
using GetStartedApp.Interface;
using GetStartedApp.Models;
using GetStartedApp.Utils.Node;
using NetTaste;
using OfficeOpenXml;
using Prism.Commands;
using SmartCommunicationForExcel;
using SmartCommunicationForExcel.EventHandle.Mitsubishi;
using SmartCommunicationForExcel.EventHandle.Omron;
using SmartCommunicationForExcel.EventHandle.Siemens;
using SmartCommunicationForExcel.Executer;
using SmartCommunicationForExcel.Implementation.Mitsubishi;
using SmartCommunicationForExcel.Implementation.Omron;
using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ursa.Controls;

namespace GetStartedApp.ViewModels.PLC
{
    /// <summary>
    /// 西门子PLC连接视图模型
    /// 负责PLC连接管理、配置加载及状态展示
    /// </summary>
    public class ConnSiemensViewModel : ViewModelBase, ISiemensEventExecuter, IOmronEventExecuter,IMitsubishiEventExecuter
    {
        #region 常量定义
        // 路径与文件相关
        private const string PLC_CONFIG_PATH = @"/Assets/PLCs/";
        private const string PLC_FILE_PREFIX = "xlsx";

        // Excel配置相关
        private readonly string[] _sheetNames = { "CpuInfo", "EapConfig", "PlcConfig", "EventConfig" };
        private const int _cpuInfoStartRow = 3;
        private const int _cpuInfoStartCol = 1;
        #endregion

        #region 私有字段
        private readonly SmartContainer _smartContainer = null;
        private readonly ISiemensEvent _siemensEvent;
        private readonly ManualResetEvent _autoConnResetEvent = new ManualResetEvent(false);
        private bool _isAutoConnPLC = false;
        private int _autoConnInterval = 2000; // 自动重连间隔(ms)
        #endregion

        #region 公共属性
        private string _plcConnStatus = string.Empty;
        /// <summary>
        /// PLC连接状态文本
        /// </summary>
        public string PLCConnStatus
        {
            get => _plcConnStatus;
            set => SetProperty(ref _plcConnStatus, value);
        }

        private ObservableCollection<PLCModel> _obPLC = new ObservableCollection<PLCModel>();
        /// <summary>
        /// PLC模型集合（绑定UI列表）
        /// </summary>
        public ObservableCollection<PLCModel> ObPLC
        {
            get => _obPLC;
            set => SetProperty(ref _obPLC, value);
        }

        private PLCModel? _selectedPLC;
        /// <summary>
        /// 当前选中的PLC模型
        /// </summary>
        public PLCModel? SelectedPLC
        {
            get => _selectedPLC;
            set => SetProperty(ref _selectedPLC, value);
        }
        #endregion

        #region 构造函数
        public ConnSiemensViewModel(ISiemensEvent siemensEvent)
        {
            _siemensEvent= siemensEvent;
            _smartContainer = new SmartContainer();
            // 注册事件执行器
            _smartContainer.RegisterInstance<ISiemensEventExecuter>(ConstName.SiemensRegisterName, this);
            _smartContainer.RegisterInstance<IOmronEventExecuter>(ConstName.OmronRegisterName, this);
            _smartContainer.RegisterInstance<IMitsubishiEventExecuter>(ConstName.MitsubishiRegisterName, this);

            // 初始化Excel许可证（仅需设置一次）
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");
            // 异步初始化PLC配置，避免阻塞UI
            _ = InitializeAsync();
        }
        #endregion

        #region 命令定义
        // 连接所有PLC
        private DelegateCommand? _connAllPlcCmd;
        public DelegateCommand ConnAllPlcCmd =>
            _connAllPlcCmd ??= new DelegateCommand(OnConnAllPlc);

        // 打开PLC配置窗口
        private DelegateCommand? _plcConfigCmd;
        public DelegateCommand PLCConfigCmd =>
            _plcConfigCmd ??= new DelegateCommand(OnOpenPLCConfig);

        // 连接单个PLC
        private DelegateCommand<object>? _connCmd;
        public DelegateCommand<object> ConnCmd =>
            _connCmd ??= new DelegateCommand<object>(OnConnPLC);

        // 断开单个PLC连接
        private DelegateCommand<object>? _unCnnCmd;
        public DelegateCommand<object> UnCnnCmd =>
            _unCnnCmd ??= new DelegateCommand<object>(OnDisconnPLC);

        // 监控PLC状态
        private DelegateCommand<object>? _monitoringCmd;
        public DelegateCommand<object> MonitoringCmd =>
            _monitoringCmd ??= new DelegateCommand<object>(OnMonitorPLC);
        #endregion

        #region 命令执行方法
        /// <summary>
        /// 连接所有PLC
        /// </summary>
        private void OnConnAllPlc()
        {
            // 遍历所有未连接的PLC并尝试连接
            foreach (var plc in ObPLC.Where(p => p.FIsConn != "已连接"))
            {
                ConnectPLC(plc);
            }
        }

        /// <summary>
        /// 打开PLC配置窗口
        /// </summary>
        private void OnOpenPLCConfig()
        {
            // 可在此处添加配置窗口打开逻辑
        }

        /// <summary>
        /// 连接指定PLC
        /// </summary>
        private void OnConnPLC(object parameter)
        {
            if (parameter is not PLCModel plcModel) return;

            SelectedPLC = plcModel;
            if (plcModel.FIsConn != "已连接")
            {
                ConnectPLC(plcModel);
            }
        }

        /// <summary>
        /// 断开指定PLC连接
        /// </summary>
        private void OnDisconnPLC(object parameter)
        {
            if (parameter is not PLCModel plcModel) return;

            SelectedPLC = plcModel;
            if (plcModel.FIsConn is "已连接" or "后台连接中")
            {
                DisconnectPLC(plcModel);
            }
        }

        /// <summary>
        /// 监控指定PLC
        /// </summary>
        private void OnMonitorPLC(object parameter)
        {
            if (parameter is not PLCModel plcModel) return;

            SelectedPLC = plcModel;
            if (plcModel.FIsConn is "已连接" or "后台连接中")
            {
                if (plcModel.FCpuType=="Siemens")
                {
                    var result = _smartContainer.ShowSiemensConfig(plcModel.FName);
                    if (!result.IsSuccess)
                    {
                        // 可添加监控窗口打开失败的处理
                    }
                }
                else if (plcModel.FCpuType == "Omron")
                {
                    var result = _smartContainer.ShowOmronConfig(plcModel.FName);
                    if (!result.IsSuccess)
                    {
                        // 可添加监控窗口打开失败的处理
                    }
                }
                else
                {
                    var result = _smartContainer.ShowMitsubishiConfig(plcModel.FName);
                    if (!result.IsSuccess)
                    {
                        // 可添加监控窗口打开失败的处理
                    }
                }
              
            }
        }
        #endregion

        #region PLC连接管理
        /// <summary>
        /// 异步初始化流程
        /// </summary>
        private async Task InitializeAsync()
        {
            try
            {
                // 加载PLC配置
                await InitPLCsAsync();

                // 自动连接PLC（若启用）
                if (_isAutoConnPLC)
                {
                   
                    await StartAutoConnectPLCs();
                }
            }
            catch (Exception ex)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    MessageBox.ShowAsync($"初始化失败：{ex.Message}");
                });
            }
           
        }
        /// <summary>
        /// 异步初始化PLC列表（从配置文件加载）
        /// </summary>
        private async Task InitPLCsAsync()
        {
            try
            {
                // 1. 后台线程执行耗时操作
                var plcModels = await Task.Run(() =>
                {
                    var fullConfigPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        PLC_CONFIG_PATH.TrimStart('/')
                    );
                    return GetPLCConfigFromFolder(fullConfigPath);
                });

                // 2. UI线程更新集合
                if (plcModels?.Any() ?? false)
                {
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        ObPLC = new ObservableCollection<PLCModel>(plcModels);
                    });
                }
                else
                {
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        ObPLC.Clear();
                        PLCConnStatus = "未找到PLC配置文件";
                    });
                }
            }
            catch (Exception ex)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    PLCConnStatus = "配置加载失败";
                    MessageBox.ShowAsync($"初始化PLC失败：{ex.Message}");
                });
            }
        }
        /// <summary>
        /// 从文件夹加载PLC配置
        /// </summary>
        private PLCModel[]? GetPLCConfigFromFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"PLC配置文件夹不存在：{folderPath}");
            }

            var plcModels = new List<PLCModel>();
            var configFiles = new DirectoryInfo(folderPath).GetFiles()
                .Where(f => f.Name.ToLower().Contains(PLC_FILE_PREFIX))
                .ToList();

            foreach (var file in configFiles)
            {
                try
                {
                    var plcModel = ParsePLCConfigFromExcel(file.FullName);
                    if (plcModel != null)
                    {
                        plcModels.Add(plcModel);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.ShowAsync($"解析配置文件 {file.Name} 失败：{ex.Message}");
                }
            }

            return plcModels.ToArray();
        }

        /// <summary>
        /// 从Excel文件解析PLC配置
        /// </summary>
        private PLCModel? ParsePLCConfigFromExcel(string filePath)
        {
            using var package = new ExcelPackage(new FileInfo(filePath));
            // 验证Sheet名称
            ValidateExcelSheets(package);

            // 读取CPU信息Sheet
            var cpuSheet = package.Workbook.Worksheets[0];
            int colIndex = 0;

            // 解析单元格数据
            var plcModel = new PLCModel
            {
                FFileName = Path.GetFileName(filePath),
                FCpuType = GetExcelCellValue<string>(cpuSheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + colIndex++]),
                FPLCType = GetExcelCellValue<string>(cpuSheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + colIndex++]),
                FMark = GetExcelCellValue<string>(cpuSheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + colIndex++]),
                FName = GetExcelCellValue<string>(cpuSheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + colIndex++]),
                FIsConn = "未连接",
                FColor = "Black",
                FAddr = GetExcelCellValue<string>(cpuSheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + (colIndex += 4)]), // 跳过4个地址偏移字段

            };

            return plcModel;
        }

        /// <summary>
        /// 启动PLC自动连接线程
        /// </summary>
        private async Task StartAutoConnectPLCs()
        {
            for (int i = 0; i < ObPLC.Count; i++)
            {
                int plcIndex = i;
                // 使用Task.Run并标记lambda为async
                _ = Task.Run(async () =>
                {
                    do
                    {
                        var plc = ObPLC[plcIndex];
                        if (plc.FIsConn != "已连接")
                        {
                            // 调用异步版本的启动方法
                            var result = await _smartContainer.StartSiemensWorkInstance(
                                plc.FName,
                                Path.Combine(PLC_CONFIG_PATH, plc.FFileName)
                            );
                            plc.FIsConn = result.IsSuccess ? "已连接" : "连接失败";
                        }
                    }
                    // 使用Task.Delay替代WaitOne，避免阻塞线程池线程
                    while (ObPLC[plcIndex].FIsConn != "已连接"
                           && !await Task.Run(() => _autoConnResetEvent.WaitOne(_autoConnInterval)));
                });

                await Task.Delay(100); // 异步等待，避免并发冲突
            }
        }
        /// <summary>
        /// 连接单个PLC
        /// </summary>
        private async void ConnectPLC(PLCModel plcModel)
        {
            var targetPlc = ObPLC.FirstOrDefault(p => p.FName == plcModel.FName);
            if (targetPlc == null) return;


            if (targetPlc.FFileName.Contains("Siemens"))
            {
                var result = await _smartContainer.StartSiemensWorkInstance(
               plcModel.FName,
               Path.Combine(PLC_CONFIG_PATH, plcModel.FFileName)
           );

                targetPlc.FIsConn = result.IsSuccess ? "已连接" : "连接失败";
                if (!result.IsSuccess)
                {
                    await MessageBox.ShowAsync(result.Message);
                }
            }
            else if (targetPlc.FFileName.Contains("Omron"))
            {
                var result = await _smartContainer.StartOmronWorkInstance(
                    plcModel.FName,
                    Path.Combine(PLC_CONFIG_PATH, plcModel.FFileName)
                );

                targetPlc.FIsConn = result.IsSuccess ? "已连接" : "连接失败";
                if (!result.IsSuccess)
                {
                    await MessageBox.ShowAsync(result.Message);
                }
            }
            else if (targetPlc.FFileName.Contains("Mitsubishi"))
            {
                var result = await _smartContainer.StartMitsubishiWorkInstance(
                  plcModel.FName,
                  Path.Combine(PLC_CONFIG_PATH, plcModel.FFileName)
              );

                targetPlc.FIsConn = result.IsSuccess ? "已连接" : "连接失败";
                if (!result.IsSuccess)
                {
                    await MessageBox.ShowAsync(result.Message);
                }
            }

        }

        /// <summary>
        /// 断开单个PLC连接
        /// </summary>
        private async void DisconnectPLC(PLCModel plcModel)
        {
            var targetPlc = ObPLC.FirstOrDefault(p => p.FName == plcModel.FName);
            if (targetPlc == null) return;
            if (targetPlc.FFileName.Contains("Siemens"))
            {
                await _smartContainer.StopSiemensWorkInstance(plcModel.FName);
            }
            else if (targetPlc.FFileName.Contains("Omron"))
            {
                await _smartContainer.StopOmronWorkInstance(plcModel.FName);

            }
            else
            {
                await _smartContainer.StopMitsubishiWorkInstance(plcModel.FName);

            }
            targetPlc.FIsConn = "未连接";
        }
        #endregion

        #region Excel工具方法
        /// <summary>
        /// 验证Excel文件中的Sheet名称是否匹配
        /// </summary>
        private void ValidateExcelSheets(ExcelPackage package)
        {
            if (package.Workbook.Worksheets.Count != _sheetNames.Length)
            {
                throw new InvalidDataException($"Excel Sheet数量不匹配，预期：{_sheetNames.Length}，实际：{package.Workbook.Worksheets.Count}");
            }

            for (int i = 0; i < _sheetNames.Length; i++)
            {
                if (package.Workbook.Worksheets[i].Name != _sheetNames[i])
                {
                    throw new InvalidDataException($"Sheet名称不匹配，预期：{_sheetNames[i]}，实际：{package.Workbook.Worksheets[i].Name}");
                }
            }
        }

        /// <summary>
        /// 安全获取Excel单元格值（泛型转换）
        /// </summary>
        private T GetExcelCellValue<T>(ExcelRange cell)
        {
            if (cell.Value == null)
            {
                return default!;
            }

            try
            {
                return (T)Convert.ChangeType(cell.Value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                return default!;
            }
        }
        #endregion


        #region ISiemensEventExecuter 实现
        /// <summary>
        /// 处理事件
        /// </summary>
        public object HandleEvent(object eventData)
        {
            // 事件处理逻辑（按需实现）
            EventSiemensThreadState se = eventData as EventSiemensThreadState;
            if (se!=null)
            {
                PLCModel pLCModel = ObPLC.Where(it => it.FName == se.InstanceName).FirstOrDefault();
                List<MyData> keyValuePairs = new List<MyData>();

                //遍历所有PLC下发数据
                for (int i = 0; i < se.SE.ListInput.Count; i++)
                {
                    if (se.SE.ListInput[i].DTType == CDataType.DTInt)
                        keyValuePairs.Add(new MyData() { Key = se.SE.ListInput[i].TagName, ValueType = MyData.MyType.Int32, ValueData = se.SE.ListInput[i].GetInt32() });
                    if (se.SE.ListInput[i].DTType == CDataType.DTShort)
                        keyValuePairs.Add(new MyData() { Key = se.SE.ListInput[i].TagName, ValueType = MyData.MyType.Int16, ValueData = se.SE.ListInput[i].GetInt16() });
                    if (se.SE.ListInput[i].DTType == CDataType.DTString)
                        keyValuePairs.Add(new MyData() { Key = se.SE.ListInput[i].TagName, ValueType = MyData.MyType.String, ValueData = se.SE.ListInput[i].GetString() });
                    if (se.SE.ListInput[i].DTType == CDataType.DTFloat)
                        keyValuePairs.Add(new MyData() { Key = se.SE.ListInput[i].TagName, ValueType = MyData.MyType.Float, ValueData = se.SE.ListInput[i].GetSingle() });
                }

                //添加当前工序 放在[EventClass]
                keyValuePairs.Add(new MyData() { Key = "WorkStage", ValueType = MyData.MyType.String, ValueData = se.SE.EventClass });

                PlcEventParamModel plcEventInputParamModel = new PlcEventParamModel()
                {
                    PlcName = se.InstanceName,
                    PlcAddr = pLCModel.FAddr,
                    EventName = se.SE.EventName,
                    EventClass = se.SE.EventClass,
                    StartTime = DateTime.Now,
                    Params = keyValuePairs
                };

                //调用处理接口
                PlcEventParamModel plcEventOutputParamModel = (PlcEventParamModel)_siemensEvent.DoEvent(plcEventInputParamModel);

                for (int i = 0; i < se.SE.ListOutput.Count; i++)
                {
                    if (plcEventOutputParamModel.Params != null)
                    {
                        //匹配相同项
                        for (int j = 0; j < plcEventOutputParamModel.Params.Count; j++)
                        {
                            //if (plcEventOutputParamModel.Params.Keys.Contains(se.SE.ListOutput[i].TagName))
                            if (plcEventOutputParamModel.Params.Any(it => it.Key == se.SE.ListOutput[i].TagName))
                            {
                                //将返回值写入plc
                                // var p = plcEventOutputParamModel.Params.Get(se.SE.ListOutput[i].TagName);
                                var p = plcEventOutputParamModel.Params.Where(it => it.Key == se.SE.ListOutput[i].TagName).SingleOrDefault();
                                if (p != null)
                                {
                                    if (p.ValueType == MyData.MyType.Int32)
                                    {
                                        se.SE.ListOutput[i].SetInt32(Convert.ToInt32(p.ValueData));
                                    }
                                    if (p.ValueType == MyData.MyType.Int16)
                                    {
                                        se.SE.ListOutput[i].SetInt16(Convert.ToInt16(p.ValueData));
                                    }
                                    if (p.ValueType == MyData.MyType.String)
                                    {
                                        se.SE.ListOutput[i].SetString(p.ValueData.ToString());
                                    }
                                    if (p.ValueType == MyData.MyType.WString)
                                    {
                                        se.SE.ListOutput[i].SetWString(p.ValueData.ToString());
                                    }
                                    if (p.ValueType == MyData.MyType.Float)
                                    {
                                        se.SE.ListOutput[i].SetFloat(Convert.ToSingle(p.ValueData));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return se;
        }

        /// <summary>
        /// 订阅通用信息
        /// </summary>
        public void SubscribeCommonInfo(
            string instanceName, bool success,
            List<SiemensEventIO> inputList, List<SiemensEventIO> outputList,
            string error = "")
        {
            // 通用信息订阅逻辑（按需实现）
            PLCModel pLCModel = ObPLC.Where(it => it.FName == instanceName).FirstOrDefault();
            pLCModel.FIsConn = success ? "已连接" : "后台连接中";

            if (success)
            {
                _smartContainer.GetSiemensEventIOByTagName(outputList, "Enable").SetInt16(1);
            }

        }

        /// <summary>
        /// 错误处理
        /// </summary>
        public void Err(string instanceName, byte[] data, string error = "")
        {
            // 错误处理逻辑（按需实现）
        }

        public void SubscribeCommonInfo(string instanceName, bool bSuccess, List<OmronEventIO> listInput, List<OmronEventIO> listOutput, string strError = "")
        {
            // 订阅逻辑（按需实现）
        }
      
        public void SubscribeCommonInfo(string strInstanceName, bool bSuccess, List<MitsubishiEventIO> listInput, List<MitsubishiEventIO> listOutput, string strError = "")
        {
           
        }
        #endregion
    }
}