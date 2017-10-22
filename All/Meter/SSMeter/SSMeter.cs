using System;
using System.Collections.Generic;
using System.Text;

namespace All.Meter.SSMeter
{
    [Serializable]
    public class SSWriteMeter
    {
        /// <summary>
        /// 返回数据接收结果类型
        /// </summary>
        public class WriteResultStyle
        {
            /// <summary>
            /// 随机数据,做数据校验检测用
            /// </summary>
            public int Random
            { get; set; }
            /// <summary>
            /// 接收结果 
            /// </summary>
            public bool Result
            { get; set; }
            public WriteResultStyle()
                : this(0, false)
            { }
            public WriteResultStyle(int random, bool result)
            {
                this.Random = random;
                this.Result = result;
            }
            /// <summary>
            /// 将类转化为发送的数据,不带返回数据
            /// </summary>
            /// <returns></returns>
            public virtual byte[] GetBytes()
            {
                List<byte> buff = new List<byte>();
                buff.AddRange(BitConverter.GetBytes(this.Random));
                buff.AddRange(BitConverter.GetBytes(this.Result));
                buff.Add(All.Class.Check.XorCheck(buff.ToArray(), 0, buff.Count));
                return buff.ToArray();
            }
            /// <summary>
            /// 将字节转化为类
            /// </summary>
            /// <param name="buff"></param>
            /// <returns></returns>
            public static WriteResultStyle Parse(byte[] buff)
            {
                if (buff == null)
                {
                    return null;
                }
                if (buff.Length != 6)
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到数据不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                if (buff[buff.Length - 1] != All.Class.Check.XorCheck(buff, 0, buff.Length - 1))
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到的校验不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                WriteResultStyle result = new WriteResultStyle();
                result.Random = BitConverter.ToInt32(buff, 0);
                result.Result = BitConverter.ToBoolean(buff, 4);
                return result;
            }
        }
        /// <summary>
        /// 发送数据类型
        /// </summary>
        public class WriteDataStyle
        {
            /// <summary>
            /// 发送或接收的数据类型
            /// </summary>
            public All.Class.TypeUse.TypeList Type
            { get; set; }
            /// <summary>
            /// 发送或接收的起始值
            /// </summary>
            public int Start
            { get; set; }
            /// <summary>
            /// 发送或接收的数据
            /// </summary>
            public object Value
            { get; set; }
            /// <summary>
            /// 随机数
            /// </summary>
            public int Random
            { get; set; }
            public WriteDataStyle()
                : this(All.Class.TypeUse.TypeList.UnKnow, 0, null)
            { }
            public WriteDataStyle(All.Class.TypeUse.TypeList type, int start, object value)
            {
                this.Random = (int)All.Class.Num.GetRandom(0, 99999);
                this.Type = type;
                this.Start = start;
                this.Value = value;
            }
            /// <summary>
            /// 将当前类,转化为字节
            /// </summary>
            /// <returns></returns>
            public byte[] GetBytes<T>()
            {
                //类型1,开始4,随机码4,数据n,校验1
                List<byte> buff = new List<byte>();
                buff.Add((byte)Type);
                buff.AddRange(BitConverter.GetBytes(this.Start));
                buff.AddRange(BitConverter.GetBytes(this.Random));
                buff.AddRange(All.Class.Serialization.ValueToBuff<T>((List<T>)this.Value));
                buff.Add(All.Class.Check.XorCheck(buff.ToArray(), 0, buff.Count));
                return buff.ToArray();
            }
            /// <summary>
            /// 将字节转化为类
            /// </summary>
            /// <param name="buff"></param>
            /// <returns></returns>
            public static WriteDataStyle Parse(byte[] buff)
            {
                if (buff == null)
                {
                    return null;
                }
                if (buff.Length < 10)
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到数据不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                if (buff[buff.Length - 1] != All.Class.Check.XorCheck(buff, 0, buff.Length - 1))
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到的校验不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                WriteDataStyle result = new WriteDataStyle();
                result.Type = (All.Class.TypeUse.TypeList)buff[0];
                result.Start = BitConverter.ToInt32(buff, 1);
                result.Random = BitConverter.ToInt32(buff, 5);
                result.Value = All.Class.Serialization.BuffToValue(buff, 9, buff.Length - 10);
                return result;
            }
        }
    }
    [Serializable]
    public class SSReadMeter
    {
        /// <summary>
        /// 返回数据接收结果类型
        /// </summary>
        public class ReadResultStyle
        {
            /// <summary>
            /// 发送或接收的数据类型
            /// </summary>
            public All.Class.TypeUse.TypeList Type
            { get; set; }
            /// <summary>
            /// 随机数据,做数据校验检测用
            /// </summary>
            public int Random
            { get; set; }
            /// <summary>
            /// 接收结果 
            /// </summary>
            public bool Result
            { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public object Value
            { get; set; }
            public ReadResultStyle()
                : this( All.Class.TypeUse.TypeList.UnKnow, 0, false,null)
            { }
            public ReadResultStyle(All.Class.TypeUse.TypeList type,int random, bool result, object value)
            {
                this.Type = type;
                this.Random = random;
                this.Result = result;
                this.Value = value;
            }
            /// <summary>
            /// 将类转化为发送的数据,不带返回数据
            /// </summary>
            /// <returns></returns>
            public virtual byte[] GetBytes<T>()
            {
                List<byte> buff = new List<byte>();
                buff.Add((byte)this.Type);
                buff.AddRange(BitConverter.GetBytes(this.Random));
                buff.AddRange(BitConverter.GetBytes(this.Result));
                buff.AddRange(All.Class.Serialization.ValueToBuff<T>((List<T>)this.Value));
                buff.Add(All.Class.Check.XorCheck(buff.ToArray(), 0, buff.Count));
                return buff.ToArray();
            }
            /// <summary>
            /// 将字节转化为类
            /// </summary>
            /// <param name="buff"></param>
            /// <returns></returns>
            public static ReadResultStyle Parse(byte[] buff)
            {
                if (buff == null)
                {
                    return null;
                }
                if (buff.Length < 7)
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到数据不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                if (buff[buff.Length - 1] != All.Class.Check.XorCheck(buff, 0, buff.Length - 1))
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到的校验不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                ReadResultStyle result = new ReadResultStyle();
                result.Type = (All.Class.TypeUse.TypeList)buff[0];
                result.Random = BitConverter.ToInt32(buff, 1);
                result.Result = BitConverter.ToBoolean(buff, 5);
                result.Value = All.Class.Serialization.BuffToValue(buff, 6, buff.Length - 7);
                return result;
            }
        }
        /// <summary>
        /// 发送数据类型
        /// </summary>
        public class ReadDataStyle
        {
            /// <summary>
            /// 发送或接收的数据类型
            /// </summary>
            public All.Class.TypeUse.TypeList Type
            { get; set; }
            /// <summary>
            /// 标志
            /// </summary>
            public Dictionary<string,string> Symbol
            { get; set; }
            /// <summary>
            /// 随机数
            /// </summary>
            public int Random
            { get; set; }
            public ReadDataStyle()
                : this(All.Class.TypeUse.TypeList.UnKnow, null)
            { }
            public ReadDataStyle(All.Class.TypeUse.TypeList type, Dictionary<string,string> symbol)
            {
                this.Type = type;
                this.Random = (int)All.Class.Num.GetRandom(0, 99999);
                this.Symbol = symbol;
            }
            /// <summary>
            /// 将当前类,转化为字节
            /// </summary>
            /// <returns></returns>
            public byte[] GetBytes()
            {
                //类型1,开始4,随机码4,长度4,校验1
                List<byte> buff = new List<byte>();
                buff.Add((byte)this.Type);
                buff.AddRange(BitConverter.GetBytes(this.Random));
                if (Symbol != null)
                {
                    buff.AddRange(All.Class.Serialization.ValueToBuff<string>(All.Class.SingleFileSave.SSFile.Dictionary2Text(Symbol)));
                }
                buff.Add(All.Class.Check.XorCheck(buff.ToArray(), 0, buff.Count));
                return buff.ToArray();
            }
            /// <summary>
            /// 将字节转化为类
            /// </summary>
            /// <param name="buff"></param>
            /// <returns></returns>
            public static ReadDataStyle Parse(byte[] buff)
            {
                if (buff == null)
                {
                    return null;
                }
                if (buff.Length < 6)
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到数据不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                if (buff[buff.Length - 1] != All.Class.Check.XorCheck(buff, 0, buff.Length - 1))
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到的校验不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                ReadDataStyle result = new ReadDataStyle();
                result.Type = (All.Class.TypeUse.TypeList)buff[0];
                result.Random = BitConverter.ToInt32(buff, 1);
                if (buff.Length > 6)
                {
                    result.Symbol = All.Class.SingleFileSave.SSFile.Text2Dictionary(((List<string>)All.Class.Serialization.BuffToValue(buff, 5, buff.Length - 6))[0]);
                }
                return result;
            }
        }
    }
}
