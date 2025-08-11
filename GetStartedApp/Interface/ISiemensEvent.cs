using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Interface
{
    public interface ISiemensEvent
    {
        object DoEvent(object param);

        void Instance(object obj);

    }
}
