using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.SiemensS7PLC
{
    public class PlcKeepData
    {
        public int OkNo { get; set; }
        public int NgNo { get; set; }
        public int TotalNo { get; set; }
        public float Yield { get; set; }
        public PlcKeepDataSub[] StDatas { get; set; } = Enumerable.Range(0, 5).Select(_ => new PlcKeepDataSub()).ToArray();
        public PlcKeepDataString[] SysDataString { get; set; } = Enumerable.Range(0, 16).Select(_ => new PlcKeepDataString()).ToArray();
    }


    public partial class PlcKeepDataSub
    {
        public short TotalNGNO { get; set; }                    // 累计NG        
        public short[] HmiSetUnit { get; set; } = new short[16];    // 人机Uint变量预留        
        public float[] HmiSetReal { get; set; } = new float[16];   // 人机Real变量预留        
        public int[] HmiSetDint { get; set; } = new int[16];    // 人机Dint变量预留
    }

    public partial class PlcKeepDataString
    {
        public byte Sys_DefineLength { get; set; }              // 定义长度
        public byte Sys_TrueLength { get; set; }                  // 实际长度
        public byte[] Sys_DataString { get; set; } = new byte[30]; // 字符串
    }
}
