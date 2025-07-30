using SmartCommunicationForExcel.EventHandle.Beckhoff;
using SmartCommunicationForExcel.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Implementation.Beckhoff
{
    public class BeckhoffEventInstance : IEventInstance<BeckhoffEventIO>
    {
        public delegate void HasEventCompleted(EventBeckhoffThreadState ets);
        public event HasEventCompleted OnEventTriggerCompleted;

        public void InvokeEventCompleted(EventBeckhoffThreadState ets)
        {
            OnEventTriggerCompleted?.Invoke(ets);
            OnEventTriggerCompleted = null;
        }
        public BeckhoffEventInstance()
        {

        }

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

        [Description("PC标签")]
        public string PC_LabelName
        {
            get;
            set;
        } = "PCLabelName";
        [Description("PLC标签")]
        public string PLC_LabelName
        {
            get;
            set;
        } = "PLCLabelName";

        [Description("事件输入参数")]
        public List<BeckhoffEventIO> ListInput
        {
            get;
            set;
        } = new List<BeckhoffEventIO>();

        [Description("事件输出参数")]
        public List<BeckhoffEventIO> ListOutput
        {
            get;
            set;
        } = new List<BeckhoffEventIO>();
    }
}
