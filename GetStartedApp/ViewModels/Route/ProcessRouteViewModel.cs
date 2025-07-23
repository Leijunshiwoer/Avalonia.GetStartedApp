using GetStartedApp.AutoMapper;
using GetStartedApp.SqlSugar.IServices;
using System;
using System.Collections.Generic;
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

        #endregion

        #region 事件

        #endregion
    }
}
