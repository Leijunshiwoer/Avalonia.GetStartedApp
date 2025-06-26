using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    /// <summary>
    /// 基础数据-物理模型数据-工厂
    /// </summary>
    [SugarTable(tableName: "Base_Model_Factory_Config")]
    public class Base_Model_Factory_Config : AutoIncrementEntity
    {
        [SugarColumn(Length =50)]
        public string Code { get; set; }
        [SugarColumn(Length =50)]
        public string Name { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Base_Model_Workcenter_Config.FactoryId))]
        public List<Base_Model_Workcenter_Config> Workcenters { get; set; }
    }
}
