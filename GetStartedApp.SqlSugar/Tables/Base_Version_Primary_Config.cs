using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Base_Version_Primary_Config")]
    public class Base_Version_Primary_Config : AutoIncrementEntity
    {
        [SugarColumn(Length = 50)]
        public string Code { get; set; }
        [SugarColumn(Length = 200)]
        public string Name { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Base_Version_Second_Config.VersionPrimaryId))]
        public List<Base_Version_Second_Config> VersionSeconds { get; set; }
    }
}
