using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HKLabs
{
    public partial class form3 : All.Window.BestWindow
    {

        [TypeConverter(typeof(TypeConverterTable2String))]
        public Type AAA
        { get; set; }
        public form3()
        {
            InitializeComponent();
        }

        All.Communicate.AutoReadAndWrite ar = new All.Communicate.AutoReadAndWrite();
        private void form3_Load(object sender, EventArgs e)
        {
            ar.Load();
            System.Threading.Thread.Sleep(1000);
            ar.Datas.StringValue.ValueChange += StringValue_ValueChange;
            ar.Communicates[1].Meters[0].Value.WriteInternal<string>("123", 2);
            //ar.Close();
        }

        void StringValue_ValueChange(int index, string lastValue, string value, string info)
        {
            MessageBox.Show(string.Format("{0},{1}", lastValue, value));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
    }
}
