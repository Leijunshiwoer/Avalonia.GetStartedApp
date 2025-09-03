using Amib.Threading;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using SmartCommunicationForExcel.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.ConnSiemensPLC
{
    /// <summary>
    /// 西门子PLC异步通信类（支持自动重连）
    /// </summary>
    public class SiemensPLCConnection : IDisposable
    {
        #region 常量配置（可根据实际需求调整）
        private const int READ_INTERVAL_MS = 10;                  // 数据读取间隔（毫秒）
        private const string AUTHORIZATION_CODE = "4672fd9a-4743-4a08-ad2f-5cd3374e496d"; // HSL授权码
        private const int INITIAL_RECONNECT_DELAY_MS = 1000;      // 初始重连延迟（毫秒）
        private const int MAX_RECONNECT_DELAY_MS = 30000;         // 最大重连延迟（30秒）
        private const double BACKOFF_MULTIPLIER = 2.0;            // 退避乘数
        private const int RECONNECT_JITTER_MS = 1000;             // 重连随机抖动范围
        private const int RECONNECT_CHECK_INTERVAL = 5;           // 重连检查频率（每5次读取循环）
        private const int PLC_CONNECT_TIMEOUT_MS = 5000;          // PLC连接超时（5秒）
        #endregion

        #region 字段与属性
        private readonly SiemensS7Net[] _siemensPLCs;             // PLC通信实例数组
        private readonly PlcInfo[] _plcInfos;                     // PLC配置与状态信息
        private Task[] _dataReadTasks;                            // 异步读取任务
        private readonly SmartThreadPool _smartThreadPool;        // 事件处理线程池
        private readonly ConcurrentQueue<EventInfo> _eventQueue01; // 事件队列
        private readonly ConcurrentQueue<EventInfo> _eventQueue02; // 事件队列
        private readonly Random _reconnectRandom = new Random();   // 重连随机数生成器
        #endregion

        #region 事件定义（供外部订阅状态）
        /// <summary>
        /// 错误事件（异常信息）
        /// </summary>
        public event Action<string> OnError;

        /// <summary>
        /// 信息事件（连接/重连状态）
        /// </summary>
        public event Action<string> OnInfo;

        /// <summary>
        /// 公共区数据回调（读取/写入数据）
        /// </summary>
        public event Action<string, object, object> OnPublicDataCallback;

        /// <summary>
        /// 事件数据回调（事件触发数据）
        /// </summary>
        public event Action<string, object> OnEventDataCallback;
        #endregion

        #region 构造函数与初始化
        /// <summary>
        /// 初始化PLC通信实例
        /// </summary>
        /// <param name="config">PLC配置信息</param>
        public SiemensPLCConnection(PlcConfig[] configs)
        {
            // 初始化PLC信息与通信实例
            _plcInfos = InitializePlcInfo(configs);
            _siemensPLCs = InitializePLCConnections();

            // 初始化线程池（限制线程数量避免资源耗尽）
            _smartThreadPool = CreateSmartThreadPool();

            // 激活HSL授权
            Authorization.SetAuthorizationCode(AUTHORIZATION_CODE);
            _eventQueue01 = new ConcurrentQueue<EventInfo>();
            _eventQueue02 = new ConcurrentQueue<EventInfo>();
            _dataReadTasks =
            [
                new(async () => await ExecuteDataReadingLoop01Async()),
                new(async () => await ExecuteDataReadingLoop02Async())
            ];
        }

        /// <summary>
        /// 初始化PLC通信实例（S1500）
        /// </summary>
        private SiemensS7Net[] InitializePLCConnections()
        {
            return new[]
            {
                new SiemensS7Net(SiemensPLCS.S1500)
                {
                    IpAddress = _plcInfos[0].Ip,
                    Port = _plcInfos[0].Port,
                    ConnectTimeOut = PLC_CONNECT_TIMEOUT_MS,
                },
                new SiemensS7Net(SiemensPLCS.S1500)
                {
                    IpAddress = _plcInfos[1].Ip,
                    Port = _plcInfos[1].Port,
                    ConnectTimeOut = PLC_CONNECT_TIMEOUT_MS,
                }
            };
        }

        /// <summary>
        /// 创建智能线程池（事件处理专用）
        /// </summary>
        private SmartThreadPool CreateSmartThreadPool()
        {
            var stpConfig = new STPStartInfo
            {
                CallToPostExecute = CallToPostExecute.Always,
                FillStateWithArgs = true,
                MaxWorkerThreads = 100,    // 最大工作线程（根据事件数量调整）
                MinWorkerThreads = 1,      // 最小工作线程
                IdleTimeout = 30000,       // 线程空闲超时（30秒）
            };
            return new SmartThreadPool(stpConfig);
        }
        #endregion

        #region 核心通信逻辑（异步+重连）
        /// <summary>
        /// 异步启动PLC通信
        /// </summary>
        public async Task<bool> StartAsync(int idx)
        {
            try
            {
                // 初始化重连延迟
                _plcInfos[idx].CurrentReconnectDelay = INITIAL_RECONNECT_DELAY_MS;

                // 异步连接PLC
                var connectResult = await _siemensPLCs[idx].ConnectServerAsync();
                if (!connectResult.IsSuccess)
                {
                    OnError?.Invoke($"PLC初始连接失败：{connectResult.Message}（IP：{_plcInfos[idx].Ip}）");
                    return false;
                }

                // 连接成功初始化状态
                _plcInfos[idx].IsConnected = true;
                OnInfo?.Invoke($"PLC初始连接成功（IP：{_plcInfos[idx].Ip}，端口：{_plcInfos[idx].Port}）");
                // 启动异步读取循环（后台任务）
                _dataReadTasks[idx].Start();

                return _plcInfos[idx].IsConnected;
            }
            catch (OperationCanceledException)
            {
                OnInfo?.Invoke("PLC启动操作已取消");
                return false;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"PLC启动异常：{ex.Message}");
                return false;
            }
        }

        public async Task<bool> StopAsync(int idx)
        {
            var connectResult = await _siemensPLCs[idx].ConnectCloseAsync();
            return connectResult.IsSuccess;
        }

        /// <summary>
        /// 异步数据读取循环（核心逻辑）
        /// </summary>
        private async Task ExecuteDataReadingLoop01Async()
        {
            while (true)
            {
                try
                {
                    // 检查连接状态：断开则触发重连
                    if (!_plcInfos[0].IsConnected)
                    {
                        await AttemptReconnectAsync(plcIndex: 0);
                    }
                    else
                    {
                        // 连接正常：处理公共区数据+事件触发
                        await ProcessPublicAreaDataAsync(0);
                    }

                    // 非阻塞等待（支持取消）
                    await Task.Delay(READ_INTERVAL_MS);
                }
                catch (OperationCanceledException)
                {
                    OnInfo?.Invoke("PLC数据读取循环已取消");
                    break;
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"PLC读取循环异常：{ex.Message}");
                    _plcInfos[0].IsConnected = false; // 标记为断开，触发重连
                }
            }
        }

        private async Task ExecuteDataReadingLoop02Async()
        {
            int readCycleCounter = 0; // 循环计数器（控制重连检查频率）

            while (true)
            {
                try
                {
                    // 检查连接状态：断开则触发重连
                    if (!_plcInfos[1].IsConnected)
                    {
                        readCycleCounter++;
                        // 每N次循环检查一次重连（避免频繁尝试）
                        if (readCycleCounter % RECONNECT_CHECK_INTERVAL == 0)
                        {
                            await AttemptReconnectAsync(plcIndex: 1);
                            readCycleCounter = 0;
                        }
                    }
                    else
                    {
                        // 连接正常：处理公共区数据+事件触发
                        await ProcessPublicAreaDataAsync(1);
                    }

                    // 非阻塞等待（支持取消）
                    await Task.Delay(READ_INTERVAL_MS);
                }
                catch (OperationCanceledException)
                {
                    OnInfo?.Invoke("PLC数据读取循环已取消");
                    break;
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"PLC读取循环异常：{ex.Message}");
                    _plcInfos[1].IsConnected = false; // 标记为断开，触发重连
                }
            }
        }

        /// <summary>
        /// PLC自动重连（指数退避策略）
        /// </summary>
        /// <param name="plcIndex">PLC索引</param>
        private async Task AttemptReconnectAsync(int plcIndex)
        {
            var plcInfo = _plcInfos[plcIndex];
            var plcClient = _siemensPLCs[plcIndex];
            int currentDelay = plcInfo.CurrentReconnectDelay;

            try
            {
                // 输出重连信息
                OnInfo?.Invoke($"PLC准备重连：IP={plcInfo.Ip}，当前延迟={currentDelay}ms");

                // 等待重连延迟（带随机抖动）
                await Task.Delay(currentDelay);

                // 关闭现有连接（避免连接泄漏）
                plcClient.ConnectClose();

                // 异步重连
                var reconnectResult = await plcClient.ConnectServerAsync();
                if (reconnectResult.IsSuccess)
                {
                    plcInfo.IsConnected = true;
                    // 连接成功，重置重连延迟
                    plcInfo.CurrentReconnectDelay = INITIAL_RECONNECT_DELAY_MS;
                    OnInfo?.Invoke($"PLC重连成功：IP={plcInfo.Ip}，端口={plcInfo.Port}");
                }
                else
                {
                    // 连接失败，计算下次重连延迟（指数退避+随机抖动）
                    CalculateNextReconnectDelay(plcIndex);
                    OnError?.Invoke($"PLC重连失败：{reconnectResult.Message}，下次延迟={plcInfo.CurrentReconnectDelay}ms");
                }
            }
            catch (Exception ex)
            {
                // 重连异常，计算下次重连延迟
                CalculateNextReconnectDelay(plcIndex);
                OnError?.Invoke($"PLC重连异常：{ex.Message}，下次延迟={plcInfo.CurrentReconnectDelay}ms");
            }
        }

        /// <summary>
        /// 计算下次重连延迟（指数退避+随机抖动）
        /// </summary>
        private void CalculateNextReconnectDelay(int plcIndex)
        {
            var plcInfo = _plcInfos[plcIndex];

            // 指数退避：当前延迟 * 乘数，但不超过最大值
            int nextDelay = (int)(plcInfo.CurrentReconnectDelay * BACKOFF_MULTIPLIER);
            nextDelay = Math.Min(nextDelay, MAX_RECONNECT_DELAY_MS);

            // 添加随机抖动，避免多设备同时重连导致网络拥塞
            if (RECONNECT_JITTER_MS > 0)
            {
                nextDelay += _reconnectRandom.Next(-RECONNECT_JITTER_MS, RECONNECT_JITTER_MS + 1);
                nextDelay = Math.Max(nextDelay, 100);  // 确保延迟不会过小
            }

            plcInfo.CurrentReconnectDelay = nextDelay;
        }
        #endregion

        #region 数据处理逻辑（公共区+事件）
        /// <summary>
        /// 异步处理公共区数据（读取+写入）
        /// </summary>
        private async Task ProcessPublicAreaDataAsync(int plcIndex)
        {
            var plcInfo = _plcInfos[plcIndex];
            var plcClient = _siemensPLCs[plcIndex];
            var publicInfo = plcInfo.PublicInfo;
            if (plcIndex == 0)
            {
                try
                {
                    // 1. 读取公共区（PLC→PC：PublicAreaToPC）
                    var readResult = await plcClient.ReadAsync<OP10PublicAreaToPC>();
                    if (!readResult.IsSuccess)
                    {
                        OnError?.Invoke($"公共区读取失败（{plcInfo.Op}）：{readResult.Message}");
                        plcInfo.IsConnected = false;
                        return;
                    }
                    publicInfo.ReadData = readResult.Content;

                    // 2. 首次读取初始化写入区（PC→PLC：PublicAreaFromPC）
                    if (publicInfo.WriteData == null)
                    {
                        var initWriteResult = await plcClient.ReadAsync<OP10PublicAreaFromPC>();
                        if (!initWriteResult.IsSuccess)
                        {
                            OnError?.Invoke($"写入区初始化失败（{plcInfo.Op}）：{initWriteResult.Message}");
                            plcInfo.IsConnected = false;
                            return;
                        }
                        publicInfo.WriteData = initWriteResult.Content;
                        OnInfo?.Invoke($"写入区初始化成功（{plcInfo.Op}）");
                    }

                    // 3. 触发外部数据回调
                    OnPublicDataCallback?.Invoke(plcInfo.Op, publicInfo.ReadData, publicInfo.WriteData);

                    // 4. 写入公共区数据（PC→PLC）
                    var writeResult = await plcClient.WriteAsync(publicInfo.WriteData);
                    if (!writeResult.IsSuccess)
                    {
                        OnError?.Invoke($"公共区写入失败（{plcInfo.Op}）：{writeResult.Message}");
                        plcInfo.IsConnected = false;
                    }

                    // 5. 处理事件触发
                    var EventTriggers = (publicInfo.ReadData as OP10PublicAreaToPC).EventsTrigger;
                    if (EventTriggers != null)
                    {
                        try
                        {
                            // 遍历所有事件检查触发状态
                            for (int i = 0; i < plcInfo.Events.Count; i++)
                            {
                                var eventInfo = plcInfo.Events[i];
                                // 事件触发且未处理完成
                                if (i < EventTriggers.Length
                                    && EventTriggers[i]
                                    && !eventInfo.TriggerCompleted)
                                {
                                    await ProcessSingleEventAsync(plcIndex, eventInfo); // 异步处理
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            OnError?.Invoke($"事件触发处理异常（{plcInfo.Op}）：{ex.Message}");
                        }
                    }

                }
                catch (OperationCanceledException)
                {
                    // 取消操作不抛出错误
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"公共区处理异常（{plcInfo.Op}）：{ex.Message}");
                    plcInfo.IsConnected = false;
                }
            }
            else if (plcIndex == 1)
            {
                try
                {
                    // 1. 读取公共区（PLC→PC：PublicAreaToPC）
                    var readResult = await plcClient.ReadAsync<OP20PublicAreaToPC>();
                    if (!readResult.IsSuccess)
                    {
                        OnError?.Invoke($"公共区读取失败（{plcInfo.Op}）：{readResult.Message}");
                        plcInfo.IsConnected = false;
                        return;
                    }
                    publicInfo.ReadData = readResult.Content;

                    // 2. 首次读取初始化写入区（PC→PLC：PublicAreaFromPC）
                    if (publicInfo.WriteData == null)
                    {
                        var initWriteResult = await plcClient.ReadAsync<OP20PublicAreaFromPC>();
                        if (!initWriteResult.IsSuccess)
                        {
                            OnError?.Invoke($"写入区初始化失败（{plcInfo.Op}）：{initWriteResult.Message}");
                            plcInfo.IsConnected = false;
                            return;
                        }
                        publicInfo.WriteData = initWriteResult.Content;
                        OnInfo?.Invoke($"写入区初始化成功（{plcInfo.Op}）");
                    }

                    // 3. 触发外部数据回调
                    OnPublicDataCallback?.Invoke(plcInfo.Op, publicInfo.ReadData, publicInfo.WriteData);

                    // 4. 写入公共区数据（PC→PLC）
                    var writeResult = await plcClient.WriteAsync(publicInfo.WriteData);
                    if (!writeResult.IsSuccess)
                    {
                        OnError?.Invoke($"公共区写入失败（{plcInfo.Op}）：{writeResult.Message}");
                        plcInfo.IsConnected = false;
                    }
                    //5 . 处理事件触发
                    var EventTriggers = (publicInfo.ReadData as OP20PublicAreaToPC).EventsTrigger;
                    if (EventTriggers != null)
                    {
                        try
                        {
                            // 遍历所有事件检查触发状态
                            for (int i = 0; i < plcInfo.Events.Count; i++)
                            {
                                var eventInfo = plcInfo.Events[i];
                                // 事件触发且未处理完成
                                if (i < EventTriggers.Length
                                    && EventTriggers[i]
                                    && !eventInfo.TriggerCompleted)
                                {
                                    await ProcessSingleEventAsync(plcIndex, eventInfo); // 异步处理
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            OnError?.Invoke($"事件触发处理异常（{plcInfo.Op}）：{ex.Message}");
                        }
                    }

                }
                catch (OperationCanceledException)
                {
                    // 取消操作不抛出错误
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"公共区处理异常（{plcInfo.Op}）：{ex.Message}");
                    plcInfo.IsConnected = false;
                }
            }
        }

        /// <summary>
        /// 异步处理单个事件（读取数据+线程池回调）
        /// </summary>
        private async Task ProcessSingleEventAsync(int plcIndex, EventInfo eventInfo)
        {
            var plcInfo = _plcInfos[plcIndex];

            try
            {
                // 1. 异步读取事件区数据
                bool readSuccess = await ReadEventAreaDataAsync(plcIndex, eventInfo);
                if (!readSuccess)
                {
                    OnError?.Invoke($"事件数据读取失败（{eventInfo.Name}）");
                    return;
                }

                // 2. 检查SequenceID（避免重复处理）
                if (eventInfo.SequenceIdWrite == eventInfo.SequenceIdRead)
                {
                    OnInfo?.Invoke($"事件SequenceID重复（{eventInfo.Name}）：{eventInfo.SequenceIdRead}");
                    return;
                }

                // 3. 标记为处理中，提交到线程池处理
                eventInfo.TriggerCompleted = true;

                if (plcIndex == 0)
                {
                    _smartThreadPool.QueueWorkItem(
                  new WorkItemCallback(OP10ProcessEventCallback),
                  eventInfo,
                  OP10OnEventProcessed);
                }
                else if (plcIndex == 1)
                {
                    _smartThreadPool.QueueWorkItem(
                    new WorkItemCallback(OP20ProcessEventCallback),
                    eventInfo,
                    OP20OnEventProcessed);
                }

            }
            catch (Exception ex)
            {
                OnError?.Invoke($"事件处理异常（{eventInfo.Name}）：{ex.Message}");
                plcInfo.IsConnected = false;
            }
        }

        /// <summary>
        /// 异步读取事件区数据（支持多事件类型）
        /// </summary>
        private async Task<bool> ReadEventAreaDataAsync(int plcIndex, EventInfo eventInfo)
        {
            var plcClient = _siemensPLCs[plcIndex];

            try
            {
                switch (eventInfo.Index)
                {
                    case 0: // 事件01：对应Event01Area类型
                        // 读取PLC→PC数据（Event01AreaFromPC）
                        var fromPcResult = await plcClient.ReadAsync<OP10Event01AreaFromPC>();
                        if (!fromPcResult.IsSuccess)
                        {
                            OnError?.Invoke($"{eventInfo.Name}FromPC读取失败：{fromPcResult.Message}");
                            return false;
                        }

                        // 读取PC→PLC数据（Event01AreaToPC）
                        var toPcResult = await plcClient.ReadAsync<OP10Event01AreaToPC>();
                        if (!toPcResult.IsSuccess)
                        {
                            OnError?.Invoke($"{eventInfo.Name}ToPC读取失败：{toPcResult.Message}");
                            return false;
                        }

                        // 存储读取结果
                        eventInfo.ObjW = fromPcResult.Content;
                        eventInfo.ObjR = toPcResult.Content;

                        // 更新SequenceID
                        eventInfo.SequenceIdWrite = eventInfo.ObjW.SequenceID;
                        eventInfo.SequenceIdRead = eventInfo.ObjR.SequenceID;
                        break;

                    // 可以添加更多事件类型处理
                    default:
                        OnError?.Invoke($"未实现的事件索引：{eventInfo.Index}");
                        return false;
                }

                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"{eventInfo.Name}数据读取异常：{ex.Message}");
                return false;
            }
        }
        #endregion

        #region 事件回调与结果处理
        /// <summary>
        /// 事件处理回调（在线程池中执行）
        /// </summary>
        private object OP10ProcessEventCallback(object state)
        {
            try
            {
                var eventInfo = state as EventInfo;
                if (eventInfo == null)
                {
                    OnError?.Invoke("事件回调：无效的事件信息");
                    return null;
                }

                // 触发外部事件回调
                OnEventDataCallback?.Invoke(_plcInfos[0].Op, eventInfo);
                return eventInfo;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"事件回调处理异常：{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 事件处理完成后执行（写回PLC结果）
        /// </summary>
        private void OP10OnEventProcessed(IWorkItemResult result)
        {
            if (result.Result is EventInfo eventInfo)
            {
                _ = WriteEventResultAsync(eventInfo, 0);
            }
        }

        /// <summary>
        /// 事件处理回调（在线程池中执行）
        /// </summary>
        private object OP20ProcessEventCallback(object state)
        {
            try
            {
                var eventInfo = state as EventInfo;
                if (eventInfo == null)
                {
                    OnError?.Invoke("事件回调：无效的事件信息");
                    return null;
                }

                // 触发外部事件回调
                OnEventDataCallback?.Invoke(_plcInfos[1].Op, eventInfo);
                return eventInfo;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"事件回调处理异常：{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 事件处理完成后执行（写回PLC结果）
        /// </summary>
        private void OP20OnEventProcessed(IWorkItemResult result)
        {
            if (result.Result is EventInfo eventInfo)
            {
                _ = WriteEventResultAsync(eventInfo, 1);
            }
        }

        /// <summary>
        /// 异步写回事件处理结果到PLC
        /// </summary>
        private async Task WriteEventResultAsync(EventInfo eventInfo, int plcIndex)
        {
            try
            {
                if (!_plcInfos[plcIndex].IsConnected)
                {
                    OnInfo?.Invoke($"{eventInfo.Name}写回跳过：PLC未连接");
                    return;
                }

                // 更新SequenceID（标记为已处理）
                eventInfo.SequenceIdWrite = eventInfo.ObjW.SequenceID = eventInfo.ObjR.SequenceID;

                // 异步写回处理结果
                var writeResult = await _siemensPLCs[plcIndex].WriteAsync(eventInfo.ObjW);
                if (!writeResult.IsSuccess)
                {
                    OnError?.Invoke($"{eventInfo.Name}写回失败：{writeResult.Message}");
                    _plcInfos[plcIndex].IsConnected = false;
                    return;
                }

                // 重置事件状态，允许下次触发
                eventInfo.TriggerCompleted = false;
                OnInfo?.Invoke($"{eventInfo.Name}处理完成，已写回PLC");
            }
            catch (OperationCanceledException)
            {
                // 取消操作不抛出错误
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"{eventInfo.Name}写回异常：{ex.Message}");
                _plcInfos[plcIndex].IsConnected = false;
            }
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
        /// 释放资源（实现IDisposable模式）
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            _smartThreadPool?.Shutdown();
            // 释放非托管资源
            if (_siemensPLCs != null)
            {
                foreach (var plc in _siemensPLCs)
                {
                    plc.ConnectClose();
                }
            }

            OnInfo?.Invoke("PLC通信资源已释放");
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~SiemensPLCConnection()
        {
            Dispose(false);
        }
        #endregion

        #region 初始化PLC信息（状态+配置）
        /// <summary>
        /// 初始化PLC信息（状态+配置）
        /// </summary>
        private PlcInfo[] InitializePlcInfo(PlcConfig[] configs)
        {
            return new[]
            {
                new PlcInfo
                {
                    Op = configs[0].Op ?? "PLC_01",
                    Ip = configs[0].Ip,
                    Port = configs[0].Port,
                    IsConnected = false,
                    CurrentReconnectDelay = INITIAL_RECONNECT_DELAY_MS,
                    PublicInfo = new PublicInfo
                    {
                        ReadLength = 0,
                        WriteLength = 0,
                        ReadData = null,
                        WriteData = null,
                        WriteBuffer = null
                    },
                    Events = new List<EventInfo>
                    {
                        new EventInfo
                        {
                            Index = 0,
                            Name = "测试事件01",
                            TriggerCompleted = false,
                            SequenceIdRead = -1,
                            SequenceIdWrite = -1,
                            ObjR = null,
                            ObjW = null
                        }
                    }
                },
                new PlcInfo
                {
                    Op = configs[1].Op ?? "PLC_02",
                    Ip = configs[1].Ip,
                    Port = configs[1].Port,
                    IsConnected = false,
                    CurrentReconnectDelay = INITIAL_RECONNECT_DELAY_MS,
                    PublicInfo = new PublicInfo
                    {
                        ReadLength = 0,
                        WriteLength = 0,
                        ReadData = null,
                        WriteData = null,
                        WriteBuffer = null
                    },
                    Events = new List<EventInfo>
                    {
                        new EventInfo
                        {
                            Index = 0,
                            Name = "测试事件01",
                            TriggerCompleted = false,
                            SequenceIdRead = -1,
                            SequenceIdWrite = -1,
                            ObjR = null,
                            ObjW = null
                        }
                    }
                }
            };
        }
        #endregion
    }

    #region 辅助类定义
    /// <summary>
    /// PLC配置信息
    /// </summary>
    public class PlcConfig
    {
        public string Op { get; set; }      // PLC操作名称
        public string Ip { get; set; }      // IP地址
        public int Port { get; set; }       // 端口号
    }

    /// <summary>
    /// PLC信息（包含状态和配置）
    /// </summary>
    public class PlcInfo
    {
        public string Op { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public bool IsConnected { get; set; }
        public int CurrentReconnectDelay { get; set; } // 当前重连延迟（毫秒）
        public PublicInfo PublicInfo { get; set; }
        public List<EventInfo> Events { get; set; }
    }

    /// <summary>
    /// 公共区信息
    /// </summary>
    public class PublicInfo
    {
        public ushort ReadLength { get; set; }
        public ushort WriteLength { get; set; }
        public byte[] WriteBuffer { get; set; }
        public dynamic ReadData { get; set; }    // PLC→PC数据
        public dynamic WriteData { get; set; } // PC→PLC数据
    }

    /// <summary>
    /// 事件信息
    /// </summary>
    public class EventInfo
    {
        public int Index { get; set; }                  // 事件索引
        public string Name { get; set; }                // 事件名称
        public bool TriggerCompleted { get; set; }      // 触发完成标记
        public int SequenceIdRead { get; set; }         // 读取序列ID
        public int SequenceIdWrite { get; set; }        // 写入序列ID
        public dynamic ObjR { get; set; }               // PLC→PC数据对象
        public dynamic ObjW { get; set; }               // PC→PLC数据对象
    }

    // 以下为PLC数据区类型定义（根据实际项目调整）
    #endregion
}
