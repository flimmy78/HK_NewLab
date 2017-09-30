using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate.Data
{
    /// <summary>
    /// 设备读取信息
    /// </summary>
    public class MeterStyle
    {
        /// <summary>
        /// 所有读取信息
        /// </summary>
        public List<ReadDetial> Reads
        { get; set; }
        /// <summary>
        /// 设备值
        /// </summary>
        public Meter.Meter Value
        { get; set; }

        public MeterStyle()
        {
            Reads = new List<ReadDetial>();
            Value = null;
        }
        /// <summary>
        /// 读取指定块的数据
        /// </summary>
        /// <param name="readDetialIndex"></param>
        /// <returns></returns>
        public bool Read(int readDetialIndex,out Dictionary<int,object> value)
        {
            bool result = true;
            value = new Dictionary<int, object>();
            switch (Reads[readDetialIndex].ReadType)
            {
                case Class.TypeUse.TypeList.Boolean:
                    List<bool> tmpBool = new List<bool>();
                    result = this.Value.Read<bool>(out tmpBool, Reads[readDetialIndex].Values);
                    if (result)
                    {
                        if (tmpBool.Count < Reads[readDetialIndex].Datas.Count)
                        {
                            All.Class.Error.Add("读取设备的返回数据量和设置要求返回的数据量不一致", Environment.StackTrace);
                            All.Class.Error.Add("错误数量", string.Format("要求数量:{0},实际数量:{1}", 
                                Reads[readDetialIndex].Datas.Count, tmpBool.Count));
                            return false;
                        }
                        foreach (int i in Reads[readDetialIndex].Datas.Keys)
                        {
                            value.Add(i, tmpBool[value.Count]);
                        }
                    }
                    break;
                case Class.TypeUse.TypeList.Byte:
                    List<byte> tmpByte = new List<byte>();
                    result = this.Value.Read<byte>(out tmpByte, Reads[readDetialIndex].Values);
                    if (result)
                    {
                        if (tmpByte.Count < Reads[readDetialIndex].Datas.Count)
                        {
                            All.Class.Error.Add("读取设备的返回数据量和设置要求返回的数据量不一致", Environment.StackTrace);
                            All.Class.Error.Add("错误数量", string.Format("要求数量:{0},实际数量:{1}",
                                Reads[readDetialIndex].Datas.Count, tmpByte.Count));
                            return false;
                        }
                        foreach (int i in Reads[readDetialIndex].Datas.Keys)
                        {
                            value.Add(i, tmpByte[value.Count]);
                        }
                    }
                    break;
                case Class.TypeUse.TypeList.Bytes:
                case Class.TypeUse.TypeList.UnKnow:
                    result = false;
                    break;
                case Class.TypeUse.TypeList.DateTime:
                    List<DateTime> tmpDateTime = new List<DateTime>();
                    result = this.Value.Read<DateTime>(out tmpDateTime, Reads[readDetialIndex].Values);
                    if (result)
                    {
                        if (tmpDateTime.Count < Reads[readDetialIndex].Datas.Count)
                        {
                            All.Class.Error.Add("读取设备的返回数据量和设置要求返回的数据量不一致", Environment.StackTrace);
                            All.Class.Error.Add("错误数量", string.Format("要求数量:{0},实际数量:{1}",
                                Reads[readDetialIndex].Datas.Count, tmpDateTime.Count));
                            return false;
                        }
                        foreach (int i in Reads[readDetialIndex].Datas.Keys)
                        {
                            value.Add(i, tmpDateTime[value.Count]);
                        }
                    }
                    break;
                case Class.TypeUse.TypeList.Double:
                    List<double> tmpDouble = new List<double>();
                    result = this.Value.Read<double>(out tmpDouble, Reads[readDetialIndex].Values);
                    if (result)
                    {
                        if (tmpDouble.Count < Reads[readDetialIndex].Datas.Count)
                        {
                            All.Class.Error.Add("读取设备的返回数据量和设置要求返回的数据量不一致", Environment.StackTrace);
                            All.Class.Error.Add("错误数量", string.Format("要求数量:{0},实际数量:{1}",
                                Reads[readDetialIndex].Datas.Count, tmpDouble.Count));
                            return false;
                        }
                        foreach (int i in Reads[readDetialIndex].Datas.Keys)
                        {
                            value.Add(i, tmpDouble[value.Count]);
                        }
                    }
                    break;
                case Class.TypeUse.TypeList.Float:
                    List<float> tmpFloat = new List<float>();
                    result = this.Value.Read<float>(out tmpFloat, Reads[readDetialIndex].Values);
                    if (result)
                    {
                        if (tmpFloat.Count < Reads[readDetialIndex].Datas.Count)
                        {
                            All.Class.Error.Add("读取设备的返回数据量和设置要求返回的数据量不一致", Environment.StackTrace);
                            All.Class.Error.Add("错误数量", string.Format("要求数量:{0},实际数量:{1}",
                                Reads[readDetialIndex].Datas.Count, tmpFloat.Count));
                            return false;
                        }
                        foreach (int i in Reads[readDetialIndex].Datas.Keys)
                        {
                            value.Add(i, tmpFloat[value.Count]);
                        }
                    }
                    break;
                case Class.TypeUse.TypeList.Int:
                    List<int> tmpInt = new List<int>();
                    result = this.Value.Read<int>(out tmpInt, Reads[readDetialIndex].Values);
                    if (result)
                    {
                        if (tmpInt.Count < Reads[readDetialIndex].Datas.Count)
                        {
                            All.Class.Error.Add("读取设备的返回数据量和设置要求返回的数据量不一致", Environment.StackTrace);
                            All.Class.Error.Add("错误数量", string.Format("要求数量:{0},实际数量:{1}",
                                Reads[readDetialIndex].Datas.Count, tmpInt.Count));
                            return false;
                        }
                        foreach (int i in Reads[readDetialIndex].Datas.Keys)
                        {
                            value.Add(i, tmpInt[value.Count]);
                        }
                    }
                    break;
                case Class.TypeUse.TypeList.Long:
                    List<long> tmpLong = new List<long>();
                    result = this.Value.Read<long>(out tmpLong, Reads[readDetialIndex].Values);
                    if (result)
                    {
                        if (tmpLong.Count < Reads[readDetialIndex].Datas.Count)
                        {
                            All.Class.Error.Add("读取设备的返回数据量和设置要求返回的数据量不一致", Environment.StackTrace);
                            All.Class.Error.Add("错误数量", string.Format("要求数量:{0},实际数量:{1}",
                                Reads[readDetialIndex].Datas.Count, tmpLong.Count));
                            return false;
                        }
                        foreach (int i in Reads[readDetialIndex].Datas.Keys)
                        {
                            value.Add(i, tmpLong[value.Count]);
                        }
                    }
                    break;
                case Class.TypeUse.TypeList.String:
                    List<string> tmpString = new List<string>();
                    result = this.Value.Read<string>(out tmpString, Reads[readDetialIndex].Values);
                    if (result)
                    {
                        if (tmpString.Count < Reads[readDetialIndex].Datas.Count)
                        {
                            All.Class.Error.Add("读取设备的返回数据量和设置要求返回的数据量不一致", Environment.StackTrace);
                            All.Class.Error.Add("错误数量", string.Format("要求数量:{0},实际数量:{1}",
                                Reads[readDetialIndex].Datas.Count, tmpString.Count));
                            return false;
                        }
                        foreach (int i in Reads[readDetialIndex].Datas.Keys)
                        {
                            value.Add(i, tmpString[value.Count]);
                        }
                    }
                    break;
                case Class.TypeUse.TypeList.UShort:
                    List<ushort> tmpUshort = new List<ushort>();
                    result = this.Value.Read<ushort>(out tmpUshort, Reads[readDetialIndex].Values);
                    if (result)
                    {
                        if (tmpUshort.Count < Reads[readDetialIndex].Datas.Count)
                        {
                            All.Class.Error.Add("读取设备的返回数据量和设置要求返回的数据量不一致", Environment.StackTrace);
                            All.Class.Error.Add("错误数量", string.Format("要求数量:{0},实际数量:{1}",
                                Reads[readDetialIndex].Datas.Count, tmpUshort.Count));
                            return false;
                        }
                        foreach (int i in Reads[readDetialIndex].Datas.Keys)
                        {
                            value.Add(i, tmpUshort[value.Count]);
                        }
                    }
                    break;
            }
            return result;
        }
    }
}
