using GetStartedApp.Models;
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

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
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

    }
}
