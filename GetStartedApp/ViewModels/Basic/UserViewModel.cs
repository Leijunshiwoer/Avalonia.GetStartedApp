using DryIoc;
using GetStartedApp.Models;
using GetStartedApp.UserControls;

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
    public class UserViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        public UserViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            AllUsers.Add(new UserDto { Name = "Alice", JobNumber = "001", Department = "HR", Role = new RoleDto { Name = "Manager" } });
            AllUsers.Add(new UserDto { Name = "Bob", JobNumber = "002", Department = "Finance", Role = new RoleDto { Name = "Analyst" } });
            AllUsers.Add(new UserDto { Name = "Charlie", JobNumber = "003", Department = "IT", Role = new RoleDto { Name = "Developer" } });
            AllUsers.Add(new UserDto { Name = "David", JobNumber = "004", Department = "Marketing", Role = new RoleDto { Name = "Executive" } });
            AllUsers.Add(new UserDto { Name = "Eve", JobNumber = "005", Department = "Sales", Role = new RoleDto { Name = "Representative" } });
            AllUsers.Add(new UserDto { Name = "Frank", JobNumber = "006", Department = "Operations", Role = new RoleDto { Name = "Coordinator" } });
            AllUsers.Add(new UserDto { Name = "Grace", JobNumber = "007", Department = "Support", Role = new RoleDto { Name = "Specialist" } });

            AllUsers.Add(new UserDto { Name = "Hank", JobNumber = "008", Department = "Legal", Role = new RoleDto { Name = "Counsel" } });
            AllUsers.Add(new UserDto { Name = "Ivy", JobNumber = "009", Department = "R&D", Role = new RoleDto { Name = "Scientist" } });


        }



        private ObservableCollection<UserDto> _Users;
        public ObservableCollection<UserDto> Users
        {
            get { return _Users ?? (_Users = new ObservableCollection<UserDto>()); }
            set { SetProperty(ref _Users, value); }
        }

        private ObservableCollection<UserDto> _allUsers;
        public ObservableCollection<UserDto> AllUsers
        {
            get => _allUsers ??= new ObservableCollection<UserDto>();
            set => SetProperty(ref _allUsers, value);
        }


        private int _itemsPerPage = 10;
        public int ItemsPerPage
        {
            get => _itemsPerPage;
            set
            {
                if (SetProperty(ref _itemsPerPage, value))
                    UpdatePagedUsers();
            }
        }

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                    UpdatePagedUsers();
            }
        }

        public int TotalItems => AllUsers.Count;

        private void UpdatePagedUsers()
        {
            Users = new ObservableCollection<UserDto>(
                AllUsers.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage)
            );
        }

        private DelegateCommand<PageChangedEventArgs> _PageChangedCommand;
        public DelegateCommand<PageChangedEventArgs> PageChangedCommand =>
            _PageChangedCommand ?? (_PageChangedCommand = new DelegateCommand<PageChangedEventArgs>(ExecutePageChangedCommand));

        void ExecutePageChangedCommand(PageChangedEventArgs parameter)
        {
            CurrentPage = parameter.CurrentPage;
            ItemsPerPage = parameter.ItemsPerPage;
            UpdatePagedUsers();
        }


        private DelegateCommand _RefreshCmd;

        public DelegateCommand RefreshCmd =>
            _RefreshCmd ?? (_RefreshCmd = new DelegateCommand(ExecuteRefreshCmd));

        void ExecuteRefreshCmd()
        {
            UpdatePagedUsers();
        }

        private DelegateCommand<object> _ModifyCmd;
        public DelegateCommand<object> ModifyCmd =>
            _ModifyCmd ?? (_ModifyCmd = new DelegateCommand<object>(ExecuteModifyCmd));

        void ExecuteModifyCmd(object parameter)
        {
            DialogParameters keyValuePairs = new DialogParameters();
            _dialogService.ShowDialog("SetUserDlg", r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    //刷新
                   
                }
            });
        }

        private DelegateCommand<object> _DeleteCmd;
      

        public DelegateCommand<object> DeleteCmd =>
            _DeleteCmd ?? (_DeleteCmd = new DelegateCommand<object>(ExecuteDeleteCmd));

        void ExecuteDeleteCmd(object parameter)
        {

        }
    }
}
