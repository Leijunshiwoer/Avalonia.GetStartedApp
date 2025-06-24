using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class RoleDto : BaseDto
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private int _Sort;
        public int Sort
        {
            get { return _Sort; }
            set { SetProperty(ref _Sort, value); }

        }
    }
}
