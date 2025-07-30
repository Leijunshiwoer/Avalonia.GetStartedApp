//===================================================
//  Copyright @  KSTOPA 2020
//  作者：Fang.Lu
//  时间：2020-05-21 14:01:40
//  说明：读写PLC定义的数据类
//===================================================

namespace SmartCommunicationForExcel.Model
{
    public enum CDataType
    {
        DTShort = 0,//双字节 Int16 Word
        DTInt = 2,//4字节 Int32 DWord
        DTFloat = 3,//4字节 Real
        DTString = 4//字符串 String

    }
}
