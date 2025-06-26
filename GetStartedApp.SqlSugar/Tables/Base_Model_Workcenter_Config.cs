using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    /// <summary>
    /// 基础数据-物理模型数据-工作中心
    /// </summary>
    [SugarTable(tableName: "Base_Model_Workcenter_Config")]
    public class Base_Model_Workcenter_Config : AutoIncrementEntity
    {
        [SugarColumn(Length = 50)]
        public string Code { get; set; }
        [SugarColumn(Length = 200)]
        public string Name { get; set; }
        public int? FactoryId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(FactoryId))]
        public Base_Model_Factory_Config Factory { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Base_Model_Line_Config.WorkcenterId))]
        public List<Base_Model_Line_Config> Lines { get; set; }
    }
}
