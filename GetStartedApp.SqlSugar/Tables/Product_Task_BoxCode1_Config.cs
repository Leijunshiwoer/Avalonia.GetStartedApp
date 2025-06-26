using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Product_Task_BoxCode1_Config")]
    public class Product_Task_BoxCode1_Config : AutoIncrementEntity
    {
        [SugarColumn(ColumnDescription = "产品代号")]
        public string CPDH { get; set; }
        [SugarColumn(ColumnDescription = "固定码")]
        public string GDM { get; set; }
        [SugarColumn(ColumnDescription = "年月日")]
        public string YYR { get; set; }
        [SugarColumn(ColumnDescription = "本盒数量")]
        public string SL { get; set; }
        [SugarColumn(ColumnDescription = "盒序号")]
        public string HXH { get; set; }
        [SugarColumn(ColumnDescription = "月日年")]
        public string YRY { get; set; }
        [SugarColumn(ColumnDescription = "下次打标流水号")]
        public int Idx { get; set; }
        public int BoxCodeId { get; set; }
    }
}
