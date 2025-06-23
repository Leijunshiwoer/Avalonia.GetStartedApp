using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GetStartedApp.Views;
using Prism.Commands;
using Prism.Navigation.Regions;
using System.Collections.ObjectModel;

namespace GetStartedApp.ViewModels;

/// <summary>
/// This is our MainViewModel in which we define the ViewModel logic to interact between our View and the TodoItems
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IRegionManager _regionManager;

    public MainWindowViewModel(IRegionManager regionManager)
    {

        _regionManager = regionManager;

     
        Title = "Sample Prism.Avalonia MVVM!";
        IsVisible01 = false;
        IsVisible02 = true;
        //初始化操作
        _regionManager.RegisterViewWithRegion("ContentRegion", typeof(DashboardView));
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
        IsVisible02 = false;
        IsVisible01 = true;
    }
}