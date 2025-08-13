using SmartCommunicationForExcel.Implementation.Beckhoff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.EventHandle.Beckhoff
{
    public interface IBeckhoffEventExecuter
    {
        //[Obsolete("8.0之前版本被遗弃", false)]
        EventBeckhoffThreadState HandleEvent(EventBeckhoffThreadState se);//object EventIstance

        // PlcEventParamModel HandleEventWithKey(object PlcEventParamModel);

        void SubscribeCommonInfo(string strInstanceName, bool bSuccess, List<BeckhoffEventIO> listInput, List<BeckhoffEventIO> listOutput, string strError = "");

        void Err(string strInstanceName, byte[] data, string strError = "");
    }
}
