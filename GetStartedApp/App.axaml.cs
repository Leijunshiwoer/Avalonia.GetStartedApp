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
            // ע��ȫ���쳣�����¼�
            RegisterGlobalExceptionHandlers();
        }
        protected override AvaloniaObject CreateShell()
        {
            var messageManagerService = ContainerLocator.Container.Resolve<IMessageManagerService>();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // 创建主窗口但不立即显示
                var mainWindow = Container.Resolve<MainWindow>();
                messageManagerService.Initialize(mainWindow);
                
                // 显示登录窗口
                var loginWindow = Container.Resolve<Views.LoginView>();
                
                // 登录成功后显示主窗口并关闭登录窗口
                if (loginWindow.DataContext is ViewModels.LoginViewModel loginViewModel)
                {
                    loginViewModel.LoginSuccess += () =>
                    {
                        mainWindow.Show();
                        loginWindow.Close();
                    };
                }
                
                // 监听登录窗口关闭事件，如果未登录就关闭，则退出应用
                loginWindow.Closing += (sender, e) =>
                {
                    // 如果主窗口还没显示，说明用户关闭了登录窗口，退出应用
                    if (!mainWindow.IsVisible)
                    {
                        desktop.Shutdown();
                    }
                };
                
                loginWindow.Show();
                
                // 返回主窗口作为Shell（但初始不显示）
                return mainWindow;
            }
            else
            {
                var sell = Container.Resolve<MainView>();
                messageManagerService.Initialize(sell);
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
                
                // ע�� Serilog.ILogger ������������ģʽ��
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

                // Views - Generic
                containerRegistry.Register<Views.LoginView>();
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
        //    #region ����sqlsuagr

        //    var connectConfigList = new List<ConnectionConfig>();
        //    var connectionString = string.Empty;
        //    //���ݿ���Ŵ�0��ʼ,Ĭ�����ݿ�Ϊ0
        //    //��������ģʽ�£���ʹ��Ĭ�ϵ������ַ���
        //    connectionString = "Data Source=localhost;Initial Catalog=avaloniadatabase;User ID=root;Password=Kstopa123?;Allow User Variables=True;max pool size=512 ";

        //    //if (Design.IsDesignMode)
        //    //{
        //    //    connectionString = "Data Source=localhost;Initial Catalog=avaloniadatabase;User ID=root;Password=Kstopa123?;Allow User Variables=True;max pool size=512 ";
        //    //}
        //    //else
        //    //{
        //    //    connectionString = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
        //    //}
        //    //Ĭ�����ݿ�
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


        //            ////ִ�г�ʱʱ��
        //            // db.Ado.CommandTimeOut = 30;

        //            // ���ü�ɾ��ȫ�ֹ�����
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
            // ����UI�߳�δ������쳣
            Dispatcher.UIThread.UnhandledException += Dispatcher_UnhandledException;

            // ������UI�߳�δ������쳣
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // ���ʹ����Task�������Դ���Task��δ�۲��쳣
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        /// <summary>
        /// ����UI�߳�δ������쳣
        /// </summary>
        private void Dispatcher_UnhandledException(object? sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, "UI�߳�δ�����쳣");

            // ����e.Handled = true��ʾ�쳣�Ѵ�����Ӧ�ó�����Լ�������
            e.Handled = true;
        }

        /// <summary>
        /// ������UI�߳�δ������쳣
        /// </summary>
        private void CurrentDomain_UnhandledException(object? sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex, "��UI�߳�δ�����쳣");
            }

            // ���������쳣��e.IsTerminating����Ϊtrue����ʾӦ�ó��򼴽���ֹ
        }

        /// <summary>
        /// ����Taskδ�۲쵽���쳣
        /// </summary>
        private void TaskScheduler_UnobservedTaskException(object? sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            HandleException(e.Exception, "Taskδ�۲쵽���쳣");

            // ����쳣Ϊ�ѹ۲�
            e.SetObserved();
        }

        /// <summary>
        /// ͳһ���쳣�����߼�
        /// </summary>
        private void HandleException(Exception ex, string exceptionType)
        {
            // ʹ��Serilog��¼�쳣
            Log.Error(ex, $"{exceptionType}��{ex.Message}");

            // ��ѡ�����û���ʾ�Ѻ���Ϣ
            //Dispatcher.UIThread.Post(async () =>
            //{
            //    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            //    {
            //        await MessageBox.ShowAsync("Ӧ�ó�����������鿴��־��ȡ��ϸ��Ϣ��", "����", MessageBoxIcon.Error, MessageBoxButton.OK);
            //    }
            //});
        }

        private static void InitializeLogging()
        {
            // ��־�ļ���·������ǰ����Ŀ¼�µ�"log"�ļ���
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "log");
            // ȷ���ļ��д���
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // ����������־�ļ�����ʽ��log\20240722.txt��
            var logFilePath = Path.Combine(logDirectory, "log.txt");

            // ����Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // ��С��־����
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // ����Microsoft��ĵͼ�����־
                .Enrich.FromLogContext() // �������� enrichment
                .WriteTo.Console() // ͬʱ���������̨����ѡ��
                .WriteTo.File(
                    path: logFilePath,
                    rollingInterval: RollingInterval.Day, // �������
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}", // ��־��ʽ
                    retainedFileCountLimit: 30, // ����30�����־�ļ�
                    encoding: System.Text.Encoding.UTF8 // ����
                )
                .CreateLogger();
        }

    }
}