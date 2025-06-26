using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Log_SiemensEvent")]
    public class Log_PlcEvent : AutoIncrementEntity
    {
        [SugarColumn(IsNullable = true)]
        public int PlcEventLogId { get; set; }
        [SugarColumn(IsNullable = true)]
        public string PlcName { get; set; }
        [SugarColumn(IsNullable = true)]
        public string Ip { get; set; }
        [SugarColumn(IsNullable = true)]
        public string StationName { get; set; }
        [SugarColumn(IsNullable = true)]
        public string EventName { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime? StartTime { get; set; }
        [SugarColumn(IsNullable = true)]
        public double SpanTime { get; set; }
        [SugarColumn(IsNullable = true)]
        public string ResultCode { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "text")]
        public string Content { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "text")]
        public string Message { get; set; }
    }
}
