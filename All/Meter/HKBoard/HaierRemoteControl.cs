using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    /// <summary>
    /// 弘科制造 海尔遥控 板
    /// </summary>
    public class HaierRemoteControl : Meter
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
        public override void Init(Dictionary<string, string> initParm)
        {
            if (!initParm.ContainsKey("Address"))
            {
                address = 0x20;
            }
            else
            {
                address = All.Class.Num.ToByte(InitParm["Address"]);
            }
            base.Init(initParm);
        }
        public override bool Test()
        {
            return base.Test();
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                try
                {
                    string str = "";
                    switch (All.Class.TypeUse.GetType<T>())
                    {
                        case Class.TypeUse.TypeList.String:
                            str = (string)(object)value[0];
                            break;
                        case Class.TypeUse.TypeList.Byte:
                            for (int i = 0; i < value.Count; i++)
                            {
                                str = string.Format("{0}{1:X2}", str, value[i]);
                            }
                            break;
                        default:
                            All.Class.Error.Add("RemoteControl只能写入string类型或byte数据");
                            break;
                    }
                    byte[] readBuff;
                    byte[] sendBuff = new byte[str.Length + 12];
                    sendBuff[0] = address;
                    sendBuff[1] = 0x43;
                    sendBuff[2] = (byte)(str.Length * 4);
                    sendBuff[3] = 0x26;//数据值为1时的时间占比
                    sendBuff[4] = 0x4D;//'M'标识符
                    sendBuff[5] = 0x03;//'M'标识符后面有3个字节
                    sendBuff[6] = 0xBB;//引导码
                    sendBuff[7] = 0xBF;//引导码
                    sendBuff[8] = 0x01;//引导码
                    byte[] buff = Encoding.ASCII.GetBytes(str);
                    Array.Copy(buff, 0, sendBuff, 9, buff.Length);
                    sendBuff[sendBuff.Length - 3] = 0x4B;//'K'
                    All.Class.Check.Crc16(sendBuff, sendBuff.Length - 2, out sendBuff[sendBuff.Length - 2], out sendBuff[sendBuff.Length - 1]);
                    if (WriteAndRead<byte[], byte[]>(sendBuff, 8, out readBuff))
                    {
                        if (readBuff[0] != sendBuff[0] || readBuff[1] != sendBuff[1])
                        {
                            All.Class.Error.Add("HaierRemoteControl返回数据校验错误");
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                    return false;
                }
                return true;
            }
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            throw new NotImplementedException();
        }
    }
}
