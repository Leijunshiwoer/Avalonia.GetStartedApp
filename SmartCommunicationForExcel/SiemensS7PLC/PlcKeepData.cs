using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.SiemensS7PLC
{
    public class PlcKeepData
    {

        
        public int OkNo { get; set; }//0
        public int NgNo { get; set; }//4
        public int TotalNo { get; set; }//8
        public float Yield { get; set; }//12
        public PlcKeepDataSub[] StDatas { get; set; } = Enumerable.Range(0, 5).Select(_ => new PlcKeepDataSub()).ToArray();//16+130*5
        public PlcKeepDataString[] SysDataString { get; set; } = Enumerable.Range(0, 16).Select(_ => new PlcKeepDataString()).ToArray();
    }


    public partial class PlcKeepDataSub
    {
        public short TotalNGNO { get; set; }                    // 累计NG      //2  
        public short[] HmiSetUnit { get; set; } = new short[16];    // 人机Uint变量预留 //32       
        public float[] HmiSetReal { get; set; } = new float[16];   // 人机Real变量预留   //32     
        public int[] HmiSetDint { get; set; } = new int[16];    // 人机Dint变量预留  //64
    }

    public partial class PlcKeepDataString
    {
        public byte Sys_DefineLength { get; set; }              // 定义长度
        public byte Sys_TrueLength { get; set; }                  // 实际长度
        public byte[] Sys_DataString { get; set; } = new byte[30]; // 字符串
    }
}
