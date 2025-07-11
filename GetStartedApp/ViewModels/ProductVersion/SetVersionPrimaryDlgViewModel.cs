using GetStartedApp.AutoMapper;
using GetStartedApp.Models;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Tables;
using Prism.Commands;
using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;
using DialogResult = Prism.Dialogs.DialogResult;

namespace GetStartedApp.ViewModels.ProductVersion
{
    public class SetVersionPrimaryDlgViewModel : ViewModelBase, IDialogAware
    {
        private readonly IBase_Version_Primary_Config_Service _version_Primary_Config_Service;
        private readonly IAppMapper _appMapper;

        public SetVersionPrimaryDlgViewModel(IBase_Version_Primary_Config_Service version_Primary_Config_Service,IAppMapper appMapper)
        {
            _version_Primary_Config_Service = version_Primary_Config_Service;
            _appMapper = appMapper;
            Title = "型号管理/主型号设置";
        }



        public DialogCloseListener RequestClose { get; }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
          
        }
        private bool _isAdd = false;
        public void OnDialogOpened(IDialogParameters parameters)
        {
            _isAdd = !parameters.Keys.Contains("Model");
            if (!_isAdd)
                VersionPrimary = parameters.GetValue<VersionPrimaryDto>("Model");
        }

        #region 属性
        private VersionPrimaryDto _VersionPrimary;
        public VersionPrimaryDto VersionPrimary
        {
            get { return _VersionPrimary ?? (_VersionPrimary = new VersionPrimaryDto()); }
            set { SetProperty(ref _VersionPrimary, value); }
        }
        #endregion

        #region 事件


        private DelegateCommand _SaveCmd;
        public DelegateCommand SaveCmd =>
            _SaveCmd ?? (_SaveCmd = new DelegateCommand(ExecuteSaveCmd));

        void ExecuteSaveCmd()
        {
            //是否为空
            if (string.IsNullOrEmpty(VersionPrimary.Code) || string.IsNullOrEmpty(VersionPrimary.Name))
            {
                MessageBox.ShowAsync("主型号代码或者主型号名称不能为空，请确认", "", MessageBoxIcon.Error);
                return;
            }
            //查看是否有重复项
            if (_version_Primary_Config_Service.IsExist(VersionPrimary.Code, VersionPrimary.Name, VersionPrimary.Id))
            {
                MessageBox.ShowAsync($"存在主型号代码[{VersionPrimary.Code}]或者主型号名称[{VersionPrimary.Name}]，请确认", "", MessageBoxIcon.Error);
                return;
            }
            var vp = _appMapper.Map<Base_Version_Primary_Config>(VersionPrimary);
            if (_isAdd) vp.Create(); else vp.Modify();
            _version_Primary_Config_Service.InsertOrUpdate(vp);

            //退出
            ButtonResult result = ButtonResult.OK;
            RequestClose.Invoke(new DialogResult(result));
        }

        private DelegateCommand _ExitCmd;
        public DelegateCommand ExitCmd =>
            _ExitCmd ?? (_ExitCmd = new DelegateCommand(ExecuteExitCmd));

        void ExecuteExitCmd()
        {
            //退出
            ButtonResult result = ButtonResult.Ignore;
            RequestClose.Invoke(new DialogResult(result));
        }

        #endregion
    }
}
