using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Model
{
    public class PlcEventParamModel
    {
        public string RandomCode { get; set; } = RandomOperate.GenerateCheckCodeNum(24);
        public int PlcEventLogId { get; set; }
        public string PlcName { get; set; }//PLC 名称
        public string PlcAddr { get; set; }//PLC IP地址
        public string EventName { get; set; }//事件名称
        public string EventClass { get; set; }//事件分类
        public DateTime StartTime { get; set; } = DateTime.Now;//开始时间

        //参数
        public List<MyData> Params { get; set; }//参数
    }
}
