using GetStartedApp.AutoMapper;
using GetStartedApp.Core.Helpers;
using GetStartedApp.Models;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Services;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels.ProductVersion
{
    public class ProductVersionViewModel:ViewModelBase
    {

        private readonly IAppMapper _appMapper;
        private readonly IBase_Version_Primary_Config_Service _version_Primary_Config_Service;
        private readonly IBase_Version_Second_Config_Service _version_Second_Config_Service;

        public ProductVersionViewModel(IAppMapper appMapper,
            IBase_Version_Primary_Config_Service version_Primary_Config_Service,
            IBase_Version_Second_Config_Service version_Second_Config_Service)
        {
            _appMapper = appMapper;
            _version_Primary_Config_Service = version_Primary_Config_Service;
            _version_Second_Config_Service = version_Second_Config_Service;
        }


        public override void  OnNavigatedTo(NavigationContext navigationContext)
        {
            //初始化型号Tree
            InitVersionTree();
            InitVersionPrimay();
            InitVersionSecond();
        }
        #region 数据

        private ObservableCollection<VersionPrimaryDto> _VersionTree;

        public ObservableCollection<VersionPrimaryDto> VersionTree
        {
            get { return _VersionTree; }
            set { SetProperty(ref _VersionTree, value); }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 初始化型号Tree
        /// </summary>
        private void InitVersionTree()
        {
            VersionTree = _appMapper.Map<List<VersionPrimaryDto>>(_version_Primary_Config_Service.GetVersionPrimayTree()).ToObservableConllection();
            ExpandAll(VersionTree);
        }

        private void ExpandAll(IEnumerable<VersionPrimaryDto> nodes)
        {
            if (nodes == null) return;
            foreach (var node in nodes)
            {
                node.IsExpanded = true;
                if (node.VersionSeconds != null)
                {
                    foreach (var child in node.VersionSeconds)
                    {
                        child.IsExpanded = true;
                    }
                }
            }
        }

        private ObservableCollection<VersionPrimaryDto> _VersionPrimarys;

        public ObservableCollection<VersionPrimaryDto> VersionPrimarys
        {
            get { return _VersionPrimarys; }
            set { SetProperty(ref _VersionPrimarys, value); }
        }

        private void InitVersionPrimay()
        {
            VersionPrimarys = _appMapper.Map<List<VersionPrimaryDto>>(_version_Primary_Config_Service.GetAll()).ToObservableConllection();
        }

        private ObservableCollection<VersionSecondDto> _VersionSeconds;

        public ObservableCollection<VersionSecondDto> VersionSeconds
        {
            get { return _VersionSeconds ?? (_VersionSeconds = new ObservableCollection<VersionSecondDto>()); }
            set { SetProperty(ref _VersionSeconds, value); }
        }

        private void InitVersionSecond()
        {
            VersionSeconds = _appMapper.Map<List<VersionSecondDto>>(_version_Second_Config_Service.GetVersionSeconds()).ToObservableConllection();
        }
        #endregion
    }
}
