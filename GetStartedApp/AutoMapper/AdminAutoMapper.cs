using AutoMapper;
using GetStartedApp.Models;
using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.AutoMapper
{
    public class AdminAutoMapper:Profile
    {
        public AdminAutoMapper()
        {
            CreateMap<SysUser, UserDto>().ReverseMap();
            CreateMap<SysRole, RoleDto>().ReverseMap();
            CreateMap<Base_Version_Primary_Config, VersionPrimaryDto>()
                .ReverseMap();
            CreateMap<Base_Version_Second_Config, VersionSecondDto>()
               .ReverseMap();
        }
    }

}
