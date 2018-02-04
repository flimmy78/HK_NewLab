using System;
using System.Collections.Generic;
using System.Text;
namespace All.Meter
{
    public class I7017 : Meter
    {
        Dictionary<string, string> initParm;

        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        byte address = 0;
        /// <summary>
        /// 模块通讯地址
        /// </summary>
        public byte Address
        {
            get { return address; }
            set { address = value; }
        }
        bool cr = true;
        /// <summary>
        /// 是否有回车符
        /// </summary>
        public bool Cr
        {
            get { return cr; }
            set { cr = value; }
        }
        bool lf = true;
        /// <summary>
        /// 是否有换行符
        /// </summary>
        public bool Lf
        {
            get { return lf; }
            set { lf = value; }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            if (initParm.ContainsKey("Cr"))
            {
                if (initParm["Cr"].ToUpper() == "FALSE" || initParm["Cr"] == "0")
                {
                    this.cr = false;
                }
            }
            if (initParm.ContainsKey("Lf"))
            {
                if (initParm["Lf"].ToUpper() == "FALSE" || initParm["Lf"] == "0")
                {
                    this.lf = false;
                }
            }
            if (!initParm.ContainsKey("Address"))
            {
                All.Class.Error.Add("参数中没有地址", Environment.StackTrace);
            }
            else
            {
                address = All.Class.Num.ToByte(initParm["Address"]);
            }
            base.Init(initParm);
        }
        /// <summary>
        /// 按指令读取指定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">读取返回数据</param>
        /// <param name="parm">读取参数,必须包含Code,Address,Len三个参数</param>
        /// <returns></returns>
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                value = new List<T>();
                try
                {
                    string sendBuff = string.Format("#{0:D2}", address);
                    if (cr)
                    {
                        sendBuff = string.Format("{0}\r", sendBuff);
                    }
                    if (lf)
                    {
                        sendBuff = string.Format("{0}\n", sendBuff);
                    }
                    string readBuff;
                    if (WriteAndRead<string, string>(sendBuff, 43, out readBuff))
                    {
                        if (readBuff == null || readBuff == "")
                        {
                            return false;
                        }
                        string[] buff = readBuff.Trim().Split(new char[] { '>', '+', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        string symbol = All.Class.Num.TrimOnlyUse(readBuff, new string[] { "+", "-" });
                        if (buff.Length != symbol.Length)
                        {
                            All.Class.Error.Add("读取到的符号和数据量不相等,没法取值", Environment.StackTrace);
                            return false;
                        }
                        for (int i = 0; i < buff.Length; i++)
                        {
                            switch (All.Class.TypeUse.GetType<T>())
                            {
                                case Class.TypeUse.TypeList.Int:
                                    value.Add((T)(object)((symbol.Substring(i, 1) == "-" ? -1 : 1) * (int)buff[i].Trim().ToFloat()));
                                    break;
                                case Class.TypeUse.TypeList.Float:
                                    value.Add((T)(object)((symbol.Substring(i, 1) == "-" ? -1 : 1) * buff[i].Trim().ToFloat()));
                                    break;
                                case Class.TypeUse.TypeList.Double:
                                    value.Add((T)(object)((symbol.Substring(i, 1) == "-" ? -1 : 1) * buff[i].Trim().ToDouble()));
                                    break;
                                case Class.TypeUse.TypeList.Long:
                                    value.Add((T)(object)((symbol.Substring(i, 1) == "-" ? -1 : 1) * (long)buff[i].Trim().ToFloat()));
                                    break;
                                case Class.TypeUse.TypeList.String:
                                    value.Add((T)(object)(symbol.Substring(i, 1) + buff[i].Trim()));
                                    break;
                                case Class.TypeUse.TypeList.UShort:
                                    value.Add((T)(object)((ushort)(0xFFFF & (int)buff[i].Trim().ToFloat())));
                                    break;
                            }
                        }
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                    result = false;
                }
                return result;
            }
        }
        public override bool Test()
        {
            ushort value = 0;
            return Read<ushort>(out value, 0);
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                All.Class.Error.Add("7017暂时没有写入参数方法", Environment.StackTrace);
                return false;
            }
        }

    }
}
