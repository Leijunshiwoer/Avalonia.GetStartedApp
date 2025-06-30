using AutoMapper;
using DryIoc;
using GetStartedApp.AutoMapper;
using GetStartedApp.Models;
using GetStartedApp.Services;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.UserControls;

using Prism.Commands;
using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;

namespace GetStartedApp.ViewModels.Basic
{
    public class UserViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly ISysUserService _sysUserService;
        private readonly IAppMapper _mapper;
        private readonly IToastService _toastService;

        public UserViewModel(
            IDialogService dialogService, 
            ISysUserService sysUserService,
            IToastService toastService,
             IAppMapper mapper)
        {
            _dialogService = dialogService;
            _sysUserService = sysUserService;
            _mapper = mapper;
            _toastService = toastService;
            // 初始化数据
            GetAllUsers();
            
        }


        #region 属性

       
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


        #endregion

        private void GetAllUsers()
        {
            var list = _sysUserService.GetUsers();
            var listDto = _mapper.Map<List<UserDto>>(list);
            AllUsers = new ObservableCollection<UserDto>(listDto);
           
        }

        private void UpdatePagedUsers()
        {
    
            Users = new ObservableCollection<UserDto>(
                AllUsers.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage)
            );
        }


        #region 事件

        
        private DelegateCommand<PageChangedEventArgs> _PageChangedCommand;
        public DelegateCommand<PageChangedEventArgs> PageChangedCommand =>
            _PageChangedCommand ?? (_PageChangedCommand = new DelegateCommand<PageChangedEventArgs>(ExecutePageChangedCommand));

        void ExecutePageChangedCommand(PageChangedEventArgs parameter)
        {
            CurrentPage = parameter.CurrentPage;
            ItemsPerPage = parameter.ItemsPerPage;
            UpdatePagedUsers();
        }


        private DelegateCommand _AddCmd;
        public DelegateCommand AddCmd =>
            _AddCmd ?? (_AddCmd = new DelegateCommand(ExecuteAddCmd));

        void ExecuteAddCmd()
        {
            DialogParameters keyValuePairs = new DialogParameters();
            _dialogService.ShowDialog("SetUserDlg", r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    //刷新
                    GetAllUsers();
                }
            });
        }


        private DelegateCommand _RefreshCmd;

        public DelegateCommand RefreshCmd =>
            _RefreshCmd ?? (_RefreshCmd = new DelegateCommand(ExecuteRefreshCmd));

        void ExecuteRefreshCmd()
        {
            GetAllUsers();
            _toastService.Show("刷新");
        }

        private DelegateCommand<object> _ModifyCmd;
        public DelegateCommand<object> ModifyCmd =>
            _ModifyCmd ?? (_ModifyCmd = new DelegateCommand<object>(ExecuteModifyCmd));

        void ExecuteModifyCmd(object parameter)
        {
            var model = parameter as UserDto;
            DialogParameters keyValuePairs = new DialogParameters();
            if (model == null)
            {
                MessageBox.ShowAsync("请选择一条记录,再编辑!","",MessageBoxIcon.Error,MessageBoxButton.OK);
                return;
            }
            keyValuePairs.Add("Model", model);

            _dialogService.ShowDialog("SetUserDlg",keyValuePairs, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    //刷新
                    GetAllUsers();
                }
            });
        }

        private DelegateCommand<object> _DeleteCmd;
      

        public DelegateCommand<object> DeleteCmd =>
            _DeleteCmd ?? (_DeleteCmd = new DelegateCommand<object>(ExecuteDeleteCmd));

        void ExecuteDeleteCmd(object parameter)
        {

        }

        #endregion
    }
}
