using CommunityToolkit.Mvvm.Input;
using GetStartedApp.Models;
using GetStartedApp.RestSharp.IServices;
using GetStartedApp.RestSharp.Services;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Services;
using GetStartedApp.SqlSugar.Tables;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using Serilog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;


namespace GetStartedApp.ViewModels;

public class SideMenuViewModel : ViewModelBase
{
    private readonly IRegionManager _regionManager;
    private readonly ISysMenuClientService _sysMenuClientService;

    public IRelayCommand NavigationCommand { get; }
    public SideMenuViewModel(IRegionManager regionManager,ISysMenuClientService sysMenuClientService)
    {
        _regionManager = regionManager;
        _sysMenuClientService = sysMenuClientService;
        NavigationCommand = new RelayCommand(OnNavigate);
        _ = LoadMenuAsync();
    }

    private MenuItem? _SelectedMenuItem;
    public MenuItem? SelectedMenuItem
    {
        get { return _SelectedMenuItem; }
        set { SetProperty(ref _SelectedMenuItem, value); }
    }

    private ObservableCollection<MenuItem> _MenuItems;
    public ObservableCollection<MenuItem> MenuItems
    {
        get { return _MenuItems ?? (_MenuItems = new ObservableCollection<MenuItem>()); }
        set { SetProperty(ref _MenuItems, value); }
    }

    private void OnNavigate()
    {
        if (SelectedMenuItem == null || string.IsNullOrEmpty(SelectedMenuItem.Header))
            return;
        _regionManager.RequestNavigate(RegionNames.ContentRegion, SelectedMenuItem.Navigate);
    }

    private async Task LoadMenuAsync()
    {
        var MenusDto = await _sysMenuClientService.GetMenuTreeAsync();
        var list = MenusDto.Data;
        MenuItems = new ObservableCollection<MenuItem>(list.Select(ToMenuItem));
    }

    private MenuItem ToMenuItem(MenuDto sysMenu)
    {
        return new MenuItem
        {
            Header = sysMenu.Name,
            IconIndex = sysMenu.Icon,
            Navigate= sysMenu.Navigate,
            NavigationCommand = NavigationCommand,
            Children = new ObservableCollection<MenuItem>(sysMenu.Menus?.Select(ToMenuItem) ?? Enumerable.Empty<MenuItem>())
        };
    }

}