using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    /// <summary>
    /// 基础数据-工序配置
    /// </summary>
    [SugarTable(tableName: "Base_Process_Config")]
    public class Base_Process_Config : AutoIncrementEntity
    {
        [SugarColumn(Length = 50)]
        public string Code { get; set; }
        [SugarColumn(Length = 200)]
        public string Name { get; set; }
        public int Index { get; set; }
        public int? RouteId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(RouteId))]
        public Base_Route_Config Route { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Base_Process_Step_Config.ProcessId))]
        public List<Base_Process_Step_Config> Steps { get; set; }
    }
}
