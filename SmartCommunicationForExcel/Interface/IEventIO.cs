//===================================================
//  Copyright @  KSTOPA 2020
//  作者：Fang.Lu
//  时间：2020-05-21 14:12:36
//  说明：事件实例接口
//===================================================
using SmartCommunicationForExcel.Model;

namespace SmartCommunicationForExcel.Interface
{
    public interface IEventIO
    {
        string TagName { get; set; } //事件名称
        byte[] DataValue { get; set; }//数据值
        CDataType DTType { get; set; }//需要将当前实例转换成.NET的类型
        bool IsEapRead { set; get; }//EapReadFromPlc

        short Length { get; set; }//数据长度 为了兼容 使用双字节计数（word类型）
        short MBAdr { get; set; }//数据起始地址 以双字节个数统计
        short MEAdr { get; }//数据结束地址 以双字节个数统计
        string Mark { get; set; }//备注
        short GlobalBeginOffset { set; get; }//全局起始偏移量
        short GlobalBeginAddress { set; get; }//全局起始地址
        string GetMBAddressTag { get; }//获取Word开始地址标签名
        string GetMEAddressTag { get; }//获取Word结束地址标签名
        string GetDirection { get; }//获取数据传输方向


        short GetInt16();//获取数据值
        short GetInt16(int nOffset);

        short[] GetInt16Arr();

        bool[] GetBitArr();

        int GetInt32();
        int GetInt32(int nOffset);

        float GetSingle();
        float GetSingle(int nOffset);

        string GetString();
        string GetString(int nOffset);


        bool SetInt16(short value);//设置数据值
        bool SetInt16(short value, int nOffset);//设置数据值

        bool SetInt32(int value);
        bool SetInt32(int value, int nOffset);

        bool SetFloat(float value);
        bool SetFloat(float value, int nOffset);

        bool SetString(string value);
        bool SetString(string value, int nOffset);


    }
}
