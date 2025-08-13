using HslCommunication;
using HslCommunication.Profinet.Siemens;
using SmartCommunicationForExcel.Executer;
using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace SmartCommunicationForExcel.EventHandle.Siemens
{
    /// <summary>
    /// 西门子PLC事件处理器，负责通讯连接、数据读写和事件触发逻辑
    /// </summary>
    public class SiemensEventHandler : IDisposable
    {
        // 事件执行器，处理具体业务逻辑
        private readonly ISiemensEventExecuter _eventExecuter;
        // 已完成事件的回写队列（线程安全）
        private readonly ConcurrentQueue<EventSiemensThreadState> _completedEventQueue = new();
        // 事件触发状态布尔数组，动态管理事件是否正在处理
        private  bool[] _eventTriggerStatus = new bool[100];
        // 取消令牌源，用于终止工作任务
        private readonly CancellationTokenSource _cts = new();
        // PLC客户端通讯对象
        private SiemensS7Net _plcClient;
        // 全局配置信息
        private SiemensGlobalConfig _globalConfig;
        // 工作循环任务
        private Task _workTask;
        // 资源释放标记
        private bool _isDisposed;

        /// <summary>
        /// 实例名称
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// 事件触发标签名（默认：eventtrigger）
        /// </summary>
        public string EventTriggerTagName { get; set; } = "eventtrigger";

        /// <summary>
        /// 初始化西门子事件处理器
        /// </summary>
        public SiemensEventHandler(IUnityContainer container)
        {
          
            if (container.IsRegistered<ISiemensEventExecuter>("Siemens"))
                _eventExecuter = container.Resolve<ISiemensEventExecuter>("Siemens");
            else
                _eventExecuter = container.Resolve<ISiemensEventExecuter>();

        }

        /// <summary>
        /// 启动事件处理器
        /// </summary>
        /// <param name="instanceName">实例名称</param>
        /// <param name="globalConfig">全局配置</param>
        /// <returns>启动是否成功</returns>
        public async Task<bool> StartAsync(string instanceName, SiemensGlobalConfig globalConfig)
        {
            InstanceName = instanceName ?? throw new ArgumentNullException(nameof(instanceName));
            _globalConfig = globalConfig ?? throw new ArgumentNullException(nameof(globalConfig));

            // 初始化PLC客户端
            if (!await InitializePlcClientAsync())
            {
                return false;
            }

            // 启动工作任务
            _workTask = Task.Run(WorkLoopAsync, _cts.Token);
            return true;
        }

        /// <summary>
        /// 停止事件处理器
        /// </summary>
        public async Task StopAsync()
        {
            _cts.Cancel();
            if (_workTask != null)
                await _workTask.WaitAsync(TimeSpan.FromSeconds(5)); // 等待任务退出

            Dispose();
        }

        /// <summary>
        /// 初始化PLC客户端并建立连接
        /// </summary>
        private async Task<bool> InitializePlcClientAsync()
        {
            // 配置Hsl授权
            Authorization.SetAuthorizationCode(ConfigurationManager.AppSettings["AuthorizationCode"]);
            _plcClient = new SiemensS7Net(_globalConfig.CpuInfo.PlcType)
            {
                IpAddress = _globalConfig.CpuInfo.IP,
                Port=_globalConfig.CpuInfo.Port,
                Rack = _globalConfig.CpuInfo.Rack,
                Slot = _globalConfig.CpuInfo.Slot
            };

            var connectResult = await _plcClient.ConnectServerAsync();
            return connectResult.IsSuccess;
        }

        /// <summary>
        /// 工作循环：周期性读取PLC数据并处理事件
        /// </summary>
        private async Task WorkLoopAsync()
        {
            try
            {
                var cycleTime = ValidateCycleTime(_globalConfig.CpuInfo.CycleTime);

                while (!_cts.Token.IsCancellationRequested)
                {
                    // 检查配置是否有效
                    if (_globalConfig.EapConfig.Count == 0 || _globalConfig.PlcConfig.Count == 0)
                    {
                        await Task.Delay(cycleTime, _cts.Token);
                        continue;
                    }

                    // 检查连接状态，自动重连
                    if (!(_plcClient.IpAddressPing() == IPStatus.Success))
                    {
                        if (!await InitializePlcClientAsync())
                        {
                            await Task.Delay(cycleTime, _cts.Token);
                            continue;
                        }
                    }
                    // 处理已完成的事件（回写结果到PLC）
                    await ProcessCompletedEventsAsync();
                   
                    // 读取PLC公共区数据
                    var readResult = await ReadPlcCommonDataAsync();
                    if (readResult.IsSuccess)
                    {
                        try
                        {
                            // 解析PLC数据到配置
                            ResolveEventData(readResult.Content, _globalConfig.PlcConfig);
                        }
                        catch (Exception ex)
                        {
                            _eventExecuter.Err(_globalConfig.FileName, readResult.Content, ex.Message);
                        }

                        // 公共事件处理
                        _eventExecuter.SubscribeCommonInfo(InstanceName, true, _globalConfig.PlcConfig, _globalConfig.EapConfig);
                        // 处理事件触发
                        await ProcessEventTriggersAsync();

                        // 回写公共区数据到PLC
                        var writeResult = await WritePlcCommonDataAsync();
                        if (!writeResult.IsSuccess)
                        {
                            Console.WriteLine("WriteCommonData Fail.");
                            _eventExecuter.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, "WriteCommonData Fail");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ReadCommonData Fail.");
                        _eventExecuter.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, "ReadCommonData Fail");
                    }

                    // 等待周期结束
                    await Task.Delay(cycleTime, _cts.Token);
                }

                // 关闭PLC连接
                _plcClient.ConnectClose();
            }
            catch (OperationCanceledException)
            {
                // 正常取消，忽略
            }
            catch (Exception)
            {
                // 异常处理
            }
        }

        private async Task ProcessCompletedEventsAsync()
        {
            if (!_completedEventQueue.IsEmpty)
            {
                EventSiemensThreadState ets = null;
                if (_completedEventQueue.TryDequeue(out ets))
                {
                    SyncSequenceId(ets.SE);

                    var rlt = await _plcClient.WriteAsync(ets.SE.ListOutput[0].GetMEAddressTag, PackageDataToPlc(ets.SE.ListOutput));
                    if (!rlt.IsSuccess)
                    {
                        Console.WriteLine("Write Single Event Data Fail.");
                        _eventExecuter.Err(InstanceName, null, $"Write Single Event Data Fail,EventName:{ets.SE.EventName}");
                    }
                    else
                    {
                        Console.WriteLine($"Write Single Event Success;ID:{ets.SE.ListOutput[0].GetInt16()}");
                    }

                    _eventTriggerStatus[ets.EventIndex] = false; // 重置事件状态
                }
                else
                {
                    _eventExecuter.Err(ets.InstanceName, null, "安全队列获取失败");
                }
            }

        }

        /// <summary>
        /// 读取PLC公共区数据
        /// </summary>
        private async Task<OperateResult<byte[]>> ReadPlcCommonDataAsync()
        {
            var address = _globalConfig.PlcConfig[0].GetMBAddressTag;
            var length = GetRWLength(_globalConfig.PlcConfig);
            return await _plcClient.ReadAsync(address, length);
        }

        /// <summary>
        /// 处理事件触发逻辑
        /// </summary>
        private async Task ProcessEventTriggersAsync()
        {
            // 查找事件触发标签
            var triggerConfig = _globalConfig.PlcConfig
                .FirstOrDefault(t => t.TagName.Trim().Equals(EventTriggerTagName, StringComparison.OrdinalIgnoreCase));

            if (triggerConfig == null)
            {
                _eventExecuter.Err(InstanceName, null, "请检查公共区EventTrigger是否定义错误");
                return;
            }

            // 解析触发位
            var triggers = triggerConfig.TransBool(triggerConfig.DataValue, 0, triggerConfig.DataValue.Length);

            for (var i = 0; i < triggers.Length; i++)
            {
                if (triggers[i] && i < _globalConfig.EventConfig.Count)
                {
                    await ProcessSingleEventTriggerAsync(i);
                }
            }
        }

        /// <summary>
        /// 处理单个事件触发
        /// </summary>
        private async Task ProcessSingleEventTriggerAsync(int eventIndex)
        {
            var eventInstance = _globalConfig.EventConfig[eventIndex];

            // 检查事件是否禁用或配置不完整
            if (eventInstance.DisableEvent || eventInstance.ListInput.Count == 0 || eventInstance.ListOutput.Count == 0)
                return;

          
            try
            {
                // 检查事件是否正在处理中
                if (_eventTriggerStatus[eventIndex])
                {
                    // 读取事件数据
                    var eventData = await ReadEventDataAsync(eventInstance);
                    if (eventData.IsSuccess)
                    {
                        try
                        {
                            // 解析事件数据
                            ResolveEventData(eventData.Content, eventInstance.ListInput);
                        }
                        catch (Exception ex)
                        {
                            _eventExecuter.Err(_globalConfig.FileName, eventData.Content, ex.Message);
                            return;
                        }

                        // 检查序列ID是否匹配（避免重复处理）
                        if (IsSequenceIdMismatch(eventInstance))
                        {
                            // 标记事件为处理中
                            _eventTriggerStatus[eventIndex] = true;
                            // 异步处理事件
                            var threadState = new EventSiemensThreadState
                            {
                                InstanceName = InstanceName,
                                EventIndex = eventIndex,
                                SE = eventInstance
                            };

                            // 用Task.Run替代线程池
                            _ = Task.Run(() =>
                            {
                                return _eventExecuter.HandleEvent(threadState);
                            }).ContinueWith(task =>
                            {
                                // 处理任务结果
                                if (task.Exception != null)
                                {
                                    _eventExecuter.Err(InstanceName, null, $"事件处理异常: {task.Exception.Message}");
                                    return;
                                }
                                // 调用回调逻辑
                                HandleEventCaLLBack(task.Result as EventSiemensThreadState);
                            }); // 去掉调度器参数，使用默认的线程池调度器
                        }
                        else
                        {
                            // 序列ID匹配，重试写入
                            var writeResult = await _plcClient.WriteAsync(
                                eventInstance.ListOutput[0].GetMBAddressTag,
                                PackageDataToPlc(eventInstance.ListOutput)
                            );

                            if (!writeResult.IsSuccess)
                                Console.WriteLine("Write Single Event Retry Data Fail.");
                            else
                                Console.WriteLine($"Write Single Event Retry Success;ID:{eventInstance.ListOutput[0].GetInt16()}");
                        }
                    }
                    else
                    {

                        Console.WriteLine("Read Single Event Data Fail. Error:" + eventData.Message);
                        _eventExecuter.SubscribeCommonInfo(InstanceName, false, _globalConfig.EapConfig, _globalConfig.PlcConfig, string.Format("Read Single Event Data Fail,EventName:{0}", eventInstance.EventName));
                    }
                }
                   
            }
            catch (Exception)
            {
                _eventTriggerStatus[eventIndex] = false;
            }
        }

        /// <summary>
        /// 事件处理完成后的回调方法
        /// </summary>
        private void HandleEventCaLLBack(EventSiemensThreadState ets)
        {
            Console.WriteLine($"Push Data ID:{ets.SE.ListOutput[0].GetInt16()}");
            // 将处理结果加入已完成事件队列
            _completedEventQueue.Enqueue(ets);
        }

        #region 工具方法（数据处理/校验）
        /// <summary>
        /// 验证周期时间（确保在10-2000ms范围内）
        /// </summary>
        private int ValidateCycleTime(int cycleTime) => Math.Clamp(cycleTime, 10, 2000);

        /// <summary>
        /// 解析PLC数据到事件配置
        /// </summary>
        private void ResolveEventData(byte[] data, List<SiemensEventIO> configs)
        {
            if (data == null || data.Length == 0) return;

            foreach (var config in configs)
            {
                var sourceOffset = config.MBAdr - configs[0].MBAdr;
                var targetLength = config.Length * 2;

                // 用Span优化内存复制
                data.AsSpan(sourceOffset, targetLength)
                    .CopyTo(config.DataValue.AsSpan(0, targetLength));

                config.GetDataValueStr();
            }
        }

        /// <summary>
        /// 打包事件数据为PLC字节数组
        /// </summary>
        private byte[] PackageDataToPlc(List<SiemensEventIO> configs)
        {
            byte[] writeData = new byte[GetRWLength(configs)];
            for (int i = 0; i < configs.Count; i++)
            {
                Array.Copy(configs[i].DataValue, 0, writeData, configs[i].MBAdr - configs[0].MBAdr, configs[i].Length * 2);
                configs[i].GetDataValueStr();
            }
            return writeData;
        }

        /// <summary>
        /// 计算数据区长度
        /// </summary>
        private ushort GetRWLength(List<SiemensEventIO> input)
        {
            short length = 0;
            foreach (var item in input)
            {
                length += item.Length;
            }
            return (ushort)(length * 2);
        }

        /// <summary>
        /// 检查序列ID是否不匹配（需要处理事件）
        /// </summary>
        private bool IsSequenceIdMismatch(SiemensEventInstance instance)
        {
            var inputId = instance.ListInput
                .FirstOrDefault(t => t.TagName.Trim().Equals("sequenceid", StringComparison.OrdinalIgnoreCase))?.GetInt16() ?? 0;

            var outputId = instance.ListOutput
                .FirstOrDefault(t => t.TagName.Trim().Equals("sequenceid", StringComparison.OrdinalIgnoreCase))?.GetInt16() ?? 0;

            return inputId != outputId;
        }

        /// <summary>
        /// 同步序列ID（PLC → EAP）
        /// </summary>
        private void SyncSequenceId(SiemensEventInstance instance)
        {
            try
            {
                var inputIdConfig = instance.ListInput
                    .FirstOrDefault(t => t.TagName.Trim().Equals("sequenceid", StringComparison.OrdinalIgnoreCase));

                var outputIdConfig = instance.ListOutput
                    .FirstOrDefault(t => t.TagName.Trim().Equals("sequenceid", StringComparison.OrdinalIgnoreCase));

                if (inputIdConfig != null && outputIdConfig != null)
                {
                    outputIdConfig.SetInt16(inputIdConfig.GetInt16());
                }
            }
            catch { }
        }

        /// <summary>
        /// 回写公共区数据到PLC
        /// </summary>
        private async Task<OperateResult> WritePlcCommonDataAsync()
        {
            var writeResult = await _plcClient.WriteAsync(_globalConfig.EapConfig[0].GetMBAddressTag,
                PackageDataToPlc(_globalConfig.EapConfig));
            return writeResult;
        }

        /// <summary>
        /// 读取事件数据
        /// </summary>
        private async Task<OperateResult<byte[]>> ReadEventDataAsync(SiemensEventInstance instance)
        {
            var address = instance.ListInput[0].GetMBAddressTag;
            var length = GetRWLength(instance.ListInput);
            return await _plcClient.ReadAsync(address, length);
        }
        #endregion

        #region 资源释放
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源的具体实现
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // 释放托管资源
                _cts.Cancel();
                _cts.Dispose();
                _completedEventQueue.Clear();
            }

            // 释放非托管资源（关闭PLC连接）
            if (_plcClient != null)
            {
                _plcClient.ConnectClose();
            }

            _isDisposed = true;
        }
        #endregion
    }
}