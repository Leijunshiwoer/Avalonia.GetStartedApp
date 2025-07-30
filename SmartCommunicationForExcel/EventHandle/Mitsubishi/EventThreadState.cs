using SmartCommunicationForExcel.Implementation.Mitsubishi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.EventHandle.Mitsubishi
{
    public class EventMitsubishiThreadState
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
        public MitsubishiEventInstance SE { get; set; }
    }
}