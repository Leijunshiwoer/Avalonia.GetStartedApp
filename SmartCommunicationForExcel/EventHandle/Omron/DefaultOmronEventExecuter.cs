using SmartCommunicationForExcel.Implementation.Omron;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.EventHandle.Omron
{
    public class DefaultOmronEventExecuter : IOmronEventExecuter
    {
        /*------------------------------事件处理----------------------------------------------------*/
        public object HandleEvent(object state)
        {
            EventOmronThreadState sei = state as EventOmronThreadState;

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

        public void SubscribeCommonInfo(string strInstanceName, bool bSuccess, List<OmronEventIO> listInput, List<OmronEventIO> listOutput, string strError)
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
    }
}
