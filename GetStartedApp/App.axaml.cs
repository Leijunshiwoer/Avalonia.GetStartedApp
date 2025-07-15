using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Example;
using GetStartedApp.AutoMapper;
using GetStartedApp.Services;
using GetStartedApp.SqlSugar.Extensions;
using GetStartedApp.ViewModels;
using GetStartedApp.ViewModels.Basic;
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
               * ��ǰ�û��ǹ���Ա��ʱ��ֱ������Ӧ�ó���
               * ������ǹ���Ա����ʹ��������������������ȷ��ʹ�ù���Ա�������
               */

                //��鵱ǰ�û��Ƿ�Ϊ����Ա
                //ֱ������Ӧ�ó���
                sell = Container.Resolve<MainWindow>();

                var messageManagerService = ContainerLocator.Container.Resolve<IMessageManagerService>();
                messageManagerService.Initialize(sell);
            }
            else
            {
                //��ʾ�û������Ѿ�������
                if (MessageBox.ShowAsync("�Ѿ���һ����������У������ظ�������", "��ʾ", MessageBoxIcon.Warning, MessageBoxButton.OK).GetAwaiter().GetResult() == MessageBoxResult.OK)
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
            #region ����sqlsuagr

            var connectConfigList = new List<ConnectionConfig>();
            var connectionString = string.Empty;
            //���ݿ���Ŵ�0��ʼ,Ĭ�����ݿ�Ϊ0
            //��������ģʽ�£���ʹ��Ĭ�ϵ������ַ���
            if (Design.IsDesignMode)
            {
                connectionString = "Data Source=localhost;Initial Catalog=F2210001;User ID=root;Password=Kstopa123?;Allow User Variables=True;max pool size=512 ";
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
            }
            //Ĭ�����ݿ�
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


                    ////ִ�г�ʱʱ��
                    // db.Ado.CommandTimeOut = 30;

                    // ���ü�ɾ��ȫ�ֹ�����
                    db.GlobalFilter();
                });

            #endregion
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // Register modules
            //// moduleCatalog.AddModule<DummyModule.DummyModule1>();
        }


    }
}