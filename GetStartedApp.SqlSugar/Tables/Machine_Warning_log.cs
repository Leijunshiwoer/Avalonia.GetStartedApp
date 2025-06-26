using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Machine_Warning_log")]
    public class Machine_Warning_log : AutoIncrementEntity
    {
        [SugarColumn(ColumnDescription = "机台编号", IsNullable = true)]
        public string OP { get; set; }
        [SugarColumn(ColumnDescription = "订单ID")]
        public int TaskId { get; set; }
        [SugarColumn(ColumnDescription = "错误列表中排序", IsNullable = true)]
        public int No { get; set; }
        [SugarColumn(ColumnDescription = "错误代码", IsNullable = true)]
        public string Code { get; set; }
        [SugarColumn(ColumnDescription = "工位名称", IsNullable = true)]
        public string StationName { get; set; }
        [SugarColumn(ColumnDescription = "目标名称", IsNullable = true)]
        public string TargetName { get; set; }
        [SugarColumn(ColumnDescription = "报警错误详情", IsNullable = true)]
        public string WarningText { get; set; }
        [SugarColumn(ColumnDescription = "报警开始时间", IsNullable = true)]
        public DateTime? StartTime { get; set; }
        [SugarColumn(ColumnDescription = "报警结束时间", IsNullable = true)]
        public DateTime? EndTime { get; set; }
        [SugarColumn(ColumnDescription = "报警耗时(s)", IsNullable = true)]
        public string ElapsedTime { get; set; }
    }
}
