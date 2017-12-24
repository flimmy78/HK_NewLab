using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    /// <summary>
    /// 三菱Q系统PLC网络通讯方法
    /// </summary>
    public class MitsubishiQNet:Meter
    {
        Dictionary<string, string> initParm;

        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        int NetCode = 0x00;
        int PlcCode = 0xFF;
        int IoCode = 0x3FF;
        int StationCode = 0x00;
        int CpuTime = 0x10;
        public override void Init(Dictionary<string, string> initParm)
        {
            base.Init(initParm);
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            throw new NotImplementedException();
            //value = new List<T>();
            //bool result = true;
            //lock (lockObject)
            //{
            //    try
            //    {
            //        int start = 0;
            //        int end = 0;
            //        string Code = "";
            //        All.Class.TypeUse.TypeList t = All.Class.TypeUse.GetType<T>();
            //        if (parm.ContainsKey("Start"))
            //        {
            //            start = parm["Start"].ToInt();
            //        }
            //        if (parm.ContainsKey("Address"))
            //        {
            //            start = parm["Address"].ToInt();
            //        }
            //        if (parm.ContainsKey("End"))
            //        {
            //            end = parm["End"].ToInt();
            //        }
            //        if (end < start)
            //        {
            //            All.Class.Error.Add("读取参数中结束点大于起始点", Environment.StackTrace);
            //            return false;
            //        }
            //        if (!parm.ContainsKey("Code"))
            //        {
            //            switch (t)
            //            {
            //                case Class.TypeUse.TypeList.Boolean:
            //                    parm.Add("Code", "M");
            //                    break;
            //                case Class.TypeUse.TypeList.String:
            //                case Class.TypeUse.TypeList.UShort:
            //                case Class.TypeUse.TypeList.Int:
            //                case Class.TypeUse.TypeList.Long:
            //                case Class.TypeUse.TypeList.Byte:
            //                case Class.TypeUse.TypeList.Float:
            //                case Class.TypeUse.TypeList.Double:
            //                    parm.Add("Code", "R");
            //                    break;
            //            }
            //        }
            //        else
            //        {
            //            switch (t)
            //            {
            //                case Class.TypeUse.TypeList.Boolean:
            //                    if (parm["Code"].ToUpper() != "X" && parm["Code"].ToUpper() != "M" && parm["Code"].ToUpper() != "Y")
            //                    {
            //                        All.Class.Error.Add("读取参数中区域类型和返回数据类型不一致", Environment.StackTrace);
            //                        return false;
            //                    }
            //                    break;
            //                case Class.TypeUse.TypeList.String:
            //                case Class.TypeUse.TypeList.UShort:
            //                case Class.TypeUse.TypeList.Int:
            //                case Class.TypeUse.TypeList.Long:
            //                case Class.TypeUse.TypeList.Byte:
            //                case Class.TypeUse.TypeList.Float:
            //                case Class.TypeUse.TypeList.Double:
            //                    if (parm["Code"].ToUpper() != "R")
            //                    {
            //                        All.Class.Error.Add("读取参数中区域类型和返回数据类型不一致", Environment.StackTrace);
            //                    }
            //                    break;
            //            }
            //        }
            //        if (parm.ContainsKey("Code"))
            //        {
            //            Code = parm["Code"];
            //        }
            //        int readLen = 0;//写入数据中要读取的长度
            //        string readValue = "";//返回字符串
            //        string sendValue = string.Format("5000{0:X2}{1:X2}{2:X4}{3:X2}{4:X4}",
            //            NetCode, PlcCode, IoCode, StationCode, 0x18);//相同的头文件
            //        string startValue = "";//不同类型的数据，Start位可能为10进制数据，可能为16进制数据
            //        sendValue = string.Format("{0}{1:X4}{2:X4}{3:X4}", sendValue, CpuTime, 0x0401, 0x0000);//0x0401为命令码
            //        switch (Class.TypeUse.GetType<T>())
            //        {
            //            case All.Class.TypeUse.TypeList.UShort:
            //            case Class.TypeUse.TypeList.Byte:
            //            case Class.TypeUse.TypeList.Double:
            //            case Class.TypeUse.TypeList.Float:
            //            case Class.TypeUse.TypeList.Int:
            //            case Class.TypeUse.TypeList.Long:
            //            case Class.TypeUse.TypeList.String:
            //                switch (Code)
            //                {
            //                    case "R":
            //                        sendValue = string.Format("{0}R*", sendValue);
            //                        readLen = end - start + 1;
            //                        startValue = string.Format("{0:D6}", start);
            //                        break;
            //                    default:
            //                        All.Class.Error.Add("当前要读取的数据区域错误,请检测是否可读取区域,或者添加读取方法", Environment.StackTrace);
            //                        break;
            //                }
            //                break;
            //            case All.Class.TypeUse.TypeList.Boolean:
            //                switch (Code)
            //                {
            //                    case "X":
            //                        sendValue = string.Format("{0}X*", sendValue);
            //                        startValue = string.Format("{0:X6}", start);
            //                        break;
            //                    case "L":
            //                        sendValue = string.Format("{0}L*", sendValue);
            //                        startValue = string.Format("{0:D6}", start);
            //                        break;
            //                    case "M"://默认M点
            //                        sendValue = string.Format("{0}M*", sendValue);
            //                        startValue = string.Format("{0:D6}", start);
            //                        break;
            //                    default:
            //                        All.Class.Error.Add("当前要读取的数据区域错误,请检测是否可读取区域,或者添加读取方法", Environment.StackTrace);
            //                        break;
            //                }
            //                readLen = (int)Math.Ceiling((end - start + 1) / 16f);
            //                break;
            //            default:
            //                All.Class.Error.Add("MitsubishiQ读取不可知数据类型", Environment.StackTrace);
            //                return false;
            //        }
            //        sendValue = string.Format("{0}{1}{2:X4}", sendValue, startValue, readLen);
            //        if (Write<string, string>(sendValue, readLen * 4 + 22, out readValue))
            //        {

            //            if (readValue.IndexOf(string.Format("D000{0:X2}{1:X2}{2:X4}{3:X2}{4:X4}0000", NetCode, PlcCode, IoCode, StationCode, readLen * 4 + 4)) == 0)
            //            {
            //                ushort[] buff = new ushort[readLen];
            //                for (int i = 0; i < readLen; i++)
            //                {
            //                    buff[i] = Convert.ToUInt16(readValue.Substring(22 + i * 4, 4), 16);
            //                }
            //                switch (Class.TypeUse.GetType<T>())
            //                {
            //                    case All.Class.TypeUse.TypeList.Boolean:
            //                        bool[] tmpBool = All.Class.Num.Ushort2Bool(buff);
            //                        for (int i = 0; i < (end - start + 1); i++)
            //                        {
            //                            value.Add((T)(object)tmpBool[i]);
            //                        }
            //                        break;
            //                    case All.Class.TypeUse.TypeList.UShort:
            //                        for (int i = 0; i < readLen; i++)
            //                        {
            //                            value.Add((T)(object)buff[i]);
            //                        }
            //                        break;
            //                    default:
            //                        All.Class.Error.Add("MitsubishiQ读取不可知数据类型", Environment.StackTrace);
            //                        break;
            //                }
            //            }
            //            else
            //            {
            //                result = false;
            //                Error = "校验错误";
            //                All.Class.Log.Add(string.Format("MitsubishiQ读取数据校验错误\r\n写入数据  ->  {0}\r\n返回数据  ->  {1}", sendValue, readValue), Environment.StackTrace);
            //            }
            //        }
            //        else
            //        {
            //            result = false;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        All.Class.Error.Add(e);
            //        result = false;
            //    }
            //}
            //return result;

        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T">当T为bool时，M点为0开始，X点在原来的基础上加100000，L点在原来的基础上加200000</typeparam>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public override bool Read<T>(out List<T> value, int start, int end)
        {
            Dictionary<string, string> parm = new Dictionary<string, string>();
            parm.Add("Start", start.ToString());
            parm.Add("End", end.ToString());

            return Read<T>(out value, parm);
        }
        public override bool Read<T>(out T value, int start)
        {
            throw new NotImplementedException();
        }
        public override bool WriteInternal<T>(List<T> value, int start, int end)
        {
            throw new NotImplementedException();
        }
        public override bool WriteInternal<T>(T value, int start)
        {
            throw new NotImplementedException();
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            throw new NotImplementedException();
        }
    }
}
