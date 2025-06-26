using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "SysRoleMenu")]
    public class SysRoleMenu : AutoIncrementEntity
    {
        public int RoleId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public SysRole Role { get; set; }
        public int MenuId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public SysMenu Menu { get; set; }
    }
}
