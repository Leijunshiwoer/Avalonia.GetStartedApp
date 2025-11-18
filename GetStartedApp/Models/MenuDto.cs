using GetStartedApp.SqlSugar.Tables;
using Prism.Mvvm;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetStartedApp.Models
{
    public class MenuDto:BindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        private string _Navigate;
        public string Navigate
        {
            get { return _Navigate; }
            set { SetProperty(ref _Navigate, value); }
        }
        private string _Icon;
        public string Icon
        {
            get { return _Icon; }
            set { SetProperty(ref _Icon, value); }
        }
        private int _Sort;
        public int Sort
        {
            get { return _Sort; }
            set { SetProperty(ref _Sort, value); }
        }
        private int _ParentId;
        public int ParentId
        {
            get { return _ParentId; }
            set { SetProperty(ref _ParentId, value); }
        }
        private ICollection<MenuDto> _Menus;
        public ICollection<MenuDto> Menus
        {
            get { return _Menus; }
            set { SetProperty(ref _Menus, value); }
        }
    }
}
