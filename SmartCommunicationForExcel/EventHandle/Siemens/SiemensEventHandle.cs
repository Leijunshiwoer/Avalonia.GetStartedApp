using HslCommunication;
using HslCommunication.Profinet.Siemens;
using Microsoft.Extensions.Logging;
using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.EventHandle.Siemens
{
    /// <summary>
    /// 西门子PLC事件处理器，负责通讯连接、数据读写和事件触发逻辑
    /// </summary>
    public class SiemensEventHandler : IDisposable
    {
       
        private readonly ISiemensEventExecuter _eventExecuter;
        private readonly ConcurrentQueue<EventSiemensThreadState> _completedEventQueue = new();
        private readonly Dictionary<int, bool> _eventTriggerStatus = new(); // 替代固定数组，动态管理事件状态
        private readonly CancellationTokenSource _cts = new();
        private SiemensS7Net _plcClient;
        private SiemensGlobalConfig _globalConfig;
        private Task _workTask;
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
        public SiemensEventHandler(ISiemensEventExecuter eventExecuter)
        {
            _eventExecuter = eventExecuter;
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

            // 启动工作任务（替代原Thread）
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
            // 配置Hsl授权（建议移到配置文件）
            Authorization.SetAuthorizationCode("4672fd9a-4743-4a08-ad2f-5cd3374e496d");

            _plcClient = new SiemensS7Net(_globalConfig.CpuInfo.PlcType)
            {
                IpAddress = _globalConfig.CpuInfo.IP,
                Rack = _globalConfig.CpuInfo.Rack,
                Slot = _globalConfig.CpuInfo.Slot
            };
            //  _plcClient.SetPersistentConnection();

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

                    // 读取PLC公共区数据
                    var readResult = await ReadPlcCommonDataAsync();
                    if (!readResult.IsSuccess)
                    {
                        await Task.Delay(cycleTime, _cts.Token);
                        continue;
                    }

                    // 处理事件触发
                    await ProcessEventTriggersAsync();

                    // 回写公共区数据到PLC
                    await WritePlcCommonDataAsync();

                    // 处理已完成的事件回写
                    await ProcessCompletedEventsAsync();

                    // 等待周期结束
                    await Task.Delay(cycleTime, _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // 正常取消，忽略
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 读取PLC公共区数据
        /// </summary>
        private async Task<OperateResult<byte[]>> ReadPlcCommonDataAsync()
        {
            var address = _globalConfig.PlcConfig[0].GetMBAddressTag;
            var length = CalculateDataLength(_globalConfig.PlcConfig);
            // 使用异步读取方法
            return await _plcClient.ReadAsync(address, length); ;
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

            // 检查事件是否正在处理中
            if (_eventTriggerStatus.TryGetValue(eventIndex, out var isProcessing) && isProcessing)
                return;

            // 标记事件为处理中
            _eventTriggerStatus[eventIndex] = true;

            try
            {
                // 读取事件数据
                var eventData = await ReadEventDataAsync(eventInstance);
                if (!eventData.IsSuccess)
                {
                    _eventTriggerStatus[eventIndex] = false;
                    return;
                }

                // 解析事件数据
                ResolveEventData(eventData.Content, eventInstance.ListInput);

                // 检查序列ID是否匹配（避免重复处理）
                if (!IsSequenceIdMismatch(eventInstance))
                {
                    // 重试回写（如果ID匹配但触发位仍激活）
                    await RetryWriteEventDataAsync(eventInstance);
                    _eventTriggerStatus[eventIndex] = false;
                    return;
                }

                // 异步处理事件（替代SmartThreadPool）
                var threadState = new EventSiemensThreadState
                {
                    InstanceName = InstanceName,
                    EventIndex = eventIndex,
                    SE = eventInstance
                };

                // 用Task.Run替代线程池，更符合.NET现代编程模式
                _ = Task.Run(() => ProcessEventAsync(threadState), _cts.Token);
            }
            catch (Exception ex)
            {
                _eventTriggerStatus[eventIndex] = false;
            }
        }

        /// <summary>
        /// 处理事件逻辑并将结果加入回写队列
        /// </summary>
        private void ProcessEventAsync(EventSiemensThreadState state)
        {
            try
            {
                // 调用事件执行器处理业务逻辑
                _eventExecuter.HandleEvent(state);
                // 将处理结果加入队列，等待回写
                _completedEventQueue.Enqueue(state);
            }
            catch (Exception ex)
            {
                _eventTriggerStatus[state.EventIndex] = false; // 异常时重置状态
            }
        }

        /// <summary>
        /// 处理已完成的事件回写
        /// </summary>
        private async Task ProcessCompletedEventsAsync()
        {
            while (_completedEventQueue.TryDequeue(out var eventState))
            {
                try
                {
                    // 同步序列ID（PLC → EAP）
                    SyncSequenceId(eventState.SE);

                    // 回写事件结果到PLC
                    var writeResult = await _plcClient.WriteAsync(eventState.SE.ListOutput[0].GetMBAddressTag,
                        PackageEventData(eventState.SE.ListOutput));

                    if (!writeResult.IsSuccess)
                    {
                       
                        // 回写失败时重新入队重试
                        _completedEventQueue.Enqueue(eventState);
                        continue;
                    }

                }
                catch (Exception ex)
                {
                }
                finally
                {
                    // 重置事件状态
                    _eventTriggerStatus[eventState.EventIndex] = false;
                }
            }
        }

        #region 工具方法（数据处理/校验）
        /// <summary>
        /// 验证周期时间（确保在10-2000ms范围内）
        /// </summary>
        private int ValidateCycleTime(int cycleTime) => Math.Clamp(cycleTime, 10, 2000);

        /// <summary>
        /// 计算数据区长度
        /// </summary>
        private ushort CalculateDataLength(List<SiemensEventIO> configs)
            => (ushort)(configs.Sum(c => c.Length) * 2);

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

                // 用Span优化内存复制（减少堆分配）
                data.AsSpan(sourceOffset, targetLength)
                    .CopyTo(config.DataValue.AsSpan(0, targetLength));

                config.GetDataValueStr();
            }
        }

        /// <summary>
        /// 打包事件数据为PLC字节数组
        /// </summary>
        private byte[] PackageEventData(List<SiemensEventIO> configs)
        {
            var length = CalculateDataLength(configs);
            var buffer = new byte[length];

            foreach (var config in configs)
            {
                var targetOffset = config.MBAdr - configs[0].MBAdr;
                var sourceLength = config.Length * 2;

                // 用Span优化内存复制
                config.DataValue.AsSpan(0, sourceLength)
                    .CopyTo(buffer.AsSpan(targetOffset, sourceLength));
            }

            return buffer;
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
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 重试回写事件数据
        /// </summary>
        private async Task RetryWriteEventDataAsync(SiemensEventInstance instance)
        {
            var writeResult = await  _plcClient.WriteAsync(instance.ListOutput[0].GetMBAddressTag,
                PackageEventData(instance.ListOutput));

           
        }

        /// <summary>
        /// 回写公共区数据到PLC
        /// </summary>
        private async Task WritePlcCommonDataAsync()
        {
            var writeResult = await _plcClient.WriteAsync(_globalConfig.EapConfig[0].GetMBAddressTag,
                PackageEventData(_globalConfig.EapConfig));

        }

        /// <summary>
        /// 读取事件数据
        /// </summary>
        private async Task<OperateResult<byte[]>> ReadEventDataAsync(SiemensEventInstance instance)
        {
            var address = instance.ListInput[0].GetMBAddressTag;
            var length = CalculateDataLength(instance.ListInput);

            return await _plcClient.ReadAsync(address, length);
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