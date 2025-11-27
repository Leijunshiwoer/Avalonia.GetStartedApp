using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Repositories;
using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Services
{
    public class SysRoleService : ISysRoleService
    {
        private readonly ISqlSugarRepository<SysRole> _roleRep;

        public SysRoleService(ISqlSugarRepository<SysRole> roleRep)
        {
            _roleRep = roleRep;
        }

        public List<SysRole> GetRoleLessSort(int sort)
        {
            return _roleRep.ToList(x => x.Sort <= sort);
        }

        public List<SysRole> GetRoleLessSortByRoleId(int roleId)
        {
            // 根据sort值获取排序小于该值的角色列表
            var role = _roleRep.Context.Queryable<SysRole>().Where(x => x.Id == roleId).First();
            // roleId参数在此方法中未使用，因为我们只是根据sort过滤角色
            return _roleRep.ToList(x => x.Sort <= role.Sort);
        }

    }
}

