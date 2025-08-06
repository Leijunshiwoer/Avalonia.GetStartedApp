using Amib.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.Profinet.Beckhoff;
using System.Collections.Concurrent;
using Unity;
using SmartCommunicationForExcel.Implementation.Beckhoff;
using HslCommunication;
using System.Diagnostics;
using System.Threading;
using System.Configuration;

namespace SmartCommunicationForExcel.EventHandle.Beckhoff
{
    public class BeckhoffEventHandle
    {
        public string InstanceName { get; set; }
        private string EventTriggerTagName { get; set; }

        private SmartThreadPool _stp;
        private IBeckhoffEventExecuter _see;

        private Thread _workThread;
        private ManualResetEvent _terminatedEvent;

        private BeckhoffAdsNet[] _plcs;
        //private SmartSharp7[] _plcSharp7s;

        // private OmronFinsNet[] _omronPlcs;

        private BeckhoffGlobalConfig _globalConfig;

        private bool[] _bEventTriggerCompleted = new bool[100];


        private ConcurrentQueue<EventBeckhoffThreadState> _queueHandleEventCompleted;

        public BeckhoffEventHandle(SmartThreadPool stp, IUnityContainer container)
        {
            _stp = stp;

            if (container.IsRegistered<IBeckhoffEventExecuter>("Beckhoff"))
                _see = container.Resolve<IBeckhoffEventExecuter>("Beckhoff");
            else
                _see = container.Resolve<IBeckhoffEventExecuter>();

            _queueHandleEventCompleted = new ConcurrentQueue<EventBeckhoffThreadState>();

            _plcs = new BeckhoffAdsNet[2];

            _workThread = new Thread(new ThreadStart(WorkThread)) { IsBackground = true };
            _terminatedEvent = new ManualResetEvent(false);
        }

        /// <summary>
        /// 启动一个单CPU事件处理实例
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strEventTriggerTagName">触发事件标签名 默认EventTrigger</param>
        /// <param name="globalConfig"></param>
        /// <returns></returns>
        public bool StartWork(string strName, BeckhoffGlobalConfig globalConfig, string strEventTriggerTagName = "EventTrigger")
        {
            InstanceName = strName;
            _globalConfig = globalConfig;
            EventTriggerTagName = strEventTriggerTagName;

            if (globalConfig.CpuInfo.CpuType == Model.CpuType.Beckhoff)
            {
                if (globalConfig.CpuInfo.Dll == Model.DllType.Type_Hsl)
                {
                    Authorization.SetAuthorizationCode(ConfigurationManager.AppSettings["AuthorizationCode"]);

                    _plcs[0] = new BeckhoffAdsNet();
                    _plcs[0].IpAddress = globalConfig.CpuInfo.IP;
                    _plcs[0].Port = globalConfig.CpuInfo.Port;
                    _plcs[0].SetTargetAMSNetId(globalConfig.CpuInfo.TargetNetId);
                    _plcs[0].SetSenderAMSNetId(globalConfig.CpuInfo.SenderNetId);
                    _plcs[0].SetPersistentConnection();

                    //_plcs[1] = new BeckhoffAdsNet();
                    //_plcs[1].IpAddress = globalConfig.CpuInfo.IP;
                    //_plcs[1].Port = globalConfig.CpuInfo.Port;
                    //_plcs[1].SetTargetAMSNetId(globalConfig.CpuInfo.TargetNetId);
                    //_plcs[1].SetSenderAMSNetId(globalConfig.CpuInfo.SenderNetId);
                    //_plcs[1].SetPersistentConnection();

                    if (!_plcs[0].ConnectServer().IsSuccess)
                        return false;
                    //if (!_plcs[1].ConnectServer().IsSuccess)
                    //    return false;
                }
            }

            _workThread.Start();

            return true;
        }

        public void WorkStop()
        {
            _terminatedEvent.Set();
        }


        private void WorkThread()
        {
            try
            {
                Stopwatch sw = new Stopwatch();

                var _cycleTime = _globalConfig.CpuInfo.CycleTime;
                if (_cycleTime < 1 || _cycleTime > 2000)
                    _cycleTime = 1;

                while (!_terminatedEvent.WaitOne(_cycleTime))
                {
                    if (_globalConfig.EapConfig.Count == 0 || _globalConfig.PlcConfig.Count == 0)
                        continue;

                    sw.Restart();

                    //处理回写事件
                    //var ets = GetQueue();
                    //if(ets != null)
                    //{ 
                    if (!_queueHandleEventCompleted.IsEmpty)
                    {
                        EventBeckhoffThreadState ets = null;
                        if (_queueHandleEventCompleted.TryDequeue(out ets))
                        {
                            /*
                             * 因为每次手动赋值SequenceID不方便，不需要手动在事件回调里面赋值，赋值工作由事件回调后自动完成
                             * 
                             * 把 PLC的 SequenceID 赋值给 EAP的SequenceID
                             */
                            try
                            {
                                BeckhoffEventIO seiEap = ets.SE.ListOutput.Where(it => it.TagName == "SequenceID").SingleOrDefault();
                                BeckhoffEventIO seiPlc = ets.SE.ListInput.Where(it => it.TagName == "SequenceID").SingleOrDefault();

                                seiEap.SetInt16(seiPlc.GetInt16());
                            }
                            catch
                            {
                                //异常暂时不做处理
                            }

                            // if (!_plcs[1].Write(ets.SE.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ets.SE.ListOutput)).IsSuccess)
                            HslCommunication.OperateResult operateResult = _plcs[0].Write(ets.SE.PC_LabelName, PackageDataToPlc(ets.SE.ListOutput));
                            if (!operateResult.IsSuccess)//通过标签写入
                            {
                                Console.WriteLine("Write Single Event Data Fail.");
                                _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, string.Format("WriteSingleEventData Fail,EventName:{0}", ets.SE.EventName));
                            }
                            else
                            {
                                Console.WriteLine("Write Single Event Success;ID:" + ets.SE.ListOutput[0].GetInt16());
                                _see.Err(ets.InstanceName, null, $"写事件失败：Code {operateResult.ErrorCode},Message {operateResult.Message}");
                            }

                            //else
                            //{
                            //    if (!_plcSharp7s[1].Write(ets.SE.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ets.SE.ListOutput)).IsSuccess)
                            //    {
                            //        Console.WriteLine("Write Single Event Data Fail.");
                            //        _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.EapConfig, _globalConfig.PlcConfig, string.Format("WriteSingleEventData Fail,EventName:{0}", ets.SE.EventName));
                            //    }
                            //}
                            _bEventTriggerCompleted[ets.EventIndex] = false;
                        }
                        else
                        {
                            _see.Err(ets.InstanceName, null, "安全队列获取失败");
                        }
                    }

                    //获取当前PLC内部写入值
                    //HslCommunication.OperateResult<byte[]> _rlt = _plcs[0].Read(_globalConfig.PlcConfig[0].GetMBAddressTag, GetRWLength(_globalConfig.PlcConfig));
                    //if (_rlt.IsSuccess)
                    //    ResolveDataToEvent(_rlt.Content, _globalConfig.PlcConfig);
                    //else
                    //{
                    //    _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.EapConfig, _globalConfig.PlcConfig);
                    //    continue;
                    //}

                    //HslCommunication.OperateResult<byte[]> rlt = _plcs[0].Read(_globalConfig.PlcConfig[0].GetMBAddressTag, GetRWLength(_globalConfig.PlcConfig));
                    string label = $"s={_globalConfig.CpuInfo.PlcConfigLabel}";
                    ushort sss = GetRWLength(_globalConfig.PlcConfig);
                    HslCommunication.OperateResult<byte[]> rlt = _plcs[0].Read($"s={_globalConfig.CpuInfo.PlcConfigLabel}", GetRWLength(_globalConfig.PlcConfig));
                    if (rlt.IsSuccess)
                    {
                        try
                        {
                            ResolveDataToEvent(rlt.Content, _globalConfig.PlcConfig);
                        }
                        catch (Exception ex)
                        {
                            _see.Err(_globalConfig.FileName, rlt.Content, ex.Message);
                        }
                        //公共事件处理
                        _see.SubscribeCommonInfo(InstanceName, true, _globalConfig.PlcConfig, _globalConfig.EapConfig);
                        //处理事件触发
                        HandleEventTrigger(_globalConfig.PlcConfig.Find(t => t.TagName == EventTriggerTagName));

                        HslCommunication.OperateResult operateResult = _plcs[0].Write($"s={_globalConfig.CpuInfo.EapConfigLabel}", PackageDataToPlc(_globalConfig.EapConfig));
                        if (!operateResult.IsSuccess)
                        {
                            Console.WriteLine("WriteCommonData Fail.");
                            _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, $"WriteCommonData Fail {operateResult.Message}");
                        }
                        else
                        {
                            //_see.Err(InstanceName, null, $"eap公共区写失败:{operateResult.Message}" );
                        }
                    }
                    else
                    {
                        Console.WriteLine("ReadCommonData Fail.");
                        _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, $"ReadCommonData Fail {rlt.Message}");
                    }

                    //else if (_globalConfig.CpuInfo.Dll == Model.DllType.Type_Sharp7)
                    //{
                    //    ////获取当前PLC内部写入值
                    //    //OperateResult<byte[]> _rlt = _plcSharp7s[0].Read(_globalConfig.PlcConfig[0].GetMBAddressTag, GetRWLength(_globalConfig.PlcConfig));
                    //    //if (_rlt.IsSuccess)
                    //    //    ResolveDataToEvent(_rlt.Content, _globalConfig.PlcConfig);
                    //    //else
                    //    //{
                    //    //    _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.EapConfig, _globalConfig.PlcConfig);
                    //    //    continue;
                    //    //}

                    //    OperateResult<byte[]> rlt = _plcSharp7s[0].Read(_globalConfig.EapConfig[0].GetMBAddressTag, GetRWLength(_globalConfig.EapConfig));
                    //    if (rlt.IsSuccess)
                    //    {
                    //        ResolveDataToEvent(rlt.Content, _globalConfig.EapConfig);
                    //        //公共事件处理
                    //        _see.SubscribeCommonInfo(InstanceName, true, _globalConfig.EapConfig, _globalConfig.PlcConfig);
                    //        //处理事件触发
                    //        HandleEventTrigger(_globalConfig.EapConfig.Find(t => t.TagName == EventTriggerTagName));

                    //        if (!_plcSharp7s[0].Write(_globalConfig.PlcConfig[0].GetMBAddressTag, PackageDataToPlc(_globalConfig.PlcConfig)).IsSuccess)
                    //        {
                    //            Console.WriteLine("WriteCommonData Fail.");
                    //            _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.EapConfig, _globalConfig.PlcConfig, "WriteCommonData Fail");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("ReadCommonData Fail.");
                    //        _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.EapConfig, _globalConfig.PlcConfig, "ReadCommonData Fail");
                    //    }
                    //}
                }

                //Close Connect
                //switch (_globalConfig.CpuInfo.Dll)
                //{
                //    case Model.DllType.Type_Hsl:
                _plcs[0].ConnectClose();
            //    _plcs[1].ConnectClose();
                //      break;
                //case Model.DllType.Type_Sharp7:
                //    _plcSharp7s[0].ConnectClose();
                //    _plcSharp7s[1].ConnectClose();
                //    break;
                //     default:
                //        break;
                // }
            }
            catch (Exception ex)
            {
                _see.Err(_globalConfig.FileName, Encoding.Default.GetBytes("Error"), ex.Message);
            }
        }

        /// <summary>
        /// 处理事件触发位
        /// </summary>
        /// <param name="siemensEventIO"></param>
        private void HandleEventTrigger(BeckhoffEventIO siemensEventIO)
        {
            if (null == siemensEventIO)
            {
                Console.WriteLine("Get SequenceID Event Fail,It's Not Find SequenceID EventInstance.");
                _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, "Get SequenceID Event Fail, It's Not Find SequenceID EventInstance.");
                return;
            }

            bool[] triggers = siemensEventIO.TransBool(siemensEventIO.DataValue, 0, siemensEventIO.DataValue.Length);

            for (int i = 0; i < triggers.Length; i++)
            {
                if (triggers[i])
                {//有事件触发
                    if (i < _globalConfig.EventConfig.Count)
                    {//事件触发位配置有事件
                        BeckhoffEventInstance ei = _globalConfig.EventConfig[i];

                        if (ei.DisableEvent)
                            continue;

                        if (ei.ListInput.Count > 0 && ei.ListOutput.Count > 0)
                        {//已经有线程处理当前事件 当前事件正在处理中
                            if (!_bEventTriggerCompleted[i])
                            {

                                //HslCommunication.OperateResult<byte[]> rlt = _plcs[1].Read(ei.ListInput[0].GetMBAddressTag, GetRWLength(ei.ListInput));
                                HslCommunication.OperateResult<byte[]> rlt = _plcs[0].Read($"s={ei.PLC_LabelName}", GetRWLength(ei.ListInput));
                                if (rlt.IsSuccess)
                                {
                                    //解析数据到事件结构
                                    try
                                    {
                                        ResolveDataToEvent(rlt.Content, ei.ListInput);
                                    }
                                    catch (Exception ex)
                                    {
                                        _see.Err(_globalConfig.FileName, rlt.Content, ex.Message);
                                        return;
                                    }

                                    short? nInputSequenceID = ei.ListInput.Find(t => t.TagName == "SequenceID")?.GetInt16() ?? 0;
                                    short? nOutSequenceID = ei.ListOutput.Find(t => t.TagName == "SequenceID")?.GetInt16() ?? 0;

                                    if (nInputSequenceID != nOutSequenceID)
                                    {//事件ID不一致 当前事件触发
                                        _bEventTriggerCompleted[i] = true;

                                        //事件回调处理完毕 将在事件处理线程上执行
                                        ei.OnEventTriggerCompleted += (ets) =>
                                        {
                                            Console.WriteLine("Handle Event Callback");
                                        };

                                        /*
                                         * 对事件的再次封装 实现key值实现交互
                                         */
                                        //启动线程处理当前事件
                                        //_stp.QueueWorkItem(HandleEvent, new EventSiemensThreadState()
                                        //{
                                        //    InstanceName = InstanceName,
                                        //    EventIndex = i,
                                        //    SE = ei
                                        //});

                                        //启动线程处理当前事件
                                        _stp.QueueWorkItem(new WorkItemCallback(_see.HandleEvent), new EventBeckhoffThreadState()
                                        {
                                            InstanceName = InstanceName,
                                            EventIndex = i,
                                            SE = ei
                                        }, HandleEventCaLLBack);
                                    }
                                    else
                                    {//触发位依旧存在 事件ID同步写入失败 重试写入
                                        if (_globalConfig.CpuInfo.Dll == Model.DllType.Type_Hsl)
                                        {
                                            if (!_plcs[0].Write($"s={ei.PC_LabelName}", PackageDataToPlc(ei.ListOutput)).IsSuccess)
                                                Console.WriteLine("Write Single Event Retry Data Fail.");
                                            else
                                                Console.WriteLine("Write Single Event Retry Success;ID:" + ei.ListOutput[0].GetInt16());
                                        }
                                        //else
                                        //{
                                        //    if (!_plcSharp7s[1].Write(ei.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ei.ListOutput)).IsSuccess)
                                        //        Console.WriteLine("Write Single Event Retry Data Fail.");
                                        //}
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Read Single Event Data Fail. Error:" + rlt.Message);
                                    _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.EapConfig, _globalConfig.PlcConfig, string.Format("Read Single Event Data Fail,EventName:{0}", ei.EventName));
                                }

                                //else if (_globalConfig.CpuInfo.Dll == Model.DllType.Type_Sharp7)
                                //{
                                //    OperateResult<byte[]> rlt = _plcSharp7s[1].Read(ei.ListInput[0].GetMBAddressTag, GetRWLength(ei.ListInput));
                                //    if (rlt.IsSuccess)
                                //    {
                                //        //解析数据到事件结构
                                //        ResolveDataToEvent(rlt.Content, ei.ListInput);

                                //        short? nInputSequenceID = ei.ListInput.Find(t => t.TagName == "SequenceID")?.GetInt16() ?? 0;
                                //        short? nOutSequenceID = ei.ListOutput.Find(t => t.TagName == "SequenceID")?.GetInt16() ?? 0;

                                //        if (nInputSequenceID != nOutSequenceID)
                                //        {//事件ID不一致 当前事件触发
                                //            _bEventTriggerCompleted[i] = true;
                                //            //事件回调处理完毕 将在事件处理线程上执行
                                //            ei.OnEventTriggerCompleted += (ets) =>
                                //            {
                                //                Console.WriteLine("Handle Event Callback");
                                //                //if (_globalConfig.CpuInfo.Dll == Model.DllType.Type_Hsl)
                                //                //{
                                //                //    var rlt1 = _plcs[1].Write(ets.SE.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ets.SE.ListOutput));
                                //                //    if (!rlt1.IsSuccess)
                                //                //        Console.WriteLine("Write Single Event Data Fail. Error:" + rlt.Message);
                                //                //    Console.WriteLine(ets.SE.ListOutput[0].GetMBAddressTag);
                                //                //}
                                //                //else
                                //                //{
                                //                //    var rlt2 = _plcSharp7s[1].Write(ets.SE.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ets.SE.ListOutput));
                                //                //    if (!rlt2.IsSuccess)
                                //                //        Console.WriteLine("Write Single Event Data Fail. Error:" + rlt.Message);
                                //                //}

                                //                //_bEventTriggerCompleted[ets.EventIndex] = false;
                                //                //Console.WriteLine("EventCallBack " + ets.SE.EventName + " " + ets.SE.ListOutput[0].GetInt16());
                                //            };
                                //            //启动线程处理当前事件
                                //            _stp.QueueWorkItem(new WorkItemCallback(_see.HandleEvent), new EventThreadState()
                                //            {
                                //                InstanceName = InstanceName,
                                //                EventIndex = i,
                                //                SE = ei
                                //            }, HandleEventCaLLBack);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        Console.WriteLine("Read Single Event Data Fail. Error:" + rlt.Message);
                                //        _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.EapConfig, _globalConfig.PlcConfig, string.Format("Read Single Event Data Fail,EventName:{0}", ei.EventName));
                                //    }
                                //}
                            }
                        }
                        else
                        {
                            Console.WriteLine("The Event Don't Have Input Or Output.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 事件处理完毕 回调写入数据到PLC ？？会出现不回调的现象  采用事件回调处理
        /// </summary>
        /// <param name="wir"></param>
        private void HandleEventCaLLBack(IWorkItemResult wir)
        {
            EventBeckhoffThreadState ets = wir.Result as EventBeckhoffThreadState;
            Console.WriteLine("Push Data ID:" + ets.SE.ListOutput[0].GetInt16());
            //_queueHandleEventCompleted.Enqueue(ets);
            _queueHandleEventCompleted.Enqueue(ets);
        }


        /// <summary>
        //获取单个事件读取/写入长度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private ushort GetRWLength(List<BeckhoffEventIO> input)
        {
            short length = 0;

            foreach (var item in input)
            {
                length += item.Length;
            }
            return (ushort)(length * 2);
        }

        /// <summary>
        /// 将配置文件对象封装到字节数组
        /// </summary>
        /// <param name="plcConfig"></param>
        private byte[] PackageDataToPlc(List<BeckhoffEventIO> listConfig)
        {
            byte[] writeData = new byte[GetRWLength(listConfig)];
            for (int i = 0; i < listConfig.Count; i++)
            {
                Array.Copy(listConfig[i].DataValue, 0, writeData, listConfig[i].MBAdr - listConfig[0].MBAdr, listConfig[i].Length * 2);
                listConfig[i].GetDataValueStr();
            }

            return writeData;
        }

        /// <summary>
        ///将读取到的PLC数据解析到配置对象 
        /// </summary>
        /// <param name="content"></param>
        private void ResolveDataToEvent(byte[] content, List<BeckhoffEventIO> listConfig)
        {
            for (int i = 0; i < listConfig.Count; i++)
            {
                Array.Copy(content, listConfig[i].MBAdr - listConfig[0].MBAdr, listConfig[i].DataValue, 0, listConfig[i].Length * 2);
                listConfig[i].GetDataValueStr();
            }
        }

        //public void HandleEvent(EventSiemensThreadState se)
        //{
        //    if (se != null)
        //    {
        //        //处理事件
        //        //将数据打包
        //        //Dictionary<string, Tuple<string, object>> keyValuePairs = new Dictionary<string, Tuple<string, object>>();
        //        List<MyData> keyValuePairs = new List<MyData>();
        //        //遍历所有PLC下发数据
        //        for (int i = 0; i < se.SE.ListInput.Count; i++)
        //        {
        //            if (se.SE.ListInput[i].DTType == CDataType.DTInt)
        //                keyValuePairs.Add(new MyData() { Key = se.SE.ListInput[i].TagName, ValueType = MyData.MyType.Int32, ValueData = se.SE.ListInput[i].GetInt32() });
        //            //keyValuePairs.Add(se.SE.ListInput[i].TagName, new Tuple<string, object>("Int32", se.SE.ListInput[i].GetInt32()));
        //            if (se.SE.ListInput[i].DTType == CDataType.DTShort)
        //                //keyValuePairs.Add(se.SE.ListInput[i].TagName, new Tuple<string, object>("Int16", se.SE.ListInput[i].GetInt16()));
        //                keyValuePairs.Add(new MyData() { Key = se.SE.ListInput[i].TagName, ValueType = MyData.MyType.Int16, ValueData = se.SE.ListInput[i].GetInt16() });
        //            if (se.SE.ListInput[i].DTType == CDataType.DTString)
        //                //keyValuePairs.Add(se.SE.ListInput[i].TagName, new Tuple<string, object>("String", se.SE.ListInput[i].GetString()));
        //                keyValuePairs.Add(new MyData() { Key = se.SE.ListInput[i].TagName, ValueType = MyData.MyType.String, ValueData = se.SE.ListInput[i].GetString() });
        //            if (se.SE.ListInput[i].DTType == CDataType.DTFloat)
        //                //keyValuePairs.Add(se.SE.ListInput[i].TagName, new Tuple<string, object>("Float", se.SE.ListInput[i].GetSingle()));
        //                keyValuePairs.Add(new MyData() { Key = se.SE.ListInput[i].TagName, ValueType = MyData.MyType.Float, ValueData = se.SE.ListInput[i].GetSingle() });
        //        }

        //        //添加当前工序 放在【EventClass】
        //        keyValuePairs.Add(new MyData() { Key = "EventClass", ValueType = MyData.MyType.String, ValueData = se.SE.EventClass });

        //        PlcEventParamModel plcEventInputParamModel = new PlcEventParamModel()
        //        {
        //            PlcName = se.InstanceName,
        //            EventName = se.SE.EventName,
        //            EventClass = se.SE.EventClass,
        //            StartTime = DateTime.Now,
        //            Params = keyValuePairs
        //        };

        //        //调用处理接口

        //        PlcEventParamModel plcEventOutputParamModel = _see.HandleEventWithKey(plcEventInputParamModel);
        //        //将消息写入到PLC
        //        for (int i = 0; i < se.SE.ListOutput.Count; i++)
        //        {
        //            if (plcEventOutputParamModel.Params != null)
        //            {
        //                //匹配相同项
        //                for (int j = 0; j < plcEventOutputParamModel.Params.Count; j++)
        //                {
        //                    //if (plcEventOutputParamModel.Params.Keys.Contains(se.SE.ListOutput[i].TagName))
        //                    if (plcEventOutputParamModel.Params.Any(it => it.Key == se.SE.ListOutput[i].TagName))
        //                    {
        //                        //将返回值写入plc
        //                        // var p = plcEventOutputParamModel.Params.Get(se.SE.ListOutput[i].TagName);
        //                        var p = plcEventOutputParamModel.Params.Where(it => it.Key == se.SE.ListOutput[i].TagName).SingleOrDefault();
        //                        if (p != null)
        //                        {
        //                            if (p.ValueType == MyData.MyType.Int32)
        //                            {
        //                                se.SE.ListOutput[i].SetInt32(Convert.ToInt32(p.ValueData));
        //                            }
        //                            if (p.ValueType == MyData.MyType.Int16)
        //                            {
        //                                se.SE.ListOutput[i].SetInt16(Convert.ToInt16(p.ValueData));
        //                            }
        //                            if (p.ValueType == MyData.MyType.String)
        //                            {
        //                                se.SE.ListOutput[i].SetString(p.ValueData.ToString());
        //                            }
        //                            if (p.ValueType == MyData.MyType.WString)
        //                            {
        //                                se.SE.ListOutput[i].SetWString(p.ValueData.ToString());
        //                            }
        //                            if (p.ValueType == MyData.MyType.Float)
        //                            {
        //                                se.SE.ListOutput[i].SetFloat(Convert.ToSingle(p.ValueData));

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    _queueHandleEventCompleted.Enqueue(se);
        //}
    }
}
