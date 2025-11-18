using SqlSugar;
using GetStartedApp.SqlSugar.Extensions;
namespace GetStartedApp.WebApi.Extensions
{
    public static class SqlSugarConfigureExtensions
    {
        public static void SqlSugarConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            var connectConfigList = new List<ConnectionConfig>();
            //数据库序号从0开始,默认数据库为0

            //默认数据库
            connectConfigList.Add(new ConnectionConfig
            {
                ConnectionString = configuration.GetConnectionString("Default"),
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,

            });
            services.AddSqlSugar(connectConfigList.ToArray()
               , db =>
               {
                   ////执行超时时间
                   // db.Ado.CommandTimeOut = 30;

                   //插入和更新过滤器
                   db.Aop.DataExecuting = (oldValue, entityInfo) =>
                   {
                       // 新增操作
                       if (entityInfo.OperationType == DataFilterType.InsertByObject)
                       {
                           // 主键(long类型)且没有值的---赋值雪花Id
                           if (entityInfo.EntityColumnInfo.IsPrimarykey && entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long))
                           {
                               var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                               //if (id == null || (long)id == 0)
                               //    entityInfo.SetValue(YitIdHelper.NextId());
                           }
                           //if (entityInfo.PropertyName == nameof(BaseEntity.CreateTime))
                           //    entityInfo.SetValue(DateTime.Now);
                       }
                       // 更新操作
                       if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                       {
                           //更新时间
                           //if (entityInfo.PropertyName == nameof(BaseEntity.UpdateTime))
                           //    entityInfo.SetValue(DateTime.Now);
                       }
                   };
                   // 配置加删除全局过滤器
                   db.GlobalFilter();
               });
        }
    }
}
