using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace All.Meter
{
    /// <summary>
    /// HP34970设备的数据读取
    /// </summary>
    public class HP34970:Meter
    {
        Dictionary<string, string> initParm = new Dictionary<string, string>();
        /// <summary>
        /// 设备的初始化数据值
        /// </summary>
        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        bool init = false;
        int len = 0;
        public override void Init(Dictionary<string, string> initParm)
        {
            this.TimeOut = 2000;
            if (initParm.ContainsKey("Len"))
            {
                len = initParm["Len"].ToInt(); 
            }
            base.Init(initParm);
        }
        private bool Init()
        {
            if ((!init || !this.Conn) && this.Parent.IsOpen)
            {
                string buff = "TRIG:SOURCE IMMediate\r\n";  //'立即连续触发
                this.Write<string>(buff);
                Thread.Sleep(100);
                buff = "TRIG:TIMer 0\r\n";                  //'通道扫描间隔
                this.Write<string>(buff);
                Thread.Sleep(100); 
                buff = "TRIG:COUNT 1\r\n";                  //'扫描次数
                this.Write<string>(buff);
                Thread.Sleep(100);
                buff = "FORM:READ:ALAR OFF\r\n";            //'去掉返回数据中的报警信息
                this.Write<string>(buff);
                Thread.Sleep(100);
                buff = "FORM:READ:CHAN OFF\r\n";            //'去掉返回数据中的通道号信息
                this.Write<string>(buff);
                Thread.Sleep(100);
                buff = "FORM:READ:TIME OFF\r\n";            //'去掉返回数据中的时间信息
                this.Write<string>(buff);
                Thread.Sleep(100);
                buff = "FORM:READ:UNIT OFF\r\n";            //'去掉返回数据中的单位信息
                this.Write<string>(buff);
                Thread.Sleep(100);
                init = true;
            }
            return init;
        }
        private void ScanHP()
        {
            string buff = "trig:sour imm\r\n";
            this.Write<string>(buff);
            Thread.Sleep(50);
            buff = "trig:coun 1\r\n";
            this.Write<string>(buff);
            Thread.Sleep(50);
            buff = "ABORT\r\n";
            this.Write<string>(buff);
            Thread.Sleep(50);
            buff = "INIT\r\n";
            this.Write<string>(buff);
            Thread.Sleep(400);
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            value = new List<T>();
            if (!Init())
            {
                return false;
            }
            ScanHP();
            lock (lockObject)
            {
                try
                {
                    string readValue = "";
                    if (this.WriteAndRead<string, string>("FETC?\r\n", 16 * len - 1, out readValue))
                    {
                        readValue = readValue.Trim();
                        string[] buff = readValue.Split(',');
                        string[] tmp;
                        All.Class.TypeUse.TypeList type = All.Class.TypeUse.GetType<T>();
                        for (int i = 0; i < buff.Length; i++)
                        {
                            tmp = buff[i].Split('E');
                            if (tmp.Length != 2)
                            {
                                All.Class.Error.Add("当前34970返回的数据,不包含E分隔的数据,请更新驱动");
                                return false;
                            }
                            switch (type)
                            {
                                case Class.TypeUse.TypeList.Double:
                                    value.Add((T)(object)(tmp[0].ToDouble() * Math.Pow(10, tmp[1].ToInt())));
                                    break;
                                case Class.TypeUse.TypeList.Float:
                                    value.Add((T)(object)(float)(tmp[0].ToFloat() * Math.Pow(10, tmp[1].ToInt())));
                                    break;
                                default:
                                    All.Class.Error.Add("当前34970无法返回指定的数据结构,请检查程序读取数据结构,或更新此驱动");
                                    return false;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                    return false;
                }
            }
            return true;
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            throw new NotImplementedException();
        }
    }
}
