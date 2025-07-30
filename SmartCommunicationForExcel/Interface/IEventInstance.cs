using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Interface
{
    public interface IEventInstance<T> where T : IEventIO
    {
        string EventClass { get; set; }//事件分类
       
        List<T> ListInput { get; set; }//事件输入列表
        List<T> ListOutput { get; set; }//事件输出列表
    }
}
