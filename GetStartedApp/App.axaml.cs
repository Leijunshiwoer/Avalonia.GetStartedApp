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
               * ��ǰ�û��ǹ���Ա��ʱ��ֱ������Ӧ�ó���
               * ������ǹ���Ա����ʹ��������������������ȷ��ʹ�ù���Ա�������
               */
              
                //��鵱ǰ�û��Ƿ�Ϊ����Ա
                   //ֱ������Ӧ�ó���
                    sell = Container.Resolve<MainWindow>();
            }
            else
            {
                //��ʾ�û������Ѿ�������
                if (MessageBox.ShowAsync("�Ѿ���һ����������У������ظ�������", "��ʾ", MessageBoxIcon.Warning, MessageBoxButton.OK).GetAwaiter().GetResult()== MessageBoxResult.OK)
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
            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();//����

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