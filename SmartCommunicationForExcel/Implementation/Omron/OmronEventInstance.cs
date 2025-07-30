using SmartCommunicationForExcel.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Implementation.Omron
{
    public class OmronEventInstance : IEventInstance<OmronEventIO>
    {
        [Description("屏蔽事件")]
        public bool DisableEvent
        {
            get;
            set;
        } = false;

        [Description("事件类别")]
        public string EventClass
        {
            get;
            set;
        } = "Class1";

        [Description("事件名称")]
        public string EventName
        {
            get;
            set;
        } = "EventName";

        [Description("事件输入参数")]
        public List<OmronEventIO> ListInput
        {
            get;
            set;
        } = new List<OmronEventIO>();

        [Description("事件输出参数")]
        public List<OmronEventIO> ListOutput
        {
            get;
            set;
        } = new List<OmronEventIO>();
    }
}
