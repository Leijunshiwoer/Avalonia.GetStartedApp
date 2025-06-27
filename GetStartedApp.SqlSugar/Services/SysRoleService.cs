using GetStartedApp.SqlSugar.Globalvariable;
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
            return _roleRep.ToList(x => x.Sort < sort);
        }

        public List<SysRole> GetRoleLessSortByUserId(int userId, int sort)
        {
            //查看是否本人
            if (userId == UserInfo.UserId)
            {
                return _roleRep.ToList(x => x.Sort <= sort);
            }
            else
            {
                return _roleRep.ToList(x => x.Sort < sort);
            }
        }

    }
}

