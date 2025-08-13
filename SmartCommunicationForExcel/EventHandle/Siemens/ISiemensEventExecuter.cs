using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.EventHandle.Siemens
{
    public interface ISiemensEventExecuter
    {
        //[Obsolete("8.0之前版本被遗弃", false)]
        // object HandleEvent(object sei);//object EventIstance
        EventSiemensThreadState HandleEvent(EventSiemensThreadState se);
        //object EventIstance
       // PlcEventParamModel HandleEventWithKey(object PlcEventParamModel);

        void SubscribeCommonInfo(string strInstanceName, bool bSuccess, List<SiemensEventIO> listInput, List<SiemensEventIO> listOutput, string strError = "");

        void Err(string strInstanceName, byte[] data, string strError = "");
    }
}
