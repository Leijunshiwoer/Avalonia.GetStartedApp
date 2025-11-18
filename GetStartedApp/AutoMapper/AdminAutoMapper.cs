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
    public class AdminAutoMapper : Profile
    {
        public AdminAutoMapper()
        {
            CreateMap<SysUser, UserDto>().ReverseMap();
            CreateMap<SysRole, RoleDto>().ReverseMap();
            CreateMap<SysMenu, MenuDto>().ReverseMap();
            CreateMap<Base_Version_Primary_Config, VersionPrimaryDto>().ReverseMap();
            CreateMap<Base_Version_Second_Config, VersionSecondDto>() .ReverseMap();
            CreateMap<Base_Version_Attribute_Config, AttributeDto>().ReverseMap();
            CreateMap<Base_Process_Step_Config, ProcessStepDto>().ReverseMap();

            CreateMap<Product_Recipe_Config, RecipeDto>().ReverseMap();
            CreateMap<Product_Recipe_Material_Config, RecipeMaterialDto>().ReverseMap();
            CreateMap<Product_Recipe_ST_Config, RecipeSTDto>().ReverseMap();
            CreateMap<Product_Recipe_ST_Parameter_Config, RecipeSTParameterDto>().ReverseMap();
            CreateMap<Base_Route_Config, RouteDto>().ReverseMap();
            CreateMap<Base_Process_Config, ProcessDto>().ReverseMap();
            CreateMap<Base_Process_Step_Config, ProcessStepDto>().ReverseMap();
        }
    }

}
