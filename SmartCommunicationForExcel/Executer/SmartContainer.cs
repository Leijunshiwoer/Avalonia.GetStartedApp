using Amib.Threading;
using Microsoft.Extensions.Logging;
using SmartCommunicationForExcel.EventHandle.Beckhoff;
using SmartCommunicationForExcel.EventHandle.Mitsubishi;
using SmartCommunicationForExcel.EventHandle.Omron;
using SmartCommunicationForExcel.EventHandle.Siemens;
using SmartCommunicationForExcel.Implementation.Beckhoff;
using SmartCommunicationForExcel.Implementation.Mitsubishi;
using SmartCommunicationForExcel.Implementation.Omron;
using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Interface;
using SmartCommunicationForExcel.Model;
using SmartCommunicationForExcel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Unity.Injection;
// 或
 using Avalonia.Controls;
using Karambolo.Extensions.Logging.File; // 如果是Avalonia

namespace SmartCommunicationForExcel.Executer
{
    /// <summary>
    /// 智能容器管理类，负责设备通讯实例的创建、销毁及配置管理
    /// </summary>
    public class SmartContainer : IDisposable
    {
        #region 私有字段
        private int _cycleTime = 0;
        private bool _isDisposed;
      

        // 配置窗口实例（使用实际的Form基类）
        private SmartConfigForExcel.SmartSiemensConfigForExcelForm _smartSiemensConfigForm;
        private SmartConfigForExcel.SmartOmronConfigForExcelForm _smartOmronConfigForm;
        private SmartConfigForExcel.SmartMitsubishiConfigForExcelForm _smartMitsubishiConfigForm;
        private SmartConfigForExcel.SmartBeckhoffConfigForExcelForm _smartBeckhoffConfigForm;
        #endregion

        #region 公共属性
        public IUnityContainer Container { get; }
        #endregion

        #region 实例管理字典（按品牌分类）
        private readonly Dictionary<string, SiemensEventHandler> _siemensInstances = new();
        private readonly Dictionary<string, ISiemensGlobalConfig<SiemensEventIO, SiemensCpuInfo, SiemensEventInstance>> _siemensConfigs = new();

        private readonly Dictionary<string, OmronEventHandle> _omronInstances = new();
        private readonly Dictionary<string, IOmronGlobalConfig<OmronEventIO, OmronCpuInfo, OmronEventInstance>> _omronConfigs = new();

        private readonly Dictionary<string, MitsubishiEventHandle> _mitsubishiInstances = new();
        private readonly Dictionary<string, IMitsubishiGlobalConfig<MitsubishiEventIO, MitsubishiCpuInfo, MitsubishiEventInstance>> _mitsubishiConfigs = new();

        private readonly Dictionary<string, BeckhoffEventHandle> _beckhoffInstances = new();
        private readonly Dictionary<string, IBeckhoffGlobalConfig<BeckhoffEventIO, BeckhoffCpuInfo, BeckhoffEventInstance>> _beckhoffConfigs = new();
        #endregion

        #region 构造函数与初始化
        public SmartContainer()
        {
            // 初始化Unity容器
            Container = new UnityContainer();

            // 注册事件执行器
            Container.RegisterType<ISiemensEventExecuter, DefaultSiemensEventExecuter>();

            //Container.RegisterType<IOmronEventExecuter, DefaultOmronEventExecuter>();
            //Container.RegisterType<IMitsubishiEventExecuter, DefaultMitsubishiEventExecuter>();
            //Container.RegisterType<IBeckhoffEventExecuter, DefaultBeckhoffEventExecuter>();

            // 注册线程池（带回调配置）
            var stpStartInfo = new STPStartInfo
            {
                CallToPostExecute = CallToPostExecute.Always,
                FillStateWithArgs = true
            };
            var smartThreadPool = new SmartThreadPool(stpStartInfo)
            {
                Name = "SmartThread From Container"
            };
            Container.RegisterInstance(smartThreadPool);
        }

        #endregion

        #region 容器注册与解析方法
        public void Register<TFrom, TTo>() where TTo : TFrom
        {
            Container.RegisterType<TFrom, TTo>();
        }

        public T Resolve<T>(string name = "")
        {
            return string.IsNullOrEmpty(name)
                ? Container.Resolve<T>()
                : Container.Resolve<T>(name, null);
        }

        public void RegisterSingleton<T>()
        {
            Container.RegisterSingleton<T>();
        }

        public void RegisterInstance<T>(string name, T instance)
        {
            Container.RegisterInstance(name, instance);
        }

        #endregion

    

        #region 配置窗口管理（修复类型转换问题）
        public ResultState ShowSiemensConfig(string instanceName = "")
        {
            var result = new ResultState { IsSuccess = true };

            if (!string.IsNullOrEmpty(instanceName))
            {
                if (_siemensConfigs.TryGetValue(instanceName, out var config))
                {
                    _smartSiemensConfigForm = new SmartConfigForExcel.SmartSiemensConfigForExcelForm();
                    _smartSiemensConfigForm.SetModel(config);
                    _smartSiemensConfigForm.Show();
                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = $"未找到实例 [{instanceName}] 的配置";
                    return result;
                }

            }
            else
            {
              

            }
            return result;
        }

        public ResultState ShowOmronConfig(string instanceName = "")
        {
            var result = new ResultState { IsSuccess = true };

            if (string.IsNullOrEmpty(instanceName))
            {
                _smartOmronConfigForm ??= new SmartConfigForExcel.SmartOmronConfigForExcelForm();
                _smartOmronConfigForm.Show();
                return result;
            }

            if (_omronConfigs.TryGetValue(instanceName, out var config))
            {
                _smartOmronConfigForm ??= new SmartConfigForExcel.SmartOmronConfigForExcelForm();
                _smartOmronConfigForm.SetModel(config);
                _smartOmronConfigForm.Show();
            }
            else
            {
                result.IsSuccess = false;
                result.Message = $"未找到实例 [{instanceName}] 的配置";
            }

            return result;
        }

        public ResultState ShowMitsubishiConfig(string instanceName = "")
        {
            var result = new ResultState { IsSuccess = true };

            if (string.IsNullOrEmpty(instanceName))
            {
                _smartMitsubishiConfigForm ??= new SmartConfigForExcel.SmartMitsubishiConfigForExcelForm();
                _smartMitsubishiConfigForm.Show();
                return result;
            }

            if (_mitsubishiConfigs.TryGetValue(instanceName, out var config))
            {
                _smartMitsubishiConfigForm ??= new SmartConfigForExcel.SmartMitsubishiConfigForExcelForm();
                _smartMitsubishiConfigForm.SetModel(config);
                _smartMitsubishiConfigForm.Show();
            }
            else
            {
                result.IsSuccess = false;
                result.Message = $"未找到实例 [{instanceName}] 的配置";
            }

            return result;
        }

        public ResultState ShowBeckhoffConfig(string instanceName = "")
        {
            var result = new ResultState { IsSuccess = true };

            if (string.IsNullOrEmpty(instanceName))
            {
                _smartBeckhoffConfigForm ??= new SmartConfigForExcel.SmartBeckhoffConfigForExcelForm();
                _smartBeckhoffConfigForm.Show();
                return result;
            }

            if (_beckhoffConfigs.TryGetValue(instanceName, out var config))
            {
                _smartBeckhoffConfigForm ??= new SmartConfigForExcel.SmartBeckhoffConfigForExcelForm();
                _smartBeckhoffConfigForm.SetModel(config);
                _smartBeckhoffConfigForm.Show();
            }
            else
            {
                result.IsSuccess = false;
                result.Message = $"未找到实例 [{instanceName}] 的配置";
            }

            return result;
        }
        #endregion

        #region 标签查找通用方法
        public SiemensEventIO GetSiemensEventIOByTagName(List<SiemensEventIO> ioList, string tagName)
        {
            return GetEventIOByTagName(ioList, tagName, nameof(SiemensEventIO));
        }

        public OmronEventIO GetOmronEventIOByTagName(List<OmronEventIO> ioList, string tagName)
        {
            return GetEventIOByTagName(ioList, tagName, nameof(OmronEventIO));
        }

        public MitsubishiEventIO GetMitsubishiEventIOByTagName(List<MitsubishiEventIO> ioList, string tagName)
        {
            return GetEventIOByTagName(ioList, tagName, nameof(MitsubishiEventIO));
        }

        public BeckhoffEventIO GetBeckhoffEventIOByTagName(List<BeckhoffEventIO> ioList, string tagName)
        {
            return GetEventIOByTagName(ioList, tagName, nameof(BeckhoffEventIO));
        }

        /// <summary>
        /// 通用标签查找逻辑
        /// </summary>
        private T GetEventIOByTagName<T>(List<T> ioList, string tagName, string ioTypeName) where T : class
        {
            if (ioList == null)
                throw new ArgumentNullException(nameof(ioList));

            try
            {
                return ioList.SingleOrDefault(io =>
                    (io as dynamic).TagName.Equals(tagName, StringComparison.OrdinalIgnoreCase));
            }
            catch (InvalidOperationException ex)
            {
                var errorMsg = $"找到多个匹配标签 [{tagName}] 的 {ioTypeName} 实例";
                throw new InvalidOperationException(errorMsg, ex);
            }
        }
        #endregion

        #region 周期配置
        public void SetCycleTime(int cycleTime)
        {
            if (cycleTime < 10 || cycleTime > 2000)
            {
                _cycleTime = 100; // 默认值
                return;
            }

            _cycleTime = cycleTime;
        }
        #endregion

        #region 西门子设备管理（异步完整实现）
        public async Task<ResultState> StartSiemensWorkInstance(string instanceName, string configFilePath)
        {
            return await StartWorkInstanceAsync(
                instanceName,
                configFilePath,
                _siemensInstances,
                _siemensConfigs,
                // 保持接口类型
                path => new MyExcelFileHelper<SiemensGlobalConfig>().ExcelToSiemensObject(path),
                (config) =>
                {
                    // 访问具体类属性时需要转换
                    var concreteConfig = config as SiemensGlobalConfig;
                    if (concreteConfig != null)
                    {
                        concreteConfig.CpuInfo.CycleTime = _cycleTime;
                    }
                    return config;
                },
                () => Container.Resolve<SiemensEventHandler>(),
                (handler, name, config) => handler.StartAsync(name, config as SiemensGlobalConfig)
            );
        }

        public async Task<ResultState> StopSiemensWorkInstance(string instanceName)
        {
            return await StopWorkInstanceAsync(
                instanceName,
                _siemensInstances,
                _siemensConfigs,
                handler => handler.StopAsync()
            );
        }
        #endregion

        #region 欧姆龙设备管理（异步改造）
        public async Task<ResultState> StartOmronWorkInstance(string instanceName, string configFilePath)
        {
            //return await StartWorkInstanceAsync(
            //    instanceName,
            //    configFilePath,
            //    _omronInstances,
            //    _omronConfigs,
            //    path => new MyExcelFileHelper<OmronGlobalConfig>().ExcelToOmronObject(path),
            //    (config) =>
            //    {
            //        config.CpuInfo.CycleTime = _cycleTime;
            //        return config;
            //    },
            //    () => Container.Resolve<OmronEventHandle>(),
            //    (handler, name, config) => handler.StartAsync(name, config)
            //);

            await Task.CompletedTask;
            return new ResultState();
        }

        public async Task<ResultState> StopOmronWorkInstance(string instanceName)
        {
            //return await StopWorkInstanceAsync(
            //    instanceName,
            //    _omronInstances,
            //    _omronConfigs,
            //    handler => handler.StopAsync()
            //);

            await Task.CompletedTask;
            return new ResultState();
        }
        #endregion

        #region 三菱设备管理（异步改造）
        public async Task<ResultState> StartMitsubishiWorkInstance(string instanceName, string configFilePath)
        {
            //return await StartWorkInstanceAsync(
            //    instanceName,
            //    configFilePath,
            //    _mitsubishiInstances,
            //    _mitsubishiConfigs,
            //    path => new MyExcelFileHelper<MitsubishiGlobalConfig>().ExcelToMitsubishiObject(path),
            //    (config) =>
            //    {
            //        config.CpuInfo.CycleTime = _cycleTime;
            //        return config;
            //    },
            //    () => Container.Resolve<MitsubishiEventHandle>(),
            //    (handler, name, config) => handler.StartAsync(name, config)
            //);

            await Task.CompletedTask;
            return new ResultState();
        }

        public async Task<ResultState> StopMitsubishiWorkInstance(string instanceName)
        {
            //return await StopWorkInstanceAsync(
            //    instanceName,
            //    _mitsubishiInstances,
            //    _mitsubishiConfigs,
            //    handler => handler.StopAsync()
            //);

            await Task.CompletedTask;
            return new ResultState();
        }
        #endregion

        #region 倍福设备管理（异步改造）
        //待实现
        public async Task<ResultState> StartBeckhoffWorkInstance(string instanceName, string configFilePath)
        {
            //return await StartWorkInstanceAsync(
            //    instanceName,
            //    configFilePath,
            //    _beckhoffInstances,
            //    _beckhoffConfigs,
            //    path => new MyExcelFileHelper<BeckhoffGlobalConfig>().ExcelToBeckhoffObject(path),
            //    (config) =>
            //    {
            //        config.CpuInfo.CycleTime = _cycleTime;
            //        return config;
            //    },
            //    () => Container.Resolve<BeckhoffEventHandle>(),
            //    (handler, name, config) => handler.StartAsync(name, config)
            //);

            await Task.CompletedTask;
            return new ResultState();
        }

        public async Task<ResultState> StopBeckhoffWorkInstance(string instanceName)
        {
            //return await StopWorkInstanceAsync(
            //    instanceName,
            //    _beckhoffInstances,
            //    _beckhoffConfigs,
            //    handler => handler.StopAsync()
            //);

            await Task.CompletedTask;
            return new ResultState();
        }
        #endregion

        #region 通用启动/停止逻辑
        /// <summary>
        /// 通用实例启动逻辑
        /// </summary>
        private async Task<ResultState> StartWorkInstanceAsync<THandler, TConfig>(
            string instanceName,
            string configFilePath,
            Dictionary<string, THandler> instanceDict,
            Dictionary<string, TConfig> configDict,
            Func<string, TConfig> loadConfigFunc,
            Func<TConfig, TConfig> configureFunc,
            Func<THandler> createHandlerFunc,
            Func<THandler, string, TConfig, Task<bool>> startHandlerFunc) where TConfig : class
        {
            var result = new ResultState { IsSuccess = true };

            if (string.IsNullOrEmpty(instanceName))
            {
                result.IsSuccess = false;
                result.Message = "实例名称不能为空";
                return result;
            }

            try
            {
                if (instanceDict.ContainsKey(instanceName))
                {
                    result.IsSuccess = false;
                    result.Message = $"实例 [{instanceName}] 已在运行中";
                    return result;
                }

                // 加载配置
                var config = loadConfigFunc(configFilePath);
                if (config == null)
                {
                    result.IsSuccess = false;
                    result.Message = $"实例 [{instanceName}] 配置解析为空";
                    return result;
                }

                // 配置周期时间
                config = configureFunc(config);

                // 创建并启动处理器
                var handler = createHandlerFunc();
                var startSuccess = await startHandlerFunc(handler, instanceName, config);
                if (!startSuccess)
                {
                    result.IsSuccess = false;
                    result.Message = $"实例 [{instanceName}] 启动失败";
                    return result;
                }

                // 加入管理字典
                instanceDict.Add(instanceName, handler);
                configDict.Add(instanceName, config);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"实例 [{instanceName}] 启动异常: {ex.Message}";
            }

            return result;
        }

        private async Task<ResultState> StopWorkInstanceAsync<THandler, TConfig>(
            string instanceName,
            Dictionary<string, THandler> instanceDict,
            Dictionary<string, TConfig> configDict,
            Func<THandler, Task> stopHandlerFunc) where TConfig : class
        {
            var result = new ResultState { IsSuccess = true };

            if (instanceDict.TryGetValue(instanceName, out var handler))
            {
                try
                {
                    // 停止处理器
                    await stopHandlerFunc(handler);

                    // 从管理字典移除
                    instanceDict.Remove(instanceName);
                    configDict.Remove(instanceName);
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = $"实例 [{instanceName}] 停止异常: {ex.Message}";
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = $"未找到运行中的实例 [{instanceName}]";
            }

            return result;
        }
        #endregion

        #region 资源释放
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // 释放托管资源

                // 停止所有实例
                Task.WaitAll(
                    StopAllInstancesAsync(_siemensInstances, h => h.StopAsync())
                    //StopAllInstancesAsync(_omronInstances, h => h.StopAsync()),
                    //StopAllInstancesAsync(_mitsubishiInstances, h => h.StopAsync()),
                    //StopAllInstancesAsync(_beckhoffInstances, h => h.StopAsync())
                );

                // 释放配置窗口
                //_smartSiemensConfigForm?.Dispose();
                //_smartOmronConfigForm?.Dispose();
                //_smartMitsubishiConfigForm?.Dispose();
                //_smartBeckhoffConfigForm?.Dispose();

                // 释放容器
                Container.Dispose();
            }

            _isDisposed = true;
        }

        /// <summary>
        /// 停止所有实例
        /// </summary>
        private async Task StopAllInstancesAsync<THandler>(
            Dictionary<string, THandler> instances,
            Func<THandler, Task> stopFunc)
        {
            var stopTasks = instances.Keys.Select(name =>
            {
                return stopFunc(instances[name]);
            });
            await Task.WhenAll(stopTasks);
        }
        #endregion
    }
}
