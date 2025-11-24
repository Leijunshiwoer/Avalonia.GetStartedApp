using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GetStartedApp.ViewModels;
using Prism.Ioc;

namespace GetStartedApp.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            
            // 通过Prism容器获取ViewModel（支持依赖注入）
            var container = ContainerLocator.Container;
            DataContext = container.Resolve<LoginViewModel>();
            
            // 订阅登录成功事件
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.LoginSuccess += OnLoginSuccess;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnLoginSuccess()
        {
            // 关闭登录窗口
            Close();
        }
    }
}

