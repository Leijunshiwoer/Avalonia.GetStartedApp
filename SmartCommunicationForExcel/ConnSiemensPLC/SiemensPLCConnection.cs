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
    public class SiemensPLCConnection : IDisposable
    {
        #region 常量与配置
        private const int READ_INTERVAL_MS = 10;
        private const string AUTHORIZATION_CODE = "AA10EE5AF72513F0CD1C766EBFDA0835";
        #endregion

        #region 字段与属性
        private readonly SiemensS7Net[] _siemensPLCs;
        private readonly Thread _dataReadThread;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly ConcurrentQueue<EventInfo> _eventQueue = new ConcurrentQueue<EventInfo>();
        private readonly SmartThreadPool _smartThreadPool;
        private readonly PlcInfo[] _plcInfos;
        private bool _isDisposed = false;
        #endregion

        #region 事件定义
        public delegate void ErrorHandler(string errMsg);
        public event ErrorHandler OnError;

        public delegate void InfoHandler(string info);
        public event InfoHandler OnInfo;

        public delegate void PublicDataCallback(string op, object readData, object writeData);
        public event PublicDataCallback OnPublicDataCallback;

        public delegate void EventDataCallback(string op, object eventData);
        public event EventDataCallback OnEventDataCallback;
        #endregion

        #region 构造函数与初始化
        public SiemensPLCConnection(PlcConfig config)
        {
            _plcInfos = InitializePlcInfo(config);
            _siemensPLCs = InitializePLCConnections();
            _smartThreadPool = CreateSmartThreadPool();
            _dataReadThread = new Thread(StartDataReadingLoop) { IsBackground = true };

            Start();
        }

        private PlcInfo[] InitializePlcInfo(PlcConfig config)
        {
            return new[] { new PlcInfo
            {
                Op = config.Op,
                Ip = config.Ip,
                Port = config.Port,
                IsConnected = false,
                PublicInfo = new PublicInfo
                {
                    ReadLength = 0,
                    WriteLength = 0
                },
                Events = new List<EventInfo>
                {
                    new EventInfo
                    {
                        Index = 0,
                        Name = "测试事件01",
                        TriggerCompleted = false,
                        SequenceIdRead = -1,
                        SequenceIdWrite = -1
                    }
                }
            }};
        }

        private SiemensS7Net[] InitializePLCConnections()
        {
            return new[] { new SiemensS7Net(SiemensPLCS.S1500)
            {
                IpAddress = _plcInfos[0].Ip,
                Port = _plcInfos[0].Port
            }};
        }

        private SmartThreadPool CreateSmartThreadPool()
        {
            var stpStartInfo = new STPStartInfo
            {
                CallToPostExecute = CallToPostExecute.Always,
                FillStateWithArgs = true
            };
            return new SmartThreadPool(stpStartInfo);
        }

        public void Start()
        {
            try
            {
                Authorization.SetAuthorizationCode(AUTHORIZATION_CODE);
                var connectResult = _siemensPLCs[0].ConnectServer();
                if (!connectResult.IsSuccess)
                {
                    OnError?.Invoke($"PLC连接失败：{connectResult.Message}");
                    return;
                }

                _plcInfos[0].IsConnected = true;
                _dataReadThread.Start();
                OnInfo?.Invoke("PLC通信已启动");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"启动失败：{ex.Message}");
            }
        }
        #endregion

        #region 数据读取与处理循环
        private void StartDataReadingLoop()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    if (!_plcInfos[0].IsConnected)
                    {
                        Thread.Sleep(READ_INTERVAL_MS);
                        continue;
                    }

                    ProcessPublicAreaData(0);
                    ProcessEventTriggers(0);
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"读取循环异常：{ex.Message}");
                }

                _cts.Token.WaitHandle.WaitOne(READ_INTERVAL_MS);
            }
        }

        private void ProcessPublicAreaData(int plcIndex)
        {
            // 读取公共区（ToPC）
            var readResult = _siemensPLCs[plcIndex].Read<PublicAreaToPC>();
            if (!readResult.IsSuccess)
            {
                OnError?.Invoke($"{_plcInfos[plcIndex].Op}公共区读取失败：{readResult.Message}");
                return;
            }
            _plcInfos[plcIndex].PublicInfo.ReadData = readResult.Content;

            // 首次读取初始化写入区（FromPC）
            if (_plcInfos[plcIndex].PublicInfo.WriteData == null)
            {
                var initWriteResult = _siemensPLCs[plcIndex].Read<PublicAreaFromPC>();
                if (!initWriteResult.IsSuccess)
                {
                    OnError?.Invoke($"{_plcInfos[plcIndex].Op}初始化写入区失败：{initWriteResult.Message}");
                    return;
                }
                _plcInfos[plcIndex].PublicInfo.WriteData = initWriteResult.Content;
            }

            // 触发公共区回调
            OnPublicDataCallback?.Invoke(
                _plcInfos[plcIndex].Op,
                _plcInfos[plcIndex].PublicInfo.ReadData,
                _plcInfos[plcIndex].PublicInfo.WriteData);

            // 写入公共区数据
            var writeResult = _siemensPLCs[plcIndex].Write(_plcInfos[plcIndex].PublicInfo.WriteData);
            if (!writeResult.IsSuccess)
            {
                OnError?.Invoke($"{_plcInfos[plcIndex].Op}公共区写入失败：{writeResult.Message}");
            }
        }

        private void ProcessEventTriggers(int plcIndex)
        {
            var plcInfo = _plcInfos[plcIndex];
            var eventTrigger = plcInfo.PublicInfo.ReadData?.EventsTrigger;
            if (eventTrigger == null) return;

            for (int i = 0; i < plcInfo.Events.Count; i++)
            {
                if (eventTrigger[i] && !plcInfo.Events[i].TriggerCompleted)
                {
                    ProcessEvent(plcIndex, i);
                }
            }
        }

        private void ProcessEvent(int plcIndex, int eventIndex)
        {
            var plcInfo = _plcInfos[plcIndex];
            var eventInfo = plcInfo.Events[eventIndex];

            // 读取事件区数据（保留dynamic类型处理）
            var readSuccess = ReadEventData(plcIndex, eventInfo);
            if (!readSuccess) return;

            // 检查SequenceID是否匹配（使用dynamic访问属性）
            if (eventInfo.SequenceIdWrite == eventInfo.SequenceIdRead)
            {
                OnInfo?.Invoke($"{eventInfo.Name}事件SequenceID相等：{eventInfo.SequenceIdRead}");
                return;
            }

            // 提交到线程池处理
            eventInfo.TriggerCompleted = true;
            _smartThreadPool.QueueWorkItem(ProcessEventCallback, eventInfo, OnEventProcessed);
        }

        // 读取事件区数据（保留dynamic类型）
        private bool ReadEventData(int plcIndex, EventInfo eventInfo)
        {
            try
            {
                // 读取事件区FromPC（PLC到PC）
                var readFromResult = _siemensPLCs[plcIndex].Read<Event01AreaFromPC>();
                if (!readFromResult.IsSuccess)
                {
                    OnError?.Invoke($"{eventInfo.Name}FromPC读取失败：{readFromResult.Message}");
                    return false;
                }

                // 读取事件区ToPC（PC到PLC）
                var readToResult = _siemensPLCs[plcIndex].Read<Event01AreaToPC>();
                if (!readToResult.IsSuccess)
                {
                    OnError?.Invoke($"{eventInfo.Name}ToPC读取失败：{readToResult.Message}");
                    return false;
                }

                // 保留dynamic类型赋值
                eventInfo.ObjW = readFromResult.Content;
                eventInfo.ObjR = readToResult.Content;

                // 动态访问SequenceID属性
                eventInfo.SequenceIdWrite = eventInfo.ObjW.SequenceID;
                eventInfo.SequenceIdRead = eventInfo.ObjR.SequenceID;

                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"{eventInfo.Name}数据读取异常：{ex.Message}");
                return false;
            }
        }
        #endregion

        #region 事件回调处理
        private object ProcessEventCallback(object state)
        {
            var eventInfo = state as EventInfo;
            OnEventDataCallback?.Invoke(_plcInfos[0].Op, eventInfo);
            return eventInfo;
        }

        private void OnEventProcessed(IWorkItemResult result)
        {
            if (result.Result is EventInfo eventInfo)
            {
                WriteEventResultToPLC(eventInfo, 0);
            }
        }

        private void WriteEventResultToPLC(EventInfo eventInfo, int plcIndex)
        {
            try
            {
                // 动态访问和修改属性
                eventInfo.SequenceIdWrite = eventInfo.ObjW.SequenceID = eventInfo.ObjR.SequenceID;
                var writeResult = _siemensPLCs[plcIndex].Write(eventInfo.ObjW);

                if (!writeResult.IsSuccess)
                {
                    OnError?.Invoke($"{eventInfo.Name}写入失败：{writeResult.Message}");
                    return;
                }

                eventInfo.TriggerCompleted = false;
                OnInfo?.Invoke($"{eventInfo.Name}处理完成，已写回PLC");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"{eventInfo.Name}写回异常：{ex.Message}");
            }
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
                _cts.Cancel();
                _dataReadThread?.Join();
                _smartThreadPool?.Shutdown();
                _cts.Dispose();
            }

            if (_siemensPLCs != null)
            {
                foreach (var plc in _siemensPLCs)
                {
                    plc.ConnectClose();
                }
            }

            _isDisposed = true;
        }

        ~SiemensPLCConnection()
        {
            Dispose(false);
        }
        #endregion
    }

    #region 辅助类与配置
    public class PlcConfig
    {
        public string Op { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
    }

    public class PlcInfo
    {
        public string Op { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public bool IsConnected { get; set; }
        public PublicInfo PublicInfo { get; set; }
        public List<EventInfo> Events { get; set; }
    }

    public class PublicInfo
    {
        public ushort ReadLength { get; set; }
        public ushort WriteLength { get; set; }
        public byte[] WriteBuffer { get; set; }
        public PublicAreaToPC ReadData { get; set; }
        public PublicAreaFromPC WriteData { get; set; }
    }

    // 保留dynamic类型的EventInfo
    public class EventInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public bool TriggerCompleted { get; set; }
        public int SequenceIdRead { get; set; }
        public int SequenceIdWrite { get; set; }
        public dynamic ObjR { get; set; }  // 保留dynamic
        public dynamic ObjW { get; set; }  // 保留dynamic
    }
    #endregion

    // PLC数据区模型类
  
}
