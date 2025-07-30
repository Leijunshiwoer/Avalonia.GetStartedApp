using SmartCommunicationForExcel.EventHandle.Mitsubishi;
using SmartCommunicationForExcel.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Implementation.Mitsubishi
{
    public class MitsubishiEventInstance : IEventInstance<MitsubishiEventIO>
    {
        public delegate void HasEventCompleted(EventMitsubishiThreadState ets);
        public event HasEventCompleted OnEventTriggerCompleted;

        public void InvokeEventCompleted(EventMitsubishiThreadState ets)
        {
            OnEventTriggerCompleted?.Invoke(ets);
            OnEventTriggerCompleted = null;
        }
        public MitsubishiEventInstance()
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

        [Description("事件输入参数")]
        public List<MitsubishiEventIO> ListInput
        {
            get;
            set;
        } = new List<MitsubishiEventIO>();

        [Description("事件输出参数")]
        public List<MitsubishiEventIO> ListOutput
        {
            get;
            set;
        } = new List<MitsubishiEventIO>();
    }
}
