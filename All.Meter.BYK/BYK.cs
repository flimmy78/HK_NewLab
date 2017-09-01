using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace All.Meter
{
    public class BYK:All.Meter.Meter
    {
        [DllImport(".\\BYK\\BykUsbCom.dll", EntryPoint = "BYKCom_Open")]
        public static extern int BYKCom_Open(int PortNumber, ref int hBYKCom);
        [DllImport(".\\BYK\\BykUsbCom.dll", EntryPoint = "BYKCom_FmtCommand")]
        public static extern int BYKCom_FmtCommand(int hBYKCom, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]byte[] buff, int buffLen, StringBuilder result, int maxResult, ref int writen);
        
        Dictionary<string, string> initParm;
        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override void Open()
        {
            if (this.initParm == null)
            {
                return;
            }
            int portNumber = 0;
            //if (!initParm.ContainsKey("PortName"))
            //{
            //    All.Class.Error.Add("参数中没有串口号", Environment.StackTrace);
            //}
            //else
            //{
            //    portNumber = All.Class.Num.ToInt(initParm["PortName"].ToUpper().Replace("COM", ""));
            //}
            if (byk == 0)
            {
                int result = BYKCom_Open(portNumber, ref byk);
                this.Conn = true;
            }
            else
            {
                this.Conn = false;
            }
            base.Open();
        }
        int byk = 0;//通讯通道号之类的
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
                switch (All.Class.TypeUse.GetType<T>())
                {
                    case Class.TypeUse.TypeList.Double:
                    case Class.TypeUse.TypeList.Float:
                        break;
                    default:
                        All.Class.Error.Add(string.Format("{0}:读取数据类型不正确", this.Text), Environment.StackTrace);
                        return false;
                }
                try
                {
                    if (byk == 0)
                    {
                        Open();
                        All.Class.Error.Add("BYK光泽度测试仪没有初始化端口");
                        return false;
                    }
                    int writeLen = 0;
                    byte[] buff = new byte[] { 0x17 };
                    int maxResult = 100;
                    StringBuilder tmpResult = new StringBuilder(maxResult);
                    if (BYKCom_FmtCommand(byk, buff, buff.Length, tmpResult, maxResult, ref writeLen) == 0)
                    {
                        string[] tmpBuff = tmpResult.ToString().Split('\t');
                        if (tmpBuff != null && tmpBuff.Length == 14)
                        {
                            switch (All.Class.TypeUse.GetType<T>())
                            {
                                case Class.TypeUse.TypeList.Double:
                                    value.Add((T)(object)All.Class.Num.ToDouble(tmpBuff[3]));
                                    value.Add((T)(object)All.Class.Num.ToDouble(tmpBuff[4]));
                                    value.Add((T)(object)All.Class.Num.ToDouble(tmpBuff[5]));
                                    value.Add((T)(object)All.Class.Num.ToDouble(tmpBuff[6]));
                                    break;
                                case Class.TypeUse.TypeList.Float:
                                    value.Add((T)(object)All.Class.Num.ToFloat(tmpBuff[3]));
                                    value.Add((T)(object)All.Class.Num.ToFloat(tmpBuff[4]));
                                    value.Add((T)(object)All.Class.Num.ToFloat(tmpBuff[5]));
                                    value.Add((T)(object)All.Class.Num.ToFloat(tmpBuff[6]));
                                    break;
                            }
                        }
                    }
                    else
                    {
                        All.Class.Error.Add("BYK光泽度测试仪读取数据错误");
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
                All.Class.Error.Add("BYK暂时没有写入参数方法", Environment.StackTrace);
                return false;
            }
        }
    }
}
