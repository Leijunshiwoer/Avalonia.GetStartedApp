using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Services;
using GetStartedApp.WebApi.Extensions;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using SqlSugar;

var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // NLog: 清除默认 provider 并使用 NLog
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    builder.Host.UseNLog(); // 需要安装 NLog.Web.AspNetCore

    //配置sqlsugar,redis,注入服务
     builder.Services.SqlSugarConfigure(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
    // 2. 添加 Swagger UI 服务（需要安装 Swashbuckle.AspNetCore 包）
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
        {
            Title = "产线管理系统WebApi",
            Version = "v1",
            Description = "API Description"
        });
    });
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        
    }
    // 启用 CORS（必须在 UseRouting 之前）
    app.UseCors("AllowAll"); // 开发环境可以使用宽松的策略
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
        c.RoutePrefix = string.Empty; // 设置为默认首页
    });
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // NLog: 捕获启动异常
    logger.Error(ex, "程序启动失败");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
