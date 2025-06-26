using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "SysRole")]
    public class SysRole : AutoIncrementEntity
    {
        public string Name { get; set; }
        [SugarColumn(ColumnDescription = "1-10数字越大等级越高 10表示程序设计人员")]
        public int Sort { get; set; }
    }
}
