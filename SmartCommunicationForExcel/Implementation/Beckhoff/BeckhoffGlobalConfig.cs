using SmartCommunicationForExcel.Implementation.Beckhoff;
using SmartCommunicationForExcel.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Implementation.Beckhoff
{
    public class BeckhoffGlobalConfig : IBeckhoffGlobalConfig<BeckhoffEventIO, BeckhoffCpuInfo, BeckhoffEventInstance>
    {
        public BeckhoffGlobalConfig()
        {

        }

        [Description("Cpu数据信息")]
        public BeckhoffCpuInfo CpuInfo
        {
            get;
            set;
        } = new BeckhoffCpuInfo();

        [Description("Eap公共信息")]
        public List<BeckhoffEventIO> EapConfig
        {
            get;
            set;
        } = new List<BeckhoffEventIO>();

        [Description("Plc公共信息")]
        public List<BeckhoffEventIO> PlcConfig
        {
            get;
            set;
        } = new List<BeckhoffEventIO>();

        [Description("事件配置信息")]
        public List<BeckhoffEventInstance> EventConfig
        {
            get;

            set;
        } = new List<BeckhoffEventInstance>();

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
