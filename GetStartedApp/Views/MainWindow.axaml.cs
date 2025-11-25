using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Threading;
using GetStartedApp.ViewModels;
using Prism.Ioc;
using System;
using System.Diagnostics;
using Ursa.Controls;

namespace GetStartedApp.Views
{
    public partial class MainWindow : Window
    {
        private LoginViewModel _loginViewModel;
        private MainViewModel _mainViewModel;

        // 声明视图实例，避免重复创建和状态丢失
        private LoginView _loginViewControl;
        private MainView _mainViewControl;

        public MainWindow()
        {
            InitializeComponent();

            // 通过容器获取 ViewModel（确保 DI 注册），没有时会考虑直接 new
            var container = ContainerLocator.Container;
            if (container != null && container.IsRegistered<LoginViewModel>())
            {
                _loginViewModel = container.Resolve<LoginViewModel>();
            }
            else
            {
                _loginViewModel = new LoginViewModel();
            }

            if (container != null && container.IsRegistered<MainViewModel>())
            {
                _mainViewModel = container.Resolve<MainViewModel>();
            }
            else
            {
                // 如果无法通过容器获取 MainViewModel，需要 IRegionManager 服务
                _mainViewModel = new MainViewModel(null!);
            }

            // 创建视图实例并绑定对应的 ViewModel
            _loginViewControl = new LoginView { DataContext = _loginViewModel };
            _mainViewControl = new MainView { DataContext = _mainViewModel };

            // 使用 LoginSuccess 事件，等待登录成功后从 ViewModel 内部通知（推荐）
            _loginViewModel.LoginSuccess += OnLoginSuccess;

            // 在关闭时解除事件绑定，防止内存泄漏
            this.Closed += (_, _) =>
            {
                _loginViewModel.LoginSuccess -= OnLoginSuccess;
            };

            // 初始显示登录界面（采用多页面模式）
            SwitchToLoginView();
        }

        private void OnLoginSuccess()
        {
            // 确保在 UI 线程中更新界面
            Dispatcher.UIThread.Post(() =>
            {
                SwitchToMainView();
            });
        }

        private void SwitchToLoginView()
        {
            ContentContainer.Content = _loginViewControl;
        }

        private void SwitchToMainView()
        {
            ContentContainer.Content = _mainViewControl;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // 重置登录状态，这里需要让 MainViewModel 提供 Reset 方法
            // 切换回登录界面并清空登录字段
            _loginViewModel.UserName = string.Empty;
            _loginViewModel.Password = string.Empty;
            _loginViewModel.ErrorMessage = string.Empty;

            // 切换到登录界面
            SwitchToLoginView();
        }
    }
}
    