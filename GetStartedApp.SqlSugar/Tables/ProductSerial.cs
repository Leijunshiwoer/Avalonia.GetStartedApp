using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "ProductSerial")]
    public class ProductSerial : AutoIncrementEntity
    {
        [SugarColumn(ColumnDescription = "底座序号")]
        public long DZ_Serial { get; set; }

        [SugarColumn(ColumnDescription = "管壳序号")]
        public long GK_Serial { get; set; }

        [SugarColumn(ColumnDescription = "返修序号")]
        public long FX_Serial { get; set; }
    }
}