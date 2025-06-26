using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    /// <summary>
    /// 基础数据-工艺路线配置
    /// </summary>
    [SugarTable(tableName: "Base_Route_Config")]
    public class Base_Route_Config : AutoIncrementEntity
    {
        [SugarColumn(Length = 50)]
        public string Code { get; set; }
        [SugarColumn(Length = 200)]
        public string Name { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Base_Process_Config.RouteId))]
        public List<Base_Process_Config> Processs { get; set; }
        public int VersionSecondId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(VersionSecondId))]
        public Base_Version_Second_Config VersionSecond { get; set; }
    }
}
