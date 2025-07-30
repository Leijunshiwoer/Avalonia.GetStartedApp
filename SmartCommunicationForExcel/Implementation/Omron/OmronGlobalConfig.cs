using SmartCommunicationForExcel.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Implementation.Omron
{
    public class OmronGlobalConfig : IOmronGlobalConfig<OmronEventIO, OmronCpuInfo, OmronEventInstance>
    {
        public OmronGlobalConfig()
        { }
        [Description("文件名称")]
        public string FileName
        {
            get;
            set;
        } = "Default";
        [Description("文件保存时间")]
        public DateTime FileSaveTime
        {
            get;
            set;
        } = DateTime.Now;
        [Description("配置版本号")]
        public string Versioin
        {
            get;
            set;
        } = "V1.0.0.0";
        [Description("配置用户")]
        public string User
        {
            get;
            set;
        } = "kstopa";
        [Description("Eap公共信息")]
        public List<OmronEventIO> EapConfig { get; set; } = new List<OmronEventIO>();
        [Description("Plc公共信息")]
        public List<OmronEventIO> PlcConfig { get; set; } = new List<OmronEventIO>();
        [Description("Cpu数据信息")]
        public OmronCpuInfo CpuInfo { get; set; } = new OmronCpuInfo();
        [Description("事件配置信息")]
        public List<OmronEventInstance> EventConfig { get; set; } = new List<OmronEventInstance>();
    }
}
