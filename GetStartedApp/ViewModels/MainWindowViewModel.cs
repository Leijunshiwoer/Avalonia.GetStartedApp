using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Services;
using GetStartedApp.Utils.Services;
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
    public MainWindowViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;
    }
}