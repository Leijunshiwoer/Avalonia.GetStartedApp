using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    /// <summary>
    /// 基础数据-物理模型数据-工作单元
    /// </summary>
    [SugarTable(tableName: "Base_Model_Unit_Config")]
    public class Base_Model_Unit_Config : AutoIncrementEntity
    {
        [SugarColumn(Length = 50)]
        public string Code { get; set; }
        [SugarColumn(Length = 200)]
        public string Name { get; set; }
        public int? LineId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(LineId))]
        public Base_Model_Line_Config Line { get; set; }
    }
}
