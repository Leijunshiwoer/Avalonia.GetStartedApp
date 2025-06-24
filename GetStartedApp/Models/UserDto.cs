using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class UserDto : BaseDto
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }

        private string _JobNumber;
        public string JobNumber
        {
            get { return _JobNumber; }
            set { SetProperty(ref _JobNumber, value); }
        }

        private string _Department;
        public string Department
        {
            get { return _Department; }
            set { SetProperty(ref _Department, value); }
        }

        private RoleDto _Role;
        public RoleDto Role
        {
            get { return _Role; }
            set { SetProperty(ref _Role, value); }
        }
    }
}
