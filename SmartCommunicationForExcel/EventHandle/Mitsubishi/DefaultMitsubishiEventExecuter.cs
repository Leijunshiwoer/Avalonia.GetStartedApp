using System;
using System.Collections.Generic;
using SmartCommunicationForExcel.Implementation.Mitsubishi;
using SmartCommunicationForExcel.Model;

namespace SmartCommunicationForExcel.EventHandle.Mitsubishi
{
    class DefaultMitsubishiEventExecuter : IMitsubishiEventExecuter
    {

        /*------------------------------事件处理----------------------------------------------------*/
        public object HandleEvent(object state)
        {
            EventMitsubishiThreadState sei = state as EventMitsubishiThreadState;

            Console.WriteLine("Event " + sei.SE.EventName + " Trigger Handle.");
            sei.SE.ListOutput[0].SetInt16(sei.SE.ListInput[1].GetInt16());

            return sei;
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

        public void SubscribeCommonInfo(string strInstanceName, bool bSuccess, List<MitsubishiEventIO> listInput, List<MitsubishiEventIO> listOutput, string strError)
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
