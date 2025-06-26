using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "SysUser")]
    public class SysUser : AutoIncrementEntity
    {
        [SugarColumn(IsNullable = true,Length = 200)]
        public string DisplayName { get; set; }
        [SugarColumn(Length = 200)]
        public string Name { get; set; }
        [SugarColumn(ColumnDescription = "工号",Length = 200)]
        public string JobNumber { get; set; }
        public string Password { get; set; }
        [SugarColumn( IsNullable = true, Length = 50)]
        public string Department { get; set; }
        public int? RoleId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(RoleId))]
        public SysRole Role { get; set; }
    }
}
