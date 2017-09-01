using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class An9632 : AnGui.AnGui
    {
        public int Address
        { get; set; }
        Dictionary<string, string> initParm;
        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            this.Address = 0;
            if (initParm.ContainsKey("Address"))
            {
                this.Address = initParm["Address"].ToInt();
            }
            base.Init(initParm);
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                value = new List<T>();
                try
                {
                    byte[] sendBuff = new byte[] { 0x7B, 0x06, (byte)Address, 00, (byte)(0x06 + Address), 0x7D };
                    byte[] readBuff;
                    int len = 20;
                    if (WriteAndRead<byte[], byte[]>(sendBuff, len,out readBuff))
                    {
                        if (sendBuff[0] != readBuff[0] || len != readBuff[1] ||
                            sendBuff[2] != readBuff[2] || sendBuff[3] != readBuff[3] ||
                            sendBuff[5] != readBuff[len - 1])
                        {
                            All.Class.Error.Add("返回数据校验失败", Environment.StackTrace);
                            All.Class.Error.Add("发送数据", All.Class.Num.Hex2Str(sendBuff));
                            All.Class.Error.Add("返回数据", All.Class.Num.Hex2Str(readBuff));
                            return false;
                        }
                        switch (All.Class.TypeUse.GetType<T>())
                        {
                            case Class.TypeUse.TypeList.Float:
                            case Class.TypeUse.TypeList.Double:
                                bool[] tmpResult = All.Class.Num.Byte2Bool(readBuff[17]);

                                value.Add((T)(object)(float)AnGui.AnGui.Projects.耐压);
                                value.Add((T)(object)(float)(readBuff[4] * 256 + readBuff[5]));
                                value.Add((T)(object)(float)(0.001f * (readBuff[6] * 256 + readBuff[7])));
                                value.Add((T)(object)(float)0);
                                value.Add((T)(object)(float)0);
                                value.Add((T)(object)(float)0);
                                value.Add((T)(object)(float)0);
                                value.Add((T)(object)(float)(tmpResult[0] ? 1 : 0));

                                value.Add((T)(object)(float)AnGui.AnGui.Projects.绝缘);
                                value.Add((T)(object)(float)(readBuff[10]*256+readBuff[11]));
                                value.Add((T)(object)(float)(0.01f * (readBuff[12] * 65536 + readBuff[13] * 256 + readBuff[14])));
                                value.Add((T)(object)(float)0);
                                value.Add((T)(object)(float)0);
                                value.Add((T)(object)(float)0);
                                value.Add((T)(object)(float)0);
                                value.Add((T)(object)(float)(tmpResult[3] ? 1 : 0));

                                value.Add((T)(object)(float)((tmpResult[6] == tmpResult[7]) ? 1 : 0));
                                value.Add((T)(object)(float)((tmpResult[0] && tmpResult[3]) ? 1 : 0));
                                result = true;
                                break;
                            case Class.TypeUse.TypeList.Byte:
                                for (int i = 4; i < 18; i++)
                                {
                                    value.Add((T)(object)(readBuff[i]));
                                }
                                result = true;
                                break;
                            default:
                                All.Class.Error.Add("读取的数据类型不正确,不支持当前的数据类型", Environment.StackTrace);
                                return false;
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
                }
                return result;
            }
        }
        /// <summary>
        /// 格式化值,达到4位数时,值须要遵守艾诺的奇葩算法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string Format(int value)
        {
            if (value <= 0)
            {
                return "0000";
            }
            if (value >= 10000)
            {
                return string.Format("{0}{1:D3}",
                    Encoding.ASCII.GetString(new byte[] { (byte)(0x30 + ((int)Math.Floor(value / 1000f) & 0x7F)) }),
                    value % 1000);
            }
            return string.Format("{0:D4}", value);
        }
        /// <summary>
        /// 格式化值,艾诺的奇葩算法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int Format(string value)
        {
            int result = 0;
            byte[] buff = Encoding.ASCII.GetBytes(value);
            for (int i = 0; i < buff.Length; i++)
            {
                result = result + (buff[i] - 0x30) * (int)Math.Pow(10, value.Length - i - 1);
            }
            return result;
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                try
                {
                    if (!parm.ContainsKey("Code"))
                    {
                        All.Class.Error.Add("数据写入参数中不包含命令", Environment.StackTrace);
                        return false;
                    }
                    string sendValue = "";
                    int len = 6;
                    switch (parm["Code"].ToUpper())
                    {
                            //7B+数据长度+地址+命令+数据+校验+7D
                        case "START":
                            sendValue = string.Format("06{0:D2}01", Address);
                            break;
                        case "STOP":
                            sendValue = string.Format("06{0:D2}02", Address);
                            break;
                        case "ACW":
                            sendValue = string.Format("07{0:D2}0300", Address);//使用耐压模式
                            break;
                        case "IR":
                            sendValue = string.Format("07{0:D2}0301", Address);//绝缘模式
                            break;
                        case "SETTING":
                            Type t = typeof(AnGui.AnGui.SendValue.StepValue);
                            int members = t.GetProperties().Count();
                            if (value == null || (value.Count % members) != 0)
                            {
                                All.Class.Error.Add("写入的数据量不正确,不能识别的数据", Environment.StackTrace);
                                return false;
                            }
                            sendValue = "";
                            for (int i = 0; i < value.Count; i = i + members)
                            {
                                switch ((AnGui.AnGui.Projects)(int)(object)value[i])//步骤
                                {
                                    case Projects.耐压:
                                        if (!WriteInternal(new Dictionary<string, string>() { {"Code", "ACW"} }))
                                        {
                                            All.Class.Error.Add("9632切换为耐压模式失败");
                                            return false;
                                        }
                                        sendValue = string.Format("15{0:D2}06{1:X4}{2:X4}{3:X4}{4:X4}{5:X2}{6:X4}{7:X4}{8:X4}",
                                            Address,
                                            (int)(Convert.ToSingle(value[i + 1])),//输出
                                            (int)(1000 * Convert.ToSingle(value[i + 3])),//最大值
                                            (int)(1000 * Convert.ToSingle(value[i + 2])),//最小值
                                            (int)(10 * Convert.ToSingle(value[i + 4])),//时间
                                            (int)(Convert.ToSingle(value[i + 6])),//频率
                                            (int)(10 * Convert.ToSingle(value[i + 7])),//缓升
                                            (int)(10 * Convert.ToSingle(value[i + 8])),//缓降
                                            0);
                                        break;
                                    case Projects.绝缘:
                                        if (!WriteInternal(new Dictionary<string, string>() { { "Code", "IR" } }))
                                        {
                                            All.Class.Error.Add("9632切换为耐压模式失败");
                                            return false;
                                        }
                                        sendValue = string.Format("10{0:D2}06{1:X4}{2:X6}{3:X6}{4:X2}",
                                            Address,
                                            (int)(Convert.ToSingle(value[i + 1])),//设置输出值
                                            (int)(100 * Convert.ToSingle(value[i + 2])),//最小值
                                            (int)(100 * Convert.ToSingle(value[i + 3])),//最大值
                                            (int)(10 * Convert.ToSingle(value[i + 4])));//测试时间
                                        break;
                                    default:
                                        All.Class.Error.Add(string.Format("出现未知的测试序号,错误序号为{0}", (int)(object)value[i]), Environment.StackTrace);
                                        return false;
                                }
                                if (sendValue != "")
                                {
                                    break;
                                }
                            }
                            break;
                        default:
                            All.Class.Error.Add(string.Format("数据写入参数命令不正确,不能识别的指令,{0}", parm["Code"]), Environment.StackTrace);
                            return false;
                    }
                    if (sendValue == "")//添加校验
                    {
                        All.Class.Error.Add("不能发送", "An9632发送空指令字符,无法完成写入操作");
                        return false;
                    }
                    sendValue = string.Format("7B{0}{1:X2}7D", sendValue, All.Class.Check.SumCheck(All.Class.Num.Str2Hex(sendValue)));
                    byte[] readValue;
                    if (WriteAndRead<byte[], byte[]>(All.Class.Num.Str2Hex(sendValue), len, out readValue))
                    {
                        if (readValue[0] != 0x7B || readValue[5] != 0x7D)
                        {
                            All.Class.Error.Add("识别码错误", string.Format("AN9632的返回字符头码或结束码不正确:{0}", All.Class.Num.Hex2Str(readValue)));
                            return false;
                        }
                        if (readValue[1] != Address)
                        {
                            All.Class.Error.Add("地址码错误", string.Format("An9632返回字符地址码不正确:{0}", All.Class.Num.Hex2Str(readValue)));
                            return false;
                        }
                        if (readValue[4] != All.Class.Check.SumCheck(readValue, 1, 3))
                        {
                            All.Class.Error.Add("校验码错误", string.Format("An9632返回字符校验错误:{0}", All.Class.Num.Hex2Str(readValue)));
                            return false;
                        }
                        switch (Encoding.ASCII.GetString(readValue, 2, 2))
                        {
                            case "OK":
                                return true;
                            case "NO":
                                All.Class.Error.Add("应答错误", "AN9632应答错误,不符合输入要求或者当前不能执行");
                                return false;
                            case "??":
                                All.Class.Error.Add("应答错误", "未知命令");
                                return false;
                            default:
                                All.Class.Error.Add("出现异常", All.Class.Num.Hex2Str(readValue));
                                return false;
                        }
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                }
                return result;
            }
        }
    }
}
