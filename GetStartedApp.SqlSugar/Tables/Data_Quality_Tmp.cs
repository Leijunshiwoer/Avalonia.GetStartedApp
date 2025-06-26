using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Data_Quality_Tmp")]
    public class Data_Quality_Tmp : AutoIncrementEntity
    {
        [SugarColumn(ColumnDescription = "工单ID")]
        public int TaskId { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "管壳逻辑编码")]
        public string PartCode { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "字符编码")]
        public string Barcode { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "批次")]
        public string Batch { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "产品结果", Length = 20)]
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

        //--------------------OP20----------------------------

        //ST1

        //ST2
        [SugarColumn(IsNullable = true, ColumnDescription = "管壳高度")]
        public float? OP20_ST2_TubeHigh { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "管壳高度结果", Length = 2)]
        public string OP20_ST2_TubeHighOK { get; set; }

        //[SugarColumn(IsNullable = true, ColumnDescription = "相机检测结果", Length = 2)]
        //public string OP20_ST2_CCDOK { get; set; }

        //ST3
        [SugarColumn(IsNullable = true, ColumnDescription = "管壳重量")]
        public float? OP20_ST3_TubeWeigh { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "管壳重量结果", Length = 2)]
        public string OP20_ST3_TubeWeighOK { get; set; }

        //ST5
        [SugarColumn(IsNullable = true, ColumnDescription = "一次加配方编号")]
        public int? OP20_ST5_RecipeNo { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "一次加药称编号")]
        public int? OP20_ST5_WeighNo { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "一次加药重量", Length = 2)]
        public float? OP20_ST5_PowerWeigh1 { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "一次加药机温度")]
        public float? OP20_ST5_Temperature { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "一次加药机湿度")]
        public float? OP20_ST5_Humidity { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "一次加药机加药时间")]
        public float? OP20_ST5_AddTime { get; set; }

        //ST7
        [SugarColumn(IsNullable = true, ColumnDescription = "一次加药重量")]
        public float? OP20_ST7_PowerWeigh1 { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "一次加药重量结果", Length = 2)]
        public string OP20_ST7_PowerWeigh1OK { get; set; }

        //ST9
        [SugarColumn(IsNullable = true, ColumnDescription = "二次加配方编号")]
        public int? OP20_ST9_RecipeNo { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "二次加药称编号")]
        public int? OP20_ST9_WeighNo { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "二次加药重量")]
        public float? OP20_ST9_PowerWeigh2 { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "二次加药机温度")]
        public float? OP20_ST9_Temperature { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "二次加药机湿度")]
        public float? OP20_ST9_Humidity { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "二次加药机加药时间")]
        public float? OP20_ST9_AddTime { get; set; }

        //ST12
        [SugarColumn(IsNullable = true, ColumnDescription = "二次加药重量")]
        public float? OP20_ST12_PowerWeigh2 { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "二次加药重量结果", Length = 2)]
        public string OP20_ST12_PowerWeigh2OK { get; set; }

        //st18
        [SugarColumn(IsNullable = true, ColumnDescription = "副产品编码")]
        public string AppendPartCode { get; set; }

        //st19

        [SugarColumn(IsNullable = true, ColumnDescription = "CCD结果", Length = 2)]
        public string OP20_ST19_CCDOK { get; set; }

        //st20
        [SugarColumn(IsNullable = true, ColumnDescription = "电极针高度")]
        public float? OP20_ST20_DJZHigh { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "电极针结果", Length = 2)]
        public string OP20_ST20_DJZHighOK { get; set; }

        //[SugarColumn(IsNullable = true, ColumnDescription = "外O型圈厚度")]
        //public float? OP20_ST10_5_WaiOThick { get; set; }
        //[SugarColumn(IsNullable = true, ColumnDescription = "外O型圈厚度结果", Length = 2)]
        //public string OP20_ST10_5_WaiOThickOK { get; set; }

        //[SugarColumn(IsNullable = true, ColumnDescription = "成品收口工位")]
        //public string OP20_ST10_6_7_ST { get; set; }

        //st22
        [SugarColumn(IsNullable = true, ColumnDescription = "成品收口压力")]
        public float? OP20_ST22_MGGpress { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "成品收口压力结果", Length = 2)]
        public string OP20_ST22_MGGpressOK { get; set; }

        //st24
        [SugarColumn(IsNullable = true, ColumnDescription = "平行度")]
        public float? OP20_ST24_CorrectAngle { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "平行度结果", Length = 2)]
        public string OP20_ST24_CorrectAngleOK { get; set; }

        //st25

        [SugarColumn(IsNullable = true, ColumnDescription = "成品收口外径")]
        public float? OP20_ST25_MggDia { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "成品收口外径结果", Length = 2)]
        public string OP20_ST25_MggDiaOK { get; set; }

        //ST13

        //ST27

        [SugarColumn(IsNullable = true, ColumnDescription = "成品收口高度")]
        public float? OP20_ST27_MggPressHigh { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "成品收口高度结果", Length = 2)]
        public string OP20_ST27_MggPressHighOK { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "总高")]
        public float? OP20_ST27_MggTotalHigh { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "总高结果", Length = 2)]
        public string OP20_ST27_MggTotalHighOk { get; set; }

        //ST15

        //ST16

        //ST17

        //ST18

        //--------------------OP30----------------------------
        //ST1

        //ST2 ST3
        //[SugarColumn(IsNullable = true, ColumnDescription = "检漏工位")]
        //public string OP30_ST2_ST3_ST { get; set; }
        //[SugarColumn(IsNullable = true, ColumnDescription = "检漏仪器")]
        //public string OP30_ST2_ST3_Machine { get; set; }
        //[SugarColumn(IsNullable = true, ColumnDescription = "泄漏率")]
        //public float? OP30_ST2_ST3_Leak { get; set; }
        //[SugarColumn(IsNullable = true, ColumnDescription = "检漏结果", Length = 2)]
        //public string OP30_ST2_ST3_LeakOK { get; set; }

        ////ST4
        //[SugarColumn(IsNullable = true, ColumnDescription = "平行度")]
        //public float? OP30_ST4_CorrectAngle { get; set; }
        //[SugarColumn(IsNullable = true, ColumnDescription = "平行度结果", Length = 2)]
        //public string OP30_ST4_CorrectAngleOK { get; set; }

        ////ST5
        //[SugarColumn(IsNullable = true, ColumnDescription = "缺口槽检验", Length = 2)]
        //public string OP30_ST5_ParallelismOK { get; set; }

        //ST6

        [SugarColumn(IsNullable = true, ColumnDescription = "短路夹压力")]
        public float? OP30_ST6_Pressure { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "短路夹压力结果", Length = 2)]
        public string OP30_ST6_PressureOK { get; set; }

        //ST8
        [SugarColumn(IsNullable = true, ColumnDescription = "短路夹高度")]
        public float? OP30_ST8_High { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "短路夹高度果", Length = 2)]
        public string OP30_ST8_HighOK { get; set; }

        //ST9
        [SugarColumn(IsNullable = true, ColumnDescription = "桥路检测仪器")]
        public string OP30_ST9_Machine { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "桥路电阻")]
        public float? OP30_ST9_BridgeR { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "桥路电阻结果", Length = 2)]
        public string OP30_ST9_BridgeROK { get; set; }

        //ST11
        [SugarColumn(IsNullable = true, ColumnDescription = "绝缘检测仪器")]
        public string OP30_ST11_Machine { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "绝缘电阻")]
        public float? OP30_ST11_InslationR { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "绝缘电阻结果", Length = 2)]
        public string OP30_ST11_InslationRROK { get; set; }

        //st12
        [SugarColumn(IsNullable = true, ColumnDescription = "短路检测仪器")]
        public string OP30_ST12_Machine { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "短路电阻")]
        public float? OP30_ST12_ShortR { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "短路电阻结果", Length = 2)]
        public string OP30_ST12_ShortROK { get; set; }

        //ST13

        //ST14
        [SugarColumn(IsNullable = true, ColumnDescription = "激光机")]
        public string OP30_ST13_Machine { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "一维码刻印")]
        public string OP30_ST13_Barcode { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "扫出一维码")]
        public string OP30_ST14_Barcode { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "短路夹间隙")]
        public float OP30_ST14_DLJ { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "相机短路夹拍照结果", Length = 2)]
        public string OP30_ST14_DLJOk { get; set; }

        //ST11
        //[SugarColumn(IsNullable = true, ColumnDescription = "外观影像检测")]
        //public string OP30_ST11_FlawOK { get; set; }
        //[SugarColumn(IsNullable = true, ColumnDescription = "字符编码检测结果", Length = 2)]
        //public string OP30_ST11_CheckCharOK { get; set; }

        //ST15

        [SugarColumn(IsNullable = true, ColumnDescription = "刻印二维码")]
        public string OP30_ST15_PartCode { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "激光机")]
        public string OP30_ST15_Machine { get; set; }

        //ST16
        [SugarColumn(IsNullable = true, ColumnDescription = "扫出二维码")]
        public string OP30_ST16_PartCode { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "扫出二维码结果", Length = 2)]
        public string OP30_ST16_PartCodeOK { get; set; }

        ////ST14
        //[SugarColumn(IsNullable = true, ColumnDescription = "总重量")]
        //public float? OP30_ST14_MggTotalWeigh { get; set; }
        //[SugarColumn(IsNullable = true, ColumnDescription = "总重量结果", Length = 2)]
        //public string OP30_ST14_MggTotalWeighOK { get; set; }
        //ST15

        //ST16

        //ST17

        //ST18
    }
}