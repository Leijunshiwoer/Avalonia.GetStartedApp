using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class BaseDto : BindableBase
    {
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }
        private DateTime? _CreatedTime;
        public DateTime? CreatedTime
        {
            get { return _CreatedTime; }
            set { SetProperty(ref _CreatedTime, value); }
        }

        private DateTime? _UpdatedTime;
        public DateTime? UpdatedTime
        {
            get { return _UpdatedTime; }
            set { SetProperty(ref _UpdatedTime, value); }
        }

        private string _CreatedUserName;
        public string CreatedUserName
        {
            get { return _CreatedUserName; }
            set { SetProperty(ref _CreatedUserName, value); }
        }

        private string _UpdatedUserName;
        public string UpdatedUserName
        {
            get { return _UpdatedUserName; }
            set { SetProperty(ref _UpdatedUserName, value); }
        }

        private string _IsDeleted;
        public string IsDeleted
        {
            get { return _IsDeleted; }
            set { SetProperty(ref _IsDeleted, value); }
        }

        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { SetProperty(ref _Remark, value); }
        }
    }
}
