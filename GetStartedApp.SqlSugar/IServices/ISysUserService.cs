﻿using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.IServices
{
    public interface ISysUserService
    {
        List<SysUser> GetUsers();
        bool Login(string userName, string password);
        int InserOrUpdateUser(SysUser sysUser);
        SysUser GetUserById(int id);
    }
}
