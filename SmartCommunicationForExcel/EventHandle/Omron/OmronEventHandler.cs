using Amib.Threading;
using HslCommunication;
using HslCommunication.Profinet.Omron;
using SmartCommunicationForExcel.Implementation.Omron;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace SmartCommunicationForExcel.EventHandle.Omron
{
    /// <summary>
    /// Omron PLC事件处理器，负责与Omron PLC通信、监控事件触发信号、处理事件逻辑并回写结果
    /// </summary>
    public class OmronEventHandler : IDisposable
    {
        #region 公共属性
        /// <summary>
        /// 实例名称（用于标识当前事件处理实例）
        /// </summary>
        public string InstanceName { get; set; }
        #endregion

        #region 私有字段
        /// <summary>
        /// 事件触发标签名（默认值："eventtrigger"）
        /// 用于监控PLC中触发事件的信号位
        /// </summary>
        private string _eventTriggerTagName = "eventtrigger";

        /// <summary>
        /// Omron事件执行器（处理具体事件逻辑的接口）
        /// </summary>
        private readonly IOmronEventExecuter _eventExecuter;

        /// <summary>
        /// 事件处理完成队列（存储已处理完成的事件，用于回写结果到PLC）
        /// 线程安全队列，支持多线程并发操作
        /// </summary>
        private readonly ConcurrentQueue<EventOmronThreadState> _completedEventQueue = new ConcurrentQueue<EventOmronThreadState>();

        /// <summary>
        /// 事件触发状态字典（记录每个事件是否正在处理中，防止重复处理）
        /// Key：事件索引，Value：是否处理中
        /// </summary>
        private readonly Dictionary<int, bool> _eventTriggerStatus = new Dictionary<int, bool>();

        /// <summary>
        /// Omron PLC通信客户端（基于HslCommunication库的Fins协议客户端）
        /// </summary>
        private OmronFinsNet _plcClient;

        /// <summary>
        /// 全局配置对象（包含PLC连接信息、事件配置等）
        /// </summary>
        private OmronGlobalConfig _globalConfig;

        /// <summary>
        /// 工作循环任务（负责周期性与PLC通信、处理事件的主任务）
        /// </summary>
        private Task _workTask;

        /// <summary>
        /// 资源释放标记（防止重复释放资源）
        /// </summary>
        private bool _isDisposed;
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化OmronEventHandler实例
        /// 通过依赖注入容器解析事件执行器（IOmronEventExecuter）
        /// </summary>
        /// <param name="container">Unity依赖注入容器</param>
        public OmronEventHandler(IUnityContainer container)
        {
            // 优先解析名为"Omron"的事件执行器，若不存在则解析默认实现
            if (container.IsRegistered<IOmronEventExecuter>("Omron"))
                _eventExecuter = container.Resolve<IOmronEventExecuter>("Omron");
            else
                _eventExecuter = container.Resolve<IOmronEventExecuter>();
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 启动事件处理实例（异步）
        /// 初始化PLC连接并启动工作循环任务
        /// </summary>
        /// <param name="instanceName">实例名称</param>
        /// <param name="globalConfig">全局配置（包含PLC连接信息、事件配置等）</param>
        /// <returns>启动成功返回true，否则返回false</returns>
        public async Task<bool> StartAsync(string instanceName, OmronGlobalConfig globalConfig)
        {
            InstanceName = instanceName;
            _globalConfig = globalConfig;

            // 设置HslCommunication库授权码（用于激活库功能）
            Authorization.SetAuthorizationCode("4672fd9a-4743-4a08-ad2f-5cd3374e496d");

            // 初始化PLC客户端并建立连接
            if (!await InitializePlcClientAsync())
            {
                return false;
            }

            // 启动工作循环任务（周期性处理PLC通信和事件）
            _workTask = Task.Run(WorkLoopAsync);
            return true;
        }

        /// <summary>
        /// 停止事件处理实例（异步）
        /// 等待工作任务结束并释放资源
        /// </summary>
        public async Task StopAsync()
        {
            // 等待工作任务退出（最多等待1秒）
            if (_workTask != null)
                await _workTask.WaitAsync(TimeSpan.FromSeconds(1));

            // 释放资源
            Dispose();
        }

        /// <summary>
        /// 释放资源（实现IDisposable接口）
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // 通知GC不需要执行终结器
        }
        #endregion

        #region 私有核心方法
        /// <summary>
        /// 初始化PLC客户端并建立连接
        /// </summary>
        /// <returns>连接成功返回true，否则返回false</returns>
        private async Task<bool> InitializePlcClientAsync()
        {
            // 配置Omron Fins协议客户端参数（从全局配置获取）
            _plcClient = new OmronFinsNet
            {
                IpAddress = _globalConfig.CpuInfo.IP,       // PLC IP地址
                Port = _globalConfig.CpuInfo.Port,         // PLC端口
                SA1 = _globalConfig.CpuInfo.SA1,           // 源地址1（本地设备地址）
                DA1 = _globalConfig.CpuInfo.DA1,           // 目标地址1（PLC地址）
                DA2 = _globalConfig.CpuInfo.DA2            // 目标地址2（PLC单元号）
            };

            // 异步连接PLC服务器
            var connectResult = await _plcClient.ConnectServerAsync();
            return connectResult.IsSuccess;
        }

        /// <summary>
        /// 工作循环（周期性执行：检查PLC连接、处理事件、读写数据）
        /// </summary>
        private async Task WorkLoopAsync()
        {
            try
            {
                // 验证并获取周期时间（确保在10-2000ms范围内，防止过短或过长）
                var cycleTime = ValidateCycleTime(_globalConfig.CpuInfo.CycleTime);

                // 循环执行，直到资源释放
                while (!_isDisposed)
                {
                    // 若配置为空（无事件或PLC配置），等待下一个周期
                    if (_globalConfig.EapConfig.Count == 0 || _globalConfig.PlcConfig.Count == 0)
                    {
                        await Task.Delay(cycleTime);
                        continue;
                    }

                    // 检查PLC连接状态，若断开则自动重连
                    if (!IsPlcConnected())
                    {
                        if (!await InitializePlcClientAsync())
                        {
                            await Task.Delay(cycleTime);
                            continue;
                        }
                    }

                    // 处理已完成的事件（回写结果到PLC）
                    await ProcessCompletedEventsAsync();

                    // 读取PLC公共区数据（包含事件触发信号和公共参数）
                    var readResult = await ReadPlcCommonDataAsync();
                    if (readResult.IsSuccess)
                    {
                        try
                        {
                            // 将读取到的字节数据解析到PLC配置对象中
                            ResolveDataToEvent(readResult.Content, _globalConfig.PlcConfig);
                        }
                        catch (Exception ex)
                        {
                            _eventExecuter.Err(_globalConfig.FileName, readResult.Content, ex.Message);
                            await Task.Delay(cycleTime);
                            continue;
                        }

                        // 触发公共事件通知（如连接状态、配置信息）
                        _eventExecuter.SubscribeCommonInfo(InstanceName, true, _globalConfig.PlcConfig, _globalConfig.EapConfig);

                        // 处理事件触发信号（检查是否有新事件需要处理）
                        await ProcessEventTriggersAsync();

                        // 回写公共区数据到PLC（如处理结果、状态更新）
                        var writeResult = await WritePlcCommonDataAsync();
                        if (!writeResult.IsSuccess)
                        {
                            Console.WriteLine("公共区数据回写失败");
                            _eventExecuter.SubscribeCommonInfo(InstanceName, false, _globalConfig.EapConfig, _globalConfig.PlcConfig, "公共区数据回写失败");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"公共区数据读取失败: {readResult.Message}");
                        _eventExecuter.SubscribeCommonInfo(InstanceName, false, _globalConfig.EapConfig, _globalConfig.PlcConfig, $"公共区数据读取失败: {readResult.Message}");
                    }

                    // 等待下一个周期
                    await Task.Delay(cycleTime);
                }
            }
            catch (Exception ex)
            {
                _eventExecuter.Err(_globalConfig.FileName, Encoding.Default.GetBytes("工作循环异常"), ex.Message);
            }
        }

        /// <summary>
        /// 处理已完成的事件（从队列中取出并回写结果到PLC）
        /// </summary>
        private async Task ProcessCompletedEventsAsync()
        {
            // 若队列不为空，取出事件并处理
            if (!_completedEventQueue.IsEmpty)
            {
                if (_completedEventQueue.TryDequeue(out var eventState))
                {
                    // 同步序列ID（确保输入输出的序列ID一致，避免重复处理）
                    SyncSequenceId(eventState.SE);

                    // 回写事件结果到PLC
                    var writeResult = await _plcClient.WriteAsync(
                        eventState.SE.ListOutput[0].GetMEAddressTag,
                        PackageDataToPlc(eventState.SE.ListOutput)
                    );

                    if (!writeResult.IsSuccess)
                    {
                        Console.WriteLine("事件结果回写失败");
                        _eventExecuter.Err(InstanceName, null, $"事件结果回写失败，事件名: {eventState.SE.EventName}");
                    }
                    else
                    {
                        Console.WriteLine($"事件结果回写成功; ID: {eventState.SE.ListOutput[0].GetInt16()}");
                    }

                    // 重置事件状态（标记为未处理，允许下次触发）
                    _eventTriggerStatus[eventState.EventIndex] = false;
                }
                else
                {
                    _eventExecuter.Err(InstanceName, null, "从完成队列获取事件失败");
                }
            }
        }

        /// <summary>
        /// 处理事件触发信号（检查触发标签，启动对应事件处理）
        /// </summary>
        private async Task ProcessEventTriggersAsync()
        {
            // 查找事件触发标签配置（从PLC公共区配置中）
            var triggerConfig = _globalConfig.PlcConfig
                .FirstOrDefault(t => t.TagName.Trim().Equals(_eventTriggerTagName, StringComparison.OrdinalIgnoreCase));

            if (triggerConfig == null)
            {
                _eventExecuter.Err(InstanceName, null, "未找到事件触发标签（EventTrigger）配置");
                return;
            }

            // 解析触发标签的位信号（每个位对应一个事件）
            var triggers = triggerConfig.TransBool(triggerConfig.DataValue, 0, triggerConfig.DataValue.Length);

            // 遍历所有触发位，处理激活的事件
            for (var i = 0; i < triggers.Length; i++)
            {
                // 若触发位为true且事件配置存在，则处理该事件
                if (triggers[i] && i < _globalConfig.EventConfig.Count)
                {
                    await ProcessSingleEventTriggerAsync(i);
                }
            }
        }

        /// <summary>
        /// 处理单个事件触发（读取事件数据、解析并执行事件逻辑）
        /// </summary>
        /// <param name="eventIndex">事件索引（对应事件配置列表中的位置）</param>
        private async Task ProcessSingleEventTriggerAsync(int eventIndex)
        {
            var eventInstance = _globalConfig.EventConfig[eventIndex];

            // 检查事件是否禁用或配置不完整（无输入/输出配置则跳过）
            if (eventInstance.DisableEvent || eventInstance.ListInput.Count == 0 || eventInstance.ListOutput.Count == 0)
                return;

            // 检查事件是否正在处理中（防止重复处理）
            if (_eventTriggerStatus.TryGetValue(eventIndex, out var isProcessing) && isProcessing)
                return;

            // 标记事件为处理中
            _eventTriggerStatus[eventIndex] = true;

            try
            {
                // 从PLC读取该事件的输入数据
                var eventDataResult = await ReadEventDataAsync(eventInstance);
                if (eventDataResult.IsSuccess)
                {
                    try
                    {
                        // 解析事件数据到输入配置中
                        ResolveEventData(eventDataResult.Content, eventInstance.ListInput);
                    }
                    catch (Exception ex)
                    {
                        _eventExecuter.Err(_globalConfig.FileName, eventDataResult.Content, ex.Message);
                        return;
                    }

                    // 检查序列ID是否匹配（避免重复处理旧事件）
                    if (!IsSequenceIdMismatch(eventInstance))
                    {
                        // 封装事件状态，提交给事件执行器处理
                        var threadState = new EventOmronThreadState
                        {
                            InstanceName = InstanceName,
                            EventIndex = eventIndex,
                            SE = eventInstance
                        };

                        // 异步执行事件逻辑（使用Task.Run避免阻塞工作循环）
                        _ = Task.Run(() => _eventExecuter.HandleEvent(threadState))
                            .ContinueWith(task =>
                            {
                                // 事件处理完成后回调（将结果加入完成队列）
                                if (task.Exception != null)
                                {
                                    _eventExecuter.Err(InstanceName, null, $"事件处理异常: {task.Exception.Message}");
                                    return;
                                }
                                HandleEventCallback(task.Result as EventOmronThreadState);
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                }
                else
                {
                    Console.WriteLine($"事件数据读取失败: {eventDataResult.Message}");
                    _eventExecuter.SubscribeCommonInfo(
                        InstanceName,
                        false,
                        _globalConfig.EapConfig,
                        _globalConfig.PlcConfig,
                        $"事件数据读取失败，事件名: {eventInstance.EventName}"
                    );
                }
            }
            catch (Exception)
            {
                // 异常时重置事件状态
                _eventTriggerStatus[eventIndex] = false;
            }
        }
        #endregion

        #region 私有辅助方法
        /// <summary>
        /// 检查PLC是否连接（通过Ping和客户端状态判断）
        /// </summary>
        /// <returns>连接正常返回true，否则返回false</returns>
        private bool IsPlcConnected()
        {
            return _plcClient != null && _plcClient.IpAddressPing() == IPStatus.Success;
        }

        /// <summary>
        /// 验证周期时间（限制在10-2000ms，防止配置错误）
        /// </summary>
        /// <param name="cycleTime">原始周期时间（ms）</param>
        /// <returns>验证后的周期时间</returns>
        private int ValidateCycleTime(int cycleTime)
        {
            return Math.Clamp(cycleTime, 10, 2000);
        }

        /// <summary>
        /// 读取PLC公共区数据（包含事件触发信号和公共参数）
        /// </summary>
        /// <returns>包含字节数据的操作结果</returns>
        private async Task<OperateResult<byte[]>> ReadPlcCommonDataAsync()
        {
            // 公共区起始地址（从第一个配置项获取）
            var address = _globalConfig.PlcConfig[0].GetMBAddressTag;
            // 公共区总长度（根据所有配置项计算）
            var length = GetRWLength(_globalConfig.PlcConfig);
            return await _plcClient.ReadAsync(address, length);
        }

        /// <summary>
        /// 回写公共区数据到PLC
        /// </summary>
        /// <returns>操作结果（成功/失败）</returns>
        private async Task<OperateResult> WritePlcCommonDataAsync()
        {
            var address = _globalConfig.EapConfig[0].GetMBAddressTag;
            var data = PackageDataToPlc(_globalConfig.EapConfig);
            return await _plcClient.WriteAsync(address, data);
        }

        /// <summary>
        /// 读取单个事件的输入数据
        /// </summary>
        /// <param name="eventInstance">事件实例（包含输入配置）</param>
        /// <returns>包含字节数据的操作结果</returns>
        private async Task<OperateResult<byte[]>> ReadEventDataAsync(OmronEventInstance eventInstance)
        {
            var address = eventInstance.ListInput[0].GetMBAddressTag;
            var length = GetRWLength(eventInstance.ListInput);
            return await _plcClient.ReadAsync(address, length);
        }

        /// <summary>
        /// 同步事件的序列ID（确保输入和输出的序列ID一致）
        /// 用于避免重复处理同一事件
        /// </summary>
        /// <param name="eventInstance">事件实例</param>
        private void SyncSequenceId(OmronEventInstance eventInstance)
        {
            try
            {
                // 获取输入和输出中的序列ID配置
                var inputIdConfig = eventInstance.ListInput
                    .FirstOrDefault(t => t.TagName.Trim().Equals("sequenceid", StringComparison.OrdinalIgnoreCase));

                var outputIdConfig = eventInstance.ListOutput
                    .FirstOrDefault(t => t.TagName.Trim().Equals("sequenceid", StringComparison.OrdinalIgnoreCase));

                // 同步值（输入→输出）
                if (inputIdConfig != null && outputIdConfig != null)
                {
                    outputIdConfig.SetInt16(inputIdConfig.GetInt16());
                }
            }
            catch { } // 忽略同步失败（不影响主流程）
        }

        /// <summary>
        /// 检查序列ID是否不匹配（用于判断是否为新事件）
        /// </summary>
        /// <param name="eventInstance">事件实例</param>
        /// <returns>不匹配返回true（需要处理），匹配返回false（重复事件）</returns>
        private bool IsSequenceIdMismatch(OmronEventInstance eventInstance)
        {
            var inputId = eventInstance.ListInput
                .FirstOrDefault(t => t.TagName.Trim().Equals("sequenceid", StringComparison.OrdinalIgnoreCase))?.GetInt16() ?? 0;

            var outputId = eventInstance.ListOutput
                .FirstOrDefault(t => t.TagName.Trim().Equals("sequenceid", StringComparison.OrdinalIgnoreCase))?.GetInt16() ?? 0;

            return inputId != outputId;
        }

        /// <summary>
        /// 解析PLC字节数据到事件输入配置中
        /// </summary>
        /// <param name="data">PLC读取的字节数组</param>
        /// <param name="configs">事件输入配置列表</param>
        private void ResolveEventData(byte[] data, List<OmronEventIO> configs)
        {
            if (data == null || data.Length == 0) return;

            foreach (var config in configs)
            {
                // 计算当前配置在数据中的偏移量（相对于第一个配置的地址）
                var sourceOffset = config.MBAdr - configs[0].MBAdr;
                // 计算需要复制的长度（每个配置项的长度×2，因16位寄存器对应2字节）
                var targetLength = config.Length * 2;

                // 使用Span优化内存复制（高效且安全）
                data.AsSpan(sourceOffset, targetLength)
                    .CopyTo(config.DataValue.AsSpan(0, targetLength));

                // 更新配置项的字符串值（用于日志或显示）
                config.GetDataValueStr();
            }
        }

        /// <summary>
        /// 将事件配置数据打包为PLC可写入的字节数组
        /// </summary>
        /// <param name="configs">事件配置列表（输入或输出）</param>
        /// <returns>打包后的字节数组</returns>
        private byte[] PackageDataToPlc(List<OmronEventIO> configs)
        {
            // 计算总长度（每个配置项的长度之和×2）
            var totalLength = GetRWLength(configs) * 2;
            byte[] writeData = new byte[totalLength];

            foreach (var config in configs)
            {
                // 计算当前配置在数组中的偏移量
                var offset = (config.MBAdr - configs[0].MBAdr) * 2;
                // 复制配置数据到字节数组
                Array.Copy(config.DataValue, 0, writeData, offset, config.Length * 2);
                // 更新配置项的字符串值
                config.GetDataValueStr();
            }

            return writeData;
        }

        /// <summary>
        /// 将PLC字节数据解析到配置列表中（公共区数据专用）
        /// </summary>
        /// <param name="content">PLC读取的字节数组</param>
        /// <param name="configs">配置列表（如PLC公共区配置）</param>
        private void ResolveDataToEvent(byte[] content, List<OmronEventIO> configs)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                // 计算偏移量并复制数据
                var offset = (configs[i].MBAdr - configs[0].MBAdr) * 2;
                Array.Copy(content, offset, configs[i].DataValue, 0, configs[i].Length * 2);
                // 更新字符串值
                configs[i].GetDataValueStr();
            }
        }

        /// <summary>
        /// 计算读写长度（所有配置项的长度之和）
        /// </summary>
        /// <param name="configs">配置列表</param>
        /// <returns>总长度（寄存器数量）</returns>
        private ushort GetRWLength(List<OmronEventIO> configs)
        {
            short totalLength = 0;
            foreach (var item in configs)
            {
                totalLength += item.Length;
            }
            return (ushort)totalLength;
        }

        /// <summary>
        /// 事件处理完成后的回调（将结果加入完成队列）
        /// </summary>
        /// <param name="eventState">事件状态对象</param>
        private void HandleEventCallback(EventOmronThreadState eventState)
        {
            Console.WriteLine($"事件处理完成，准备回写结果。ID: {eventState.SE.ListOutput[0].GetInt16()}");
            _completedEventQueue.Enqueue(eventState);
        }

        /// <summary>
        /// 释放资源的具体实现（分离托管和非托管资源）
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // 释放托管资源
                _completedEventQueue.Clear();
                _eventTriggerStatus.Clear();
            }

            // 释放非托管资源（关闭PLC连接）
            if (_plcClient != null)
            {
                _plcClient.ConnectClose();
                _plcClient = null;
            }

            _isDisposed = true;
        }
        #endregion
    }
}