using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Beckhoff_Event_log")]
    public class Beckhoff_Event_log : AutoIncrementEntity
    {
        public int Idx { get; set; }
        public string Name { get; set; }
        public string ReadLabel { get; set; }
        public ushort ReadLen { get; set; }
        public string ReadClassName { get; set; }
        public string WirteLabel { get; set; }
        public ushort WirteLen { get; set; }
        public string WirteClassName { get; set; }
        public bool TriggerCompleted { get; set; }
        public int SequenceIDR { get; set; }
        public int SequenceIDW { get; set; }
        [SugarColumn(ColumnDataType = "text")]
        public string ObjR { get; set; }
        [SugarColumn(ColumnDataType = "text")]
        public string ObjW { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double SpanTime { get; set; }
    }
}
