using Amib.Threading;
using HslCommunication;
using HslCommunication.Profinet.Omron;
using SmartCommunicationForExcel.Implementation.Omron;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace SmartCommunicationForExcel.EventHandle.Omron
{
    public class OmronEventHandle
    {
        public string InstanceName { get; set; }
        private string EventTriggerTagName { get; set; }

        private SmartThreadPool _stp;
        private IOmronEventExecuter _see;

        private Thread _workThread;
        private ManualResetEvent _terminatedEvent;

        private OmronFinsNet[] _plcs;
        private OmronGlobalConfig _globalConfig;

        private ConcurrentQueue<EventOmronThreadState> _queueHandleEventCompleted;

        private bool[] _bEventTriggerCompleted = new bool[100];

        public OmronEventHandle(SmartThreadPool stp, IUnityContainer container)
        {
            _stp = stp;

            if (container.IsRegistered<IOmronEventExecuter>("Omron"))
                _see = container.Resolve<IOmronEventExecuter>("Omron");
            else
                _see = container.Resolve<IOmronEventExecuter>();

            _queueHandleEventCompleted = new ConcurrentQueue<EventOmronThreadState>();

            _plcs = new OmronFinsNet[2];
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
        public bool StartWork(string strName, OmronGlobalConfig globalConfig, string strEventTriggerTagName = "EventTrigger")
        {
            InstanceName = strName;
            _globalConfig = globalConfig;
            EventTriggerTagName = strEventTriggerTagName;

            Authorization.SetAuthorizationCode("4672fd9a-4743-4a08-ad2f-5cd3374e496d");

            _plcs[0] = new OmronFinsNet();
            _plcs[0].IpAddress = globalConfig.CpuInfo.IP;
            _plcs[0].Port = globalConfig.CpuInfo.Port;
            _plcs[0].SA1 = globalConfig.CpuInfo.SA1;
            _plcs[0].DA1 = globalConfig.CpuInfo.DA1;
            _plcs[0].DA2 = globalConfig.CpuInfo.DA2;

            //_plcs[0].SetPersistentConnection();

            //_plcs[1] = new OmronFinsNet();
            //_plcs[1].IpAddress = globalConfig.CpuInfo.IP;
            //_plcs[1].Port = globalConfig.CpuInfo.Unit;
            //_plcs[1].SetPersistentConnection();

            if (!_plcs[0].ConnectServer().IsSuccess)
                return false;
            //if (!_plcs[1].ConnectServer().IsSuccess)
            //    return false;

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

                while (!_terminatedEvent.WaitOne(1))
                {
                    if (_globalConfig.EapConfig.Count == 0 || _globalConfig.PlcConfig.Count == 0)
                        continue;

                    sw.Restart();


                    //处理回写事件
                    if (!_queueHandleEventCompleted.IsEmpty)
                    {
                        EventOmronThreadState ets = null;
                        if (_queueHandleEventCompleted.TryDequeue(out ets))
                        {
                            /*
                             * 因为每次手动赋值SequenceID不方便，不需要手动在事件回调里面赋值，赋值工作由事件回调后自动完成
                             * 
                             * 把 PLC的 SequenceID 赋值给 EAP的SequenceID
                             */
                            try
                            {
                                OmronEventIO seiEap = ets.SE.ListOutput.Where(it => it.TagName == "SequenceID").SingleOrDefault();
                                OmronEventIO seiPlc = ets.SE.ListInput.Where(it => it.TagName == "SequenceID").SingleOrDefault();

                                seiEap.SetInt16(seiPlc.GetInt16());
                            }
                            catch (Exception ex)
                            {
                                //异常暂时不做处理
                                Console.WriteLine($"SequenceID:{ex.Message}");
                            }


                            if (!_plcs[0].Write(ets.SE.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ets.SE.ListOutput)).IsSuccess)
                            {
                                Console.WriteLine("Write Single Event Data Fail.");
                                _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, string.Format("WriteSingleEventData Fail,EventName:{0}", ets.SE.EventName));
                            }
                            else
                                Console.WriteLine("Write Single Event Success;ID:" + ets.SE.ListOutput[0].GetInt16());

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
                    //    continue;
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
                        HandleEventTrigger(_globalConfig.PlcConfig.Find(t => t.TagName == EventTriggerTagName));

                        if (!_plcs[0].Write(_globalConfig.EapConfig[0].GetMBAddressTag, PackageDataToPlc(_globalConfig.EapConfig)).IsSuccess)
                        {
                            Console.WriteLine("WriteCommonData Fail.");
                            _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, "WriteCommonData Fail");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"ReadCommonData Fail. {rlt.Message}");
                        _see.SubscribeCommonInfo(InstanceName, false, _globalConfig.PlcConfig, _globalConfig.EapConfig, $"ReadCommonData Fail {rlt.Message}");
                    }
                }

                _plcs[0].ConnectClose();
                //plc0[1].ConnectClose();
            }catch(Exception ex)
            {
                _see.Err(_globalConfig.FileName, Encoding.Default.GetBytes("Error"), ex.Message);
            }
        }

        /// <summary>
        /// 处理事件触发位
        /// </summary>
        /// <param name="omronEventIO"></param>
        private void HandleEventTrigger(OmronEventIO omronEventIO)
        {
            if (null == omronEventIO)
            {
                Console.WriteLine("Get SequenceID Event Fail,It's Not Find SequenceID EventInstance.");
                return;
            }

            bool[] triggers = omronEventIO.GetBitArr();

            for (int i = 0; i < triggers.Length; i++)
            {
                if (triggers[i])
                {//有事件触发
                    if (i < _globalConfig.EventConfig.Count)
                    {//事件触发位配置有事件
                        OmronEventInstance ei = _globalConfig.EventConfig[i];
                        if (!_bEventTriggerCompleted[i])
                        {//已经有线程处理当前事件 当前事件正在处理中
                            if (ei.ListInput.Count > 0 && ei.ListOutput.Count > 0)
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

                                    short? nInputSequenceID = ei.ListInput.Find(t => t.TagName == "SequenceID")?.GetInt16() ?? 0;
                                    short? nOutSequenceID = ei.ListOutput.Find(t => t.TagName == "SequenceID")?.GetInt16()?? 0;

                                    if (nInputSequenceID != nOutSequenceID)
                                    {//事件ID不一致 当前事件触发
                                        _bEventTriggerCompleted[i] = true;
                                        //启动线程处理当前事件
                                        _stp.QueueWorkItem(new WorkItemCallback(_see.HandleEvent), new EventOmronThreadState()
                                        {
                                            InstanceName = InstanceName,
                                            EventIndex = i,
                                            SE = ei
                                        }, HandleEventCaLLBack);
                                    }
                                    else
                                    {//触发位依旧存在 事件ID同步写入失败 重试写入
                                        
                                        if (!_plcs[0].Write(ei.ListOutput[0].GetMBAddressTag, PackageDataToPlc(ei.ListOutput)).IsSuccess)
                                            Console.WriteLine("Write Single Event Retry Data Fail.");
                                        else
                                            Console.WriteLine("Write Single Event Retry Success;ID:" + ei.ListOutput[0].GetInt16());
                                        
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
                            }
                            else
                            {
                                Console.WriteLine("The Event Don't Have Input Or Output.");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 事件处理完毕 回调写入数据到PLC
        /// </summary>
        /// <param name="wir"></param>
        private void HandleEventCaLLBack(IWorkItemResult wir)
        {
            EventOmronThreadState ets = wir.Result as EventOmronThreadState;
            Console.WriteLine("Push Data ID:" + ets.SE.ListOutput[0].GetInt16());
            _queueHandleEventCompleted.Enqueue(ets);
        }

        /// <summary>
        //获取单个事件读取/写入长度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private ushort GetRWLength(List<OmronEventIO> input)
        {
            short length = 0;

            foreach (var item in input)
            {
                length += item.Length;
            }
            return (ushort)(length);
        }

        /// <summary>
        /// 将配置文件对象封装到字节数组
        /// </summary>
        /// <param name="plcConfig"></param>
        private byte[] PackageDataToPlc(List<OmronEventIO> listConfig)
        {
            byte[] writeData = new byte[GetRWLength(listConfig) * 2];
            for (int i = 0; i < listConfig.Count; i++)
            {
                Array.Copy(listConfig[i].DataValue, 0, writeData, (listConfig[i].MBAdr - listConfig[0].MBAdr) * 2, listConfig[i].Length * 2);
                listConfig[i].GetDataValueStr();
            }

            return writeData;
        }

        /// <summary>
        ///将读取到的PLC数据解析到配置对象 
        /// </summary>
        /// <param name="content"></param>
        private void ResolveDataToEvent(byte[] content, List<OmronEventIO> listConfig)
        {
            for (int i = 0; i < listConfig.Count; i++)
            {
                Array.Copy(content, (listConfig[i].MBAdr - listConfig[0].MBAdr) * 2, listConfig[i].DataValue, 0, listConfig[i].Length * 2);
                listConfig[i].GetDataValueStr();
            }
        }
    }
}
