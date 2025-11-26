
using Avalonia;
using GetStartedApp;
using GetStartedApp.Utils;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.MaterialDesign;
using System;
using Velopack;
using System.IO;
using System.Threading.Tasks;
using Serilog;

namespace Avalonia.GetStartedApp.Desktop;

internal sealed class Program
{
    public static MemoryLogger Log { get; private set; } = new();

    [STAThread]
    public static void Main(string[] args)
    {
        // 先初始化文件日志（这样即使 App 本身初始化失败也能记录）
        string logDirectory = Path.Combine(AppContext.BaseDirectory ?? AppDomain.CurrentDomain.BaseDirectory, "log");
        try
        {
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
        }
        catch
        {
            // 回退到用户本地应用数据目录
            logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GetStartedApp", "log");
            try
            {
                if (!Directory.Exists(logDirectory))
                    Directory.CreateDirectory(logDirectory);
            }
            catch
            {
                // 最后兜底到临时目录
                logDirectory = Path.Combine(Path.GetTempPath(), "GetStartedApp", "log");
                Directory.CreateDirectory(logDirectory);
            }
        }

        // Serilog 初始化（使用全限定名以避免与本类 Program.Log 冲突）
        var logFilePattern = Path.Combine(logDirectory, "desktop-log-.txt"); // Serilog 会按日期生成实际文件名
        Serilog.Log.Logger = new Serilog.LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                path: logFilePattern,
                rollingInterval:RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                retainedFileCountLimit: 30,
                encoding: System.Text.Encoding.UTF8)
            .CreateLogger();

        Serilog.Log.Information("Desktop exe starting. LogDirectory: {LogDirectory}", logDirectory);

        // 全局未捕获异常捕获（尽早绑定）
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            if (e.ExceptionObject is Exception ex)
                Serilog.Log.Fatal(ex, "Unhandled exception (AppDomain)");
            else
                Serilog.Log.Fatal("Unhandled exception (AppDomain) - non-exception object");

            Serilog.Log.CloseAndFlush();
        };
        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            Serilog.Log.Error(e.Exception, "Unobserved task exception");
            e.SetObserved();
        };

        try
        {
            // Velopack 初始化
            VelopackApp.Build()
                .OnFirstRun((v) => { /* Your first run code here */ })
                .SetLogger(Log)
                .Run();

            // 启动 Avalonia
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            Serilog.Log.Information("Application exited normally.");
        }
        catch (Exception ex)
        {
            // 记录启动或运行时异常
            Serilog.Log.Fatal(ex, "Application start failed");
            Console.WriteLine("Unhandled exception: " + ex.ToString());
            throw;
        }
        finally
        {
            // 确保日志被刷新
            Serilog.Log.CloseAndFlush();
        }
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current.Register<MaterialDesignIconProvider>();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}