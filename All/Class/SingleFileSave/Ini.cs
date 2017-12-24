using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Class.SingleFileSave
{
    /// <summary>
    /// INI文件格式保存文本
    /// </summary>
    public static class Ini
    {
        /// <summary>
        /// 将指定数据写入指定ini文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="section">写入小节名称通常包含在[]之间</param>
        /// <param name="key">写入字段名称</param>
        /// <param name="value">写入值</param>
        public static void Write(string fileName, string section, string key, string value)
        {
            All.Class.Api.WritePrivateProfileString(section, key, value, fileName);
        }
        /// <summary>
        /// 从文件读取指定数据
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="section">读取小节名称</param>
        /// <param name="key">读取字段名称</param>
        /// <param name="defaultValue">当读取字段不存在时,返回的默认值</param>
        /// <returns>返回读取后的数据</returns>
        public static string Read(string fileName, string section, string key, string defaultValue)
        {
            StringBuilder result = new StringBuilder(255);
            try
            {
                All.Class.Api.GetPrivateProfileString(section, key, defaultValue, result, 255, fileName);
            }
            catch { }
            return result.ToString().Trim();
        }
        /// <summary>
        /// 将字典数据,转换为按ini存储的字符串样式
        /// </summary>
        /// <param name="section">储存的小节名称</param>
        /// <param name="buff">存储数据的字典</param>
        /// <returns>返回ini格式的数据</returns>
        public static string Dictionary2Text(string section, Dictionary<string, string> buff)
        {
            StringBuilder result = new StringBuilder(500);
            result.Append(string.Format("[{0}]\r\n", section));
            buff.Keys.ToList().ForEach(key =>
            {
                result.Append(string.Format("{0}={1}\r\n", key, buff[key].Trim()));
            });
            return result.ToString().Trim();
        }
        /// <summary>
        /// 从ini格式的文本字符串中,将数据解析为1对1的字典数据
        /// </summary>
        /// <param name="section">存储数据的小节名称</param>
        /// <param name="value">ini格式的文本字符串</param>
        /// <returns>1对1的字典数据</returns>
        public static Dictionary<string, string> Text2Dictionary(string section, string value)
        {
            Dictionary<string, string> buff = new Dictionary<string, string>();
            string[] tmp = value.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            if (tmp != null && tmp.Length > 0)
            {
                for (int i = 0; i < tmp.Length - 1; i++)
                {
                    if (tmp[i].ToUpper() == section.ToUpper())
                    {
                        string[] tmpSection = tmp[i + 1].Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (tmpSection != null && tmpSection.Length > 0)
                        {
                            string key, data;
                            for (int j = 0; j < tmp.Length; j++)
                            {
                                key = tmpSection[j].Substring(0, tmpSection[j].IndexOf('=')).Trim();
                                data = tmpSection[j].Substring(tmpSection[j].IndexOf('=') + 1).Trim();
                                if (!buff.ContainsKey(key))
                                {
                                    buff.Add(key, data);
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return buff;
        }
    }
}
