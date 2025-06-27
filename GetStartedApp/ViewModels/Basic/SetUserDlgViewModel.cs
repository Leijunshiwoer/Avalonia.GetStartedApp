using DryIoc;
using GetStartedApp.Core.Helpers;
using GetStartedApp.Models;
using GetStartedApp.SqlSugar.Globalvariable;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Services;
using GetStartedApp.SqlSugar.Tables;
using Prism.Commands;
using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;
using DialogResult = Prism.Dialogs.DialogResult;

namespace GetStartedApp.ViewModels.Basic
{
    public class SetUserDlgViewModel : ViewModelBase, IDialogAware
    {
        private List<SysRole> _roles;
        private readonly ISysRoleService _sysRoleService;
        private readonly ISysUserService _sysUserService;

        public SetUserDlgViewModel(ISysRoleService sysRoleService, ISysUserService sysUserService)
        {
            Title = "Set User";
            _sysRoleService = sysRoleService;
            _sysUserService = sysUserService;
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
            var userDto = parameters.GetValue<UserDto>("Model");
            _isAdd = userDto == null;
            if (_isAdd)
                _roles = _sysRoleService.GetRoleLessSort(UserInfo.User.Role.Sort);
            else
                _roles = _sysRoleService.GetRoleLessSortByUserId(userDto.Id, UserInfo.User.Role.Sort);
            Roles = _roles.Select(x => x.Name + "(" + x.Sort + ")").ToObservableConllection();


            if (!_isAdd)
            {
                //修改
                userDto.Password = "";//去除密码
                Role = userDto.Role.Name + "(" + userDto.Role.Sort + ")";
                UserDto = userDto;
            }

        }
        private UserDto _UserDto;
        public UserDto UserDto
        {
            get { return _UserDto ?? (_UserDto = new UserDto()); }
            set { SetProperty(ref _UserDto, value); }
        }

        private string _Role;
        public string Role
        {
            get { return _Role; }
            set { SetProperty(ref _Role, value); }
        }

        private ObservableCollection<string> _Roles;
        public ObservableCollection<string> Roles
        {
            get { return _Roles; }
            set { SetProperty(ref _Roles, value); }
        }

        private int _RoleIdx;
        public int RoleIdx
        {
            get { return _RoleIdx; }
            set { SetProperty(ref _RoleIdx, value); }
        }


        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand =>
            _SaveCommand ?? (_SaveCommand = new DelegateCommand(ExecuteSaveCommand));

        void ExecuteSaveCommand()
        {

            if (string.IsNullOrEmpty(UserDto.Name) || string.IsNullOrEmpty(UserDto.Password) || RoleIdx < 0 || string.IsNullOrEmpty(UserDto.JobNumber))
            {
                MessageBox.ShowAsync("请将信息填写完全", "", MessageBoxIcon.Error, MessageBoxButton.OK);
                return;
            }
            try
            {
                SysUser sysUser = new SysUser()
                {
                    Id = UserDto.Id,
                    Name = UserDto.Name,
                    Department = UserDto.Department,
                    JobNumber = UserDto.JobNumber,
                    Password = MD5Helper.MD5Encryp(UserDto.Password),
                    Remark = UserDto.Remark,
                    RoleId = _roles[RoleIdx].Id,

                    CreatedUserName = UserDto.CreatedUserName,
                    CreatedTime = UserDto.CreatedTime,
                    UpdatedUserName = UserDto.UpdatedUserName,
                    UpdatedTime = UserDto.UpdatedTime,
                };
                if (_isAdd) sysUser.Create(); else sysUser.Modify();

                if (_sysUserService.InserOrUpdateUser(sysUser) == 0)
                {
                    MessageBox.ShowAsync("数据库操作失败", "", MessageBoxIcon.Error, MessageBoxButton.OK);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.ShowAsync($"保存失败:{ex.Message}", "", MessageBoxIcon.Error, MessageBoxButton.OK);
                return;
            }

            //退出
            ButtonResult result = ButtonResult.OK;
            RequestClose.Invoke(new DialogResult(result));
        }

        private DelegateCommand _CancelCommand;

        public DelegateCommand CancelCommand =>
            _CancelCommand ?? (_CancelCommand = new DelegateCommand(ExecuteCancelCommand));

        void ExecuteCancelCommand()
        {
            
            //退出
            ButtonResult result = ButtonResult.Ignore;
            RequestClose.Invoke(new DialogResult(result));
        }
    }
}
