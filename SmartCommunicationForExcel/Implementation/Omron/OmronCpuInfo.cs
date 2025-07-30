using SmartCommunicationForExcel.Interface;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Implementation.Omron
{
    public class OmronCpuInfo : IOmronCpuInfo
    {
        [Description("CPU名称")]
        public string Name { get; set; }

        [Description("CPU类型")]
        public CpuType CpuType { get; } = CpuType.Omron;

        [Description("备注")]
        public string Mark
        {
            get;
            set;
        } = string.Empty;

        [Description("Plc地址")]
        public string IP
        {
            get;
            set;
        } = string.Empty;

        [Description("端口号")]
        public short Port
        {
            get;
            set;
        } = 102;

        [Description("EAP配置起始地址")]
        public short EapConfigBeginAddress
        {
            get;
            set;
        } = 0;

        [Description("EAP配置起始偏移")]
        public short EapConfigBeginOffset
        {
            get;
            set;
        } = 0;

        [Description("Eap事件配置起始地址")]
        public short EapEventBeginAddress
        {
            get;
            set;
        } = 0;

        [Description("Eap事件配置起始偏移")]
        public short EapEventBeginOffset
        {
            get;
            set;
        } = 0;

        [Description("Plc配置起始地址")]
        public short PlcConfigBeginAddress
        {
            get;
            set;
        } = 0;

        [Description("Plc配置起始偏移")]
        public short PlcConfigBeginOffset
        {
            get;
            set;
        } = 0;

        [Description("PLC事件配置起始地址")]
        public short PlcEventBeginAddress
        {
            get;
            set;
        } = 0;

        [Description("PLC事件配置起始偏移")]
        public short PlcEventBeginOffset
        {
            get;
            set;
        } = 0;

        [Description("Plc导轨号[Ignore]")]
        public byte SA1
        {
            get;
            set;
        } = 0;

        [Description("DA1")]
        public byte DA1
        {
            get;
            set;
        } = 0;
        [Description("DA2")]
        public byte DA2
        {
            get;
            set;
        } = 0;

        [Description("扫描周期")]
        public int CycleTime { get; set; } = 1;
    }
}
