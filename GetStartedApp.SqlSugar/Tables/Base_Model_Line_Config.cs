using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    /// <summary>
    /// 基础数据-物理模型数据-产线
    /// </summary>
    [SugarTable(tableName: "Base_Model_Line_Config")]
    public class Base_Model_Line_Config : AutoIncrementEntity
    {
        [SugarColumn(Length = 50)]
        public string Code { get; set; }
        [SugarColumn(Length = 200)]
        public string Name { get; set; }
        public int? WorkcenterId { get; set; }
        [Navigate(NavigateType.OneToOne,  nameof(WorkcenterId))]
        public Base_Model_Workcenter_Config Workcenter { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Base_Model_Unit_Config.LineId))]
        public List<Base_Model_Unit_Config> Units { get; set; }
    }
}
