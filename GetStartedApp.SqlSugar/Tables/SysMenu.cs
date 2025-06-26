using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "SysMenu")]
    public class SysMenu : NotAutoIncrementEntity
    {
        public string Name { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Navigate { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Icon { get; set; }

        public int Sort { get; set; }
        public int ParentId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public ICollection<SysMenu> Menus { get; set; }
    }
}