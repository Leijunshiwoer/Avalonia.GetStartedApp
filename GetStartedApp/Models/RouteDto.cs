using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class RouteDto : BaseDto
    {
        private string _Code;
        public string Code
        {
            get { return _Code; }
            set { SetProperty(ref _Code, value); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private int _VersionSecondId;
        public int VersionSecondId
        {
            get { return _VersionSecondId; }
            set { SetProperty(ref _VersionSecondId, value); }
        }

        private VersionSecondDto _VersionSecond;
        public VersionSecondDto VersionSecond
        {
            get { return _VersionSecond; }
            set { SetProperty(ref _VersionSecond, value); }
        }
    }
}
