using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Product_Task_OneCode_Config")]
    public class Product_Task_OneCode_Config : AutoIncrementEntity
    {
        [SugarColumn(ColumnDescription = "公司代号")]
        public string GSDH { get; set; }
        [SugarColumn(ColumnDescription = "产品名称")]
        public string CPMC { get; set; }
        [SugarColumn(ColumnDescription = "产品型号")]
        public string CPXH { get; set; }
        [SugarColumn(ColumnDescription = "年号")]
        public string YH { get; set; }
        [SugarColumn(ColumnDescription = "产品批号")]
        public string CPPH { get; set; }
        [SugarColumn(ColumnDescription = "产线号")]
        public string CXH { get; set; }
        [SugarColumn(ColumnDescription = "产品序列号")]
        public string CPXLH { get; set; }
        [SugarColumn(ColumnDescription = "下次打标流水号")]
        public int Idx { get; set; }

        [SugarColumn(ColumnDescription = "管壳编码01",IsNullable =true)]
        public string GKCode01 { get; set; }
        [SugarColumn(ColumnDescription = "管壳编码02", IsNullable = true)]
        public string GKCode02 { get; set; }

        [SugarColumn(ColumnDescription = "管壳编码01刻印一维码", IsNullable =true)]
        public string BarCode01 { get; set; }
        [SugarColumn(ColumnDescription = "管壳编码02刻印一维码", IsNullable = true)]
        public string BarCode02 { get; set; }
        public int TaskId { get; set; }
    }
}
