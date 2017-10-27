using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate
{
    /// <summary>
    /// 空通讯类,因为部分设备,压根不须要通讯类,它自带通讯驱动,如USB通讯等
    /// </summary>
    public class Nothing:Communicate
    {
        Dictionary<string, string> initParm = new Dictionary<string, string>();

        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override void Read<T>(out T value)
        {
            throw new NotImplementedException();
        }
        public override void Send<T>(T value)
        {
            throw new NotImplementedException();
        }
        public override void Send<T>(T value, Dictionary<string, string> buff)
        {
            throw new NotImplementedException();
        }
        protected override void OnGetArgs(object sender, Base.Base.ReciveArgs reciveArgs)
        {
            throw new NotImplementedException();
        }
        public override void Open()
        {
            if (this.Meters != null && this.Meters.Count > 0)
            {
                this.Meters.ForEach(meter => meter.Open());
            }
        }
        public override void Close()
        {
            if (this.Meters != null && this.Meters.Count > 0)
            {
                this.Meters.ForEach(meter => meter.Close());
            }
        }
        public override void InitCommunite(Dictionary<string, string> buff)
        {
            throw new NotImplementedException();
        }
        public override bool IsOpen
        {
            get { return true; }
        }
        public override int DataRecive
        {
            get { throw new NotImplementedException(); }
        }
    }
}
