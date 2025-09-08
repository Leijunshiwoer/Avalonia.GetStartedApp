using CommunityToolkit.Mvvm.ComponentModel;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.SiemensS7PLC
{
    public partial class PlcKeepData: ObservableObject
    {
        [ObservableProperty] private int _okNo;            // OK产量
        [ObservableProperty] private int _ngNo;            // NG产量4
        [ObservableProperty] private float _yield;         // 良率8
        [ObservableProperty] private int _totalNO;         // 总产品12

        [ObservableProperty]
        [property: S7StringAttribute(S7StringType.S7String, 20)]
        private string[] _sysDataString  = new string[2]; //16  38

        [ObservableProperty]
        [property: S7StringAttribute(S7StringType.S7WString, 60)]//60
        private string _sysDataWString;

        [ObservableProperty]
        private PlcKeepDataSub[] _stDatas  = Enumerable.Range(0, 5).Select(_ => new PlcKeepDataSub()).ToArray();//36
    }


    public partial class PlcKeepDataSub: ObservableObject
    {
        [ObservableProperty] private short _totalNGNO;                    // 累计NG        
        [ObservableProperty] private short[] _hmiSetUnit = new short[16]; // 人机Uint变量预留        
        [ObservableProperty] private float[] _hmiSetReal = new float[16]; // 人机Real变量预留        
        [ObservableProperty] private int[] _hmiSetDint = new int[16];     // 人机Dint变量预留
    }

    public partial class PlcKeepDataString: ObservableObject
    {
        [ObservableProperty] private byte _sysDefineLength;              // 定义长度
        [ObservableProperty] private byte _sysTrueLength;                  // 实际长度
        [ObservableProperty] private byte[] _sysDataString = new byte[30]; // 字符串
    }
}
