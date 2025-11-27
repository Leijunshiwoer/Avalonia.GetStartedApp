using GetStartedApp.Core.Helpers;
using GetStartedApp.Globalvariable;
using GetStartedApp.Models;
using GetStartedApp.RestSharp;
using GetStartedApp.RestSharp.IServices;
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
        private List<RoleDto> _roles;
        private readonly ISysRoleClientService _sysRoleService;
        private readonly ISysUserClientService _sysUserService;

        public SetUserDlgViewModel(ISysRoleClientService sysRoleService, ISysUserClientService sysUserService)
        {
            Title = "用户设置";
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
        public async void OnDialogOpened(IDialogParameters parameters)
        {
            var userDto = parameters.GetValue<UserDto>("Model");
            _isAdd = userDto == null;
            
            try
            {
                if (_isAdd)
                {
                    var response = await _sysRoleService.GetRoleLessSortAsync(UserInfo.User.Role.Sort);
                    if (response.Status)
                    {
                        _roles = response.Data;
                    }
                }
                else
                {
                    var response = await _sysRoleService.GetRoleLessSortAsync( UserInfo.User.Role.Sort);
                    if (response.Status)
                    {
                        _roles = response.Data;
                    }
                }
                
                Roles = _roles.Select(x => x.Name + "(" + x.Sort + ")").ToObservableConllection();

                if (!_isAdd)
                {
                    //修改
                    userDto.Password = "";//去除密码
                    Role = userDto.Role.Name + "(" + userDto.Role.Sort + ")";
                    UserDto = userDto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.ShowAsync($"获取角色数据失败:{ex.Message}", "", MessageBoxIcon.Error, MessageBoxButton.OK);
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

        async void ExecuteSaveCommand()
        {
            if (string.IsNullOrEmpty(UserDto.Name) || RoleIdx < 0 || string.IsNullOrEmpty(UserDto.JobNumber))
            {
                MessageBox.ShowAsync("请将信息填写完全", "", MessageBoxIcon.Error, MessageBoxButton.OK);
                return;
            }
            
            // 更新操作时密码可以为空
            if (_isAdd && string.IsNullOrEmpty(UserDto.Password))
            {
                MessageBox.ShowAsync("请输入密码", "", MessageBoxIcon.Error, MessageBoxButton.OK);
                return;
            }
            
            try
            {
                var user = new UserDto
                {
                    Id = UserDto.Id,
                    Name = UserDto.Name,
                    Department = UserDto.Department,
                    JobNumber = UserDto.JobNumber,
                    Password = string.IsNullOrEmpty(UserDto.Password) ? null : MD5Helper.MD5Encryp(UserDto.Password),
                    Remark = UserDto.Remark,
                    RoleId = _roles[RoleIdx].Id,
                  
                };
                if (_isAdd) user.Create();
                else user.Modify();

                ApiResponse<UserDto> response;
                if (_isAdd)
                {
                    response = await _sysUserService.CreateUserAsync(user);
                }
                else
                {
                    response = await _sysUserService.UpdateUserAsync(user.Id, user);
                }
                
                if (!response.Status)
                {
                   await MessageBox.ShowAsync($"保存失败:{response.Message}", "", MessageBoxIcon.Error, MessageBoxButton.OK);
                    return;
                }
            }
            catch (Exception ex)
            {
                await MessageBox.ShowAsync($"保存失败:{ex.Message}", "", MessageBoxIcon.Error, MessageBoxButton.OK);
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
