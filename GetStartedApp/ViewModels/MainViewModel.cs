using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.Utils.Services;
using GetStartedApp.Views;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetStartedApp.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IMessageManagerService _messageManagerService;

        public MainViewModel(IRegionManager regionManager, IMessageManagerService messageManagerService)
        {

            _regionManager = regionManager;
            _messageManagerService = messageManagerService;
          
            //初始化操作
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(DashboardView));
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ConnSiemens));
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(EventSiemens));

        }
    }
}
