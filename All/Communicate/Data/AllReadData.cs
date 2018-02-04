using System;
using System.Collections.Generic;
using System.Text;

namespace All.Communicate.Data
{
    /// <summary>
    /// 所有读取数据
    /// </summary>
    public class AllData
    {
        /// <summary>
        /// 字节数据
        /// </summary>
        public DataDetial<byte> ByteValue
        { get; set; }
        /// <summary>
        /// 整形数据
        /// </summary>
        public DataDetial<int> IntValue
        { get; set; }
        /// <summary>
        /// 无符号整数
        /// </summary>
        public DataDetial<ushort> UshortValue
        { get; set; }
        /// <summary>
        /// 单精度浮点数
        /// </summary>
        public DataDetial<float> FloatValue
        { get; set; }
        /// <summary>
        /// 双精度浮点数
        /// </summary>
        public DataDetial<double> DoubleValue
        { get; set; }
        /// <summary>
        /// 长整形数据
        /// </summary>
        public DataDetial<long> LongValue
        { get; set; }
        /// <summary>
        /// 日期数据
        /// </summary>
        public DataDetial<DateTime> DateTimeValue
        { get; set; }
        /// <summary>
        /// 布尔数据
        /// </summary>
        public DataDetial<bool> BoolValue
        { get; set; }
        /// <summary>
        /// 字符串数据
        /// </summary>
        public DataDetial<string> StringValue
        { get; set; }
        /// <summary>
        /// 字符数组数据
        /// </summary>
        public DataDetial<byte[]> BytesValue
        { get; set; }
        public AllData()
        {
            this.BoolValue = new DataDetial<bool>();
            this.BytesValue = new DataDetial<byte[]>();
            this.ByteValue = new DataDetial<byte>();
            this.DateTimeValue = new DataDetial<DateTime>();
            this.DoubleValue = new DataDetial<double>();
            this.FloatValue = new DataDetial<float>();
            this.IntValue = new DataDetial<int>();
            this.LongValue = new DataDetial<long>();
            this.StringValue = new DataDetial<string>();
            this.UshortValue = new DataDetial<ushort>();
        }
        /// <summary>
        /// 设置读取后的数据
        /// </summary>
        /// <param name="t"></param>
        /// <param name="buff"></param>
        public void SetValue(All.Class.TypeUse.TypeList t, Dictionary<int, object> buff)
        {
            if (buff == null || buff.Count <= 0)
            {
                return;
            }
            switch (t)
            {
                case Class.TypeUse.TypeList.Boolean:
                    foreach (int i in buff.Keys)
                    {
                        this.BoolValue.SetValue(i, (bool)buff[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.Byte:
                    foreach (int i in buff.Keys)
                    {
                        this.ByteValue.SetValue(i, (byte)buff[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.Bytes:
                case Class.TypeUse.TypeList.UnKnow:
                    break;
                case Class.TypeUse.TypeList.DateTime:
                    foreach (int i in buff.Keys)
                    {
                        this.DateTimeValue.SetValue(i, (DateTime)buff[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.Double:
                    foreach (int i in buff.Keys)
                    {
                        this.DoubleValue.SetValue(i, (double)buff[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.Float:
                    foreach (int i in buff.Keys)
                    {
                        this.FloatValue.SetValue(i, (float)buff[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.Int:
                    foreach (int i in buff.Keys)
                    {
                        this.IntValue.SetValue(i, (int)buff[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.Long:
                    foreach (int i in buff.Keys)
                    {
                        this.LongValue.SetValue(i, (long)buff[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.String:
                    foreach (int i in buff.Keys)
                    {
                        this.StringValue.SetValue(i, (string)buff[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.UShort:
                    foreach (int i in buff.Keys)
                    {
                        this.UshortValue.SetValue(i, (ushort)buff[i]);
                    }
                    break;
            }
        }
        /// <summary>
        /// 读取的数据组,添加批量读取项目
        /// </summary>
        /// <param name="t"></param>
        /// <param name="buff"></param>
        public void AddRange(All.Class.TypeUse.TypeList t, Dictionary<int, string> buff)
        {
            switch (t)
            {
                case Class.TypeUse.TypeList.Boolean:
                    this.BoolValue.AddRange(buff);
                    break;
                case Class.TypeUse.TypeList.Byte:
                    this.ByteValue.AddRange(buff);
                    break;
                case Class.TypeUse.TypeList.Bytes:
                    this.BytesValue.AddRange(buff);
                    break;
                case Class.TypeUse.TypeList.DateTime:
                    this.DateTimeValue.AddRange(buff);
                    break;
                case Class.TypeUse.TypeList.Double:
                    this.DoubleValue.AddRange(buff);
                    break;
                case Class.TypeUse.TypeList.Float:
                    this.FloatValue.AddRange(buff);
                    break;
                case Class.TypeUse.TypeList.Int:
                    this.IntValue.AddRange(buff);
                    break;
                case Class.TypeUse.TypeList.Long:
                    this.LongValue.AddRange(buff);
                    break;
                case Class.TypeUse.TypeList.String:
                    this.StringValue.AddRange(buff);
                    break;
                case Class.TypeUse.TypeList.UnKnow:
                    break;
                case Class.TypeUse.TypeList.UShort:
                    this.UshortValue.AddRange(buff);
                    break;
            }
        }
    }
}
