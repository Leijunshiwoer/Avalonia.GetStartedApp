using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Implementation.Mitsubishi
{
    public class MitsubishiGlobalConfig : IMitsubishiGlobalConfig<MitsubishiEventIO, MitsubishiCpuInfo, MitsubishiEventInstance>
    {
        public MitsubishiGlobalConfig()
        {

        }

        [Description("Cpu数据信息")]
        public MitsubishiCpuInfo CpuInfo
        {
            get;
            set;
        } = new MitsubishiCpuInfo();

        [Description("Eap公共信息")]
        public List<MitsubishiEventIO> EapConfig
        {
            get;
            set;
        } = new List<MitsubishiEventIO>();

        [Description("Plc公共信息")]
        public List<MitsubishiEventIO> PlcConfig
        {
            get;
            set;
        } = new List<MitsubishiEventIO>();

        [Description("事件配置信息")]
        public List<MitsubishiEventInstance> EventConfig
        {
            get;

            set;
        } = new List<MitsubishiEventInstance>();

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

        [Description("配置用户")]
        public string User
        {
            get;
            set;
        } = "kstopa";

        [Description("配置版本号")]
        public string Versioin
        {
            get;
            set;
        } = "V1.0.0.0";
    }
}
