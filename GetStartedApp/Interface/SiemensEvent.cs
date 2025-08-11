using GetStartedApp.ViewModels.PLC;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Interface
{
    public class SiemensEvent : ISiemensEvent
    {
        EventSiemensViewModel _m;
        public void Instance(object o)
        {
            _m = o as EventSiemensViewModel;
        }
        public object DoEvent(object param)
        {
            if (_m != null)
            {
                PlcEventParamModel p = param as PlcEventParamModel;
                return _m.DoEvent(p);
            }
            return null;
        }
    }
}
