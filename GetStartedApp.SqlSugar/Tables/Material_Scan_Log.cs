using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Material_Scan_Log")]
    public class Material_Scan_Log : AutoIncrementEntity
    {

        [SugarColumn(ColumnDescription = "物料名称")]
        public string Name { get; set; }
        [SugarColumn(ColumnDescription = "物料编号")]
        public string Code { get; set; }
        [SugarColumn(ColumnDescription = "物料批次")]
        public string Batch { get; set; }
        [SugarColumn(ColumnDescription = "内容")]
        public string Content { get; set; }

        [SugarColumn(ColumnDescription = "扫码时间")]
        public DateTime ScanTime { get; set; }
    }
}
