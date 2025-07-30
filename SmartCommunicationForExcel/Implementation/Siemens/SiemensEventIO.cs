//===================================================
//  Copyright @  KSTOPA 2020
//  作者：Fang.Lu
//  时间：2020-05-21 14:02:04
//  说明：单行事件元素定义,优化PropertyGrid显示方式
//===================================================

using SmartCommunicationForExcel.Core;
using SmartCommunicationForExcel.Interface;
using SmartCommunicationForExcel.Model;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;

//TODO:限定数据类型和长度信息  需要多添加数组数据类型 如果为数组类型 则需要添加Set/Get重载函数能够设置数组值
//字符串类型处理 两种字符串类型  String[Siemens]/CharArray

namespace SmartCommunicationForExcel.Implementation.Siemens
{
    public class SiemensEventIO : ReverseBytesTransform, IEventIO
    {
        public SiemensEventIO():base(DataFormat.BADC)
        {
            
        }

        public SiemensEventIO(DataFormat dataFormat) : base(dataFormat)
        {
            
        }

        [Description("数据值")]
        public byte[] DataValue { get; set; }

        private short _sLength = 1;
        [Description("数据长度[双字节为一个单位]"), ReadOnly(false)]
        public short Length
        {
            get
            {
                return _sLength;
            }
            set
            {
                _sLength = value;
                DataValue = new byte[value * 2];
            }
        }

        [Description("展现数据")]
        public string DataValueStr { get; set; }

        /// <summary>
        /// 设置属性的ReadOnly值
        /// </summary>
        /// <param name="strAttribute"></param>
        /// <param name="value"></param>
        private void SetReadOnlyAttribute(string strAttribute, bool value)
        {
            PropertyDescriptorCollection appSetingAttributes = TypeDescriptor.GetProperties(this);
            Type displayType = typeof(ReadOnlyAttribute);
            FieldInfo fieldInfo = displayType.GetField("isReadOnly", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance);

            fieldInfo.SetValue(appSetingAttributes[strAttribute].Attributes[displayType], value);
        }

        [Description("起始地址"), ReadOnly(true)]
        public short MBAdr
        {
            get;
            set;
        } = 0;

        [Description("结束地址")]
        public short MEAdr
        {
            get;
            //get
            //{
            //    //return (short)(MBAdr + (Length - 1) * 2);
            //}
            set;
        }

        [Description(".NET数据类型")]
        private CDataType _dtType = CDataType.DTShort;
        public CDataType DTType
        {
            get
            {
                return _dtType;
            }
            set
            {
                //CDataType tmp = _dtType;

                //switch (value)
                //{
                //    case CDataType.DTShort:
                //        Length = 1;
                //        SetReadOnlyAttribute("Length", true);
                //        tmp = value;
                //        break;
                //    case CDataType.DTInt:
                //        SetReadOnlyAttribute("Length", true);
                //        tmp = value;
                //        Length = 2;
                //        break;
                //    case CDataType.DTFloat:
                //        SetReadOnlyAttribute("Length", true);
                //        tmp = value;
                //        Length = 2;
                //        break;
                //    case CDataType.DTString:
                //        SetReadOnlyAttribute("Length", false);
                //        tmp = value;
                //        break;
                //}
                _dtType = value;
            }
        }

        [Description("EAP读取？"), BrowsableAttribute(false)]
        public bool IsEapRead
        {
            set;
            get;
        } = true;

        [Description("备注")]
        public string Mark
        {
            get;
            set;
        }
        = "事件备注";

        [Description("事件输入输出参数名")]
        public string TagName
        {
            get;
            set;
        } = "事件名称";

        [Description("全局数据起始地址"), ReadOnly(true)]
        public short GlobalBeginAddress { set; get; } = 0;
        [Description("全局数据起始偏移"), ReadOnly(true)]
        public short GlobalBeginOffset { set; get; } = 0;
        [Description("输入输出方向")]
        public string GetDirection
        {
            get { return IsEapRead ? "PLC->EAP" : "EAP->PLC"; }
        }

        [Description("获取数据起始地址")]
        public string GetMBAddressTag
        {
            //get { return "DB" + GlobalBeginAddress + "." + MBAdr; }
            get;set;
        }

        [Description("获取数据结束地址")]
        public string GetMEAddressTag
        {
            //get { return "DB" + GlobalBeginAddress + "." + MEAdr; }
            get;set;
        }

        //private DataFormat _DataFormat = DataFormat.CDAB;
        //[Description("编码规格")]
        //public DataFormat DataFormat
        //{
        //    get { return _DataFormat; }
        //    set
        //    {
        //        _DataFormat = value;
        //        base.DataFormat = _DataFormat;
        //    }
        //}

        public void GetDataValueStr()
        {
            if (TagName == "EventTrigger")
            {
                //显示对于的bit位
                var bsT = TransBool(DataValue, 0, DataValue.Length);
                int c = bsT.Length / 16;
                DataValueStr = "[";
                for (int i = 0; i < c; i++)
                {
                    if (i > 0)
                        DataValueStr += ",";
                    for (int j = 0; j < 16; j++)
                        DataValueStr += bsT[i * 16 + j] ? "1" : "0";
                }
                DataValueStr += "]";
            }
            else
            {
                switch (DTType)
                {
                    case CDataType.DTShort:
                        {
                            //查看长度 判定是数值还是单值
                            bool isSingle = Length == 1;
                            if (DataValue.Length % Length == 0)
                            {
                                if (isSingle)
                                {
                                    DataValueStr = GetInt16().ToString();
                                }
                                else
                                {
                                    //数组
                                    int buffLen = Length;
                                    DataValueStr = "[";
                                    for (int i = 0; i < buffLen; i++)
                                    {
                                        if (i > 0)
                                            DataValueStr += ",";
                                        DataValueStr += GetInt16(i * 2).ToString();
                                    }
                                    DataValueStr += "]";
                                }
                            }
                            else
                            {
                                //有错误
                                DataValueStr = "数据长度不符合要求,请查看配置Excel";
                            }

                            break;
                        }
                    case CDataType.DTInt:
                        {
                            //查看长度 判定是数值还是单值
                            bool isSingle = Length == 2;
                            if (DataValue.Length % Length == 0)
                            {
                                if (isSingle)
                                {
                                    DataValueStr = GetInt32().ToString();
                                }
                                else
                                {
                                    //数组
                                    int buffLen = Length / 2;
                                    DataValueStr = "[";
                                    for (int i = 0; i < buffLen; i++)
                                    {
                                        if (i > 0)
                                            DataValueStr += ",";
                                        DataValueStr += GetInt32(i * 4).ToString();
                                    }
                                    DataValueStr += "]";
                                }
                            }
                            else
                            {
                                //有错误
                                DataValueStr = "数据长度不符合要求,请查看配置Excel";
                            }
                            break;
                        }
                    case CDataType.DTFloat:
                        {
                            //查看长度 判定是数值还是单值
                            bool isSingle = Length == 2;
                            if (DataValue.Length % Length == 0)
                            {
                                if (isSingle)
                                {
                                    DataValueStr = GetSingle().ToString();
                                }
                                else
                                {
                                    //数组
                                    int buffLen = Length / 2;
                                    DataValueStr = "[";
                                    for (int i = 0; i < buffLen; i++)
                                    {
                                        if (i > 0)
                                            DataValueStr += ",";
                                        DataValueStr += GetSingle(i * 4).ToString();
                                    }
                                    DataValueStr += "]";
                                }
                            }
                            else
                            {
                                //有错误
                                DataValueStr = "数据长度不符合要求,请查看配置Excel";
                            }
                            break;
                        }
                    case CDataType.DTString:
                        {
                            DataValueStr = GetString();
                            break;
                        }
                }
            }
        }

        public short GetInt16()
        {
            if (DTType == CDataType.DTShort)
                return TransInt16(DataValue, 0);
            else
                return 0;
        }

        public int GetInt32()
        {
            if (DTType == CDataType.DTInt)
                return TransInt32(DataValue, 0);
            else
                return 0;
        }

        public float GetSingle()
        {
            if (DTType == CDataType.DTFloat)
                return TransSingle(DataValue, 0);
            else
                return 0f;
        }

        public string GetString()
        {
            if (DTType == CDataType.DTString)
                //return TransString(DataValue, 0, DataValue.Length, Encoding.ASCII);
                return TransString(DataValue);
            else
                return string.Empty;
        }

        public string GetWString()
        {
            if (DTType == CDataType.DTString)
                //return TransString(DataValue, 0, DataValue.Length, Encoding.ASCII);
                return TransWString(DataValue);
            else
                return string.Empty;
        }


        public bool SetInt16(short value)
        {
            if (DTType == CDataType.DTShort)
            {
                DataValue = TransByte(value);
                return true;
            }
            else
                return false;
        }

        public bool SetInt32(int value)
        {
            if (DTType == CDataType.DTInt)
            {
                DataValue = TransByte(value);
                return true;
            }
            else
                return false;
        }

        public bool SetFloat(float value)
        {
            if (DTType == CDataType.DTFloat)
            {
                DataValue = TransByte(value);
                return true;
            }
            else
                return false;
        }

        public bool SetString(string value)
        {
            if (DTType == CDataType.DTString)
            {
                string str = "";
                if(value.Length > (Length * 2 - 2))
                {
                    str = value.Substring(0, Length * 2 - 2);
                }
                else
                {
                    str = value;
                }
                int len = str.Length;
                byte b = (byte)len;
                byte[] bb = new byte[] { (byte)(Length*2-2), b};
                byte[] bbb = Encoding.ASCII.GetBytes(str);
                Array.Resize(ref bb, bb.Length + bbb.Length);
                bbb.CopyTo(bb, bb.Length - bbb.Length);
                for (int i = 0; i < Length * 2; i++)
                {
                    if (i < bb.Length)
                        DataValue[i] = bb[i];
                    else
                        DataValue[i] = (byte)'\0';
                }
                return true;
            }
            else
                return false;
        }

        public bool SetWString(string value)
        {
            if (DTType == CDataType.DTString)
            {

                byte[] bbb = 中文转字节流ToPLC(value);
                DataValue = bbb;
                return true;
            }
            else
                return false;
        }

        private byte[] 中文转字节流ToPLC(string msg)
        {
            if (msg.Length > Length - 2)
            {
                msg = msg.Substring(0, Length - 2);
            }
            byte[] msgb = new byte[Length * 2];
            msgb[0] = (byte)((Length - 2) << 8);
            msgb[1] = (byte)(Length - 2);
            msgb[2] = (byte)(msg.Length << 8);
            msgb[3] = (byte)msg.Length;

            byte[] bb = Encoding.Unicode.GetBytes(msg);
            byte[] bbb = new byte[bb.Length];
            for (int i = 0; i < bb.Length; i++)
            {
                int m = i / 2;
                int n = i % 2;

                if (n == 0)
                    bbb[i] = bb[i + 1];
                else
                    bbb[i] = bb[i - 1];
            }
            Array.Copy(bbb, 0, msgb, 4, bbb.Length);
            return msgb;
        }

        public short GetInt16(int nOffset = 0)
        {
            if (DTType == CDataType.DTShort)
                return TransInt16(DataValue, nOffset);
            else
                return 0;
        }



        public int GetInt32(int nOffset = 0)
        {
            if (DTType == CDataType.DTInt)
                return TransInt32(DataValue, nOffset);
            else
                return 0;
        }
        public bool SetUInt32(uint value)
        {
            if (DTType == CDataType.DTFloat)
            {
                DataValue = TransByte(value);
                return true;
            }
            else
                return false;
        }
        public uint GetUInt32(int nOffset = 0)
        {
            if (DTType == CDataType.DTInt)
                return TransUInt32(DataValue, nOffset);
            else
                return 0;
        }

        public float GetSingle(int nOffset)
        {
            if (DTType == CDataType.DTFloat)
                return TransSingle(DataValue, nOffset);
            else
                return 0;
        }

        public string GetString(int nOffset)
        {
            throw new NotImplementedException();
        }




        public bool SetInt16(short value, int nOffset)
        {
            throw new NotImplementedException();
        }

        public bool SetInt32(int value, int nOffset)
        {
            throw new NotImplementedException();
        }

        public bool SetFloat(float value, int nOffset)
        {
            throw new NotImplementedException();
        }

        public bool SetString(string value, int nOffset)
        {
            throw new NotImplementedException();
        }

        public short[] GetInt16Arr()
        {
            short[] ss = new short[Length];
            if (DTType == CDataType.DTShort)
            {
                for (int i = 0; i < Length; i++)
                {
                    ss[i] = TransInt16(DataValue, i * 2);
                }
                return ss;
            }
            else
                return null;
        }

        public bool[] GetBitArr()
        {
            bool[] bb = new bool[Length * 16];
            for (int i = 0; i < Length * 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(i % 2 == 0)//偶数
                        bb[i * 8 + j] = (DataValue[i + 1] & (1 << j)) == (1 << j);
                    else
                        bb[i * 8 + j] = (DataValue[i - 1] & (1 << j)) == (1 << j);
                }
            }
            return bb;
        }
    }
}
