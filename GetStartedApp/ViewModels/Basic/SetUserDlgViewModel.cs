using GetStartedApp.Models;
using Prism.Commands;
using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels.Basic
{
    public class SetUserDlgViewModel : ViewModelBase, IDialogAware
    {

        public SetUserDlgViewModel()
        {
            Title = "Set User";
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
