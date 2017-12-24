using System;
using System.Collections.Generic;
using System.Text;

namespace All.Class
{
    /// <summary>
    /// 将All.Class.TypeUse.TypeList中指定类型数据与可以传输的序列化数据(即数组)相互转换
    /// </summary>
    public class Serialization
    {
        /// <summary>
        /// 将单个数据转换为字节
        /// </summary>
        /// <typeparam name="T">数据类型,只支持All.Class.TypeUse.TypeList中指定的数据类型</typeparam>
        /// <param name="value">数据值</param>
        /// <returns>返回序列化后的数据</returns>
        public static byte[] ValueToBuff<T>(T value)
        {
            List<T> tmp = new List<T>();
            tmp.Add(value);
            return ValueToBuff<T>(tmp);
        }
        /// <summary>
        /// 将数组数据转化为字节
        /// </summary>
        /// <typeparam name="T">数据类型,只支持All.Class.TypeUse.TypeList中指定的数据类型</typeparam>
        /// <param name="value">数组数据</param>
        /// <returns>返回序列化后的数据</returns>
        public static byte[] ValueToBuff<T>(List<T> value)
        {
            if (value == null)
            {
                return null;
            }
            List<byte> result = new List<byte>();
            TypeUse.TypeList t = TypeUse.GetType<T>();
            result.Add((byte)(t));
            switch (t)
            {
                case TypeUse.TypeList.Boolean:
                    for (int i = 0; i < value.Count; i++)
                    {
                        result.AddRange(BitConverter.GetBytes((bool)(object)(value[i])));
                    }
                    break;
                case TypeUse.TypeList.Byte:
                    for (int i = 0; i < value.Count; i++)
                    {
                        result.Add((byte)(object)value[i]);
                    }
                    break;
                case TypeUse.TypeList.Bytes:
                    break;
                case TypeUse.TypeList.DateTime:
                    DateTime tmpTime;
                    for (int i = 0; i < value.Count; i++)
                    {
                        tmpTime = (DateTime)(object)value[i];
                        result.Add((byte)(tmpTime.Year - 1900));
                        result.Add((byte)tmpTime.Month);
                        result.Add((byte)tmpTime.Day);
                        result.Add((byte)tmpTime.Hour);
                        result.Add((byte)tmpTime.Minute);
                        result.Add((byte)tmpTime.Second);
                        result.Add((byte)((tmpTime.Millisecond >> 4) & 0xFF));//毫秒精度缩小4倍来用
                    }
                    break;
                case TypeUse.TypeList.Double:
                    for (int i = 0; i < value.Count; i++)
                    {
                        result.AddRange(BitConverter.GetBytes((double)(object)(value[i])));
                    }
                    break;
                case TypeUse.TypeList.Float:
                    for (int i = 0; i < value.Count; i++)
                    {
                        result.AddRange(BitConverter.GetBytes((float)(object)(value[i])));
                    }
                    break;
                case TypeUse.TypeList.Int:
                    for (int i = 0; i < value.Count; i++)
                    {
                        result.AddRange(BitConverter.GetBytes((int)(object)(value[i])));
                    }
                    break;
                case TypeUse.TypeList.Long:
                    for (int i = 0; i < value.Count; i++)
                    {
                        result.AddRange(BitConverter.GetBytes((long)(object)(value[i])));
                    }
                    break;
                case TypeUse.TypeList.String:
                    result.Add((byte)((value.Count >> 8) & 0xFF));
                    result.Add((byte)((value.Count >> 0) & 0xFF));
                    byte[] tmpStr;
                    for (int i = 0; i < value.Count; i++)
                    {
                        tmpStr = Encoding.UTF8.GetBytes((string)(object)(value[i]));
                        result.Add((byte)((tmpStr.Length >> 8) & 0xFF));
                        result.Add((byte)((tmpStr.Length >> 0) & 0xFF));
                        result.AddRange(tmpStr);
                    }
                    break;
                case TypeUse.TypeList.UShort:
                    for (int i = 0; i < value.Count; i++)
                    {
                        result.AddRange(BitConverter.GetBytes((ushort)(object)(value[i])));
                    }
                    break;
            }
            return result.ToArray();
        }
        /// <summary>
        /// 将序列化后的数据还原为All.Class.TypeUse.TypeList指定类型的数组
        /// </summary>
        /// <param name="buff">序列化后的数组</param>
        /// <param name="start">序列化数据开始的位置</param>
        /// <param name="len">序列化数据的长度</param>
        /// <returns>充列化数据还原后的数组</returns>
        public static object BuffToValue(byte[] buff, int start, int len)
        {

            byte[] tmpBuff = new byte[len];
            for (int i = 0; i < len; i++)
            {
                tmpBuff[i] = buff[start + i];
            }
            if (tmpBuff == null || tmpBuff.Length < 1)
            {
                return null;
            }
            TypeUse.TypeList t = (TypeUse.TypeList)tmpBuff[0];
            switch (t)
            {
                case TypeUse.TypeList.Boolean:
                    List<bool> tmpBool = new List<bool>();
                    for (int i = 1; i < tmpBuff.Length; i++)
                    {
                        tmpBool.Add(BitConverter.ToBoolean(tmpBuff, i));
                    }
                    return tmpBool;
                case TypeUse.TypeList.Byte:
                    List<byte> tmpByte = new List<byte>();
                    for (int i = 1; i < tmpBuff.Length; i++)
                    {
                        tmpByte.Add(tmpBuff[i]);
                    }
                    return tmpByte;
                case TypeUse.TypeList.DateTime:
                    List<DateTime> tmpDateTime = new List<DateTime>();
                    for (int i = 1; i < tmpBuff.Length; i = i + 7)
                    {
                        tmpDateTime.Add(new DateTime(tmpBuff[i] + 1900,
                            tmpBuff[i + 1], tmpBuff[i + 2], tmpBuff[i + 3], tmpBuff[i + 4], tmpBuff[i + 5], (tmpBuff[i + 6] << 4)));
                    }
                    return tmpDateTime;
                case TypeUse.TypeList.Double:
                    List<double> tmpDouble = new List<double>();
                    for (int i = 1; i < tmpBuff.Length; i = i + 8)
                    {
                        tmpDouble.Add(BitConverter.ToDouble(tmpBuff, i));
                    }
                    return tmpDouble;
                case TypeUse.TypeList.Long:
                    List<long> tmplong = new List<long>();
                    for (int i = 1; i < tmpBuff.Length; i = i + 8)
                    {
                        tmplong.Add(BitConverter.ToInt64(tmpBuff, i));
                    }
                    return tmplong;
                case TypeUse.TypeList.Float:
                    List<float> tmpFloat = new List<float>();
                    for (int i = 1; i < tmpBuff.Length; i = i + 4)
                    {
                        tmpFloat.Add(BitConverter.ToSingle(tmpBuff, i));
                    }
                    return tmpFloat;
                case TypeUse.TypeList.Int:
                    List<int> tmpInt = new List<int>();
                    for (int i = 1; i < tmpBuff.Length; i = i + 4)
                    {
                        tmpInt.Add(BitConverter.ToInt32(tmpBuff, i));
                    }
                    return tmpInt;
                case TypeUse.TypeList.UShort:
                    List<ushort> tmpUshort = new List<ushort>();
                    for (int i = 1; i < tmpBuff.Length; i = i + 2)
                    {
                        tmpUshort.Add(BitConverter.ToUInt16(tmpBuff, i));
                    }
                    return tmpUshort;
                case TypeUse.TypeList.String:
                    List<string> tmpString = new List<string>();
                    int strLen = 0, strStart = 3;
                    for (int i = 0; i < ((tmpBuff[1] << 8) + tmpBuff[2]); i++)
                    {
                        strLen = (tmpBuff[strStart] << 8) + tmpBuff[strStart + 1];
                        tmpString.Add(Encoding.UTF8.GetString(tmpBuff, strStart + 2, strLen));
                        strStart = strStart + 2 + strLen;
                    }
                    return tmpString;
            }

            return null;
        }
    }
}
