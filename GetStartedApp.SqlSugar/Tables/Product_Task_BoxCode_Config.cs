using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Product_Task_BoxCode_Config")]
    public class Product_Task_BoxCode_Config : AutoIncrementEntity
    {
        [SugarColumn(ColumnDescription = "1 表示型号Ⅰ,2表示型号Ⅱ")]
        public int Select { get; set; }
        public int TaskId { get; set; }
    }
}
