using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Product_Task_TwoCode1_Config")]
    public class Product_Task_TwoCode1_Config : AutoIncrementEntity
    {
        [SugarColumn(ColumnDescription = "生产日期")]
        public string SCRQ { get; set; }
        [SugarColumn(ColumnDescription = "序列号")]
        public string XLH { get; set; }
        [SugarColumn(ColumnDescription = "产品型号")]
        public string CPXH { get; set; }
        [SugarColumn(ColumnDescription = "客户代码")]
        public string KHDM { get; set; }
        [SugarColumn(ColumnDescription = "下次打标流水号")]
        public int Idx { get; set; }
        [SugarColumn(ColumnDescription = "防错字符串")]
        public string FCZFC { get; set; }
        public int TwoCodeId { get; set; }


    }
}
