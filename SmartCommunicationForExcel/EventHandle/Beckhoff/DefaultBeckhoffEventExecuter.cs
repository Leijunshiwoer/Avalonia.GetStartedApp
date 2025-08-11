using SmartCommunicationForExcel.EventHandle.Siemens;
using SmartCommunicationForExcel.Implementation.Beckhoff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.EventHandle.Beckhoff
{
    internal class DefaultBeckhoffEventExecuter : IBeckhoffEventExecuter
    {
        public void Err(string strInstanceName, byte[] data, string strError = "")
        {
            
        }

        public object HandleEvent(object state)
        {
            var sei = state as EventBeckhoffThreadState;

            Console.WriteLine("Event " + sei.SE.EventName + " Trigger Handle.");
            sei.SE.ListOutput[0].SetInt16(sei.SE.ListInput[1].GetInt16());

            return sei;
        }

        public void SubscribeCommonInfo(string strInstanceName, bool bSuccess, List<BeckhoffEventIO> listInput, List<BeckhoffEventIO> listOutput, string strError = "")
        {
            
        }
    }
}
