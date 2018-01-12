using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Text;
namespace HKLabs
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            string ss = "DA01 1324 0200 000A 2C62 1859 0000 9D00 0000 5273 0000".Replace(" ", "");
            byte[] buff = ss.ToHexBytes();
            byte check = (byte)(((byte)(All.Class.Check.SumCheck(buff, 1, 21) ^ 0xFF) + 1) & 0xFF);
            ss= buff.ToHexString() + string.Format("{0:X2}", check) + "55";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new  测试多播委托());
        }
        public enum Modes
        {
            关机,
            制冷,
            制热,
            除湿
        }
        public class SetValue
        {
            public Modes Mode
            { get; set; }
            public byte Hz
            { get; set; }
            public byte SetTemp
            { get; set; }
            public byte HuanJing
            { get; set; }
            public byte PanGuan
            { get; set; }
            public byte JiXing
            { get; set; }
            public byte Speed
            { get; set; }
            public SetValue()
            {
                this.Mode = Modes.关机;
                this.Hz = 0;
                this.SetTemp = 81;
                this.HuanJing = 89;
                this.PanGuan = 81;
                this.JiXing = 0x24;
                this.Speed = 0;
            }
        }
        /// <summary>
        /// 从设置数据中生成指令
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Class2Str(SetValue value)
        {
            string result = "DA01 1324 0200 000A 2C62 1859 0000 9D00 0000 5273 0000".Replace(" ", "");
            byte[] buff = result.ToHexBytes();
            buff[3] = value.JiXing;

            switch (value.Mode)
            {
                case Modes.关机:
                    buff[4] = 0x00;
                    break;
                case Modes.制冷:
                    buff[4] = 0x01;
                    break;
                case Modes.制热:
                    buff[4] = 0x03;
                    break;
                case Modes.除湿:
                    buff[4] = 0x02;
                    break;
            }
            buff[5] = value.Hz;
            buff[6] = (byte)((value.SetTemp + 64) & 0xFF);
            buff[7] = (byte)((value.HuanJing + 64) & 0xFF);
            buff[9] = (byte)((value.PanGuan + 64) & 0xFF);
            buff[10] = 0x05;
            buff[11] = (byte)(((int)(value.Speed / 10)) & 0xFF);
            byte check = All.Class.Check.SumCheck(buff, 1, 21);
            return buff.ToHexString() + string.Format("{0:X2}", check) + "55";
        }
    }
}
