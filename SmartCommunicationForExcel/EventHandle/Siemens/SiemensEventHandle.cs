using Amib.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using HslCommunication.Profinet.Siemens;
using SmartCommunicationForExcel.Implementation.Siemens;
using Unity;
using System.Diagnostics;
using HslCommunication.Profinet.Omron;
using HslCommunication;
using System.Linq;
using System.Text;
using SmartCommunicationForExcel.Model;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.EventHandle.Siemens
{
    public class SiemensEventHandle
    {
        public string InstanceName { get; set; }
        private string EventTriggerTagName { get; set; }

        private SmartThreadPool _stp;
        private ISiemensEventExecuter _see;

        private Thread _workThread;
        private ManualResetEvent _terminatedEvent;

        private SiemensS7Net[] _plcs;
        //private SmartSharp7[] _plcSharp7s;

       // private OmronFinsNet[] _omronPlcs;

        private SiemensGlobalConfig _globalConfig;

        private bool[] _bEventTriggerCompleted = new bool[100];


        private ConcurrentQueue<EventSiemensThreadState> _queueHandleEventCompleted;

        public SiemensEventHandle(SmartThreadPool stp, IUnityContainer container)
        {
            _stp = stp;

            if (container.IsRegistered<ISiemensEventExecuter>("Siemens"))
                _see = container.Resolve<ISiemensEventExecuter>("Siemens");
            else
                _see = container.Resolve<ISiemensEventExecuter>();

            _queueHandleEventCompleted = new ConcurrentQueue<EventSiemensThreadState>();

            _plcs = new SiemensS7Net[1];
            // _plcSharp7s = new SmartSharp7[2];

           // _omronPlcs = new OmronFinsNet[2];

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
        public bool StartWork(string strName, SiemensGlobalConfig globalConfig, string strEventTriggerTagName = "eventtrigger")
        {
            InstanceName = strName;
            _globalConfig = globalConfig;
            EventTriggerTagName = strEventTriggerTagName;

            if (globalConfig.CpuInfo.CpuType == Model.CpuType.Siemens)
            {
                if (globalConfig.CpuInfo.Dll == Model.DllType.Type_Hsl)
                {
                    Authorization.SetAuthorizationCode("4672fd9a-4743-4a08-ad2f-5cd3374e496d");


                    _plcs[0] = new SiemensS7Net(globalConfig.CpuInfo.PlcType);
                    _plcs[0].IpAddress = globalConfig.CpuInfo.IP;
                    _plcs[0].Rack = globalConfig.CpuInfo.Rack;
                    _plcs[0].Slot = globalConfig.CpuInfo.Slot;
                    _plcs[0].SetPersistentConnection();

                    //_plcs[1] = new SiemensS7Net(globalConfig.CpuInfo.PlcType);
                    //_plcs[1].IpAddress = globalConfig.CpuInfo.IP;
                    //_plcs[1].Rack = globalConfig.CpuInfo.Rack;
                    //_plcs[1].Slot = globalConfig.CpuInfo.Slot;
                    //_plcs[1].SetPersistentConnection();

                    if (!_plcs[0].ConnectServer().IsSuccess)
                        return false;
                }
            }

            _workThread.Start();

            return true;
        }

        public void WorkStop()
        {
            _terminatedEvent.Set();
        }

        /*
         * 将 EAPConfig 定义为PC对应于DB区  PLCConfig对应于PLC的DB区
         * 
         * 与以前的正好相反，逻辑容易理解
         * 
         * 韩顺发 跟新于 2021.2.20
         * 
         */

       // private object _lock;

        //private void AddQueue(EventSiemensThreadState ets)
        //{
        //    lock(_lock)
        //    {
        //        _queueHandleEventCompleted.Enqueue(ets);
        //    }
        //}

        //private EventSiemensThreadState GetQueue()
        //{
        //    lock (_lock)
        //    {
        //        if (_queueHandleEventCompleted.Count > 0)
        //            return _queueHandleEventCompleted.Dequeue();
        //        else
        //            return null;
        //    }
        //}

        private void WorkThread()
        {
            try
            {
                Stopwatch sw = new Stopwatch();

                var _cycleTime = _globalConfig.CpuInfo.CycleTime;
                if (_cycleTime < 10 || _cycleTime > 2000)
                    _cycleTime = 10;

                while (!_terminatedEvent.WaitOne(_cycleTime))
                {
                    if (_globalConfig.EapConfig.Count == 0 || _globalConfig.PlcConfig.Count == 0)
                        continue;

                    sw.Restart();

                    /*
                     * 
                     * 主线程中不回写事件信息  
                     * 2023.2.17 韩顺发修改
                     * 
                     */


                    //处理回写事件

                    if (!_queueHandleEventCompleted.IsEmpty)
                    {
                        EventSiemensThreadState ets = null;
                        if (_queueHandleEventCompleted.TryDequeue(out ets))
                        {
                            /*
                             * 因为每次手动赋值SequenceID不方便，不需要手动在事件回调里面赋值，赋值工作由事件回调后自动完成
                             * 
                             * 把 PLC的 SequenceID 赋值给 EAP的SequenceID
                             */
                            try
                            {
                                SiemensEventIO seiEap = ets.SE.ListOutput.Where(it => it.TagName.Trim().ToLower() == "sequenceid").SingleOrDefault();
                                SiemensEventIO seiPlc = ets.SE.ListInput.Where(it => it.TagName.Trim().ToLower() == "sequenceid").SingleOrDefault();

                                seiEap.SetInt16(seiPlc.GetInt16());
                            }
                            catch
                            {
                                //异常暂时不做处理
                                _see.Err(InstanceName, null, string.Format("没有找到sequenceid,EventName:{0}", ets.SE.EventName));
                            }

                            if (!_plcs[0].Write(ets.SE.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ets.SE.ListOutput)).IsSuccess)
                            {
                                Console.WriteLine("Write Single Event Data Fail.");
                                _see.Err(InstanceName, null, string.Format("Write Single Event Data Fail,EventName:{0}", ets.SE.EventName));
                                //_see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, string.Format("WriteSingleEventData Fail,EventName:{0}", ets.SE.EventName));
                            }
                            else
                                Console.WriteLine("Write Single Event Success;ID:" + ets.SE.ListOutput[0].GetInt16());

                            _bEventTriggerCompleted[ets.EventIndex] = false;
                        }
                        else
                        {
                            _see.Err(ets.InstanceName, null, "安全队列获取失败");
                        }
                    }


                    HslCommunication.OperateResult<byte[]> rlt = _plcs[0].Read(_globalConfig.PlcConfig[0].GetMBAddressTag, GetRWLength(_globalConfig.PlcConfig));
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
                        HandleEventTrigger(_globalConfig.PlcConfig.Find(t => t.TagName.Trim().ToLower() == EventTriggerTagName));

                        if (!_plcs[0].Write(_globalConfig.EapConfig[0].GetMBAddressTag, PackageDataToPlc(_globalConfig.EapConfig)).IsSuccess)
                        {
                            Console.WriteLine("WriteCommonData Fail.");
                            _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, "WriteCommonData Fail");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ReadCommonData Fail.");
                        _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, "ReadCommonData Fail");
                    }

                  
                }

                //Close Connect
                //switch (_globalConfig.CpuInfo.Dll)
                //{
                //    case Model.DllType.Type_Hsl:
                _plcs[0].ConnectClose();
               // _plcs[1].ConnectClose();
                //      break;
                //case Model.DllType.Type_Sharp7:
                //    _plcSharp7s[0].ConnectClose();
                //    _plcSharp7s[1].ConnectClose();
                //    break;
                //     default:
                //        break;
                // }
            }catch(Exception ex)
            {
                _see.Err(_globalConfig.FileName, Encoding.Default.GetBytes("Error"), ex.Message);
            }
}

        /// <summary>
        /// 处理事件触发位
        /// </summary>
        /// <param name="siemensEventIO"></param>
        private void HandleEventTrigger(SiemensEventIO siemensEventIO)
        {
            if (null == siemensEventIO)
            {
                //Console.WriteLine("Get SequenceID Event Fail,It's Not Find SequenceID EventInstance.");
                //_see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, "Get SequenceID Event Fail, It's Not Find SequenceID EventInstance.");
                _see.Err(InstanceName, null, "请检查公共区EventTrigger是否定义错误");
                return;
            }

            bool[] triggers = siemensEventIO.TransBool(siemensEventIO.DataValue, 0, siemensEventIO.DataValue.Length);

            for (int i = 0; i < triggers.Length; i++)
            {
                if (triggers[i])
                {//有事件触发
                    if (i < _globalConfig.EventConfig.Count)
                    {//事件触发位配置有事件
                        SiemensEventInstance ei = _globalConfig.EventConfig[i];

                        if (ei.DisableEvent)
                            continue;

                        if (ei.ListInput.Count > 0 && ei.ListOutput.Count > 0)
                        {//已经有线程处理当前事件 当前事件正在处理中
                            if (!_bEventTriggerCompleted[i])
                            {
                               
                                HslCommunication.OperateResult<byte[]> rlt = _plcs[0].Read(ei.ListInput[0].GetMBAddressTag, GetRWLength(ei.ListInput));
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

                                    short? nInputSequenceID = ei.ListInput.Find(t => t.TagName.Trim().ToLower() == "sequenceid")?.GetInt16() ?? 0;
                                    short? nOutSequenceID = ei.ListOutput.Find(t => t.TagName.Trim().ToLower() == "sequenceid")?.GetInt16() ?? 0;

                                    if (nInputSequenceID != nOutSequenceID)
                                    {//事件ID不一致 当前事件触发
                                        _bEventTriggerCompleted[i] = true;

                                        //事件回调处理完毕 将在事件处理线程上执行
                                        //ei.OnEventTriggerCompleted += (ets) =>
                                        //{
                                        //    Console.WriteLine("Handle Event Callback");
                                        //};

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
                                        _stp.QueueWorkItem(new WorkItemCallback(_see.HandleEvent), new EventSiemensThreadState()
                                        {
                                            InstanceName = InstanceName,
                                            EventIndex = i,
                                            SE = ei
                                        }, HandleEventCaLLBack);

                                        // //回调事件处理
                                        // TaskEventCallback(new EventSiemensThreadState()
                                        // {
                                        //    InstanceName = InstanceName,
                                        //     EventIndex = i,
                                        //     SE = ei
                                        // });
                                    }
                                    else
                                    {//触发位依旧存在 事件ID同步写入失败 重试写入
                                       
                                        if (!_plcs[0].Write(ei.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ei.ListOutput)).IsSuccess)
                                            Console.WriteLine("Write Single Event Retry Data Fail.");
                                        else
                                            Console.WriteLine("Write Single Event Retry Success;ID:" + ei.ListOutput[0].GetInt16());
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Read Single Event Data Fail. Error:" + rlt.Message);
                                    _see.Err(InstanceName, null, string.Format("Read Single Event Data Fail,EventName:{0}", ei.EventName));
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

        // private void TaskEventCallback(EventSiemensThreadState e)
        // {
        //     Task.Run(async () =>
        //     {
        //         await _see.HandleEvent(e);
        //         _queueHandleEventCompleted.Enqueue(e);
        //     });
        // }
        /// <summary>
        /// 事件处理完毕 回调写入数据到PLC ？？会出现不回调的现象  采用事件回调处理
        /// </summary>
        /// <param name="wir"></param>
        private void HandleEventCaLLBack(IWorkItemResult wir)
        {
            EventSiemensThreadState ets = wir.Result as EventSiemensThreadState;
            Console.WriteLine("Push Data ID:" + ets.SE.ListOutput[0].GetInt16());

            //不放入队列中了
            _queueHandleEventCompleted.Enqueue(ets);  

           

            /*
             * 
             * 2023.02.17 由原来的主线程统一将事件写入PLC，改成由当前事件处理线程直接写入PLC
             * 修改人：韩顺发
             * 
             */
            /*
                             * 因为每次手动赋值SequenceID不方便，不需要手动在事件回调里面赋值，赋值工作由事件回调后自动完成
                             * 
                             * 把 PLC的 SequenceID 赋值给 EAP的SequenceID
                             */


            /*
            try
            {
                SiemensEventIO seiEap = ets.SE.ListOutput.Where(it => it.TagName.Trim().ToLower() == "sequenceid").SingleOrDefault();
                SiemensEventIO seiPlc = ets.SE.ListInput.Where(it => it.TagName.Trim().ToLower() == "sequenceid").SingleOrDefault();

                seiEap.SetInt16(seiPlc.GetInt16());
            }
            catch
            {
                //异常暂时不做处理
            }

            if (!_plcs[1].Write(ets.SE.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ets.SE.ListOutput)).IsSuccess)
            {
                Console.WriteLine("Write Single Event Data Fail.");
                _see.Err(InstanceName, null, string.Format("Write Single Event Data Fail,EventName:{0}", ets.SE.EventName));
                //_see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, string.Format("WriteSingleEventData Fail,EventName:{0}", ets.SE.EventName));
            }
            else
                Console.WriteLine("Write Single Event Success;ID:" + ets.SE.ListOutput[0].GetInt16());

            _bEventTriggerCompleted[ets.EventIndex] = false;


            //if (_globalConfig.CpuInfo.Dll == Model.DllType.Type_Hsl)
            //{
            //    if (!_plcs[1].Write(ets.SE.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ets.SE.ListOutput)).IsSuccess)
            //        Console.WriteLine("Write Single Event Data Fail.");
            //}
            //else
            //{
            //    if (!_plcSharp7s[1].Write(ets.SE.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ets.SE.ListOutput)).IsSuccess)
            //        Console.WriteLine("Write Single Event Data Fail.");
            //}
            //_bEventTriggerCompleted[ets.EventIndex] = false;
            */
            
        }


        /// <summary>
        //获取单个事件读取/写入长度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        /// 将配置文件对象封装到字节数组
        /// </summary>
        /// <param name="plcConfig"></param>
        private byte[] PackageDataToPlc(List<SiemensEventIO> listConfig)
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
        private void ResolveDataToEvent(byte[] content, List<SiemensEventIO> listConfig)
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
