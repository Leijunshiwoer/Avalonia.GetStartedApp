using GetStartedApp.SqlSugar.Tables;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Extensions
{
    public static class QueryFilterExtensions
    {
        public static void GlobalFilter(this ISqlSugarClient client)
        {

            client.QueryFilter.Add(new TableFilterItem<SysUser>(it => it.IsDeleted == "N" || it.IsDeleted == null));
        }
    }
}
