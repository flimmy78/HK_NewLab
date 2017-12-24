using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    /// <summary>
    /// 三菱FX网络通讯
    /// </summary>
    public class MitsubishiFxNet:Meter
    {
        public override Dictionary<string, string> InitParm
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            base.Init(initParm);
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            throw new NotImplementedException();
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            throw new NotImplementedException();
        }
    }
}
