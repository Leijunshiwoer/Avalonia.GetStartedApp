using GetStartedApp.AutoMapper;
using GetStartedApp.Core.Helpers;
using GetStartedApp.Models;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.Utils.UserControls;
using Prism.Commands;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GetStartedApp.ViewModels.Route
{
    public class ProcessRouteViewModel : ViewModelBase
    {
        private readonly IAppMapper appMapper;
        private readonly IBase_Route_Config_Service base_Route_Config_Service;
        private readonly IBase_Process_Config_Service base_Process_Config_Service;
        private readonly IBase_Process_Step_Config_Service base_Process_Step_Config_Service;
        private readonly IBase_Route_ProcessStep_Config_Service base_Route_ProcessStep_Config_Service;

        public ProcessRouteViewModel(IAppMapper appMapper, IBase_Route_Config_Service base_Route_Config_Service,
            IBase_Process_Config_Service base_Process_Config_Service,
            IBase_Process_Step_Config_Service base_Process_Step_Config_Service, IBase_Route_ProcessStep_Config_Service base_Route_ProcessStep_Config_Service)
        {
            this.appMapper = appMapper;
            this.base_Route_Config_Service = base_Route_Config_Service;
            this.base_Process_Config_Service = base_Process_Config_Service;
            this.base_Process_Step_Config_Service = base_Process_Step_Config_Service;
            this.base_Route_ProcessStep_Config_Service = base_Route_ProcessStep_Config_Service;

        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            InitRoutes();
            InitProcesses();
        }

        #region 工艺路线

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

        private int _itemsPerPage = 10;
        public int ItemsPerPage
        {
            get => _itemsPerPage;
            set
            {
                if (SetProperty(ref _itemsPerPage, value))
                    UpdatePaged();
            }
        }



        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                    UpdatePaged();
            }
        }

        public int TotalItems => AllRoutes.Count;


        #endregion

        #region 方法
        private void UpdatePaged()
        {
            Routes = new ObservableCollection<RouteDto>(
                 AllRoutes.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage)
             );
        }

        private void InitRoutes()
        {
            //获取所有的Route
            var total = 0;
            AllRoutes = appMapper.Map<List<RouteDto>>(base_Route_Config_Service.GetAllPage(ref total, 1)).ToObservableConllection();
            UpdatePaged();
        }
        #endregion

        #region 事件

        private DelegateCommand<PageChangedEventArgs> _PageChangedCommand;
        public DelegateCommand<PageChangedEventArgs> PageChangedCommand =>
            _PageChangedCommand ?? (_PageChangedCommand = new DelegateCommand<PageChangedEventArgs>(ExecutePageChangedCommand));

        void ExecutePageChangedCommand(PageChangedEventArgs parameter)
        {
            CurrentPage = parameter.CurrentPage;
            ItemsPerPage = parameter.ItemsPerPage;
            UpdatePaged();
        }


        private DelegateCommand<object> _AttributeModifyCmd;
        public DelegateCommand<object> AttributeModifyCmd =>
            _AttributeModifyCmd ?? (_AttributeModifyCmd = new DelegateCommand<object>(ExecuteAttributeModifyCmd));

        void ExecuteAttributeModifyCmd(object parameter)
        {

        }
        #endregion
        #endregion

        #region 工序
    
        #region 属性
        private ObservableCollection<ProcessDto> _Processs;

        public ObservableCollection<ProcessDto> Processs
        {
            get { return _Processs ?? (_Processs = new ObservableCollection<ProcessDto>()); }
            set { SetProperty(ref _Processs, value); }
        }

        private ObservableCollection<ProcessDto> _AllProcesss;

        public ObservableCollection<ProcessDto> AllProcesss
        {
            get { return _AllProcesss ?? (_AllProcesss = new ObservableCollection<ProcessDto>()); }
            set { SetProperty(ref _AllProcesss, value); }
        }


        private int _processItemsPerPage = 10;
        public int ProcessItemsPerPage
        {
            get => _processItemsPerPage;
            set
            {
                if (SetProperty(ref _processItemsPerPage, value))
                    UpdateProcessPaged();
            }
        }

        private int _processCurrentPage = 1;
        public int ProcessCurrentPage
        {
            get => _processCurrentPage;
            set
            {
                if (SetProperty(ref _processCurrentPage, value))
                    UpdateProcessPaged();
            }
        }

        public int ProcessTotalItems => AllProcesss.Count;

        #endregion

        #region 方法
        private void InitProcesses()
        {
            //获取所有的Process
            var total = 0;
            AllProcesss = appMapper.Map<List<ProcessDto>>(base_Process_Config_Service.GetAllPage(ref total, 1)).ToObservableConllection();

        }


        private void UpdateProcessPaged()
        {
            Processs = new ObservableCollection<ProcessDto>(
                 AllProcesss.Skip((ProcessCurrentPage - 1) * ProcessItemsPerPage).Take(ProcessItemsPerPage)
             );
        }

        #endregion

        #region 事件
        private DelegateCommand<PageChangedEventArgs> _ProcessPageChangedCommand;
        public DelegateCommand<PageChangedEventArgs> ProcessPageChangedCommand =>
            _ProcessPageChangedCommand ?? (_ProcessPageChangedCommand = new DelegateCommand<PageChangedEventArgs>(ExecuteProcessPageChangedCommand));

        void ExecuteProcessPageChangedCommand(PageChangedEventArgs parameter)
        {
            ProcessCurrentPage = parameter.CurrentPage;
            ProcessItemsPerPage = parameter.ItemsPerPage;
            UpdateProcessPaged();
        }

        private DelegateCommand<object> _ProModifyCmd;
        public DelegateCommand<object> ProModifyCmd =>
            _ProModifyCmd ?? (_ProModifyCmd = new DelegateCommand<object>(ExecuteProModifyCmd));

        void ExecuteProModifyCmd(object parameter)
        {

        }
        #endregion
        #endregion


        #region  工位
        #region 属性

        #endregion

        #region 方法

        #endregion

        #region 事件

        #endregion
        #endregion
    }
}
