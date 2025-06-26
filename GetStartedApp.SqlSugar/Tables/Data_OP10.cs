using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Data_OP10")]
    public class Data_OP10 : AutoIncrementEntity
    {
        [SugarColumn(ColumnDescription = "工单ID")]
        public int TaskId { get; set; }
        [SugarColumn(ColumnDescription = "底座逻辑编码")]
        public string AppendPartCode { get; set; }
        [SugarColumn(IsNullable = true, ColumnDescription = "批次")]
        public string Batch { get; set; }
        [SugarColumn(IsNullable = true, ColumnDescription = "主产品结果", Length = 2)]
        public string PartOK { get; set; }
        //--------------------OP10----------------------------
        //ST1
        [SugarColumn(IsNullable = true, DefaultValue = null, ColumnDescription = "底座高度")]
        public float? OP10_ST1_DizuoHigh { get; set; }
        [SugarColumn(IsNullable = true, ColumnDescription = "底座高度结果", Length = 2)]
        public string OP10_ST1_DizuoHighOK { get; set; }

        //[SugarColumn(IsNullable = true, ColumnDescription = "缺口是否检测")]
        //public float? OP10_ST1_NeedCheck { get; set; } 
        //[SugarColumn(IsNullable = true, ColumnDescription = "缺口检测结果", Length = 2)]
        //public string OP10_ST1_QueKouCheckOK { get; set; } 

        //ST2
        [SugarColumn(IsNullable = true, ColumnDescription = "内O型圈直径")]
        public float? OP10_ST2_NeiODia { get; set; }

        //ST3
        [SugarColumn(IsNullable = true, ColumnDescription = "内O厚度")]
        public float? OP10_ST3_NeiOThick { get; set; }
        [SugarColumn(IsNullable = true, ColumnDescription = "内O厚度结果", Length = 2)]
        public string OP10_ST3_NeiOThickOK { get; set; }

        //ST4

        //ST5 And ST6
        [SugarColumn(IsNullable = true, ColumnDescription = "发火体工位")]
        public string OP10_ST5_ST6_ST { get; set; }
        [SugarColumn(IsNullable = true, ColumnDescription = "发火体收口压力")]
        public float? OP10_ST5_ST6_FahuotiPres { get; set; }
        [SugarColumn(IsNullable = true, ColumnDescription = "发火体收口压力结果", Length = 2)]
        public string OP10_ST5_ST6_FahuotiPressOk { get; set; }

        ////ST6
        //[SugarColumn(IsNullable = true, ColumnDescription = "发火体收口压力")]
        //public float? OP10_ST6_FahuotiPress { get; set; } 
        //[SugarColumn(IsNullable = true, ColumnDescription = "发火体收口压力结果", Length = 2)]
        //public string OP10_ST6_FahuotiPressOk { get; set; } 

        //ST7
        //[SugarColumn(IsNullable = true, ColumnDescription = "发火体收口直径")]
        //public float? OP10_ST7_FahuotiDia { get; set; } 
        //[SugarColumn(IsNullable = true, ColumnDescription = "发火体收口直径结果", Length = 2)]
        //public string OP10_ST7_FahuotiDiaOK { get; set; } 


        [SugarColumn(IsNullable = true, ColumnDescription = "发火体收口高度")]
        public float? OP10_ST7_FahuotiHigh { get; set; }
        [SugarColumn(IsNullable = true, ColumnDescription = "发火体收口高度结果", Length = 2)]
        public string OP10_ST7_FahuotiHighOK { get; set; }

        //ST8

        //ST9
        //[SugarColumn(IsNullable = true, ColumnDescription = "外O型圈直径")]
        //public float? OP10_ST9_WaiODia { get; set; } 

        //ST10
        [SugarColumn(IsNullable = true, ColumnDescription = "外O型圈厚度")]
        public float? OP10_ST10_WaiOThick { get; set; }
        [SugarColumn(IsNullable = true, ColumnDescription = "外O型圈厚度结果", Length = 2)]
        public string OP10_ST10_WaiOThickOK { get; set; }

        //ST11
        [SugarColumn(IsNullable = true, ColumnDescription = "顶部相机检测结果", Length = 2)]
        public string OP10_ST11_CCDOK { get; set; }

        //ST12
        [SugarColumn(IsNullable = true, ColumnDescription = "侧面相机检测结果", Length = 2)]
        public string OP10_ST12_CCDOK { get; set; }
    }
}
