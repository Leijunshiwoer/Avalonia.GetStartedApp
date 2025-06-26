using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    /// <summary>
    /// 基础数据-工位配置
    /// </summary>
    [SugarTable(tableName: "Base_Process_Step_Config")]
    public class Base_Process_Step_Config : AutoIncrementEntity
    {
        [SugarColumn(Length = 50)]
        public string Code { get; set; }
        [SugarColumn(Length = 200)]
        public string Name { get; set; }
        [SugarColumn(IsNullable = true)]
        public string UnitCode { get; set; }
        public int Index { get; set; }
        public Em_Step_Type StepType { get; set; }
        [SugarColumn(ColumnDescription = "是否属于PLC数据上报")]
        public int IsInPLC { get; set; }
        public int ProcessId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(ProcessId))]
        public Base_Process_Config Process { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Base_Version_Attribute_Config.StepId))]
        public List<Base_Version_Attribute_Config> Attributes { get; set; }
    }
    /// <summary>
    /// 工位类型
    /// </summary>
    public enum Em_Step_Type
    {
        [Display( Name = "真实工位")]
        RealStep = 1,
        [Display( Name = "虚拟工位")]
        VirtualStep = 2
    }
}
