using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Machine_QuickWearPart_Config")]
    public class Machine_QuickWearPart_Config : AutoIncrementEntity
    {
            
        [SugarColumn(ColumnDescription = "编号")]
        public string Code { get; set; }
        [SugarColumn(ColumnDescription = "名称")]
        public string Name { get; set; }
        [SugarColumn(ColumnDescription = "跟换时间")]
        public DateTime ChangedTime { get; set; }
        [SugarColumn(ColumnDescription = "预警时间")]
        public int EarlyWarningTime { get; set; }
        [SugarColumn(ColumnDescription = "报警时间")]
        public int WarningTime { get; set; }
        [SugarColumn(ColumnDescription = "扣除次数")]
        public int DeductCount { get; set; }
        [SugarColumn(ColumnDescription = "时间单位")]
        public Em_QuickWearPart_Unit Unit { get; set; }
        [SugarColumn(ColumnDescription = "使用次数")]
        public int UseCount { get; set; }
        [SugarColumn(ColumnDescription = "预警次数，易损件预警时开始工单会出现提示，可继续下发工单")]
        public int EarlyWarningCount { get; set; }
        [SugarColumn(ColumnDescription = "报警次数，易损件报警时开始工单会出现错误，不可下发工单")]
        public int WarningCount { get; set; }
        [SugarColumn(ColumnDescription = "条件")]
        public Em_QuickWearPart_Condition Condition { get; set; }
        [SugarColumn(ColumnDescription = "状态")]
        public Em_QuickWearPart_Status Status { get; set; }
        [SugarColumn(ColumnDescription = "对应工位")]
        public string ST { get; set; }
    }
    public enum Em_QuickWearPart_Unit
    {
        [Display(Name = "天")]
        Day = 1,
        [Display(Name = "周")]
        Week = 2,
        [Display(Name = "月")]
        Month = 3,
        [Display(Name = "季")]
        Quarter = 4,
        [Display(Name = "年")]
        Yeer = 5,
    }
    public enum Em_QuickWearPart_Condition
    {
        [Display(Name = "时间条件")]
        Time = 1,
        [Display(Name = "次数条件")]
        Count = 2,
        [Display(Name = "时间与次数")]
        TimeAndCount = 3,
        [Display(Name = "时间或次数")]
        TimeOrCount = 4,
    }
    public enum Em_QuickWearPart_Status
    {
        [Display(Name = "正常")]
        Normal = 1,
        [Display(Name = "警告")]
        Warning = 2,
        [Display(Name = "到期")]
        Error = 3,
    }
}
