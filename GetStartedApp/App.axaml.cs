using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using GetStartedApp.ViewModels;
using GetStartedApp.ViewModels.Basic;
using GetStartedApp.Views;
using Prism.Dialogs;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ursa.Controls;



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
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    {
                        desktop.Shutdown();
                    }
                    return;
                }
            });

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
            }
            else
            {
                //提示用户程序已经在运行
                if (MessageBox.ShowAsync("已经有一个软件运行中，请勿重复开启！", "提示", MessageBoxIcon.Warning, MessageBoxButton.OK).GetAwaiter().GetResult()== MessageBoxResult.OK)
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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Add Services and ViewModel registrations here

            Console.WriteLine("RegisterTypes()");

            // Services
            //// containerRegistry.RegisterSingleton<ISampleService, ISampleService>();
            ///

            // Dialogs
            //// containerRegistry.RegisterDialog<MessageBoxView, MessageBoxViewModel>();
            //// containerRegistry.RegisterDialogWindow<CustomDialogWindow>(nameof(CustomDialogWindow));
            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();//登入

            // Views - Generic
            containerRegistry.RegisterForNavigation<MainWindow,MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<SideMenuView,SideMenuViewModel>();

            // Views - Region Navigation
             containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>();
            containerRegistry.RegisterForNavigation<UserView, UserViewModel>();

          

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // Register modules
            //// moduleCatalog.AddModule<DummyModule.DummyModule1>();
        }


    }
}