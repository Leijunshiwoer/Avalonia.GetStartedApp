using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Device_Carrier_Config")]
    public class Device_Carrier_Config : AutoIncrementEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        [SugarColumn(IsNullable = true,ColumnDescription = "第一个位置产品SN")]
        public string Part01 { get; set; }
        [SugarColumn(IsNullable = true, ColumnDescription = "第二个位置产品SN")]
        public string Part02 { get; set; }
        public int OkCount { get; set; }
        public int NgCount { get; set; }
    }
}
