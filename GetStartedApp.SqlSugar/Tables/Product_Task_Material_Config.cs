using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Product_Task_Material_Config")]
    public class Product_Task_Material_Config : AutoIncrementEntity
    {
        [SugarColumn(ColumnDescription = "工单ID")]
        public int TaskId { get; set; }
        [SugarColumn(ColumnDescription = "原料名称")]
        public string Name { get; set; }
        [SugarColumn(ColumnDescription = "物料编码")]
        public string Code { get; set; }
        [SugarColumn(ColumnDescription = "|个数1")]
        public int Count1 { get; set; }
        [SugarColumn(ColumnDescription = "|位置1")]
        public int Idx1 { get; set; }
        [SugarColumn(ColumnDescription = "|个数2")]
        public int Count2 { get; set; }
        [SugarColumn(ColumnDescription = "|位置2")]
        public int Idx2 { get; set; }
    }
}
