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
        public bool Read(int readDetialIndex)
        {
            bool result = true;
            switch (Reads[readDetialIndex].ReadType)
            {
                case Class.TypeUse.TypeList.Boolean:
                    List<bool> tmpBool = new List<bool>();
                    result = this.Value.Read<bool>(out tmpBool, Reads[readDetialIndex].Values);
                    break;
            }
            return result;
        }
    }
}
