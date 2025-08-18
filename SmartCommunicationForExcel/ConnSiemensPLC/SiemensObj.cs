using HslCommunication.Profinet.Beckhoff;
using HslCommunication.Profinet.Siemens;
using HslCommunication.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.ConnSiemensPLC
{

    #region 公共区域

    public class PublicAreaFromPC
    {
        [HslDeviceAddress("s=PublicAreaFromPC.Heartbeat", 2,typeof(BeckhoffAdsNet))]
        [HslDeviceAddress ("DB5000.0", 2, typeof(SiemensS7Net))]
        public short Heartbeat { get; set; }
        [HslDeviceAddress("s=PublicAreaFromPC.Command", 2, typeof(BeckhoffAdsNet))]
        [HslDeviceAddress("DB5000.2", 2, typeof(SiemensS7Net))]
        public short Command { get; set; }//PC控制设备[1:Run, 2:Stop, 3:Reset]
        [HslDeviceAddress("s=PublicAreaFromPC.RecipeNo", 2, typeof(BeckhoffAdsNet))]
        [HslDeviceAddress("DB5000.4", 2, typeof(SiemensS7Net))]
        public short RecipeNo { get; set; }//下载的配方编号 1
    }
    public class PublicAreaToPC
    {

        [HslDeviceAddress("s=PublicAreaToPC.Heartbeat", 2, typeof(BeckhoffAdsNet))]
        [HslDeviceAddress("DB5001.0", 2, typeof(SiemensS7Net))]
        public short Heartbeat { get; set; }
        
        [HslDeviceAddress("s=PublicAreaToPC.Status", 20, typeof(BeckhoffAdsNet))]
        [HslDeviceAddress("DB5001.2", 20, typeof(SiemensS7Net))]
        public short[] Status { get; set; }
        
        [HslDeviceAddress("s=PublicAreaToPC.MachineCycleTime", 2, typeof(BeckhoffAdsNet))]
        [HslDeviceAddress("DB5001.22", 2, typeof(SiemensS7Net))]
        public short MachineCycleTime { get; set; }
        
        [HslDeviceAddress("s=PublicAreaToPC.EventsTrigger", 64, typeof(BeckhoffAdsNet))]
        [HslDeviceAddress("DB5001.24", 64, typeof(SiemensS7Net))]

        public bool[] EventsTrigger { get; set; }
        [HslDeviceAddress("DB5001.88", 1000, typeof(SiemensS7Net))]
        [HslDeviceAddress("s=PublicAreaToPC.Alarms", 1000, typeof(BeckhoffAdsNet))]
        public bool[] Alarms { get; set; }
    }
    #region PC事件

    public class Event01AreaFromPC
    {
        [HslDeviceAddress("s=Event01AreaFromPC.SequenceID", 2, typeof(BeckhoffAdsNet))]
        [HslDeviceAddress("DB5000.3000", 2, typeof(SiemensS7Net))]

        public short SequenceID { get; set; }
    }

    public class Event01AreaToPC
    {
        [HslDeviceAddress("s=Event01AreaToPC.SequenceID", 2, typeof(BeckhoffAdsNet))]
        [HslDeviceAddress("DB5001.3001", 2, typeof(SiemensS7Net))]
        public short SequenceID { get; set; }
    }
    #endregion

    #endregion

}
