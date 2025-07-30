using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Interface
{
    public interface ISiemensCpuInfo
    {
        string Name { get; set; }//CPU名称
        CpuType CpuType { get; }//CPU类型
        string Mark { get; set; }//CPU备注
        string IP { get; set; }//IP地址
        short Port { get; }//端口号
        byte Rack { get; set; }//导轨号
        byte Slot { get; set; }//模块槽

        int CycleTime { get; set; }//扫描周期

        short EapConfigBeginAddress { get; set; }//Eap 配置区起始地址
        short EapConfigBeginOffset { get; set; }//Eap 配置区起始偏移

        short EapEventBeginAddress { get; set; }//Eap 事件区起始地址
        short EapEventBeginOffset { get; set; }//Eap 事件区起始偏移

        short PlcConfigBeginAddress { get; set; }//Plc 配置区起始地址
        short PlcConfigBeginOffset { get; set; }//Plc 配置区起始偏移

        short PlcEventBeginAddress { get; set; }//Plc 事件区起始地址
        short PlcEventBeginOffset { get; set; }//Plc 事件区起始偏移
    }
}
