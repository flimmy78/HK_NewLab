using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate
{
    public class Nothing:Communicate
    {
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
        public override void Init(Dictionary<string, string> buff)
        {
            if (buff.ContainsKey("Text"))
            {
                this.Text = buff["Text"];
            }
        }
        public override int DataRecive
        {
            get { throw new NotImplementedException(); }
        }
    }
}
