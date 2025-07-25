using GetStartedApp.AutoMapper;
using GetStartedApp.Models;
using GetStartedApp.SqlSugar.IServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels.Route
{
    public class ProcessRouteViewModel:ViewModelBase
    {
        private readonly IAppMapper appMapper;
        private readonly IBase_Route_Config_Service base_Route_Config_Service;

        public ProcessRouteViewModel(IAppMapper appMapper,IBase_Route_Config_Service base_Route_Config_Service)
        {
            this.appMapper = appMapper;
            this.base_Route_Config_Service= base_Route_Config_Service;
        }



        #region 属性
        private ObservableCollection<RouteDto> _Routes;

        public ObservableCollection<RouteDto> Routes
        {
            get { return _Routes ?? (_Routes = new ObservableCollection<RouteDto>()); }
            set { SetProperty(ref _Routes, value); }
        }


        private ObservableCollection<RouteDto> _AllRoutes;

        public ObservableCollection<RouteDto> AllRoutes
        {
            get { return _AllRoutes ?? (_AllRoutes = new ObservableCollection<RouteDto>()); }
            set { SetProperty(ref _AllRoutes, value); }
        }



        #endregion

        #region 事件

        #endregion
    }
}
