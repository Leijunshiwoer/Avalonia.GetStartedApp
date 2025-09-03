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

    #region OP10

   
    public class OP10PublicAreaFromPC
    {
        [HslDeviceAddress ("DB5000.0", 2, typeof(SiemensS7Net))]
        public short Heartbeat { get; set; }
        [HslDeviceAddress("DB5000.2", 2, typeof(SiemensS7Net))]
        public short Command { get; set; }//PC控制设备[1:Run, 2:Stop, 3:Reset]
        [HslDeviceAddress("DB5000.4", 2, typeof(SiemensS7Net))]
        public short RecipeNo { get; set; }//下载的配方编号 1
    }

   
    public class OP10PublicAreaToPC
    {

        
        [HslDeviceAddress("DB5001.0", 2, typeof(SiemensS7Net))]
        public short Heartbeat { get; set; }
        
        [HslDeviceAddress("DB5001.2", 20, typeof(SiemensS7Net))]
        public short[] Status { get; set; }
        
        [HslDeviceAddress("DB5001.22", 2, typeof(SiemensS7Net))]
        public short MachineCycleTime { get; set; }
        
        [HslDeviceAddress("DB5001.24", 64, typeof(SiemensS7Net))]

        public bool[] EventsTrigger { get; set; }
        [HslDeviceAddress("DB5001.88", 1000, typeof(SiemensS7Net))]
        public bool[] Alarms { get; set; }
    }
    #region PC事件

    public class OP10Event01AreaFromPC
    {
        [HslDeviceAddress("DB5000.3000", 2, typeof(SiemensS7Net))]

        public short SequenceID { get; set; }
    }

    public class OP10Event01AreaToPC
    {
        [HslDeviceAddress("DB5001.3001", 2, typeof(SiemensS7Net))]
        public short SequenceID { get; set; }
    }
    #endregion

    #endregion


    #region OP20


    public class OP20PublicAreaFromPC
    {
        [HslDeviceAddress("DB5000.0", 2, typeof(SiemensS7Net))]
        public short Heartbeat { get; set; }
        [HslDeviceAddress("DB5000.2", 2, typeof(SiemensS7Net))]
        public short Command { get; set; }//PC控制设备[1:Run, 2:Stop, 3:Reset]
        [HslDeviceAddress("DB5000.4", 2, typeof(SiemensS7Net))]
        public short RecipeNo { get; set; }//下载的配方编号 1
    }


    public class OP20PublicAreaToPC
    {


        [HslDeviceAddress("DB5001.0", 2, typeof(SiemensS7Net))]
        public short Heartbeat { get; set; }

        [HslDeviceAddress("DB5001.2", 20, typeof(SiemensS7Net))]
        public short[] Status { get; set; }

        [HslDeviceAddress("DB5001.22", 2, typeof(SiemensS7Net))]
        public short MachineCycleTime { get; set; }

        [HslDeviceAddress("DB5001.24", 64, typeof(SiemensS7Net))]

        public bool[] EventsTrigger { get; set; }
        [HslDeviceAddress("DB5001.88", 1000, typeof(SiemensS7Net))]
        public bool[] Alarms { get; set; }
    }
    #region PC事件

    public class OP20Event01AreaFromPC
    {
        [HslDeviceAddress("DB5000.3000", 2, typeof(SiemensS7Net))]

        public short SequenceID { get; set; }
    }

    public class OP20Event01AreaToPC
    {
        [HslDeviceAddress("DB5001.3001", 2, typeof(SiemensS7Net))]
        public short SequenceID { get; set; }
    }
    #endregion

    #endregion

}
