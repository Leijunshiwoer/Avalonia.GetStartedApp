using SmartCommunicationForExcel.Interface;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Implementation.Beckhoff
{
    public class BeckhoffCpuInfo : IBeckhoffCpuInfo
    {
        [Description("CPU类型")]
        public CpuType CpuType
        {
            get;
            set;
        } = CpuType.Beckhoff;

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

        [Description("EAP公共区标签")]
        public string EapConfigLabel
        {
            get;
            set;
        } = "EapLabel";

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

        [Description("PLC公共区标签")]
        public string PlcConfigLabel
        {
            get;
            set;
        } = "PlcLabel";

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

        [Description("PLC端口号")]
        public int Port
        {
            set;
            get;
        } = 48898;
        [Description("TargetNetId")]
        public string TargetNetId
        {
            set;
            get;
        }
        [Description("SenderNetId")]
        public string SenderNetId
        {
            set;
            get;
        }

        //[Description("PLC导轨号")]
        //public byte Rack
        //{
        //    get;
        //    set;
        //} = 0;

        //[Description("PLC插槽号")]
        //public byte Slot
        //{
        //    get;
        //    set;
        //} = 0;
        [Description("底层通讯库")]
        public DllType Dll
        {
            get;
            set;
        } = DllType.Type_Hsl;

        private int _nCycleTime = 1;
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
