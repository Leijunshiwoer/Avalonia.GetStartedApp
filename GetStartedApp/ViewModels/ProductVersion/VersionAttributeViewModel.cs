using GetStartedApp.AutoMapper;
using GetStartedApp.Core.Helpers;
using GetStartedApp.Models;
using GetStartedApp.SqlSugar.IServices;
using Prism.Commands;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels.ProductVersion
{
    public class VersionAttributeViewModel:ViewModelBase
    {
        private readonly IAppMapper _appMapper;
        private readonly IBase_Version_Primary_Config_Service _version_Primary_Config_Service;
        private readonly IBase_Version_Attribute_Config_Service _version_Attribute_Config_Service;

        public VersionAttributeViewModel(IAppMapper appMapper,
            IBase_Version_Primary_Config_Service version_Primary_Config_Service,
            IBase_Version_Attribute_Config_Service version_Attribute_Config_Service)
        {
            _appMapper = appMapper;
            _version_Primary_Config_Service = version_Primary_Config_Service;
            _version_Attribute_Config_Service = version_Attribute_Config_Service;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //初始化型号Tree
            InitVersionTree();
           
        }



        #region 属性


        private ObservableCollection<VersionPrimaryDto> _VersionTree;

        public ObservableCollection<VersionPrimaryDto> VersionTree
        {
            get { return _VersionTree; }
            set { SetProperty(ref _VersionTree, value); }
        }


        private ObservableCollection<AttributeDto> _Attributes;

        public ObservableCollection<AttributeDto> Attributes
        {
            get { return _Attributes ?? (_Attributes = new ObservableCollection<AttributeDto>()); }
            set { SetProperty(ref _Attributes, value); }
        }

        private VersionSecondDto _selectedItem;
        public VersionSecondDto selectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    if (value != null)
                    {
                        InitAttribute(new List<int> { value.Id });
                    }
                }
            }
        }
        #endregion

        #region 方法
        private void InitVersionTree()
        {
            VersionTree = _appMapper.Map<List<VersionPrimaryDto>>(_version_Primary_Config_Service.GetVersionPrimayTree()).ToObservableConllection();
        }


        private void InitAttribute(List<int> secondIds)
        {
            //分页复位
            //PageIndex = 1;
            //获取当前次类型所有的属性
            var total = 0;
            Attributes = _appMapper.Map<List<AttributeDto>>(_version_Attribute_Config_Service.GetPageAttributeBySecondIds(secondIds, ref total, 1)).ToObservableConllection();
        }


        #endregion

        #region 事件


        private DelegateCommand<object> _AttributeModifyCmd;
        public DelegateCommand<object> AttributeModifyCmd =>
            _AttributeModifyCmd ?? (_AttributeModifyCmd = new DelegateCommand<object>(ExecuteAttributeModifyCmd, CanExecuteAttributeModifyCmd));

        void ExecuteAttributeModifyCmd(object parameter)
        {

        }

        bool CanExecuteAttributeModifyCmd(object parameter)
        {
            return true;
        }
        #endregion
    }
}
