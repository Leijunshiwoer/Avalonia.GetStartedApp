using CommunityToolkit.Mvvm.Input;
using GetStartedApp.Globalvariable;
using GetStartedApp.RestSharp.IServices;
using GetStartedApp.Utils.Services;
using Prism.Ioc;
using System;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly ISysUserClientService _userService;
        private readonly IMessageManagerService _messageService;

        private string _userName = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isLoading = false;

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public LoginViewModel()
        {
            // 通过容器获取服务
            var container = ContainerLocator.Container;
            _userService = container.Resolve<ISysUserClientService>();
            _messageService = container.Resolve<IMessageManagerService>();

#if DEBUG

            UserName = "developer";
            Password = "123";
#endif
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                ErrorMessage = "请输入用户名";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "请输入密码";
                return;
            }

            ErrorMessage = string.Empty;
            IsLoading = true;

            try
            {
                var result = await _userService.LoginAsync(UserName, Password);
                
                if (result.Status)
                {
                    // 登录成功，关闭登录窗口，显示主窗口
                    ErrorMessage = string.Empty;
                    
                    UserInfo.User= result.Data;
                    UserInfo.UserName= UserName;
                    // 触发登录成功事件，让App.axaml.cs处理窗口切换
                    LoginSuccess?.Invoke();
                }
                else
                {
                    ErrorMessage = result.Message ?? "登录失败";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"登录异常: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event Action? LoginSuccess;
    }
}

