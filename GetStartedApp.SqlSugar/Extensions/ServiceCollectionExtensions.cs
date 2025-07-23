
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Repositories;
using GetStartedApp.SqlSugar.Services;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 SqlSugar 拓展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, ConnectionConfig config, Action<ISqlSugarClient> buildAction = default)
        {
            return services.AddSqlSugar(new ConnectionConfig[] { config }, buildAction);
        }

        /// <summary>
        /// 添加 SqlSugar 拓展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configs"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, ConnectionConfig[] configs, Action<ISqlSugarClient> buildAction = default)
        {
            services.AddSingleton<ISqlSugarClient>(u =>
            {
                var sqlSugarClient = new SqlSugarScope(configs.ToList());
                buildAction?.Invoke(sqlSugarClient);
                return sqlSugarClient;
            });
            services.AddSingleton<ITenant>(u =>
            {
                var tenant = new SqlSugarScope(configs.ToList());
                buildAction?.Invoke(tenant);
                return tenant;
            });

            // 注册非泛型仓储
            services.AddScoped<ISqlSugarRepository, SqlSugarRepository>();

            // 注册
            services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));

            // 增加业务服务

            services.AddTransient<ISysUserService, SysUserService>();
            services.AddTransient<ISysRoleService, SysRoleService>();
            services.AddTransient<ISysMenuService, SysMenuService>();

            services.AddTransient<IBase_Version_Primary_Config_Service, Base_Version_Primary_Config_Service>();
            services.AddTransient<IBase_Version_Second_Config_Service, Base_Version_Second_Config_Service>();
            services.AddTransient<IBase_Version_Attribute_Config_Service, Base_Version_Attribute_Config_Service>();
            services.AddTransient<IProduct_Recipe_Service, Product_Recipe_Service>();
            services.AddTransient<IBase_Route_Config_Service, Base_Route_Config_Service>();
            ////单例服务
            //services.AddSingleton<IProduct_Task_Time_Service, Product_Task_Time_Service>();

            return services;
        }
    }
}