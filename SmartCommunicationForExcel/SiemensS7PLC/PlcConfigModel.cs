using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.SiemensS7PLC
{
    public class PlcConfigModel
    {
        public string PlcName { get; set; }

        public Plc  Plc { get; set; }


        public PlcKeepData PlcKeepData { get; set; } // PLC保存的数据

        public string Message { get; set; } // 要绑定的消息（双向绑定核心属性）

        public bool IsConnected { get; set; }
    }
}
