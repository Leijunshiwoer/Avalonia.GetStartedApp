using System;
using System.Collections.Generic;
using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Model;

namespace SmartCommunicationForExcel.EventHandle.Siemens
{
    class DefaultSiemensEventExecuter : ISiemensEventExecuter
    {

        /*------------------------------事件处理----------------------------------------------------*/
       
        public EventSiemensThreadState HandleEvent(EventSiemensThreadState se)
        {
            Console.WriteLine("Event " + se.SE.EventName + " Trigger Handle.");
            se.SE.ListOutput[0].SetInt16(se.SE.ListInput[1].GetInt16());
            return se;
        }
        /*------------------------------公共区订阅----------------------------------------------------*/
        /*-----------------------------可选择性创建 用于事件处理参数识别-----------------------------*/
        public enum InputEnum
        {
            SequenceID,
            MachineID,
            MachineState,
            MachineCycleTime,
            CountOK,
            CountNG,
            CountBuffer,
            LifeControl_1,
            LifeControl_2,
            LifeControlBuffer,
            AlarmCode
        }

        public enum OutputEnum
        {

        }

        public void SubscribeCommonInfo(string strInstanceName, bool bSuccess, List<SiemensEventIO> listInput, List<SiemensEventIO> listOutput, string strError)
        {
            if (bSuccess)
            {

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("PLC RW ERROR.");
                Console.ResetColor();
            }
        }

        public void Err(string strInstanceName, byte[] data, string strError = "")
        {
            
        }

        public PlcEventParamModel HandleEventWithKey(object PlcEventParamModel)
        {
            throw new NotImplementedException();
        }

   
    }
}
