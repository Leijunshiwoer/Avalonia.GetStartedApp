using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.IServices
{
    public interface ISysRoleService
    {
        List<SysRole> GetRoleLessSort(int sort);
        List<SysRole> GetRoleLessSortByRoleId(int roleId);
    }
}
