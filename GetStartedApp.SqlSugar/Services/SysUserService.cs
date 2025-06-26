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
    public class SysUserService: ISysUserService
    {
        private readonly ISqlSugarRepository<SysUser> _suerRep;

        public SysUserService(ISqlSugarRepository<SysUser> suerRep)
        {
            _suerRep = suerRep;
        }


        public List<SysUser> GetUsers()
        {
            return _suerRep.Context.Queryable<SysUser>().Includes(x => x.Role).ToList();
        }


    }
}
