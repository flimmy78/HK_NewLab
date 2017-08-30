using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class BYK:All.Meter.Meter
    {
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
