using SmartCommunicationForExcel.Implementation.Omron;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.EventHandle.Omron
{
    public interface IOmronEventExecuter
    {
        object HandleEvent(object sei);//object EventIstance

        void SubscribeCommonInfo(string instanceName, bool bSuccess, List<OmronEventIO> listInput, List<OmronEventIO> listOutput, string strError = "");
        void Err(string strInstanceName, byte[] data, string strError = "");
    }
}
