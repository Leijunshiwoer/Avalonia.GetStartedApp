using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class VersionSecondDto : BaseDto
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
        private int _VersionPrimaryId;
        public int VersionPrimaryId
        {
            get { return _VersionPrimaryId; }
            set { SetProperty(ref _VersionPrimaryId, value); }
        }

        private VersionPrimaryDto _VersionPrimary;
        public VersionPrimaryDto VersionPrimary
        {
            get { return _VersionPrimary; }
            set { SetProperty(ref _VersionPrimary, value); }
        }
        private int _RouteId;
        public int RouteId
        {
            get { return _RouteId; }
            set { SetProperty(ref _RouteId, value); }
        }
        private VersionSecondDto _VersionSecond;
        public VersionSecondDto VersionSecond
        {
            get { return _VersionSecond; }
            set { SetProperty(ref _VersionSecond, value); }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        public override string ToString()
        {
            return $"{Code} ({Name})";
        }
    }
}
