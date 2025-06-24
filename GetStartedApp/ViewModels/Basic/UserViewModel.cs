using GetStartedApp.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels.Basic
{
    public class UserViewModel:ViewModelBase
    {
        public UserViewModel()
        {
         
        }

        private ObservableCollection<UserDto> _Users;
        public ObservableCollection<UserDto> Users
        {
            get { return _Users ?? (_Users = new ObservableCollection<UserDto>() { new UserDto()
            {Id=1,Name="张三" } }); }
            set { SetProperty(ref _Users, value); }
        }


        private DelegateCommand _RefreshCmd;
        public DelegateCommand RefreshCmd =>
            _RefreshCmd ?? (_RefreshCmd = new DelegateCommand(ExecuteRefreshCmd));

        void ExecuteRefreshCmd()
        {
         
        }
    }
}
