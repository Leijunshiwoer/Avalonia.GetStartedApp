using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GetStartedApp.Services;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Services;
using GetStartedApp.Views;
using Prism.Commands;
using Prism.Navigation.Regions;
using System.Collections.ObjectModel;
using Ursa.Controls;

namespace GetStartedApp.ViewModels;

/// <summary>
/// This is our MainViewModel in which we define the ViewModel logic to interact between our View and the TodoItems
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IRegionManager _regionManager;
    private readonly ISysUserService _sysUserService;
    private readonly IMessageManagerService _messageManagerService;

    public MainWindowViewModel(IRegionManager regionManager, ISysUserService sysUserService, IMessageManagerService messageManagerService)
    {

        _regionManager = regionManager;
        _sysUserService = sysUserService;
        _messageManagerService = messageManagerService;
        Title = "Sample Prism.Avalonia MVVM!";
        IsVisible01 = false;
        IsVisible02 = true;
        //初始化操作
        _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(DashboardView));
    }

    // -- Adding new Items --

    private string _User;
    public string User
    {
        get { return _User; }
        set { SetProperty(ref _User, value); }
    }


    private string _Password;
    public string Password
    {
        get { return _Password; }
        set { SetProperty(ref _Password, value); }
    }

    private string _Tips;
    public string Tips
    {
        get { return _Tips; }
        set { SetProperty(ref _Tips, value); }
    }
    private bool _IsVisible01;
    public bool IsVisible01
    {
        get { return _IsVisible01; }
        set { SetProperty(ref _IsVisible01, value); }
    }
    private bool _IsVisible02;
    public bool IsVisible02
    {
        get { return _IsVisible02; }
        set { SetProperty(ref _IsVisible02, value); }
    }
    private DelegateCommand _SubmitCmd;
    public DelegateCommand SubmitCmd =>
        _SubmitCmd ?? (_SubmitCmd = new DelegateCommand(ExecuteSubmitCmd));

    void ExecuteSubmitCmd()
    {

        if (string.IsNullOrEmpty(User)||string.IsNullOrEmpty(Password))
        {
            Tips = "用户或密码不能为空!";
            return;
        }

        var b=_sysUserService.Login(User, Password);

        if (b)
        {
            IsVisible02 = false;
            IsVisible01 = true;
            _messageManagerService.Show("登录成功!");
        }
        else
        {
            Tips = "用户名或密码错误!";
           // ToastManager.Show(new Toast("用户名或密码错误!"));
            return;
        }
    }


}