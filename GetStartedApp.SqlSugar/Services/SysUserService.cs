using GetStartedApp.Core.Helpers;
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
    public class SysUserService : ISysUserService
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

        public SysUser GetUserById(int id)
        {
            return _suerRep.Context.Queryable<SysUser>().Where(x => x.Id == id).Includes(x => x.Role).First();
        }

        public bool Login(string userName, string password)
        {
            var pwd = MD5Helper.MD5Encryp(password);
            var user = _suerRep.Context.Queryable<SysUser>().Where(x => x.Name == userName && x.Password == pwd).First();

            if (user != null)
            {
                UserInfo.User = GetUserById(user.Id);
                UserInfo.UserName = userName;
                UserInfo.UserId = user.Id;
                UserInfo.Password = pwd;
                return true;
            }
            return false;
        }

        public int InserOrUpdateUser(SysUser sysUser)
        {
            if (_suerRep.IsExists(x => x.Id == sysUser.Id))
            {
                //跟新
                return _suerRep.Update(sysUser);
            }
            else
            {
                //新增
                return _suerRep.Insert(sysUser);
            }
        }
    }
}
