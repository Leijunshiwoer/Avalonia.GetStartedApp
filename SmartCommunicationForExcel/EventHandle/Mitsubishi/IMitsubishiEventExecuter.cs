using SmartCommunicationForExcel.Implementation.Mitsubishi;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.EventHandle.Mitsubishi
{
    public interface IMitsubishiEventExecuter
    {
        //[Obsolete("8.0之前版本被遗弃", false)]
        object HandleEvent(object sei);//object EventIstance

        PlcEventParamModel HandleEventWithKey(object PlcEventParamModel);

        void SubscribeCommonInfo(string strInstanceName, bool bSuccess, List<MitsubishiEventIO> listInput, List<MitsubishiEventIO> listOutput, string strError = "");

        void Err(string strInstanceName, byte[] data, string strError = "");
    }
}
