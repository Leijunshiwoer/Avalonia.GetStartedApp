using CommunityToolkit.Mvvm.Input;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ursa.Controls;

namespace GetStartedApp.ViewModels;

public class SideMenuViewModel : ViewModelBase
{
    private readonly IRegionManager _regionManager;
    public IRelayCommand NavigationCommand { get; }
    public SideMenuViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;
        NavigationCommand = new RelayCommand(OnNavigate);
        CreateMenu();
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

        switch (SelectedMenuItem.Header)
        {
            case "首页":
                _regionManager.RequestNavigate("ContentRegion", "DashboardView");
                break;
            case "用户管理":
                _regionManager.RequestNavigate("ContentRegion", "UserView");
                break;
            case "权限管理":
                // TODO: Add navigation for 权限管理
                break;
            case "型号管理":
                // TODO: Add navigation for 型号管理
                break;
            case "型号属性":
                // TODO: Add navigation for 型号属性
                break;
            case "工艺路线":
                // TODO: Add navigation for 工艺路线
                break;
            case "工单任务":
                // TODO: Add navigation for 工单任务
                break;
            case "标签监控":
                // TODO: Add navigation for 标签监控
                break;
            case "产品配方":
                // TODO: Add navigation for 产品配方
                break;
            case "设备监控":
                // TODO: Add navigation for 设备监控
                break;
            case "实时报警":
                // TODO: Add navigation for 实时报警
                break;
            case "PLC管理":
                // TODO: Add navigation for PLC管理
                break;
            case "PLC事件":
                // TODO: Add navigation for PLC事件
                break;
            case "数据(历史)":
                // TODO: Add navigation for 数据(历史)
                break;
            case "数据(实时)":
                // TODO: Add navigation for 数据(实时)
                break;
            default:
                // TODO: Handle unknown menu
                break;
        }
    }

    private void CreateMenu()
    {
        MenuItems.Add(new MenuItem { Header = "首页", IconIndex = "mdi-home", NavigationCommand = NavigationCommand });
        MenuItems.Add(new MenuItem
        {
            Header = "基础数据",
            IconIndex = "mdi-file",
            Children = {
                new MenuItem{ Header="用户管理",IconIndex="mdi-account", NavigationCommand = NavigationCommand},
                new MenuItem { Header= "权限管理",IconIndex="mdi-shield-account", NavigationCommand = NavigationCommand },
                new MenuItem{Header="型号管理",IconIndex="mdi-car-electric", NavigationCommand = NavigationCommand},
                new MenuItem{ Header="型号属性",IconIndex="mdi-cog-outline", NavigationCommand = NavigationCommand },
                new MenuItem{Header="工艺路线",IconIndex="mdi-router", NavigationCommand = NavigationCommand}
            }
        });

        MenuItems.Add(new MenuItem
        {
            Header = "产品管理",
            IconIndex = "mdi-package-variant-closed-check",
            Children = {
            new MenuItem { Header = "工单任务", IconIndex = "mdi-format-list-numbered-rtl", NavigationCommand = NavigationCommand },
            new MenuItem { Header = "标签监控", IconIndex = "mdi-barcode", NavigationCommand = NavigationCommand },
            new MenuItem { Header = "产品配方", IconIndex = "mdi-format-list-checkbox", NavigationCommand = NavigationCommand }
        }
        });

        MenuItems.Add(new MenuItem
        {
            Header = "设备管理",
            IconIndex = "mdi-steam",
            Children = {
            new MenuItem { Header = "设备监控", IconIndex = "mdi-monitor-eye", NavigationCommand = NavigationCommand },
            new MenuItem { Header = "实时报警", IconIndex = "mdi-bell-alert", NavigationCommand = NavigationCommand }
        }
        });

        MenuItems.Add(new MenuItem
        {
            Header = "外设管理",
            IconIndex = "mdi-devices",
            Children = {
            new MenuItem { Header = "PLC管理", IconIndex = "mdi-microsoft-visual-studio", NavigationCommand = NavigationCommand },
            new MenuItem { Header = "PLC事件", IconIndex = "mdi-microsoft-visual-studio", NavigationCommand = NavigationCommand }
        }
        });

        MenuItems.Add(new MenuItem
        {
            Header = "数据管理",
            IconIndex = "mdi-database",
            Children = {
            new MenuItem { Header = "数据(历史)", IconIndex = "mdi-database-search-outline", NavigationCommand = NavigationCommand },
            new MenuItem { Header = "数据(实时)", IconIndex = "mdi-database-eye-outline", NavigationCommand = NavigationCommand }
        }
        });
    }
}

public class MenuItem
{
    public string? Header { get; set; }
    public string IconIndex { get; set; }
    public bool IsSeparator { get; set; }
    public ICommand NavigationCommand { get; set; }
    public ObservableCollection<MenuItem> Children { get; set; } = new ObservableCollection<MenuItem>();

    public IEnumerable<MenuItem> GetLeaves()
    {
        if (this.Children.Count == 0)
        {
            yield return this;
            yield break;
        }

        foreach (var child in Children)
        {
            var items = child.GetLeaves();
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }
}
