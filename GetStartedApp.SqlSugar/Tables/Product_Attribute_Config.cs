using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Base_Version_Attribute_Config")]
    public class Base_Version_Attribute_Config : AutoIncrementEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        [SugarColumn(ColumnDescription = "属性类型")]
        public Em_Attribute_Type AttributeType { get; set; }
        [SugarColumn(ColumnDescription = "值类型")]
        public Em_Value_Type ValueType { get; set; }
        public string Value { get; set; }
        [SugarColumn(IsNullable = true, ColumnDescription = "映射目标")]
        public string Target { get; set; }
        public int SecondId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(SecondId))]
        public Base_Version_Second_Config VersionSecond { get; set; }
        public int StepId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(StepId))]
        public Base_Process_Step_Config Step { get; set; }
    }

    public enum Em_Attribute_Type
    {
        [Display(Name = "标准值")]
        Standard = 1,
        [Display(Name = "下限值")]
        LowerLimit = 2,
        [Display(Name = "上限值")]
        UpperLimit = 3
    }
    public enum Em_Value_Type
    {
        [Display(Name = "Int16")]
        Int16 = 1,
        [Display(Name = "Int32")]
        Int32 = 2,
        [Display(Name = "Float")]
        Float = 3,
        [Display(Name = "String")]
        String = 4,
        [Display(Name = "WString")]
        WString = 5
    }
}
