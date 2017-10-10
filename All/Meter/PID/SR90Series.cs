using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class SR90Series:Meter
    {
        /// <summary>
        /// 读取寄存器类型
        /// </summary>
        public enum ReadItem : int
        {
            /// <summary>
            /// 测量值
            /// </summary>
            PV = 0x100,
            /// <summary>
            /// 当前执行的设定值 
            /// </summary>
            SV_W = 0x101,
            /// <summary>
            /// 当前输出值
            /// </summary>
            Out1W = 0x102,
            /// <summary>
            /// 当前输出值
            /// </summary>
            Out2W = 0x103,
            /// <summary>
            /// 设定值 
            /// </summary>
            SV1 = 0x300,
            /// <summary>
            /// 设定值下限
            /// </summary>
            SV_L = 0x30A,
            /// <summary>
            /// 设定值上限
            /// </summary>
            SV_H = 0x30B,
            /// <summary>
            /// 控制输出一的比例带
            /// </summary>
            PB1 = 0x400,
            /// <summary>
            /// 控制一输出的下限
            /// </summary>
            O1_L = 0x405,
            /// <summary>
            /// 控制一输出的上线
            /// </summary>
            O1_H = 0x406,
            /// <summary>
            /// 控制输出二的比例带
            /// </summary>
            PB2 = 0x460,
            /// <summary>
            /// 控制二输出的下限
            /// </summary>
            O2_L = 0x465,
            /// <summary>
            /// 控制二输出的上限
            /// </summary>
            O2_H = 0x466,
            /// <summary>
            /// 小数点位数
            /// </summary>
            DP = 0x707
        }
        /// <summary>
        /// 写入寄存器类型
        /// </summary>
        public enum WriteItem : int
        {
            /// <summary>
            /// 通讯状态
            /// </summary>
            COM = 0x18C,
            /// <summary>
            /// 手动模式下输出一
            /// </summary>
            Out1_W = 0x182,
            /// <summary>
            /// 手动模式下输出二
            /// </summary>
            Out2_W = 0x183,
            /// <summary>
            /// 手,自动模式切换,0=自动,1=手动
            /// </summary>
            Man = 0x185,
            /// <summary>
            /// 设定值 
            /// </summary>
            SV1 = 0x300,
            /// <summary>
            /// 设定值下限
            /// </summary>
            SV_L = 0x30A,
            /// <summary>
            /// 设定值上限
            /// </summary>
            SV_H = 0x30B,
            /// <summary>
            /// 控制输出一的比例带
            /// </summary>
            PB1 = 0x400,
            /// <summary>
            /// 控制一输出的下限
            /// </summary>
            O1_L = 0x405,
            /// <summary>
            /// 控制一输出的上线
            /// </summary>
            O1_H = 0x406,
            /// <summary>
            /// 控制输出二的比例带
            /// </summary>
            PB2 = 0x460,
            /// <summary>
            /// 控制二输出的下限
            /// </summary>
            O2_L = 0x465,
            /// <summary>
            /// 控制二输出的上限
            /// </summary>
            O2_H = 0x466
        }
        Dictionary<string, int> readItem = new Dictionary<string, int>();
        Dictionary<string, int> writeItem = new Dictionary<string, int>();
        int address = 0;
        int dp = 0;//小数点
        Dictionary<string, string> initParm = new Dictionary<string, string>();
        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            if (!initParm.ContainsKey("Address"))
            {
                All.Class.Error.Add("参数中没有地址", Environment.StackTrace);
            }
            else
            {
                address = All.Class.Num.ToByte(initParm["Address"]);
            }
            if (initParm.ContainsKey("DP"))
            {
                dp = All.Class.Num.ToInt(initParm["DP"]);
            }
            string[] item = Enum.GetNames(typeof(ReadItem));
            for (int i = 0; i < item.Length; i++)
            {
                if (!readItem.ContainsKey(item[i]))
                {
                    readItem.Add(item[i], (int)(ReadItem)Enum.Parse(typeof(ReadItem), item[i]));
                }
            } 
            item = Enum.GetNames(typeof(WriteItem));
            for (int i = 0; i < item.Length; i++)
            {
                if (!writeItem.ContainsKey(item[i]))
                {
                    writeItem.Add(item[i], (int)(WriteItem)Enum.Parse(typeof(WriteItem), item[i]));
                }
            }
            base.Init(initParm);
        }
        /// <summary>
        /// 直接读取SR90模块指定通道数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Read(out float value,ReadItem item)
        {
            value = 0;
            List<float> tmp = new List<float>();
            Dictionary<string, string> parm = new Dictionary<string, string>();
            parm.Add("Code", item.ToString());
            bool result = Read<float>(out tmp, parm);
            if (result)
            {
                if (tmp.Count > 0)
                {
                    value = tmp[0];
                }
            }
            return result;
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                value = new List<T>();
                try
                {
                    int item = (int)ReadItem.PV;
                    if (parm.ContainsKey("Code") && readItem.ContainsKey(parm["Code"]))
                    {
                        item = readItem[parm["Code"]];
                    }
                    if (dp == 0 && item != (int)ReadItem.DP)//当没有小数点位时,先读小数点位数
                    {
                        float tmpDp = 0;
                        if (!Read(out tmpDp, ReadItem.DP))
                        {
                            return false;
                        }
                        dp = (int)Math.Pow(10, (int)tmpDp);
                    }
                    byte[] sendBuff = new byte[12];
                    sendBuff[0] = 0x02;
                    Array.Copy(Encoding.ASCII.GetBytes(string.Format("{0:D2}1R{1:X4}0", address, item)), 0, sendBuff, 1, 9);
                    sendBuff[10] = 0x03;
                    sendBuff[11] = 0x0D;
                    byte[] readBuff;
                    if (WriteAndRead<byte[], byte[]>(sendBuff, 14, out readBuff, parm))
                    {
                        if (readBuff[0] != sendBuff[0] || readBuff[1] != sendBuff[1] || readBuff[2] != sendBuff[2] || readBuff[3] != sendBuff[3])
                        {
                            All.Class.Error.Add("SR90系列表,读取数据校验错误");
                            All.Class.Error.Add("通讯数据", string.Format("发送数据:{0}\r\n读取数据:{1}", sendBuff.ToHexString(), readBuff.ToHexString()));
                            return false;
                        }
                        float readValue = Convert.ToInt16(Encoding.ASCII.GetString(readBuff, 8, 4), 16);
                        switch ((ReadItem)item)
                        {
                            case ReadItem.PV:
                            case ReadItem.SV_H:
                            case ReadItem.SV_L:
                            case ReadItem.SV_W:
                            case ReadItem.SV1:
                                readValue /= dp;
                                break;
                            case ReadItem.Out1W:
                            case ReadItem.Out2W:
                            case ReadItem.O1_H:
                            case ReadItem.O1_L:
                            case ReadItem.O2_H:
                            case ReadItem.O2_L:
                            case ReadItem.PB1:
                            case ReadItem.PB2:
                                readValue /= 10;
                                break;
                        }
                        switch (All.Class.TypeUse.GetType<T>())
                        {
                            case Class.TypeUse.TypeList.Int:
                                value.Add((T)(object)(int)readValue);
                                break;
                            case Class.TypeUse.TypeList.Float:
                                value.Add((T)(object)(float)readValue);
                                break;
                            case Class.TypeUse.TypeList.Double:
                                value.Add((T)(object)(double)readValue);
                                break;
                            case Class.TypeUse.TypeList.Long:
                                value.Add((T)(object)(long)readValue);
                                break;
                            case Class.TypeUse.TypeList.UShort:
                                value.Add((T)(object)(ushort)readValue);
                                break;
                            default:
                                All.Class.Error.Add("当前仪表不支持当前数据类型");
                                break;
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
        }/// <summary>
        /// 直接读取SR90模块指定通道数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool WriteInternal(float value, WriteItem item)
        {
            List<float> tmp = new List<float>();
            tmp.Add(value);
            Dictionary<string, string> parm = new Dictionary<string, string>();
            parm.Add("Code", item.ToString());
            return WriteInternal<float>(tmp, parm);
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                try
                {
                    int item = (int)WriteItem.SV1;
                    if (value == null || value.Count <= 0)
                    {
                        All.Class.Error.Add("S90系列,没有要写入的数据,不能写入");
                        return false;
                    }
                    if (parm.ContainsKey("Code") && writeItem.ContainsKey(parm["Code"]))
                    {
                        item = writeItem[parm["Code"]];
                    }
                    if (dp == 0)//当没有小数点位时,先读小数点位数
                    {
                        float tmpDp = 0;
                        if (!Read(out tmpDp, ReadItem.DP))
                        {
                            return false;
                        }
                        dp = (int)Math.Pow(10, (int)tmpDp);
                    }
                    float writeValue = 0;
                    switch (All.Class.TypeUse.GetType<T>())
                    {
                        case Class.TypeUse.TypeList.Int:
                            writeValue = (float)(int)(object)value[0];
                            break;
                        case Class.TypeUse.TypeList.Long:
                            writeValue = (float)(long)(object)value[0];
                            break;
                        case Class.TypeUse.TypeList.Double:
                            writeValue = (float)(double)(object)value[0];
                            break;
                        case Class.TypeUse.TypeList.Float:
                            writeValue = (float)(object)value[0];
                            break;
                        case Class.TypeUse.TypeList.UShort:
                            writeValue = (float)(ushort)(object)value[0];
                            break;
                        case Class.TypeUse.TypeList.Byte:
                            writeValue = (float)(byte)(object)value[0];
                            break;
                        default:
                            All.Class.Error.Add("SR90系列,要写入的数据类型不正确,无法完成写入操作");
                            break;
                    }
                    switch ((WriteItem)item)
                    {
                        case WriteItem.SV_H:
                        case WriteItem.SV_L:
                        case WriteItem.SV1:
                            writeValue *= dp;
                            break;
                        case WriteItem.O1_H:
                        case WriteItem.O1_L:
                        case WriteItem.O2_H:
                        case WriteItem.O2_L:
                        case WriteItem.PB1:
                        case WriteItem.PB2:
                            writeValue *= 10;
                            break;
                    }
                    byte[] sendBuff = new byte[17];
                    sendBuff[0] = 0x02;
                    Array.Copy(Encoding.ASCII.GetBytes(string.Format("{0:D2}1W{1:X4}0,{2:X4}", address, item, (int)writeValue)), 0, sendBuff, 1, 14);
                    sendBuff[15] = 0x03;
                    sendBuff[16] = 0x0D;
                    byte[] readBuff;
                    if (WriteAndRead<byte[], byte[]>(sendBuff, 9, out readBuff, parm))
                    {
                        if (readBuff[0] != sendBuff[0] || readBuff[1] != sendBuff[1] || readBuff[2] != sendBuff[2] || readBuff[3] != sendBuff[3])
                        {
                            All.Class.Error.Add("SR90系列表,写入数据校验错误");
                            All.Class.Error.Add("通讯数据", string.Format("发送数据:{0}\r\n读取数据:{1}", sendBuff.ToHexString(), readBuff.ToHexString()));
                            return false;
                        }
                        switch (readBuff[6])
                        {
                            case 0x30:
                                result = true;
                                break;
                            case 0x31:
                                All.Class.Error.Add("SR90系列当发生硬件错误例如帧溢出或奇偶校验错误被检测到时");
                                break;
                            case 0x37:
                                All.Class.Error.Add("SR90系列数据格式错误");
                                break;
                            case 0x38:
                                All.Class.Error.Add("SR90系列命令或数据的数量错误");
                                break;
                            case 0x39:
                                All.Class.Error.Add("SR90系列被写入的数据不是有效的可被设定的范围");
                                break;
                            case 0x41:
                                All.Class.Error.Add("SR90系列执行命令错误");
                                break;
                            case 0x42:
                                All.Class.Error.Add("SR90系列写模式错误");
                                break;
                            default:
                                All.Class.Error.Add("SR90系列未知错误");
                                break;
                        }
                        if (readBuff[5] != 0x30)
                        {
                            result = false;
                        }
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
            float tmp = 0;
            return Read(out tmp, ReadItem.DP);
        }
    }
}
