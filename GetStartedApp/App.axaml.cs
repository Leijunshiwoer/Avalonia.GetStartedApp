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
using GetStartedApp.SqlSugar.Extensions;
using GetStartedApp.Utils.Services;
using GetStartedApp.ViewModels;
using GetStartedApp.ViewModels.Basic;
using GetStartedApp.ViewModels.PLC;
using GetStartedApp.ViewModels.Product;
using GetStartedApp.ViewModels.ProductVersion;
using GetStartedApp.ViewModels.ProgramPack;
using GetStartedApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Prism.Container.DryIoc;
using Prism.Dialogs;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private static Mutex mutex;
        protected override AvaloniaObject CreateShell()
        {
            Console.WriteLine("CreateShell()");
            MainWindow sell = null;
            mutex = new Mutex(true, "GetStartApp");
            if (mutex.WaitOne(0, false))
            {
                /**
               * 当前用户是管理员的时候，直接启动应用程序
               * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行
               */

                //检查当前用户是否为管理员
                //直接启动应用程序
                sell = Container.Resolve<MainWindow>();

                var messageManagerService = ContainerLocator.Container.Resolve<IMessageManagerService>();
                messageManagerService.Initialize(sell);
            }
            else
            {
                //提示用户程序已经在运行
                if (MessageBox.ShowAsync("已经有一个软件运行中，请勿重复开启！", "提示", MessageBoxIcon.Warning, MessageBoxButton.OK).GetAwaiter().GetResult() == MessageBoxResult.OK)
                {
                    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    {
                        desktop.Shutdown();
                    }
                }
            }
            return sell;
            //return Container.Resolve<MainWindow>();
        }

        // App.xaml.cs
        public override void OnFrameworkInitializationCompleted()
        {
            // 注册全局异常处理事件
             RegisterGlobalExceptionHandlers();
            base.OnFrameworkInitializationCompleted();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Add Services and ViewModel registrations here

            Console.WriteLine("RegisterTypes()");
            containerRegistry.RegisterSingleton<IMessageManagerService, MessageManagerService>();
            // Services
            //// containerRegistry.RegisterSingleton<ISampleService, ISampleService>();
            ///

            containerRegistry.RegisterSingleton<IAppMapper, AppMapper>();

            containerRegistry.Register<IDialogWindow, DialogStyleView>(nameof(DialogStyleView));
            // Dialogs
            //// containerRegistry.RegisterDialog<MessageBoxView, MessageBoxViewModel>();
            //// containerRegistry.RegisterDialogWindow<CustomDialogWindow>(nameof(CustomDialogWindow));
            containerRegistry.RegisterDialog<SetUserDlg, SetUserDlgViewModel>();
            containerRegistry.RegisterDialog<SetVersionPrimaryDlg, SetVersionPrimaryDlgViewModel>();
            containerRegistry.RegisterDialog<SetVersionSecondDlg, SetVersionSecondDlgViewModel>();

            // Views - Generic
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<SideMenuView, SideMenuViewModel>();
            containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>();
            containerRegistry.RegisterForNavigation<UserView, UserViewModel>();
            containerRegistry.RegisterForNavigation<ProductVersion, ProductVersionViewModel>();
            containerRegistry.RegisterForNavigation<VersionAttribute, VersionAttributeViewModel>();
            containerRegistry.RegisterForNavigation<ProgramPackView, ProgramPackViewModel>();
            containerRegistry.RegisterForNavigation<Recipe, RecipeViewModel>();
            containerRegistry.RegisterForNavigation <ConnSiemens, ConnSiemensViewModel>();
        }


        protected override IContainerExtension CreateContainerExtension()
        {
            var serviceCollection = new ServiceCollection();

            // SqlSugar
            SqlSugarConfigure(serviceCollection);

            return new DryIocContainerExtension(new Container(CreateContainerRules())
               .WithDependencyInjectionAdapter(serviceCollection)
              );
        }

        private void SqlSugarConfigure(IServiceCollection services)
        {
            #region 配置sqlsuagr

            var connectConfigList = new List<ConnectionConfig>();
            var connectionString = string.Empty;
            //数据库序号从0开始,默认数据库为0
            //如果是设计模式下，则使用默认的连接字符串
            if (Design.IsDesignMode)
            {
                connectionString = "Data Source=localhost;Initial Catalog=F2210001;User ID=root;Password=Kstopa123?;Allow User Variables=True;max pool size=512 ";
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
            }
            //默认数据库
            connectConfigList.Add(new ConnectionConfig
            {
                ConnectionString = connectionString,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true
            });
            services.AddSqlSugar(connectConfigList.ToArray()
                , db =>
                {
                    //db.Aop.OnLogExecuting = (sql, pars) =>
                    //{
                    //    if (sql.StartsWith("SELECT"))
                    //    {
                    //        Console.ForegroundColor = ConsoleColor.Green;
                    //    }
                    //    if (sql.StartsWith("UPDATE") || sql.StartsWith("INSERT"))
                    //    {
                    //        Console.ForegroundColor = ConsoleColor.White;
                    //    }
                    //    if (sql.StartsWith("DELETE"))
                    //    {
                    //        Console.ForegroundColor = ConsoleColor.Blue;
                    //    }
                    //    //App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                    //    Console.WriteLine(sql + "\r\n\r\n" + SqlProfiler.ParameterFormat(sql, pars));
                    //};


                    ////执行超时时间
                    // db.Ado.CommandTimeOut = 30;

                    // 配置加删除全局过滤器
                    db.GlobalFilter();
                });

            #endregion
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // Register modules
            //// moduleCatalog.AddModule<DummyModule.DummyModule1>();
        }


        /// <summary>
        /// 注册全局异常处理程序
        /// </summary>
        private void RegisterGlobalExceptionHandlers()
        {
            // 处理UI线程未捕获的异常
            Dispatcher.UIThread.UnhandledException += Dispatcher_UnhandledException;

            // 处理非UI线程未捕获的异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // 如果使用了Task，还可以处理Task的未观察异常
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        /// <summary>
        /// 处理UI线程未捕获的异常
        /// </summary>
        private void Dispatcher_UnhandledException(object? sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, "UI线程未处理异常");

            // 设置e.Handled = true表示异常已处理，应用程序可以继续运行
            e.Handled = true;
        }

        /// <summary>
        /// 处理非UI线程未捕获的异常
        /// </summary>
        private void CurrentDomain_UnhandledException(object? sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex, "非UI线程未处理异常");
            }

            // 对于严重异常，e.IsTerminating可能为true，表示应用程序即将终止
        }

        /// <summary>
        /// 处理Task未观察到的异常
        /// </summary>
        private void TaskScheduler_UnobservedTaskException(object? sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            HandleException(e.Exception, "Task未观察到的异常");

            // 标记异常为已观察
            e.SetObserved();
        }

        /// <summary>
        /// 统一的异常处理逻辑
        /// </summary>
        private void HandleException(Exception ex, string exceptionType)
        {
            // 1. 记录异常信息到日志
            var errorMessage = $"{exceptionType}：{ex.Message}\n堆栈跟踪：{ex.StackTrace}";
            Console.WriteLine(errorMessage);

            // 这里可以添加日志框架的代码，如Serilog、NLog等
            // Logger.Error(ex, exceptionType);

            // 2. 向用户显示友好的错误信息
           // ShowErrorMessage(ex, exceptionType);
        }

        /// <summary>
        /// 显示错误信息给用户
        /// </summary>
        //private void ShowErrorMessage(Exception ex, string exceptionType)
        //{
        //    // 确保在UI线程上显示消息框
        //    Dispatcher.UIThread.Post(async () =>
        //    {
        //        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        //        {
        //            var messageBox = new MessageBox.Avalonia.MessageBoxManager();
        //            var result = await messageBox.ShowMessageBox(
        //                desktop.MainWindow,
        //                "应用程序错误",
        //                $"{exceptionType}\n\n错误信息：{ex.Message}\n\n是否关闭应用程序？",
        //                MessageBox.Avalonia.Enums.ButtonEnum.YesNo,
        //                MessageBox.Avalonia.Enums.Icon.Error
        //            );

        //            if (result == MessageBox.Avalonia.Enums.ButtonResult.Yes)
        //            {
        //                desktop.Shutdown(1);
        //            }
        //        }
        //    });
        //}

    }
}