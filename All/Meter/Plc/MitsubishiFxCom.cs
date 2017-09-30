using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter.Plc
{
    public class MitsubishiFxCom:Meter
    {
        bool check = false;
        bool crlf = false;
        int address = 0;
        int delay = 1;
        Dictionary<string, string> initParm;
        public override Dictionary<string, string> InitParm
        {
            get
            {
                return initParm;
            }
            set
            {
                initParm = value;
            }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            if (initParm.ContainsKey("Check"))
            {
                check = initParm["Check"].ToBool();
            }
            if (initParm.ContainsKey("Address"))
            {
                address = initParm["Address"].ToInt();
            }
            if (initParm.ContainsKey("Crlf"))
            {
                crlf = initParm["Crlf"].ToBool();
            }
            if (initParm.ContainsKey("Delay"))
            {
                delay = initParm["Delay"].ToInt();
            }
            if (delay <= 0)
            {
                delay = 1;
            }
            if (delay > 15)
            {
                delay = 15;
            }
            base.Init(initParm);
        }
        public override bool Test()
        {
            ushort value = 0;
            return Read<ushort>(out value, 0);
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                value = new List<T>();
                if (!parm.ContainsKey("Start") && !parm.ContainsKey("Address"))
                {
                    All.Class.Error.Add("数据读取参数中不包含寄存器地址", Environment.StackTrace);
                    return false;
                }
                ushort tmpAddress = 0;
                if (parm.ContainsKey("Start"))
                {
                    tmpAddress = parm["Start"].ToUshort();
                }
                if (parm.ContainsKey("Address"))
                {
                    tmpAddress = parm["Address"].ToUshort();
                }
                if (!parm.ContainsKey("End"))
                {
                    parm.Add("End", tmpAddress.ToString());
                }
                ushort end = parm["End"].ToUshort();
                if (end < tmpAddress)
                {
                    All.Class.Error.Add("读取参数中结束点大于起始点", Environment.StackTrace);
                    return false;
                }
                Class.TypeUse.TypeList t = All.Class.TypeUse.GetType<T>();
                string code = "BR";
                //读取区域
                if (!parm.ContainsKey("Code"))
                {
                    switch (t)
                    {
                        case Class.TypeUse.TypeList.Boolean:
                            parm.Add("Code", "M");
                            code = "BR";
                            break;
                        case Class.TypeUse.TypeList.String:
                        case Class.TypeUse.TypeList.UShort:
                        case Class.TypeUse.TypeList.Int:
                        case Class.TypeUse.TypeList.Long:
                        case Class.TypeUse.TypeList.Byte:
                        case Class.TypeUse.TypeList.Float:
                        case Class.TypeUse.TypeList.Double:
                            parm.Add("Code", "D");
                            code = "WR";
                            break;
                    }
                }
                else
                {
                    switch (t)
                    {
                        case Class.TypeUse.TypeList.Boolean:
                            if (parm["Code"].ToUpper() != "X" && parm["Code"].ToUpper() != "M" && parm["Code"].ToUpper() != "Y")
                            {
                                All.Class.Error.Add("读取参数中区域类型和返回数据类型不一致", Environment.StackTrace);
                                return false;
                            }
                            code = "BR";
                            break;
                        case Class.TypeUse.TypeList.String:
                        case Class.TypeUse.TypeList.UShort:
                        case Class.TypeUse.TypeList.Int:
                        case Class.TypeUse.TypeList.Long:
                        case Class.TypeUse.TypeList.Byte:
                        case Class.TypeUse.TypeList.Float:
                        case Class.TypeUse.TypeList.Double:
                            if (parm["Code"].ToUpper() != "D")
                            {
                                All.Class.Error.Add("读取参数中区域类型和返回数据类型不一致", Environment.StackTrace);
                            }
                            code = "WR";
                            break;
                    }
                }
                //读取长度
                int readCount = end - tmpAddress + 1;
                if (parm["Code"].ToUpper() == "X" || parm["Code"].ToUpper() == "Y")
                {
                    try
                    {
                        readCount = Convert.ToInt16(string.Format("{0}", tmpAddress), 8) - Convert.ToInt16(string.Format("{0}", end), 8) + 1;
                    }
                    catch
                    {
                        All.Class.Error.Add(string.Format("读取参数中,读取地址错误,读取的地址{0}或结束的地址{1}不是8进制数", tmpAddress, end), Environment.StackTrace);
                        return false;
                    }
                }
                //返回长度
                int len = 6 + readCount + (check ? 2 : 0) + (crlf ? 2 : 0);
                if (parm["Code"].ToUpper() == "D")
                {
                    len = 6 + readCount * 4 + (check ? 2 : 0) + (crlf ? 2 : 0);
                }
                List<byte> sendBuff = new List<byte>();
                sendBuff.Add(0x05);
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", address)));
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", 0xFF)));
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0}", code)));
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X1}", delay)));
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0}", parm["Code"].ToUpper())));
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X4}", tmpAddress)));
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", readCount)));
                if (check)
                {
                    sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", All.Class.Check.SumCheck(sendBuff.ToArray(), 1, sendBuff.Count - 1))));
                }
                if (crlf)
                {
                    sendBuff.Add(0x0D);
                    sendBuff.Add(0x0A);
                }
                byte[] readBuff;
                if (WriteAndRead<byte[], byte[]>(sendBuff.ToArray(), len, out readBuff))
                {
                    if (readBuff[0] != sendBuff[0] || readBuff[1] != sendBuff[1] || readBuff[2] != sendBuff[2] || readBuff[3] != sendBuff[3] || readBuff[4] != sendBuff[4])
                    {
                        All.Class.Error.Add("读取参数帧头或帧尾校验失败", Environment.StackTrace);
                        All.Class.Error.Add(string.Format("读取帧为:{0}", All.Class.Num.Hex2Str(readBuff)));
                        return false;
                    }
                    switch (parm["Code"].ToUpper())
                    {
                        case "X":
                        case "Y":
                        case "M":
                            for (int i = 0; i < readCount; i++)
                            {
                                value.Add((T)(object)(readBuff[5 + i] == 0x31));
                            }
                            break;
                        case "D":
                            switch (t)
                            {
                                case Class.TypeUse.TypeList.String:
                                    value.Add((T)(object)Encoding.ASCII.GetString(readBuff, 5, readCount * 4));
                                    break;
                                case Class.TypeUse.TypeList.Byte:
                                    for (int i = 0; i < readCount * 4; i++)
                                    {
                                        value.Add((T)(object)readBuff[5 + i]);
                                    }
                                    break;
                                case Class.TypeUse.TypeList.Double:
                                    if ((readCount % 4) != 0)
                                    {
                                        All.Class.Error.Add("Fxplc读取双精度浮点数时,必须一次读4个D点");
                                        return false;
                                    }
                                    for (int i = 0; i < readCount; i++)
                                    {
                                        value.Add((T)(object)BitConverter.ToDouble(All.Class.Num.Str2Hex(Encoding.ASCII.GetString(readBuff, 5 + i * 16, 16)), 0));
                                    }
                                    break;
                                case Class.TypeUse.TypeList.Float:
                                    if ((readCount % 2) != 0)
                                    {
                                        All.Class.Error.Add("Fxplc读取单精度浮点数时,必须一次读2个D点");
                                        return false;
                                    }
                                    for (int i = 0; i < readCount; i++)
                                    {
                                        value.Add((T)(object)BitConverter.ToDouble(All.Class.Num.Str2Hex(Encoding.ASCII.GetString(readBuff, 5 + i * 8, 8)), 0));
                                    }
                                    break;
                                case Class.TypeUse.TypeList.Int:
                                    int tmpInt = 0;
                                    for (int i = 0; i < readCount; i++)
                                    {
                                        tmpInt = (Convert.ToInt32(Encoding.ASCII.GetString(readBuff, 5 + i * 4, 2), 16) << 8) + (Convert.ToInt32(Encoding.ASCII.GetString(readBuff, 7 + i * 4, 2), 16));
                                        if ((tmpInt & 0x8000) == 0x8000)
                                        {
                                            tmpInt = -((tmpInt ^ 0xFFFF) + 1);
                                        }
                                        value.Add((T)(object)tmpInt);
                                    }
                                    break;
                                case Class.TypeUse.TypeList.Long:
                                    for (int i = 0; i < readCount; i++)
                                    {
                                        value.Add((T)(object)(long)((Convert.ToInt32(Encoding.ASCII.GetString(readBuff, 5 + i * 4, 2), 16) << 8) + (Convert.ToInt32(Encoding.ASCII.GetString(readBuff, 7 + i * 4, 2), 16))));
                                    }
                                    break;
                                case Class.TypeUse.TypeList.UShort:
                                    for (int i = 0; i < readCount; i++)
                                    {
                                        value.Add((T)(object)(ushort)((Convert.ToInt32(Encoding.ASCII.GetString(readBuff, 5 + i * 4, 2), 16) << 8) + (Convert.ToInt32(Encoding.ASCII.GetString(readBuff, 7 + i * 4, 2), 16))));
                                    }
                                    break;
                            }
                            break;
                        default:
                            throw new Exception("Error");
                    }
                }
                else
                {
                    result = false;
                }
                return result;
            }
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                //写入数据
                if (value == null || value.Count <= 0)
                {
                    All.Class.Error.Add("写入数据为空,不能写入空数据", Environment.StackTrace);
                    return false;
                }
                //单个boolean值写入
                All.Class.TypeUse.TypeList t = All.Class.TypeUse.GetType<T>();
                //写入地址
                if (!parm.ContainsKey("Start") && !parm.ContainsKey("Address"))
                {
                    All.Class.Error.Add("数据写入参数中不包含寄存器地址", Environment.StackTrace);
                    return false;
                }
                ushort tmpAddress = 0;
                if (parm.ContainsKey("Start"))
                {
                    tmpAddress = parm["Start"].ToUshort();
                }
                if (parm.ContainsKey("Address"))
                {
                    tmpAddress = parm["Address"].ToUshort();
                }
                //写入区域
                string code = "BW";
                if (!parm.ContainsKey("Code"))
                {
                    switch (t)
                    {
                        case Class.TypeUse.TypeList.Boolean:
                            parm.Add("Code", "M");
                            break;
                        case Class.TypeUse.TypeList.UShort:
                        case Class.TypeUse.TypeList.Byte:
                        case Class.TypeUse.TypeList.Int:
                        case Class.TypeUse.TypeList.String:
                        case Class.TypeUse.TypeList.Long:
                            parm.Add("Code", "D");
                            code = "WW";
                            break;
                        default:
                            All.Class.Error.Add("数据写入类型不正确,须要先添加并测试", Environment.StackTrace);
                            break;
                    }
                }
                List<byte> sendBuff = new List<byte>();
                sendBuff.Add(0x05);
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", address)));
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", 0xFF)));
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0}", code)));
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X1}", delay)));
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0}", parm["Code"].ToUpper())));
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X4}", tmpAddress)));
                switch (t)
                {
                    case Class.TypeUse.TypeList.Boolean:
                        sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", value.Count)));
                        for (int i = 0; i < value.Count; i++)
                        {
                            sendBuff.Add((byte)((bool)(object)value[i] ? 0x31 : 0x30));
                        }
                        break;
                    case Class.TypeUse.TypeList.Double:
                        sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", 4 * value.Count)));
                        for (int i = 0; i < value.Count; i++)
                        {
                            sendBuff.AddRange(Encoding.ASCII.GetBytes(All.Class.Num.Hex2Str(BitConverter.GetBytes((double)(object)value[i]))));
                        }
                        break;
                    case Class.TypeUse.TypeList.Float:
                        sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", 2 * value.Count)));
                        for (int i = 0; i < value.Count; i++)
                        {
                            sendBuff.AddRange(Encoding.ASCII.GetBytes(All.Class.Num.Hex2Str(BitConverter.GetBytes((float)(object)value[i]))));
                        }
                        break;
                    case Class.TypeUse.TypeList.Int:
                        sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", value.Count)));
                        int tmpInt = 0;
                        for (int i = 0; i < value.Count; i++)
                        {
                            tmpInt = (int)(object)value[i];
                            if (tmpInt < 0)
                            {
                                tmpInt = -((tmpInt ^ 0xFFFF) + 1);
                            }
                            sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X4}", tmpInt)));
                        }
                        break;
                    case Class.TypeUse.TypeList.Long:
                        sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", value.Count)));
                        for (int i = 0; i < value.Count; i++)
                        {
                            sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X4}", (long)(object)value[i])));
                        }
                        break;
                    case Class.TypeUse.TypeList.UShort:
                        sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", value.Count)));
                        for (int i = 0; i < value.Count; i++)
                        {
                            sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X4}", (ushort)(object)value[i])));
                        }
                        break;
                }
                if (check)
                {
                    sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", All.Class.Check.SumCheck(sendBuff.ToArray(), 1, sendBuff.Count - 1))));
                }
                if (crlf)
                {
                    sendBuff.Add(0x0D);
                    sendBuff.Add(0x0A);
                }
                byte[] readBuff;
                if (WriteAndRead<byte[], byte[]>(sendBuff.ToArray(), 5, out readBuff))
                {
                    if (readBuff[0] != sendBuff[0] || readBuff[1] != sendBuff[1] || readBuff[2] != sendBuff[2] || readBuff[3] != sendBuff[3] || readBuff[4] != sendBuff[4])
                    {
                        All.Class.Error.Add("读取参数帧头或帧尾校验失败", Environment.StackTrace);
                        All.Class.Error.Add(string.Format("读取帧为:{0}", All.Class.Num.Hex2Str(readBuff)));
                        return false;
                    }
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
        }
    }
}
