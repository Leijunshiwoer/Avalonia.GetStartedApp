using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GetStartedApp.Views;
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

        //初始化操作
        _regionManager.RegisterViewWithRegion("ContentRegion", typeof(DashboardView));
    }

    // -- Adding new Items --

    
}