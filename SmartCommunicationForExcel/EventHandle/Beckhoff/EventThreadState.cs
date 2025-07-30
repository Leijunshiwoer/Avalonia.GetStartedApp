using SmartCommunicationForExcel.Implementation.Beckhoff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.EventHandle.Beckhoff
{
    public class EventBeckhoffThreadState
    {
        /// <summary>
        /// 容器实例名称
        /// </summary>
        public string InstanceName { get; set; }
        /// <summary>
        /// 事件索引
        /// </summary>
        public int EventIndex { get; set; }
        /// <summary>
        /// 事件实例
        /// </summary>
        public BeckhoffEventInstance SE { get; set; }
    }
}
