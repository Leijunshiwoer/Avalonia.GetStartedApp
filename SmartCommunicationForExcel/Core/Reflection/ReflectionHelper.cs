using HslCommunication;
using HslCommunication.Core;
using HslCommunication.Enthernet.Redis;
using HslCommunication.Reflection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Core.Reflection
{
    public class HslReflectionHelper
    {
        /// <summary>
        /// 从属性中获取对应的设备类型的地址特性信息
        /// </summary>
        /// <param name="deviceType">设备类型信息</param>
        /// <param name="property">属性信息</param>
        /// <returns>设备类型信息</returns>
        public static HslDeviceAddressAttribute GetHslDeviceAddressAttribute(Type deviceType, PropertyInfo property)
        {
            var attribute = property.GetCustomAttributes(typeof(HslDeviceAddressAttribute), false);
            if (attribute == null) return null;

            HslDeviceAddressAttribute hslAttribute = null;
            for (int i = 0; i < attribute.Length; i++)
            {
                HslDeviceAddressAttribute tmp = (HslDeviceAddressAttribute)attribute[i];
                if (tmp.DeviceType != null && tmp.DeviceType == deviceType)
                {
                    hslAttribute = tmp;
                    break;
                }
            }

            if (hslAttribute == null)
            {
                for (int i = 0; i < attribute.Length; i++)
                {
                    HslDeviceAddressAttribute tmp = (HslDeviceAddressAttribute)attribute[i];
                    if (tmp.DeviceType == null)
                    {
                        hslAttribute = tmp;
                        break;
                    }
                }
            }

            return hslAttribute;
        }

        /// <inheritdoc cref="GetHslDeviceAddressAttribute(Type, PropertyInfo)"/>
        public static HslDeviceAddressAttribute[] GetHslDeviceAddressAttributeArray(Type deviceType, PropertyInfo property)
        {
            var attribute = property.GetCustomAttributes(typeof(HslDeviceAddressAttribute), false);
            if (attribute == null) return null;

            List<HslDeviceAddressAttribute> hslAttribute = new List<HslDeviceAddressAttribute>();
            for (int i = 0; i < attribute.Length; i++)
            {
                HslDeviceAddressAttribute tmp = (HslDeviceAddressAttribute)attribute[i];
                if (tmp.DeviceType != null && tmp.DeviceType == deviceType)
                {
                    hslAttribute.Add(tmp);
                }
            }

            if (hslAttribute.Count == 0)
            {
                for (int i = 0; i < attribute.Length; i++)
                {
                    HslDeviceAddressAttribute tmp = (HslDeviceAddressAttribute)attribute[i];
                    if (tmp.DeviceType == null)
                    {
                        hslAttribute.Add(tmp);
                    }
                }
            }

            return hslAttribute.ToArray();
        }

      

        /// <summary>
        /// 从设备里读取支持Hsl特性的数据内容，该特性为<see cref="HslDeviceAddressAttribute"/>，详细参考论坛的操作说明。
        /// </summary>
        /// <typeparam name="T">自定义的数据类型对象</typeparam>
        /// <param name="readWrite">读写接口的实现</param>
        /// <returns>包含是否成功的结果对象</returns>
        public static OperateResult<T> Read<T>(IReadWriteNet readWrite) where T : class, new()
        {
            var type = typeof(T);
            // var constrcuor = type.GetConstructors( System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic );
            var obj = type.Assembly.CreateInstance(type.FullName);

            var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var property in properties)
            {
                Type propertyType = property.PropertyType;
                if (propertyType == typeof(string[]))
                {
                    HslDeviceAddressAttribute[] hslAttributes = GetHslDeviceAddressAttributeArray(readWrite.GetType(), property);
                    if (hslAttributes == null || hslAttributes.Length == 0) continue;

                    string[] strings = new string[hslAttributes.Length];
                    for (int i = 0; i < hslAttributes.Length; i++)
                    {
                        OperateResult<string> valueResult = readWrite.ReadString(hslAttributes[i].Address, (ushort)(hslAttributes[i].Length >= 0 ? hslAttributes[i].Length : 1), hslAttributes[i].GetEncoding());
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        strings[i] = valueResult.Content;
                    }
                    property.SetValue(obj, strings, null);
                }
                else
                {
                    HslDeviceAddressAttribute hslAttribute = GetHslDeviceAddressAttribute(readWrite.GetType(), property);
                    if (hslAttribute == null) continue;

                    if (propertyType == typeof(byte))
                    {
                        MethodInfo readByteMethod = readWrite.GetType().GetMethod("ReadByte", new Type[] { typeof(string) });
                        if (readByteMethod == null) return new OperateResult<T>($"{readWrite.GetType().Name} not support read byte value. ");

                        OperateResult<byte> valueResult = (OperateResult<byte>)readByteMethod.Invoke(readWrite, new object[] { hslAttribute.Address });
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(short))
                    {
                        OperateResult<short> valueResult = readWrite.ReadInt16(hslAttribute.Address);
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(short[]))
                    {
                        OperateResult<short[]> valueResult = readWrite.ReadInt16(hslAttribute.Address, (ushort)(hslAttribute.Length >= 0 ? hslAttribute.Length : 1));
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(ushort))
                    {
                        OperateResult<ushort> valueResult = readWrite.ReadUInt16(hslAttribute.Address);
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(ushort[]))
                    {
                        OperateResult<ushort[]> valueResult = readWrite.ReadUInt16(hslAttribute.Address, (ushort)(hslAttribute.Length >= 0 ? hslAttribute.Length : 1));
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(int))
                    {
                        OperateResult<int> valueResult = readWrite.ReadInt32(hslAttribute.Address);
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(int[]))
                    {
                        OperateResult<int[]> valueResult = readWrite.ReadInt32(hslAttribute.Address, (ushort)(hslAttribute.Length >= 0 ? hslAttribute.Length : 1));
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(uint))
                    {
                        OperateResult<uint> valueResult = readWrite.ReadUInt32(hslAttribute.Address);
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(uint[]))
                    {
                        OperateResult<uint[]> valueResult = readWrite.ReadUInt32(hslAttribute.Address, (ushort)(hslAttribute.Length >= 0 ? hslAttribute.Length : 1));
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(long))
                    {
                        OperateResult<long> valueResult = readWrite.ReadInt64(hslAttribute.Address);
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(long[]))
                    {
                        OperateResult<long[]> valueResult = readWrite.ReadInt64(hslAttribute.Address, (ushort)(hslAttribute.Length >= 0 ? hslAttribute.Length : 1));
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(ulong))
                    {
                        OperateResult<ulong> valueResult = readWrite.ReadUInt64(hslAttribute.Address);
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(ulong[]))
                    {
                        OperateResult<ulong[]> valueResult = readWrite.ReadUInt64(hslAttribute.Address, (ushort)(hslAttribute.Length >= 0 ? hslAttribute.Length : 1));
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(float))
                    {
                        OperateResult<float> valueResult = readWrite.ReadFloat(hslAttribute.Address);
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(float[]))
                    {
                        OperateResult<float[]> valueResult = readWrite.ReadFloat(hslAttribute.Address, (ushort)(hslAttribute.Length >= 0 ? hslAttribute.Length : 1));
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(double))
                    {
                        OperateResult<double> valueResult = readWrite.ReadDouble(hslAttribute.Address);
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(double[]))
                    {
                        OperateResult<double[]> valueResult = readWrite.ReadDouble(hslAttribute.Address, (ushort)(hslAttribute.Length >= 0 ? hslAttribute.Length : 1));
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(string))
                    {
                        OperateResult<string> valueResult = readWrite.ReadString(hslAttribute.Address, (ushort)(hslAttribute.Length >= 0 ? hslAttribute.Length : 1), hslAttribute.GetEncoding());
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(byte[]))
                    {
                        OperateResult<byte[]> valueResult = readWrite.Read(hslAttribute.Address, (ushort)(hslAttribute.Length >= 0 ? hslAttribute.Length : 1));
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(bool))
                    {
                        OperateResult<bool> valueResult = readWrite.ReadBool(hslAttribute.Address);
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                    else if (propertyType == typeof(bool[]))
                    {
                        OperateResult<bool[]> valueResult = readWrite.ReadBool(hslAttribute.Address, (ushort)(hslAttribute.Length >= 0 ? hslAttribute.Length : 1));
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);

                        property.SetValue(obj, valueResult.Content, null);
                    }
                }
            }

            return OperateResult.CreateSuccessResult((T)obj);
        }

        /// <summary>
        /// 从设备里读取支持Hsl特性的数据内容，该特性为<see cref="HslDeviceAddressAttribute"/>，详细参考论坛的操作说明。
        /// </summary>
        /// <typeparam name="T">自定义的数据类型对象</typeparam>
        /// <param name="data">自定义的数据对象</param>
        /// <param name="readWrite">数据读写对象</param>
        /// <returns>包含是否成功的结果对象</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static OperateResult Write<T>(T data, IReadWriteNet readWrite) where T : class, new()
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var type = typeof(T);
            var obj = data;

            var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var property in properties)
            {
                Type propertyType = property.PropertyType;

                if (propertyType == typeof(string[]))
                {
                    HslDeviceAddressAttribute[] hslAttributes = GetHslDeviceAddressAttributeArray(readWrite.GetType(), property);
                    if (hslAttributes == null || hslAttributes.Length == 0) continue;

                    string[] strings = (string[])property.GetValue(obj, null);
                    for (int i = 0; i < hslAttributes.Length; i++)
                    {
                        OperateResult writeResult = readWrite.Write(hslAttributes[i].Address, strings[i], hslAttributes[i].GetEncoding());
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                }
                else
                {
                    HslDeviceAddressAttribute hslAttribute = GetHslDeviceAddressAttribute(readWrite.GetType(), property);
                    if (hslAttribute == null) continue;

                    if (propertyType == typeof(byte))
                    {
                        MethodInfo method = readWrite.GetType().GetMethod("Write", new Type[] { typeof(string), typeof(byte) });
                        if (method == null) return new OperateResult<T>($"{readWrite.GetType().Name} not support write byte value. ");

                        byte value = (byte)property.GetValue(obj, null);

                        OperateResult valueResult = (OperateResult)method.Invoke(readWrite, new object[] { hslAttribute.Address, value });
                        if (!valueResult.IsSuccess) return OperateResult.CreateFailedResult<T>(valueResult);
                    }
                    else if (propertyType == typeof(short))
                    {
                        short value = (short)property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(short[]))
                    {
                        short[] value = (short[])property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(ushort))
                    {
                        ushort value = (ushort)property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(ushort[]))
                    {
                        ushort[] value = (ushort[])property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(int))
                    {
                        int value = (int)property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(int[]))
                    {
                        int[] value = (int[])property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(uint))
                    {
                        uint value = (uint)property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(uint[]))
                    {
                        uint[] value = (uint[])property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(long))
                    {
                        long value = (long)property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(long[]))
                    {
                        long[] value = (long[])property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(ulong))
                    {
                        ulong value = (ulong)property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(ulong[]))
                    {
                        ulong[] value = (ulong[])property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(float))
                    {
                        float value = (float)property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(float[]))
                    {
                        float[] value = (float[])property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(double))
                    {
                        double value = (double)property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(double[]))
                    {
                        double[] value = (double[])property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(string))
                    {
                        string value = (string)property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value, hslAttribute.GetEncoding());
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(byte[]))
                    {
                        byte[] value = (byte[])property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(bool))
                    {
                        bool value = (bool)property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                    else if (propertyType == typeof(bool[]))
                    {
                        bool[] value = (bool[])property.GetValue(obj, null);

                        OperateResult writeResult = readWrite.Write(hslAttribute.Address, value);
                        if (!writeResult.IsSuccess) return writeResult;
                    }
                }
            }

            return OperateResult.CreateSuccessResult((T)obj);
        }

        /// <summary>
        /// 根据类型信息，直接从原始字节解析出类型对象，然后赋值给对应的对象，该对象的属性需要支持特性 <see cref="HslStructAttribute"/> 才支持设置
        /// </summary>
        /// <typeparam name="T">类型信息</typeparam>
        /// <param name="buffer">缓存信息</param>
        /// <param name="startIndex">起始偏移地址</param>
        /// <param name="byteTransform">数据变换规则对象</param>
        /// <returns>新的实例化的类型对象</returns>
        public static T PraseStructContent<T>(byte[] buffer, int startIndex, IByteTransform byteTransform) where T : class, new()
        {
            var type = typeof(T);
            var obj = type.Assembly.CreateInstance(type.FullName);
            PraseStructContent(obj, buffer, startIndex, byteTransform);

            return (T)obj;
        }

        /// <summary>
        /// 根据结构体的定义，将原始字节的数据解析出来，然后赋值给对应的对象，该对象的属性需要支持特性 <see cref="HslStructAttribute"/> 才支持设置
        /// </summary>
        /// <param name="obj">类型对象信息</param>
        /// <param name="buffer">读取的缓存数据信息</param>
        /// <param name="startIndex">起始的偏移地址</param>
        /// <param name="byteTransform">数据变换规则对象</param>
        public static void PraseStructContent(object obj, byte[] buffer, int startIndex, IByteTransform byteTransform)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(HslStructAttribute), false);
                if (attribute == null) continue;

                HslStructAttribute hslAttribute = attribute.Length > 0 ? (HslStructAttribute)attribute[0] : null;

                if (hslAttribute == null) continue;

                Type propertyType = property.PropertyType;
                if (propertyType == typeof(byte)) property.SetValue(obj, buffer[startIndex + hslAttribute.Index], null);
                else if (propertyType == typeof(byte[])) property.SetValue(obj, buffer.SelectMiddle(startIndex + hslAttribute.Index, hslAttribute.Length), null);
                else if (propertyType == typeof(short)) property.SetValue(obj, byteTransform.TransInt16(buffer, startIndex + hslAttribute.Index), null);
                else if (propertyType == typeof(short[])) property.SetValue(obj, byteTransform.TransInt16(buffer, startIndex + hslAttribute.Index, hslAttribute.Length), null);
                else if (propertyType == typeof(ushort)) property.SetValue(obj, byteTransform.TransUInt16(buffer, startIndex + hslAttribute.Index), null);
                else if (propertyType == typeof(ushort[])) property.SetValue(obj, byteTransform.TransUInt16(buffer, startIndex + hslAttribute.Index, hslAttribute.Length), null);
                else if (propertyType == typeof(int)) property.SetValue(obj, byteTransform.TransInt32(buffer, startIndex + hslAttribute.Index), null);
                else if (propertyType == typeof(int[])) property.SetValue(obj, byteTransform.TransInt32(buffer, startIndex + hslAttribute.Index, hslAttribute.Length), null);
                else if (propertyType == typeof(uint)) property.SetValue(obj, byteTransform.TransUInt32(buffer, startIndex + hslAttribute.Index), null);
                else if (propertyType == typeof(uint[])) property.SetValue(obj, byteTransform.TransUInt32(buffer, startIndex + hslAttribute.Index, hslAttribute.Length), null);
                else if (propertyType == typeof(long)) property.SetValue(obj, byteTransform.TransInt64(buffer, startIndex + hslAttribute.Index), null);
                else if (propertyType == typeof(long[])) property.SetValue(obj, byteTransform.TransInt64(buffer, startIndex + hslAttribute.Index, hslAttribute.Length), null);
                else if (propertyType == typeof(ulong)) property.SetValue(obj, byteTransform.TransUInt64(buffer, startIndex + hslAttribute.Index), null);
                else if (propertyType == typeof(ulong[])) property.SetValue(obj, byteTransform.TransUInt64(buffer, startIndex + hslAttribute.Index, hslAttribute.Length), null);
                else if (propertyType == typeof(float)) property.SetValue(obj, byteTransform.TransSingle(buffer, startIndex + hslAttribute.Index), null);
                else if (propertyType == typeof(float[])) property.SetValue(obj, byteTransform.TransSingle(buffer, startIndex + hslAttribute.Index, hslAttribute.Length), null);
                else if (propertyType == typeof(double)) property.SetValue(obj, byteTransform.TransDouble(buffer, startIndex + hslAttribute.Index), null);
                else if (propertyType == typeof(double[])) property.SetValue(obj, byteTransform.TransDouble(buffer, startIndex + hslAttribute.Index, hslAttribute.Length), null);
                else if (propertyType == typeof(string))
                {
                    Encoding encoding = Encoding.UTF8;
                    if (string.IsNullOrEmpty(hslAttribute.Encoding)) encoding = Encoding.ASCII;
                    else if (hslAttribute.Encoding.Equals("ASCII", StringComparison.OrdinalIgnoreCase)) encoding = Encoding.ASCII;
                    else if (hslAttribute.Encoding.Equals("UNICODE", StringComparison.OrdinalIgnoreCase)) encoding = Encoding.Unicode;
                    else if (hslAttribute.Encoding.Equals("ANSI", StringComparison.OrdinalIgnoreCase)) encoding = Encoding.Default;
                    else if (hslAttribute.Encoding.Equals("UTF8", StringComparison.OrdinalIgnoreCase)) encoding = Encoding.UTF8;
                    else if (hslAttribute.Encoding.Equals("BIG-UNICODE", StringComparison.OrdinalIgnoreCase)) encoding = Encoding.BigEndianUnicode;
                    else if (hslAttribute.Encoding.Equals("GB2312", StringComparison.OrdinalIgnoreCase)) encoding = Encoding.GetEncoding("GB2312");
                    else encoding = Encoding.GetEncoding(hslAttribute.Encoding);

                    if (hslAttribute.StringMode == 0) property.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslAttribute.Index + 0, hslAttribute.Length, encoding), null);
                    else if (hslAttribute.StringMode == 1) property.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslAttribute.Index + 1, buffer[startIndex + hslAttribute.Index], encoding), null);
                    else if (hslAttribute.StringMode == 2) property.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslAttribute.Index + 2, buffer[startIndex + hslAttribute.Index + 1], encoding), null);
                    else if (hslAttribute.StringMode == 3) property.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslAttribute.Index + 2, byteTransform.TransUInt16(buffer, startIndex + hslAttribute.Index), encoding), null);
                    else if (hslAttribute.StringMode == 4) property.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslAttribute.Index + 4, byteTransform.TransUInt16(buffer, startIndex + hslAttribute.Index + 2), encoding), null);
                    else if (hslAttribute.StringMode == 5) property.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslAttribute.Index + 4, byteTransform.TransInt32(buffer, startIndex + hslAttribute.Index), encoding), null);
                }
                else if (propertyType == typeof(bool))
                {
                    property.SetValue(obj, buffer.GetBoolByIndex(startIndex * 8 + hslAttribute.Index), null);
                }
                else if (propertyType == typeof(bool[]))
                {
                    bool[] array = new bool[hslAttribute.Length];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = buffer.GetBoolByIndex(startIndex * 8 + hslAttribute.Index + i);
                    }
                    property.SetValue(obj, array, null);
                }
                else
                {
                    // 自定义的类，继续去解析操作
                    var childObj = propertyType.Assembly.CreateInstance(propertyType.FullName);
                    PraseStructContent(childObj, buffer, startIndex + hslAttribute.Index, byteTransform);
                    property.SetValue(obj, childObj, null);
                }
            }

        }

        internal static void SetPropertyObjectValue(PropertyInfo property, object obj, string value)
        {
            Type propertyType = property.PropertyType;
            if (propertyType == typeof(short))
                property.SetValue(obj, short.Parse(value), null);
            else if (propertyType == typeof(ushort))
                property.SetValue(obj, ushort.Parse(value), null);
            else if (propertyType == typeof(int))
                property.SetValue(obj, int.Parse(value), null);
            else if (propertyType == typeof(uint))
                property.SetValue(obj, uint.Parse(value), null);
            else if (propertyType == typeof(long))
                property.SetValue(obj, long.Parse(value), null);
            else if (propertyType == typeof(ulong))
                property.SetValue(obj, ulong.Parse(value), null);
            else if (propertyType == typeof(float))
                property.SetValue(obj, float.Parse(value), null);
            else if (propertyType == typeof(double))
                property.SetValue(obj, double.Parse(value), null);
            else if (propertyType == typeof(string))
                property.SetValue(obj, value, null);
            else if (propertyType == typeof(byte))
                property.SetValue(obj, byte.Parse(value), null);
            else if (propertyType == typeof(bool))
                property.SetValue(obj, bool.Parse(value), null);
            else
                property.SetValue(obj, value, null);
        }

        internal static void SetPropertyObjectValueArray(PropertyInfo property, object obj, string[] values)
        {
            Type propertyType = property.PropertyType;
            if (propertyType == typeof(short[]))
                property.SetValue(obj, values.Select(m => short.Parse(m)).ToArray(), null);
            else if (propertyType == typeof(List<short>))
                property.SetValue(obj, values.Select(m => short.Parse(m)).ToList(), null);
            else if (propertyType == typeof(ushort[]))
                property.SetValue(obj, values.Select(m => ushort.Parse(m)).ToArray(), null);
            else if (propertyType == typeof(List<ushort>))
                property.SetValue(obj, values.Select(m => ushort.Parse(m)).ToList(), null);
            else if (propertyType == typeof(int[]))
                property.SetValue(obj, values.Select(m => int.Parse(m)).ToArray(), null);
            else if (propertyType == typeof(List<int>))
                property.SetValue(obj, values.Select(m => int.Parse(m)).ToList(), null);
            else if (propertyType == typeof(uint[]))
                property.SetValue(obj, values.Select(m => uint.Parse(m)).ToArray(), null);
            else if (propertyType == typeof(List<uint>))
                property.SetValue(obj, values.Select(m => uint.Parse(m)).ToList(), null);
            else if (propertyType == typeof(long[]))
                property.SetValue(obj, values.Select(m => long.Parse(m)).ToArray(), null);
            else if (propertyType == typeof(List<long>))
                property.SetValue(obj, values.Select(m => long.Parse(m)).ToList(), null);
            else if (propertyType == typeof(ulong[]))
                property.SetValue(obj, values.Select(m => ulong.Parse(m)).ToArray(), null);
            else if (propertyType == typeof(List<ulong>))
                property.SetValue(obj, values.Select(m => ulong.Parse(m)).ToList(), null);
            else if (propertyType == typeof(float[]))
                property.SetValue(obj, values.Select(m => float.Parse(m)).ToArray(), null);
            else if (propertyType == typeof(List<float>))
                property.SetValue(obj, values.Select(m => float.Parse(m)).ToList(), null);
            else if (propertyType == typeof(double[]))
                property.SetValue(obj, values.Select(m => double.Parse(m)).ToArray(), null);
            else if (propertyType == typeof(double[]))
                property.SetValue(obj, values.Select(m => double.Parse(m)).ToList(), null);
            else if (propertyType == typeof(string[]))
                property.SetValue(obj, values, null);
            else if (propertyType == typeof(List<string>))
                property.SetValue(obj, new List<string>(values), null);
            else if (propertyType == typeof(byte[]))
                property.SetValue(obj, values.Select(m => byte.Parse(m)).ToArray(), null);
            else if (propertyType == typeof(List<byte>))
                property.SetValue(obj, values.Select(m => byte.Parse(m)).ToList(), null);
            else if (propertyType == typeof(bool[]))
                property.SetValue(obj, values.Select(m => bool.Parse(m)).ToArray(), null);
            else if (propertyType == typeof(List<bool>))
                property.SetValue(obj, values.Select(m => bool.Parse(m)).ToList(), null);
            else
                property.SetValue(obj, values, null);
        }


        #region Parameters From json

        /// <summary>
        /// 从Json数据里解析出真实的数据信息，根据方法参数列表的类型进行反解析，然后返回实际的数据数组<br />
        /// Analyze the real data information from the Json data, perform de-analysis according to the type of the method parameter list, 
        /// and then return the actual data array
        /// </summary>
        /// <param name="context">当前的会话内容</param>
        /// <param name="request">当用于Http请求的时候关联的请求头对象</param>
        /// <param name="parameters">提供的参数列表信息</param>
        /// <param name="json">参数变量信息</param>
        /// <returns>已经填好的实际数据的参数数组对象</returns>
        public static object[] GetParametersFromJson(ISessionContext context, HttpListenerRequest request, ParameterInfo[] parameters, string json)
        {
            JObject jObject = string.IsNullOrEmpty(json) ? new JObject() : JObject.Parse(json);
            object[] paras = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                string propertyName = parameters[i].Name;
                // 对方法定义的value参数名进行容错处理，如果找不到，就找 values 参数名
                if (!jObject.ContainsKey(propertyName))
                {
                    if (propertyName == "value" && jObject.ContainsKey("values")) propertyName = "values";
                    else if (propertyName == "values" && jObject.ContainsKey("value")) propertyName = "value";
                }

                if (parameters[i].ParameterType == typeof(byte)) paras[i] = jObject.Value<byte>(propertyName);
                else if (parameters[i].ParameterType == typeof(short)) paras[i] = jObject.Value<short>(propertyName);
                else if (parameters[i].ParameterType == typeof(ushort)) paras[i] = jObject.Value<ushort>(propertyName);
                else if (parameters[i].ParameterType == typeof(int)) paras[i] = jObject.Value<int>(propertyName);
                else if (parameters[i].ParameterType == typeof(uint)) paras[i] = jObject.Value<uint>(propertyName);
                else if (parameters[i].ParameterType == typeof(long)) paras[i] = jObject.Value<long>(propertyName);
                else if (parameters[i].ParameterType == typeof(ulong)) paras[i] = jObject.Value<ulong>(propertyName);
                else if (parameters[i].ParameterType == typeof(double)) paras[i] = jObject.Value<double>(propertyName);
                else if (parameters[i].ParameterType == typeof(float)) paras[i] = jObject.Value<float>(propertyName);
                else if (parameters[i].ParameterType == typeof(bool)) paras[i] = jObject.Value<bool>(propertyName);
                else if (parameters[i].ParameterType == typeof(string)) paras[i] = jObject.Value<string>(propertyName);
                else if (parameters[i].ParameterType == typeof(DateTime)) paras[i] = jObject.Value<DateTime>(propertyName);
                else if (parameters[i].ParameterType == typeof(byte[])) paras[i] = jObject.Value<string>(propertyName).ToHexBytes();
                else if (parameters[i].ParameterType == typeof(short[])) paras[i] = jObject[propertyName].ToArray().Select(m => m.Value<short>()).ToArray();
                else if (parameters[i].ParameterType == typeof(ushort[])) paras[i] = jObject[propertyName].ToArray().Select(m => m.Value<ushort>()).ToArray();
                else if (parameters[i].ParameterType == typeof(int[])) paras[i] = jObject[propertyName].ToArray().Select(m => m.Value<int>()).ToArray();
                else if (parameters[i].ParameterType == typeof(uint[])) paras[i] = jObject[propertyName].ToArray().Select(m => m.Value<uint>()).ToArray();
                else if (parameters[i].ParameterType == typeof(long[])) paras[i] = jObject[propertyName].ToArray().Select(m => m.Value<long>()).ToArray();
                else if (parameters[i].ParameterType == typeof(ulong[])) paras[i] = jObject[propertyName].ToArray().Select(m => m.Value<ulong>()).ToArray();
                else if (parameters[i].ParameterType == typeof(float[])) paras[i] = jObject[propertyName].ToArray().Select(m => m.Value<float>()).ToArray();
                else if (parameters[i].ParameterType == typeof(double[])) paras[i] = jObject[propertyName].ToArray().Select(m => m.Value<double>()).ToArray();
                else if (parameters[i].ParameterType == typeof(bool[])) paras[i] = jObject[propertyName].ToArray().Select(m => m.Value<bool>()).ToArray();
                else if (parameters[i].ParameterType == typeof(string[])) paras[i] = jObject[propertyName].ToArray().Select(m => m.Value<string>()).ToArray();
                else if (parameters[i].ParameterType == typeof(DateTime[])) paras[i] = jObject[propertyName].ToArray().Select(m => m.Value<DateTime>()).ToArray();
                else if (parameters[i].ParameterType == typeof(ISessionContext)) paras[i] = context;
                else if (parameters[i].ParameterType == typeof(HttpListenerRequest)) paras[i] = request;
                else if (parameters[i].ParameterType.IsArray) paras[i] = ((JArray)jObject[propertyName]).ToObject(parameters[i].ParameterType);
                else if (parameters[i].ParameterType == typeof(JObject))
                {
                    // 如果定义了JSON类型的对象，就尝试再json对象及其字符串表述形式下都解析一次，如果还是不行，就报终极错误
                    try
                    {
                        paras[i] = (JObject)jObject[propertyName];
                    }
                    catch
                    {
                        paras[i] = JObject.Parse(jObject.Value<string>(propertyName));
                    }
                }
                else
                {
                    try
                    {
                        paras[i] = jObject[propertyName].ToObject(parameters[i].ParameterType);
                    }
                    catch
                    {
                        paras[i] = JObject.Parse(jObject.Value<string>(propertyName)).ToObject(parameters[i].ParameterType);
                    }
                }
                //else throw new Exception( $"Can't support parameter [{propertyName}] type : {parameters[i].ParameterType}"  );
            }
            return paras;
        }

        /// <summary>
        /// 从url数据里解析出真实的数据信息，根据方法参数列表的类型进行反解析，然后返回实际的数据数组<br />
        /// Analyze the real data information from the url data, perform de-analysis according to the type of the method parameter list, 
        /// and then return the actual data array
        /// </summary>
        /// <param name="context">当前的会话内容</param>
        /// <param name="request">当用于Http请求的时候关联的请求头对象</param>
        /// <param name="parameters">提供的参数列表信息</param>
        /// <param name="url">参数变量信息</param>
        /// <returns>已经填好的实际数据的参数数组对象</returns>
        public static object[] GetParametersFromUrl(ISessionContext context, HttpListenerRequest request, ParameterInfo[] parameters, string url)
        {
            if (url.IndexOf('?') > 0) url = url.Substring(url.IndexOf('?') + 1);
            string[] splits = url.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> dict = new Dictionary<string, string>(splits.Length);
            for (int i = 0; i < splits.Length; i++)
            {
                if (!string.IsNullOrEmpty(splits[i]))
                {
                    if (splits[i].IndexOf('=') > 0)
                    {
                        dict.Add(splits[i].Substring(0, splits[i].IndexOf('=')).Trim(' '), splits[i].Substring(splits[i].IndexOf('=') + 1));
                    }
                }
            }

            object[] paras = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType == typeof(byte)) paras[i] = byte.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(short)) paras[i] = short.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(ushort)) paras[i] = ushort.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(int)) paras[i] = int.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(uint)) paras[i] = uint.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(long)) paras[i] = long.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(ulong)) paras[i] = ulong.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(double)) paras[i] = double.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(float)) paras[i] = float.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(bool)) paras[i] = bool.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(string)) paras[i] = dict[parameters[i].Name];
                else if (parameters[i].ParameterType == typeof(DateTime)) paras[i] = DateTime.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(byte[])) paras[i] = dict[parameters[i].Name].ToHexBytes();
                else if (parameters[i].ParameterType == typeof(short[])) paras[i] = dict[parameters[i].Name].ToStringArray<short>();
                else if (parameters[i].ParameterType == typeof(ushort[])) paras[i] = dict[parameters[i].Name].ToStringArray<ushort>();
                else if (parameters[i].ParameterType == typeof(int[])) paras[i] = dict[parameters[i].Name].ToStringArray<int>();
                else if (parameters[i].ParameterType == typeof(uint[])) paras[i] = dict[parameters[i].Name].ToStringArray<uint>();
                else if (parameters[i].ParameterType == typeof(long[])) paras[i] = dict[parameters[i].Name].ToStringArray<long>();
                else if (parameters[i].ParameterType == typeof(ulong[])) paras[i] = dict[parameters[i].Name].ToStringArray<ulong>();
                else if (parameters[i].ParameterType == typeof(float[])) paras[i] = dict[parameters[i].Name].ToStringArray<float>();
                else if (parameters[i].ParameterType == typeof(double[])) paras[i] = dict[parameters[i].Name].ToStringArray<double>();
                else if (parameters[i].ParameterType == typeof(bool[])) paras[i] = dict[parameters[i].Name].ToStringArray<bool>();
                else if (parameters[i].ParameterType == typeof(string[])) paras[i] = dict[parameters[i].Name].ToStringArray<string>();
                else if (parameters[i].ParameterType == typeof(DateTime[])) paras[i] = dict[parameters[i].Name].ToStringArray<DateTime>();
                else if (parameters[i].ParameterType == typeof(ISessionContext)) paras[i] = context;
                else if (parameters[i].ParameterType == typeof(HttpListenerRequest)) paras[i] = request;
                else paras[i] = JToken.Parse(dict[parameters[i].Name]).ToObject(parameters[i].ParameterType);
                //else throw new Exception( $"Can't support parameter [{parameters[i].Name}] type : {parameters[i].ParameterType}"  );
            }
            return paras;
        }
        /// <summary>
        /// 从方法的参数列表里，提取出实际的示例参数信息，返回一个json对象，注意：该数据是示例的数据，具体参数的限制参照服务器返回的数据声明。<br />
        /// From the parameter list of the method, extract the actual example parameter information, and return a json object. Note: The data is the example data, 
        /// and the specific parameter restrictions refer to the data declaration returned by the server.
        /// </summary>
        /// <param name="method">当前需要解析的方法名称</param>
        /// <param name="parameters">当前的参数列表信息</param>
        /// <returns>当前的参数对象信息</returns>
        public static JObject GetParametersFromJson(MethodInfo method, ParameterInfo[] parameters)
        {
            JObject jObject = new JObject();
            for (int i = 0; i < parameters.Length; i++)
            {
#if NET20 || NET35
				if      (parameters[i].ParameterType == typeof( byte ))       jObject.Add( parameters[i].Name, new JValue( default( byte ) ) );
				else if (parameters[i].ParameterType == typeof( short ))      jObject.Add( parameters[i].Name, new JValue( default( short ) ) );
				else if (parameters[i].ParameterType == typeof( ushort ))     jObject.Add( parameters[i].Name, new JValue( default( ushort ) ) );
				else if (parameters[i].ParameterType == typeof( int ))        jObject.Add( parameters[i].Name, new JValue( default( int ) ) );
				else if (parameters[i].ParameterType == typeof( uint ))       jObject.Add( parameters[i].Name, new JValue( default( uint ) ) );
				else if (parameters[i].ParameterType == typeof( long ))       jObject.Add( parameters[i].Name, new JValue( default( long ) ) );
				else if (parameters[i].ParameterType == typeof( ulong ))      jObject.Add( parameters[i].Name, new JValue( default( ulong ) ) );
				else if (parameters[i].ParameterType == typeof( double ))     jObject.Add( parameters[i].Name, new JValue( default( double ) ) );
				else if (parameters[i].ParameterType == typeof( float ))      jObject.Add( parameters[i].Name, new JValue( default( float ) ) );
				else if (parameters[i].ParameterType == typeof( bool ))       jObject.Add( parameters[i].Name, new JValue( default( bool ) ) );
				else if (parameters[i].ParameterType == typeof( string ))     jObject.Add( parameters[i].Name, new JValue( "" ) );
				else if (parameters[i].ParameterType == typeof( DateTime ))   jObject.Add( parameters[i].Name, new JValue( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") ) );
				else if (parameters[i].ParameterType == typeof( byte[] ))     jObject.Add( parameters[i].Name, new JValue( "00 1A 2B 3C 4D" ) );
				else if (parameters[i].ParameterType == typeof( short[] ))    jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( ushort[] ))   jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( int[] ))      jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( uint[] ))     jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( long[] ))     jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( ulong[] ))    jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( float[] ))    jObject.Add( parameters[i].Name, new JArray( new float[] { 1f, 2f, 3f } ) );
				else if (parameters[i].ParameterType == typeof( double[] ))   jObject.Add( parameters[i].Name, new JArray( new double[] { 1d, 2d, 3d } ) );
				else if (parameters[i].ParameterType == typeof( bool[] ))     jObject.Add( parameters[i].Name, new JArray( new bool[] { true, false, false } ) );
				else if (parameters[i].ParameterType == typeof( string[] ))   jObject.Add( parameters[i].Name, new JArray( new string[] { "1", "2", "3" } ) );
				else if (parameters[i].ParameterType == typeof( DateTime[] )) jObject.Add( parameters[i].Name, new JArray( new string[] { DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ) } ) );
				else if (parameters[i].ParameterType == typeof( ISessionContext )) continue;
				else if (parameters[i].ParameterType == typeof( System.Net.HttpListenerRequest )) continue;
				else if (parameters[i].ParameterType.IsArray)                 jObject.Add( parameters[i].Name,  JToken.FromObject( GetObjFromArrayParameterType( parameters[i].ParameterType ) ) );
				else jObject.Add( parameters[i].Name, JToken.FromObject( Activator.CreateInstance( parameters[i].ParameterType ) ) );
				//else throw new Exception( $"Can't support parameter [{parameters[i].Name}] type : {parameters[i].ParameterType}" );
#else
                if (parameters[i].ParameterType == typeof(byte)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (byte)parameters[i].DefaultValue : default(byte)));
                else if (parameters[i].ParameterType == typeof(short)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (short)parameters[i].DefaultValue : default(short)));
                else if (parameters[i].ParameterType == typeof(ushort)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (ushort)parameters[i].DefaultValue : default(ushort)));
                else if (parameters[i].ParameterType == typeof(int)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (int)parameters[i].DefaultValue : default(int)));
                else if (parameters[i].ParameterType == typeof(uint)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (uint)parameters[i].DefaultValue : default(uint)));
                else if (parameters[i].ParameterType == typeof(long)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (long)parameters[i].DefaultValue : default(long)));
                else if (parameters[i].ParameterType == typeof(ulong)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (ulong)parameters[i].DefaultValue : default(ulong)));
                else if (parameters[i].ParameterType == typeof(double)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (double)parameters[i].DefaultValue : default(double)));
                else if (parameters[i].ParameterType == typeof(float)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (float)parameters[i].DefaultValue : default(float)));
                else if (parameters[i].ParameterType == typeof(bool)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (bool)parameters[i].DefaultValue : default(bool)));
                else if (parameters[i].ParameterType == typeof(string)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (string)parameters[i].DefaultValue : ""));
                else if (parameters[i].ParameterType == typeof(DateTime)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((DateTime)parameters[i].DefaultValue).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                else if (parameters[i].ParameterType == typeof(byte[])) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((byte[])parameters[i].DefaultValue).ToHexString() : "00 1A 2B 3C 4D"));
                else if (parameters[i].ParameterType == typeof(short[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (short[])parameters[i].DefaultValue : new short[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(ushort[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (ushort[])parameters[i].DefaultValue : new ushort[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(int[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (int[])parameters[i].DefaultValue : new int[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(uint[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (uint[])parameters[i].DefaultValue : new uint[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(long[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (long[])parameters[i].DefaultValue : new long[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(ulong[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (ulong[])parameters[i].DefaultValue : new ulong[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(float[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (float[])parameters[i].DefaultValue : new float[] { 1f, 2f, 3f }));
                else if (parameters[i].ParameterType == typeof(double[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (double[])parameters[i].DefaultValue : new double[] { 1d, 2d, 3d }));
                else if (parameters[i].ParameterType == typeof(bool[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (bool[])parameters[i].DefaultValue : new bool[] { true, false, false }));
                else if (parameters[i].ParameterType == typeof(string[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (string[])parameters[i].DefaultValue : new string[] { "1", "2", "3" }));
                else if (parameters[i].ParameterType == typeof(DateTime[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? ((DateTime[])parameters[i].DefaultValue).Select(m => m.ToString("yyyy-MM-dd HH:mm:ss")).ToArray() : new string[] { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }));
                else if (parameters[i].ParameterType == typeof(ISessionContext)) continue;
                else if (parameters[i].ParameterType == typeof(System.Net.HttpListenerRequest)) continue;
                else if (parameters[i].ParameterType.IsArray) jObject.Add(parameters[i].Name, parameters[i].HasDefaultValue ? JToken.FromObject(parameters[i].DefaultValue) : JToken.FromObject(GetObjFromArrayParameterType(parameters[i].ParameterType)));
                else jObject.Add(parameters[i].Name, JToken.FromObject(parameters[i].HasDefaultValue ? parameters[i].DefaultValue : Activator.CreateInstance(parameters[i].ParameterType)));
                // else throw new Exception( $"Can't support parameter [{parameters[i].Name}] type : {parameters[i].ParameterType}" );
#endif
            }
            return jObject;
        }

        private static object GetObjFromArrayParameterType(Type parameterType)
        {
            Type actualType = null;
            Type[] types = parameterType.GetGenericArguments();
            if (types.Length > 0)
            {
                actualType = types[0];
            }
            else
            {
                actualType = parameterType.GetElementType();
            }
            Array array = Array.CreateInstance(actualType, 3);
            for (int j = 0; j < 3; j++)
            {
                array.SetValue(Activator.CreateInstance(actualType), j);
            }
            return array;
        }

        /// <summary>
        /// 将一个对象转换成 <see cref="OperateResult{T}"/> 的string 类型的对象，用于远程RPC的数据交互 
        /// </summary>
        /// <param name="obj">自定义的对象</param>
        /// <returns>转换之后的结果对象</returns>
        public static OperateResult<string> GetOperateResultJsonFromObj(object obj)
        {
            if (obj is OperateResult result)
            {
                OperateResult<string> ret = new OperateResult<string>();
                ret.IsSuccess = result.IsSuccess;
                ret.ErrorCode = result.ErrorCode;
                ret.Message = result.Message;

                if (result.IsSuccess)
                {
                    var property = obj.GetType().GetProperty("Content");
                    if (property != null)
                    {
                        var retObject = property.GetValue(obj, null);
                        if (retObject != null) ret.Content = retObject.ToJsonString();
                        return ret;
                    }

                    var propertyContent1 = obj.GetType().GetProperty("Content1");
                    if (propertyContent1 == null) return ret;

                    var propertyContent2 = obj.GetType().GetProperty("Content2");
                    if (propertyContent2 == null)
                    {
                        ret.Content = new { Content1 = propertyContent1.GetValue(obj, null) }.ToJsonString();
                        return ret;
                    }

                    var propertyContent3 = obj.GetType().GetProperty("Content3");
                    if (propertyContent3 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent4 = obj.GetType().GetProperty("Content4");
                    if (propertyContent4 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent5 = obj.GetType().GetProperty("Content5");
                    if (propertyContent5 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent6 = obj.GetType().GetProperty("Content6");
                    if (propertyContent6 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent7 = obj.GetType().GetProperty("Content7");
                    if (propertyContent7 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                            Content6 = propertyContent6.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent8 = obj.GetType().GetProperty("Content8");
                    if (propertyContent8 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                            Content6 = propertyContent6.GetValue(obj, null),
                            Content7 = propertyContent7.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent9 = obj.GetType().GetProperty("Content9");
                    if (propertyContent9 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                            Content6 = propertyContent6.GetValue(obj, null),
                            Content7 = propertyContent7.GetValue(obj, null),
                            Content8 = propertyContent8.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent10 = obj.GetType().GetProperty("Content10");
                    if (propertyContent10 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                            Content6 = propertyContent6.GetValue(obj, null),
                            Content7 = propertyContent7.GetValue(obj, null),
                            Content8 = propertyContent8.GetValue(obj, null),
                            Content9 = propertyContent9.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }
                    else
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                            Content6 = propertyContent6.GetValue(obj, null),
                            Content7 = propertyContent7.GetValue(obj, null),
                            Content8 = propertyContent8.GetValue(obj, null),
                            Content9 = propertyContent9.GetValue(obj, null),
                            Content10 = propertyContent10.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }
                }
                return ret;
            }
            else
            {
                return OperateResult.CreateSuccessResult(obj == null ? string.Empty : obj.ToJsonString());
            }
        }

        /// <summary>
        /// 根据提供的类型对象，解析出符合 <see cref="HslDeviceAddressAttribute"/> 特性的地址列表
        /// </summary>
        /// <param name="valueType">数据类型</param>
        /// <param name="deviceType">设备类型</param>
        /// <param name="obj">类型的对象信息</param>
        /// <param name="byteTransform">数据变换对象</param>
        /// <returns>地址列表信息</returns>
        public static List<HslAddressProperty> GetHslPropertyInfos(Type valueType, Type deviceType, object obj, IByteTransform byteTransform)
        {
            List<HslAddressProperty> array = new List<HslAddressProperty>();
            var properties = valueType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int index = 0;
            foreach (var property in properties)
            {
                HslDeviceAddressAttribute hslAttribute = HslReflectionHelper.GetHslDeviceAddressAttribute(deviceType, property);
                if (hslAttribute == null) continue;

                HslAddressProperty adsPropertyInfo = new HslAddressProperty();
                adsPropertyInfo.PropertyInfo = property;
                adsPropertyInfo.DeviceAddressAttribute = hslAttribute;
                adsPropertyInfo.ByteOffset = index;

                Type propertyType = property.PropertyType;
                if (propertyType == typeof(byte))
                {
                    index++;
                    if (obj != null) adsPropertyInfo.Buffer = new byte[] { (byte)property.GetValue(obj, null) };
                }
                else if (propertyType == typeof(short))
                {
                    index += 2;
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((short)property.GetValue(obj, null));
                }
                else if (propertyType == typeof(short[]))
                {
                    index += 2 * (hslAttribute.Length > 0 ? hslAttribute.Length : 1);
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((short[])property.GetValue(obj, null));
                }
                else if (propertyType == typeof(ushort))
                {
                    index += 2;
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((ushort)property.GetValue(obj, null));
                }
                else if (propertyType == typeof(ushort[]))
                {
                    index += 2 * (hslAttribute.Length > 0 ? hslAttribute.Length : 1);
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((ushort[])property.GetValue(obj, null));
                }
                else if (propertyType == typeof(int))
                {
                    index += 4;
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((int)property.GetValue(obj, null));
                }
                else if (propertyType == typeof(int[]))
                {
                    index += 4 * (hslAttribute.Length > 0 ? hslAttribute.Length : 1);
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((int[])property.GetValue(obj, null));
                }
                else if (propertyType == typeof(uint))
                {
                    index += 4;
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((uint)property.GetValue(obj, null));
                }
                else if (propertyType == typeof(uint[]))
                {
                    index += 4 * (hslAttribute.Length > 0 ? hslAttribute.Length : 1);
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((uint[])property.GetValue(obj, null));
                }
                else if (propertyType == typeof(long))
                {
                    index += 8;
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((long)property.GetValue(obj, null));
                }
                else if (propertyType == typeof(long[]))
                {
                    index += 8 * (hslAttribute.Length > 0 ? hslAttribute.Length : 1);
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((long[])property.GetValue(obj, null));
                }
                else if (propertyType == typeof(ulong))
                {
                    index += 8;
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((ulong)property.GetValue(obj, null));
                }
                else if (propertyType == typeof(ulong[]))
                {
                    index += 8 * (hslAttribute.Length > 0 ? hslAttribute.Length : 1);
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((ulong[])property.GetValue(obj, null));
                }
                else if (propertyType == typeof(float))
                {
                    index += 4;
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((float)property.GetValue(obj, null));
                }
                else if (propertyType == typeof(float[]))
                {
                    index += 4 * (hslAttribute.Length > 0 ? hslAttribute.Length : 1);
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((float[])property.GetValue(obj, null));
                }
                else if (propertyType == typeof(double))
                {
                    index += 8;
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((double)property.GetValue(obj, null));
                }
                else if (propertyType == typeof(double[]))
                {
                    index += 8 * (hslAttribute.Length > 0 ? hslAttribute.Length : 1);
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((double[])property.GetValue(obj, null));
                }
                else if (propertyType == typeof(string))
                {
                    index += hslAttribute.Length > 0 ? hslAttribute.Length : 1;
                    if (obj != null) adsPropertyInfo.Buffer = byteTransform.TransByte((string)property.GetValue(obj, null), Encoding.ASCII);
                }
                else if (propertyType == typeof(byte[]))
                {
                    index += hslAttribute.Length > 0 ? hslAttribute.Length : 1;
                    if (obj != null) adsPropertyInfo.Buffer = (byte[])property.GetValue(obj, null);
                }
                else if (propertyType == typeof(bool))
                {
                    index++;
                    if (obj != null) adsPropertyInfo.Buffer = (bool)property.GetValue(obj, null) ? new byte[1] { 0x01 } : new byte[] { 0x00 };
                }
                else if (propertyType == typeof(bool[]))
                {
                    index += hslAttribute.Length > 0 ? hslAttribute.Length : 1;
                    if (obj != null) adsPropertyInfo.Buffer = ((bool[])property.GetValue(obj, null)).Select(m => m ? (byte)0x01 : (byte)0x00).ToArray();
                }

                adsPropertyInfo.ByteLength = index - adsPropertyInfo.ByteOffset;
                array.Add(adsPropertyInfo);
            }
            return array;
        }

        /// <summary>
        /// 根据地址列表信息，数据缓存，自动解析基础类型的数据，赋值到自定义的对象上去
        /// </summary>
        /// <param name="byteTransform">数据解析对象</param>
        /// <param name="obj">数据对象信息</param>
        /// <param name="properties">地址属性列表</param>
        /// <param name="buffer">缓存数据信息</param>
        public static void SetPropertyValueFrom(IByteTransform byteTransform, object obj, List<HslAddressProperty> properties, byte[] buffer)
        {
            foreach (var propertyInfo in properties)
            {
                Type propertyType = propertyInfo.PropertyInfo.PropertyType;
                object value = null;
                if (propertyType == typeof(byte)) value = buffer[propertyInfo.ByteOffset];
                else if (propertyType == typeof(short)) value = byteTransform.TransInt16(buffer, propertyInfo.ByteOffset);
                else if (propertyType == typeof(short[])) value = byteTransform.TransInt16(buffer, propertyInfo.ByteOffset, propertyInfo.DeviceAddressAttribute.GetDataLength());
                else if (propertyType == typeof(ushort)) value = byteTransform.TransUInt16(buffer, propertyInfo.ByteOffset);
                else if (propertyType == typeof(ushort[])) value = byteTransform.TransUInt16(buffer, propertyInfo.ByteOffset, propertyInfo.DeviceAddressAttribute.GetDataLength());
                else if (propertyType == typeof(int)) value = byteTransform.TransInt32(buffer, propertyInfo.ByteOffset);
                else if (propertyType == typeof(int[])) value = byteTransform.TransInt32(buffer, propertyInfo.ByteOffset, propertyInfo.DeviceAddressAttribute.GetDataLength());
                else if (propertyType == typeof(uint)) value = byteTransform.TransUInt32(buffer, propertyInfo.ByteOffset);
                else if (propertyType == typeof(uint[])) value = byteTransform.TransUInt32(buffer, propertyInfo.ByteOffset, propertyInfo.DeviceAddressAttribute.GetDataLength());
                else if (propertyType == typeof(long)) value = byteTransform.TransInt64(buffer, propertyInfo.ByteOffset);
                else if (propertyType == typeof(long[])) value = byteTransform.TransInt64(buffer, propertyInfo.ByteOffset, propertyInfo.DeviceAddressAttribute.GetDataLength());
                else if (propertyType == typeof(ulong)) value = byteTransform.TransUInt64(buffer, propertyInfo.ByteOffset);
                else if (propertyType == typeof(ulong[])) value = byteTransform.TransUInt64(buffer, propertyInfo.ByteOffset, propertyInfo.DeviceAddressAttribute.GetDataLength());
                else if (propertyType == typeof(float)) value = byteTransform.TransSingle(buffer, propertyInfo.ByteOffset);
                else if (propertyType == typeof(float[])) value = byteTransform.TransSingle(buffer, propertyInfo.ByteOffset, propertyInfo.DeviceAddressAttribute.GetDataLength());
                else if (propertyType == typeof(double)) value = byteTransform.TransDouble(buffer, propertyInfo.ByteOffset);
                else if (propertyType == typeof(double[])) value = byteTransform.TransDouble(buffer, propertyInfo.ByteOffset, propertyInfo.DeviceAddressAttribute.GetDataLength());
                else if (propertyType == typeof(string)) value = Encoding.ASCII.GetString(buffer, propertyInfo.ByteOffset, propertyInfo.DeviceAddressAttribute.GetDataLength());
                else if (propertyType == typeof(byte[])) value = buffer.SelectMiddle(propertyInfo.ByteOffset, propertyInfo.DeviceAddressAttribute.GetDataLength());
                else if (propertyType == typeof(bool)) value = buffer[propertyInfo.ByteOffset] != 0x00;
                else if (propertyType == typeof(bool[])) value = buffer.SelectMiddle(propertyInfo.ByteOffset, propertyInfo.DeviceAddressAttribute.GetDataLength()).Select(m => m != 0x00).ToArray();

                if (value != null) propertyInfo.PropertyInfo.SetValue(obj, value, null);
            }
        }

        #endregion

    }
}
