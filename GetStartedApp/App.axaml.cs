using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Example;
using GetStartedApp.AutoMapper;
using GetStartedApp.Interface;
using GetStartedApp.RestSharp;
using GetStartedApp.RestSharp.IServices;
using GetStartedApp.RestSharp.Services;
using GetStartedApp.SqlSugar.Extensions;
using GetStartedApp.Utils.Services;
using GetStartedApp.ViewModels;
using GetStartedApp.ViewModels.Basic;
using GetStartedApp.ViewModels.PLC;
using GetStartedApp.ViewModels.Product;
using GetStartedApp.ViewModels.ProductVersion;
using GetStartedApp.ViewModels.ProgramPack;
using GetStartedApp.ViewModels.Route;
using GetStartedApp.Views;
using IKMqttClient;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.DryIoc;
using Prism.Dialogs;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;
using Serilog;
using Serilog.Events;
using SmartCommunicationForExcel.MQTTClient;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ursa.Controls;
using Ursa.PrismExtension;
using DbType = SqlSugar.DbType;



namespace GetStartedApp
{
    public partial class App : PrismApplication
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();  // Required to initialize Prism.Avalonia - DO NOT REMOVE
            InitializeLogging();
            
            RegisterGlobalExceptionHandlers();
        }
        protected override AvaloniaObject CreateShell()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // 创建主窗口
                var mainWindow = Container.Resolve<MainWindow>();
                return mainWindow;
            }
            else
            {
                var sell = Container.Resolve<MainView>();
                return sell;
            }

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Add Services and ViewModel registrations here
            try
            {
                Console.WriteLine("RegisterTypes()");
                containerRegistry.RegisterSingleton<IMessageManagerService, MessageManagerService>();
                containerRegistry.RegisterSingleton<IMqttClientService, MqttClientService>();
                containerRegistry.RegisterSingleton<MqttClientHelper>();

                containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
                if (Debugger.IsAttached)
                {
                    containerRegistry.GetContainer().RegisterInstance(@"http://localhost:9527/", serviceKey: "webUrl");
                }
                else
                {
                   // containerRegistry.GetContainer().RegisterInstance(@"http://192.168.0.230:10010/", serviceKey: "webUrl");
                }
                // Services
                //// containerRegistry.RegisterSingleton<ISampleService, ISampleService>();
                containerRegistry.Register<ISysMenuClientService,SysMenuClientService>();
                containerRegistry.Register<GetStartedApp.RestSharp.IServices.ISysUserClientService, GetStartedApp.RestSharp.Services.SysUserClientService>();
                containerRegistry.RegisterSingleton<GetStartedApp.RestSharp.IServices.ISysRoleClientService, GetStartedApp.RestSharp.Services.SysRoleClientService>();
                
                //
                containerRegistry.RegisterSingleton<ILogger>(() => Log.Logger);
                containerRegistry.RegisterSingleton<IAppMapper, AppMapper>();
                containerRegistry.Register<IDialogWindow, DialogStyleView>(nameof(DialogStyleView));
                containerRegistry.RegisterInstance<ISiemensEvent>(new SiemensEvent());
                // Dialogs
                //// containerRegistry.RegisterDialog<MessageBoxView, MessageBoxViewModel>();
                //// containerRegistry.RegisterDialogWindow<CustomDialogWindow>(nameof(CustomDialogWindow));
                containerRegistry.RegisterDialog<SetUserDlg, SetUserDlgViewModel>();
                containerRegistry.RegisterDialog<SetVersionPrimaryDlg, SetVersionPrimaryDlgViewModel>();
                containerRegistry.RegisterDialog<SetVersionSecondDlg, SetVersionSecondDlgViewModel>();

                // ViewModels as Services
                containerRegistry.RegisterSingleton<LoginViewModel>();
                containerRegistry.RegisterSingleton<MainViewModel>();
                containerRegistry.RegisterSingleton<MainWindowViewModel>();
                
                // Views - Generic
                containerRegistry.RegisterForNavigation<LoginView,LoginViewModel>();
                containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
                containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
                containerRegistry.RegisterForNavigation<SideMenuView, SideMenuViewModel>();
                containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>();
                containerRegistry.RegisterForNavigation<UserView, UserViewModel>();
                containerRegistry.RegisterForNavigation<ProductVersion, ProductVersionViewModel>();
                containerRegistry.RegisterForNavigation<VersionAttribute, VersionAttributeViewModel>();
                containerRegistry.RegisterForNavigation<ProgramPackView, ProgramPackViewModel>();
                containerRegistry.RegisterForNavigation<Recipe, RecipeViewModel>();
                containerRegistry.RegisterForNavigation<ConnSiemens, ConnSiemensViewModel>();
                containerRegistry.RegisterForNavigation<EventSiemens, EventSiemensViewModel>();
                containerRegistry.RegisterForNavigation<ProcessRouteView, ProcessRouteViewModel>("ProcessRoute");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        }


        protected override IContainerExtension CreateContainerExtension()
        {
            var serviceCollection = new ServiceCollection();

            // SqlSugar
           // SqlSugarConfigure(serviceCollection);

            return new DryIocContainerExtension(new Container(CreateContainerRules())
               .WithDependencyInjectionAdapter(serviceCollection)
              );
        }

        protected override Rules CreateContainerRules()
        {
            return Rules.Default.WithConcreteTypeDynamicRegistrations(reuse: Reuse.Transient)
                                .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
                                .WithFuncAndLazyWithoutRegistration()
                                .WithTrackingDisposableTransients()
                                //.WithoutFastExpressionCompiler()
                                .WithFactorySelector(Rules.SelectLastRegisteredFactory());
        }

        //private void SqlSugarConfigure(IServiceCollection services)
        //{
        //    #region 

        //    var connectConfigList = new List<ConnectionConfig>();
        //    var connectionString = string.Empty;
        //    //
        //    //
        //    connectionString = "Data Source=localhost;Initial Catalog=avaloniadatabase;User ID=root;Password=Kstopa123?;Allow User Variables=True;max pool size=512 ";

        //    //if (Design.IsDesignMode)
        //    //{
        //    //    connectionString = "Data Source=localhost;Initial Catalog=avaloniadatabase;User ID=root;Password=Kstopa123?;Allow User Variables=True;max pool size=512 ";
        //    //}
        //    //else
        //    //{
        //    //    connectionString = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
        //    //}
        //    //
        //    connectConfigList.Add(new ConnectionConfig
        //    {
        //        ConnectionString = connectionString,
        //        DbType = DbType.MySql,
        //        IsAutoCloseConnection = true
        //    });
        //    services.AddSqlSugar(connectConfigList.ToArray()
        //        , db =>
        //        {
        //            //db.Aop.OnLogExecuting = (sql, pars) =>
        //            //{
        //            //    if (sql.StartsWith("SELECT"))
        //            //    {
        //            //        Console.ForegroundColor = ConsoleColor.Green;
        //            //    }
        //            //    if (sql.StartsWith("UPDATE") || sql.StartsWith("INSERT"))
        //            //    {
        //            //        Console.ForegroundColor = ConsoleColor.White;
        //            //    }
        //            //    if (sql.StartsWith("DELETE"))
        //            //    {
        //            //        Console.ForegroundColor = ConsoleColor.Blue;
        //            //    }
        //            //    //App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
        //            //    Console.WriteLine(sql + "\r\n\r\n" + SqlProfiler.ParameterFormat(sql, pars));
        //            //};


        //            //
        //            // db.Ado.CommandTimeOut = 30;

        //            // 
        //            db.GlobalFilter();
        //        });

        //    #endregion
        //}

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // Register modules
            //// moduleCatalog.AddModule<DummyModule.DummyModule1>();
        }


        /// <summary>
        /// ע��ȫ���쳣��������
        /// </summary>
        private void RegisterGlobalExceptionHandlers()
        {
            Dispatcher.UIThread.UnhandledException += Dispatcher_UnhandledException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Dispatcher_UnhandledException(object? sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, "");

            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CurrentDomain_UnhandledException(object? sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex, "");
            }

            
        }

        /// <summary>
        /// 
        /// </summary>
        private void TaskScheduler_UnobservedTaskException(object? sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            HandleException(e.Exception, "Taskδ�２쵽���쳣");

          
            e.SetObserved();
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleException(Exception ex, string exceptionType)
        {
            Log.Error(ex, $"{exceptionType}��{ex.Message}");
        }

        private static void InitializeLogging()
        {
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "log");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            var logFilePath = Path.Combine(logDirectory, "log.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() 
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) 
                .Enrich.FromLogContext() 
                .WriteTo.Console() 
                .WriteTo.File(
                    path: logFilePath,
                    rollingInterval: RollingInterval.Day, 
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    retainedFileCountLimit: 30, 
                    encoding: System.Text.Encoding.UTF8 
                )
                .CreateLogger();
        }

    }
}