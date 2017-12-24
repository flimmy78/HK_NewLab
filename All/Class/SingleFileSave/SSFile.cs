using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace All.Class.SingleFileSave
{
    /// <summary>
    /// 自定义的XML文件格式保存文本
    /// </summary>
    public class SSFile
    {
        /// <summary>
        /// 序列化数据中间类,数据序列化与数据反解析
        /// </summary>
        [Serializable]
        public class Nodes//此处因为用到序列化,所以必须为public
        {
            /// <summary>
            /// 单一数据序列化
            /// </summary>
            [Serializable]
            public class Node
            {
                /// <summary>
                /// 标题
                /// </summary>
                public string Title
                { get; set; }
                /// <summary>
                /// 数据
                /// </summary>
                public string Value
                { get; set; }
                /// <summary>
                /// 初始化一个标题为"",数据也为""的待序列化数据
                /// </summary>
                public Node()
                {
                    Title = "";
                    Value = "";
                }
                /// <summary>
                /// 初始化一个指定标题和指定数据的待序列化的数据
                /// </summary>
                /// <param name="title">数据标题</param>
                /// <param name="value">数据值</param>
                public Node(string title, string value)
                {
                    this.Title = title;
                    this.Value = value;
                }
            }
            /// <summary>
            /// 所有待序列化的数据
            /// </summary>
            public List<Node> AllNode
            { get; set; }
            /// <summary>
            /// 初始化一个待序列化的数据集合
            /// </summary>
            public Nodes()
            {
                AllNode = new List<Node>();
            }
            /// <summary>
            /// 将所有数据集合序列化为字符串
            /// </summary>
            /// <returns>序列化后的值</returns>
            public override string ToString()
            {
                string result = "";
                XmlSerializer xs = new XmlSerializer(typeof(Nodes));
                using (MemoryStream stream = new MemoryStream())
                {
                    xs.Serialize(stream, this);
                    stream.Position = 0;
                    StreamReader sr = new StreamReader(stream);
                    result = sr.ReadToEnd();
                }
                return result;
            }
            /// <summary>
            /// 将序列化的字符串解析还原为数据集合
            /// </summary>
            /// <param name="value">待解析的字符串</param>
            public void Init(string value)
            {
                XmlSerializer xs = new XmlSerializer(typeof(Nodes));
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(value);
                        sw.Flush();
                        stream.Position = 0;
                        Nodes tmp = (Nodes)xs.Deserialize(stream);
                        this.AllNode = tmp.AllNode;
                    }
                }
            }
        }
        /// <summary>
        /// 将字典数据转化为标准字符串
        /// </summary>
        /// <param name="buff">待转化的字典</param>
        /// <returns>转换后的字符串</returns>
        public static string Dictionary2Text(Dictionary<string, string> buff)
        {
            Nodes nodes = new Nodes();
            Nodes.Node node;
            buff.Keys.ToList().ForEach(key =>
            {
                node = new Nodes.Node(key, buff[key]);
                nodes.AllNode.Add(node);
            });
            return nodes.ToString();
        }
        /// <summary>
        /// 将序列化后的字符串还原为字典
        /// </summary>
        /// <param name="value">要还原的字符串</param>
        /// <returns>还原后的字典</returns>
        public static Dictionary<string, string> Text2Dictionary(string value)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                Nodes nodes = new Nodes();
                nodes.Init(value);
                nodes.AllNode.ForEach(node =>
                {
                    if (!result.ContainsKey(node.Title))
                    {
                        result.Add(node.Title, node.Value);
                    }
                });
            }
            catch (Exception e)
            {
                All.Class.Error.Add(e);
                All.Class.Error.Add("出错字符", value);
            }
            return result;
        }
        /// <summary>
        /// 将任意类型序列化为字节数组
        /// </summary>
        /// <param name="value">任意类型的数据</param>
        /// <returns>序列化后的字节数组</returns>
        public static byte[] Object2Byte(object value)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, value);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// 将序列化后的数组还原为原类型
        /// </summary>
        /// <param name="buff">序列化的数组</param>
        /// <returns>序列化后还原的数据类型</returns>
        public static object Byte2Object(byte[] buff)
        {
            return Byte2Object(buff, 0, buff.Length);
        }
        /// <summary>
        /// 将序列化后的数组还原为原类型
        /// </summary>
        /// <param name="buff">序列化的数组</param>
        /// <param name="start">序列化数据起始点</param>
        /// <param name="len">序列化数据的长度</param>
        /// <returns>序列化后还原的数据类型</returns>
        public static object Byte2Object(byte[] buff,int start,int len)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(buff, start, len);
                ms.Flush();
                ms.Position = 0;
                return bf.Deserialize(ms);
            }
        }
        /// <summary>
        /// 将序列化后的数组还原为原类型
        /// </summary>
        /// <typeparam name="T">原数据类型</typeparam>
        /// <param name="buff">序列化的数组</param>
        /// <param name="start">序列化数据起始点</param>
        /// <param name="len">序列化数据的长度</param>
        /// <returns>序列化后还原的数据类型</returns>
        public static T Byte2Object<T>(byte[] buff, int start, int len)
        {
            return (T)Byte2Object(buff, start, len);
        }
    }
}
