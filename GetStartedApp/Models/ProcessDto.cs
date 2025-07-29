using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class ProcessDto : BaseDto
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

        private int _Index;
        public int Index
        {
            get { return _Index; }
            set { SetProperty(ref _Index, value); }
        }
        private int _RouteId;
        public int RouteId
        {
            get { return _RouteId; }
            set { SetProperty(ref _RouteId, value); }
        }
        private RouteDto _Route;
        public RouteDto Route
        {
            get { return _Route; }
            set { SetProperty(ref _Route, value); }
        }
        private ObservableCollection<ProcessStepDto> _Steps;
        public ObservableCollection<ProcessStepDto> Steps
        {
            get { return _Steps; }
            set { SetProperty(ref _Steps, value); }
        }
    }
}
