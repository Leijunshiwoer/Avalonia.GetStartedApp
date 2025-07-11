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
    public class SetVersionSecondDlgViewModel: ViewModelBase, IDialogAware
    {
        private readonly IBase_Version_Second_Config_Service _version_Second_Config_Service;
        private readonly IBase_Version_Primary_Config_Service _version_Primary_Config_Service;
        private readonly IAppMapper _appMapper;

        public SetVersionSecondDlgViewModel(IBase_Version_Primary_Config_Service version_Primary_Config_Service,
            IBase_Version_Second_Config_Service version_Second_Config_Service,IAppMapper appMapper)
        {
            _version_Second_Config_Service = version_Second_Config_Service;
            _version_Primary_Config_Service = version_Primary_Config_Service;   
            _appMapper = appMapper;
            Title = "型号管理/次型号设置";
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
        private List<Base_Version_Primary_Config> _versionPrimarys;
        public void OnDialogOpened(IDialogParameters parameters)
        {

            //查询所有的工厂
            _versionPrimarys = _version_Primary_Config_Service.GetAll().ToList();
            PrimaryNameAndCodes = _versionPrimarys.Select(f =>
            {
                return string.Format($"{f.Name}({f.Code})");
            }).ToList();

            _isAdd = !parameters.Keys.Contains("Model");
            if (!_isAdd)
            {
                VersionSecond = parameters.GetValue<VersionSecondDto>("Model");
                //查看当前工厂在列表中idx
                PrimaryIdx = PrimaryNameAndCodes.IndexOf(string.Format($"{VersionSecond.VersionPrimary.Name}({VersionSecond.VersionPrimary.Code})"));
            }
        }

        private int _PrimaryIdx = -1;

        public int PrimaryIdx
        {
            get { return _PrimaryIdx; }
            set { SetProperty(ref _PrimaryIdx, value); }
        }

        private List<string> _PrimaryNameAndCodes;

        public List<string> PrimaryNameAndCodes
        {
            get { return _PrimaryNameAndCodes; }
            set { SetProperty(ref _PrimaryNameAndCodes, value); }
        }

        private VersionSecondDto _VersionSecond;

        public VersionSecondDto VersionSecond
        {
            get { return _VersionSecond ?? (_VersionSecond = new VersionSecondDto()); }
            set { SetProperty(ref _VersionSecond, value); }
        }


        private DelegateCommand _SaveCmd;
        public DelegateCommand SaveCmd =>
            _SaveCmd ?? (_SaveCmd = new DelegateCommand(ExecuteSaveCmd));

        void ExecuteSaveCmd()
        {
            //是否为空
            if (string.IsNullOrEmpty(VersionSecond.Code) || string.IsNullOrEmpty(VersionSecond.Name) || PrimaryIdx < 0)
            {
                MessageBox.ShowAsync("所属主型号，次型号代码，次型号名称不能为空，请确认");
                return;
            }
            var second = _appMapper.Map<Base_Version_Second_Config>(VersionSecond);
            second.VersionPrimaryId = _versionPrimarys[PrimaryIdx].Id;
            //查看是否有重复项
            if (_version_Second_Config_Service.IsExistByParentId(second.VersionPrimaryId, VersionSecond.Code, VersionSecond.Name, VersionSecond.Id))
            {
                MessageBox.ShowAsync($"存在次型号代码[{VersionSecond.Code}]或者次型号名称[{VersionSecond.Name}]，请确认");
                return;
            }

            if (_isAdd)
            {
                second.Create();
            }
            else second.Modify();
            var id = _version_Second_Config_Service.InsertOrUpdate(second);

            if (_isAdd)
            {
                //更新方法

                //CreatAttribute(id);
                //CreatRecipe(id);
            }

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
    }
}
