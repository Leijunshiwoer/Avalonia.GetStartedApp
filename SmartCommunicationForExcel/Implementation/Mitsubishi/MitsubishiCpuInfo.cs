//===================================================
//  Copyright @  KSTOPA 2020
//  作者：Fang.Lu
//  时间：2020-10-23 13:16:24
//  说明：优化选项说明
//===================================================
using HslCommunication.Profinet.Siemens;
using SmartCommunicationForExcel.Interface;
using System.ComponentModel;
using SmartCommunicationForExcel.Model;

namespace SmartCommunicationForExcel.Implementation.Mitsubishi
{
    public class MitsubishiCpuInfo : IMitsubishiCpuInfo
    {
        [Description("CPU类型")]
        public CpuType CpuType
        {
            get;
            set;
        } = CpuType.Mitsubishi;

        [Description("通信协议")]
        public string PlcType
        {
            get;
            set;
        } = "MC(Binary)";

        [Description("备注")]
        public string Mark
        {
            get;

            set;
        } = string.Empty;

        [Description("CPU名称")]
        public string Name
        {
            get;

            set;
        } = string.Empty;

        [Description("EAP配置起始地址[DB****,填入后面数字即可]")]
        public short EapConfigBeginAddress
        {
            get;
            set;
        } = 0;

        [Description("EAP配置起始偏移[DB****.**,填入点号后面偏移量数字]")]
        public short EapConfigBeginOffset
        {
            get;
            set;
        } = 0;

        [Description("Eap事件配置起始地址[DB****,填入后面数字即可]")]
        public short EapEventBeginAddress
        {
            get;
            set;
        } = 0;

        [Description("Eap事件配置起始偏移[DB****.**,填入点号后面偏移量数字]")]
        public short EapEventBeginOffset
        {
            get;
            set;
        } = 0;

        [Description("PLC配置起始地址[DB****,填入后面数字即可]")]
        public short PlcConfigBeginAddress
        {
            get;
            set;
        } = 0;

        [Description("PLC配置起始偏移[DB****.**,填入点号后面偏移量数字]")]
        public short PlcConfigBeginOffset
        {
            get;
            set;
        } = 0;

        [Description("PLC事件配置起始地址[DB****,填入后面数字即可]")]
        public short PlcEventBeginAddress
        {
            get;
            set;
        } = 0;

        [Description("PLC事件配置起始偏移[DB****.**,填入点号后面偏移量数字]")]
        public short PlcEventBeginOffset
        {
            get;
            set;
        } = 0;

        [Description("PLC IP地址")]
        public string IP
        {
            get;
            set;
        } = string.Empty;

        [Description("PLC端口号1")]
        public short Port1
        {
            set;
            get;
        } = 102;

        [Description("PLC端口号2")]
        public short Port2
        {
            get;
            set;
        } = 0;
        [Description("底层通讯库")]
        public DllType Dll
        {
            get;
            set;
        } = DllType.Type_Hsl;

        private int _nCycleTime = 10;
        [Description("单循环扫描周期[1-2000]ms")]
        public int CycleTime
        {
            get => _nCycleTime;

            set
            {
                if (value < 1 || value > 200)
                    _nCycleTime = 1;
                else
                    _nCycleTime = value;
            }
        }
    }
}
