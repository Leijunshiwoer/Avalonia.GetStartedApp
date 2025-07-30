using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Interface
{
    public interface IBeckhoffGlobalConfig<T1, T2, T3> where T1 : IEventIO where T2 : IBeckhoffCpuInfo where T3 : IEventInstance<T1>
    {
        string FileName { get; set; }//配置文件名
        DateTime FileSaveTime { get; set; }//保存时间
        string Versioin { get; set; }//文件版本
        string User { get; set; }

        List<T1> EapConfig { get; set; }
        List<T1> PlcConfig { get; set; }
        T2 CpuInfo { get; set; }
        List<T3> EventConfig { get; set; }
    }
}
