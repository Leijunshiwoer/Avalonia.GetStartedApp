using Avalonia;
using Avalonia.Threading;
using GetStartedApp.Interface;
using GetStartedApp.Models;
using HslCommunication;
using Newtonsoft.Json;
using Prism.Commands;
using SmartCommunicationForExcel;
using SmartCommunicationForExcel.Model;
using SmartCommunicationForExcel.MQTTClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TouchSocket.Mqtt;
using Ursa.Controls;

namespace GetStartedApp.ViewModels.PLC
{
    public class EventSiemensViewModel : ViewModelBase
    {
        // 直接初始化集合，确保始终使用同一个实例
        private readonly ObservableCollection<PlcEventModel> _obPlcEvnet = new ObservableCollection<PlcEventModel>();
        private readonly IMqttClientService _mqttService;
        // 2. 线程安全的消息队列（存储待入库的 MQTT 消息）
        private readonly ConcurrentQueue<(string topic, string payload, DateTime receiveTime)> _messageQueue = new();
        // 3. 标记队列是否正在消费（避免重复启动多个后台线程）
        private bool _isProcessingQueue;
        public ObservableCollection<PlcEventModel> ObPlcEvent
        {
            get { return _obPlcEvnet; }
        }

        private List<PlcEventModel> m_plcEvent = new List<PlcEventModel>();

        public List<PlcEventModel> M_plcEvent
        {
            get { return m_plcEvent; }
            set { SetProperty(ref m_plcEvent, value); }
        }

        public EventSiemensViewModel(ISiemensEvent siemensEvent,IMqttClientService mqttService)
        {
            siemensEvent.Instance(this);
             _mqttService = mqttService;
          // _ = StartClientAsync();
        }

        #region PLC事件操作
        public PlcEventParamModel DoEvent(PlcEventParamModel param)
        {
            // 核心业务逻辑处理
            List<MyData> keyValuePairs = new List<MyData>();
            string st = param.Params.GetMyData("WorkStage").ValueData.ToString();
            PlcEventParamModel toPlcParam = new PlcEventParamModel
            {
                PlcAddr = param.PlcAddr,
                PlcName = param.PlcName,
                EventName = param.EventName,
                StartTime = param.StartTime,
                EventClass = st,
                Params = keyValuePairs
            };


            string eventNameType = ExtractEventNameType(param.EventName);

            try
            {
                switch (eventNameType)
                {
                    case "PartReq":
                        toPlcParam.Params = PartReq(param.Params);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            toPlcParam.RandomCode = param.RandomCode;

            // 添加事件记录，确保在UI线程执行
            AddEventRecord(toPlcParam);

            return toPlcParam;
        }


        #region 事件实现
        private List<MyData> PartReq(List<MyData> @params)
        {
            List<MyData> toPcDatas = new List<MyData>();
            toPcDatas.SetValue_ResultAndMessageW(1, "成功");
            return toPcDatas;
        }
        #endregion

        #region 私有方法
        private string ExtractEventNameType(string eventName)
        {
            string[] cs = eventName.Split('_');
            return cs.Length > 1 ? cs[cs.Length - 1] : string.Empty;
        }

        // 适配Avalonia的UI线程更新逻辑
        private void AddEventRecord(PlcEventParamModel param)
        {
            // 检查当前是否在UI线程
            if (Dispatcher.UIThread.CheckAccess())
            {
                // 在UI线程，直接更新
                UpdateCollection(param);
            }
            else
            {
                // 不在UI线程，调度到UI线程执行
                Dispatcher.UIThread.Post(() => UpdateCollection(param));
                // 如果需要等待执行完成，可以使用InvokeAsync:
                //await Dispatcher.UIThread.InvokeAsync(() => UpdateCollection(param));
            }
        }

        // 实际执行集合更新的方法
        private void UpdateCollection(PlcEventParamModel param)
        {
            var newRecord = new PlcEventModel
            {
                PlcEventLogId = param.PlcEventLogId,
                RandomCode = param.RandomCode,
                FName = param.PlcName,
                FStationName = param.EventClass,
                FAddr = param.PlcAddr,
                FEvent = param.EventName,
                FStartTime = param.StartTime,
                FDoTime = (DateTime.Now - param.StartTime).TotalMilliseconds,
                FResult = param.Params.FirstOrDefault(it => it.Key == "ResultCode")?.ValueData.ToString(),
                FResultMark = param.Params.FirstOrDefault(it => it.Key == "Message")?.ValueData.ToString(),
            };

            if (ObPlcEvent.Count >= 100)
            {
                ObPlcEvent.RemoveAt(ObPlcEvent.Count - 1);
            }

            ObPlcEvent.Insert(0, newRecord);
        }
        #endregion

        #endregion


        private DelegateCommand _ConnMqttCmd;
        public DelegateCommand ConnMqttCmd =>
            _ConnMqttCmd ?? (_ConnMqttCmd = new DelegateCommand(ExecuteConnMqttCmd));

        void ExecuteConnMqttCmd()
        {
            _ = StartClientAsync(); 
        }

        private ObservableCollection<MqttReciveModel> _ObMqttMessage=new ObservableCollection<MqttReciveModel>();
        public ObservableCollection<MqttReciveModel> ObMqttMessage
        {
            get { return _ObMqttMessage; }
            set { SetProperty(ref _ObMqttMessage, value); }
        }
        private string _MqttStatus;
        public string MqttStatus
        {
            get { return _MqttStatus; }
            set { SetProperty(ref _MqttStatus, value); }
        }
        #region mqtt
        public async Task StartClientAsync()
        {
            try
            {
                await _mqttService.InitClientAsync("192.168.115.208:1883", "kstopa");
                await _mqttService.ConnectAsync();
                if (_mqttService.IsConnected)
                {
                    _mqttService.ConnectionStatusChanged += OnConnectionStatusChanged;
                    _mqttService.MessageReceived += OnMessageReceived;
                    //_mqttService.NetworkErrorOccurred += OnNetworkError;


                    Dispatcher.UIThread.Post(() =>
                    {
                        // UI 操作代码
                        MqttStatus = "已连接";
                    });
                }


            }
            catch (Exception ex)
            {
                await MessageBox.ShowAsync($"连接异常：{ex.Message}", "错误", MessageBoxIcon.Error);
            }

        }

        private void OnMessageReceived(MqttArrivedMessage message)
        {
                var receiveTime = DateTime.Now;
           var payload = System.Text.Encoding.UTF8.GetString(message.Payload.ToArray());
            // 入队（ConcurrentQueue 线程安全，O(1) 操作，不阻塞 UI）
            _messageQueue.Enqueue((message.TopicName, payload, receiveTime));
               // 启动后台消费队列（确保只启动一个线程）
                ProcessMessageQueueAsync();
        }

        private void OnConnectionStatusChanged(string obj)
        {
            
        }

        //private void OnNetworkError(Exception exception)
        //{

        //    Dispatcher.UIThread.Post(() =>
        //    {
        //        MqttStatus = "连接中断";
        //    });
        //}

        //private void OnMessageReceived(string topic, string payload)
        //{
        //    var receiveTime = DateTime.Now;
        //    // 入队（ConcurrentQueue 线程安全，O(1) 操作，不阻塞 UI）
        //    _messageQueue.Enqueue((topic, payload, receiveTime));
        //    // 启动后台消费队列（确保只启动一个线程）
        //    ProcessMessageQueueAsync();
        //}

        //private void OnConnectionStatusChanged(OperateResult result)
        //{
        //    if (result.Message== "Reconnected to MQTT server successfully")
        //    {
        //        Dispatcher.UIThread.Post(() =>
        //        {
        //            MqttStatus = "已连接";
        //        });
        //    }
        //}


        private bool _isFirst=true;
        private ushort _id;
        private async void ProcessMessageQueueAsync()
        {
            // 避免重复启动消费线程（如果已在处理，直接返回）
            if (_isProcessingQueue) return;
            _isProcessingQueue = true;

            try
            {
                // 循环消费队列，直到队列为空
                while (_messageQueue.TryDequeue(out var message))
                {
                    switch (message.topic)
                    {
                        case "devices/event01R/telemetry":
                            // 处理上行消息

                          var a=  JsonConvert.DeserializeObject<Event01R>(message.payload);
                            if (_isFirst)
                            {
                                _id = a.event01R_sequnceId;
                                var w = new Event01W()
                                {
                                    event01W_sequnceId = a.event01R_sequnceId
                                };
                                _isFirst = false;
                                await _mqttService.PublishAsync("devices/PLC01/rpc/request/write/kstopa", JsonConvert.SerializeObject(w));
                            }
                            else
                            {
                                var w = new Event01W()
                                {
                                    event01W_sequnceId = ++_id
                                };
                                await _mqttService.PublishAsync("devices/PLC01/rpc/request/write/kstopa", JsonConvert.SerializeObject(w));
                            }

                              
                            break;
                        default:
                            break;
                    } // 模拟处理消息（如写入数据库）
                      // 2. 创建要显示的MQTT消息记录
                    //var mqttRecord = new MqttReciveModel
                    //{
                    //    FTopic = message.topic,
                    //    FMessage = message.payload,
                    //    FRecivedTime = message.receiveTime,
                    //    // 其他属性根据你的MqttReciveModel定义补充
                    //};

                    //// 3. 在UI线程更新集合（Avalonia特有方式）
                    //await Dispatcher.UIThread.InvokeAsync(() =>
                    //{
                    //    // 确保集合不超过100条，超过则移除最旧的
                    //    if (ObMqttMessage.Count >= 100)
                    //    {
                    //        ObMqttMessage.RemoveAt(ObMqttMessage.Count - 1); // 移除最后一条（最旧）
                    //    }

                    //    // 添加新记录到最前面（最新的显示在顶部）
                    //    ObMqttMessage.Insert(0, mqttRecord);
                    //});



                    // await  _mqttService.PublishAsync("","");


                    //Task.Delay(100).Wait(); // 模拟一些处理时间
                }
            }
            catch (Exception ex)
            {
                // 捕获队列处理的全局异常（不崩溃 UI）
                await MessageBox.ShowAsync($"队列处理异常：{ex.Message}", "错误", MessageBoxIcon.Error);
            }
            finally
            {
                // 无论成功失败，标记为“未处理”，允许下次启动
                _isProcessingQueue = false;
            }
        }
        #endregion
    }
}
