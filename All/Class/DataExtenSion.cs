using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace System
{
    public static partial class DataExtension
    {
        #region//string
        /// <summary>
        /// 字节数组转化为显示的字符串
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string ToString(this byte[] buff)
        {
            return All.Class.Num.Hex2Str(buff);
        }
        /// <summary>
        /// 十六进制字符串转化为字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToHexBytes(this string str)
        {
            return All.Class.Num.Str2Hex(str);
        }
        /// <summary>
        /// 字符串转化为布尔型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBool(this string str)
        {
            return All.Class.Num.ToBool(str);
        }
        /// <summary>
        /// 字符串转化为字节
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte ToByte(this string str)
        {
            return All.Class.Num.ToByte(str);
        }
        /// <summary>
        /// 字符串转化为整形
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            return All.Class.Num.ToInt(str);
        }
        /// <summary>
        /// 字符串转化为16位无符号整形
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ushort ToUshort(this string str)
        {
            return All.Class.Num.ToUshort(str);
        }
        /// <summary>
        /// 字符串转化长整形
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ToLong(this string str)
        {
            return All.Class.Num.ToLong(str);
        }
        /// <summary>
        /// 字符串转化为单精度 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
        {
            return All.Class.Num.ToFloat(str);
        }
        /// <summary>
        /// 字符串转化为双精度 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            return All.Class.Num.ToDouble(str);
        }
        /// <summary>
        /// 字符串转化为时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            return All.Class.Num.ToDateTime(str);
        }
        #endregion
        #region//字典

        /// <summary>
        /// 将指定文件加载成字典数据
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>返回字典</returns>
        public static Dictionary<string, string> GetBuffFromFile(this string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                return All.Class.SingleFileSave.SSFile.Text2Dictionary(All.Class.FileIO.ReadFile(fileName));
            }
            return new Dictionary<string, string>();
        }
        /// <summary>
        /// 将字典数据保存到文件 
        /// </summary>
        /// <param name="buff">字典数据</param>
        /// <param name="fileName">文件名称</param>
        public static void SaveBuffToFile(this Dictionary<string, string> buff, string fileName)
        {
            if (buff == null)
            {
                return;
            }
            All.Class.FileIO.Write(fileName, All.Class.SingleFileSave.SSFile.Dictionary2Text(buff));
        }
        /// <summary>
        /// 将字典转化为直接显示的数据
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string ToText(this Dictionary<string, string> buff)
        {
            if (buff == null)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            buff.Keys.ToList().ForEach(key =>
                {
                    sb.Append(key);
                    sb.Append("->");
                    sb.Append(buff[key]);
                    sb.Append("\r\n");
                });
            return sb.ToString();
        }
        /// <summary>
        /// 将数组转化为对应的16进制字符串
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] buff)
        {
            return All.Class.Num.Hex2Str(buff);
        }
        #endregion
        #region//类
        /// <summary>
        /// 获取当前类型的深度副本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object DeepClone(this object value)
        {
            try
            {
                return All.Class.SingleFileSave.SSFile.Byte2Object(All.Class.SingleFileSave.SSFile.Object2Byte(value));
            }
            catch
            {
                throw new Exception("请将要克隆的类标记为[Serializable]");
            }
        }
        #endregion
    }
}
