using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class VersionPrimaryDto : BaseDto
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
        private ObservableCollection<VersionSecondDto> _VersionSeconds;
        public ObservableCollection<VersionSecondDto> VersionSeconds
        {
            get { return _VersionSeconds; }
            set { SetProperty(ref _VersionSeconds, value); }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }
    }
}
