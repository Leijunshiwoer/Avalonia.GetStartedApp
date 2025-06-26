using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Product_Task_Time_Log", tableDescription:"工单执行时间记录")]
    public class Product_Task_Time_Log : AutoIncrementEntity
    {
        [SugarColumn(IsNullable = true,ColumnDescription = "订单开始时间")]
        public DateTime? StartTime { get; set; }
        [SugarColumn(IsNullable = true,ColumnDescription = "订单开始运行时间")]
        public DateTime? StartRunTime { get; set; }
        [SugarColumn(IsNullable = true,ColumnDescription = "运行时间")]
        public long RunTime { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime? StartStopTime { get; set; }
        [SugarColumn(IsNullable = true)]
        public long StopTime { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime? StartErrTime { get; set; }
        [SugarColumn(IsNullable = true)]
        public long ErrTime { get; set; }
        public int ProductTaskId { get; set; }
    }
}
