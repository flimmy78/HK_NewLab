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
    public partial class 测试自动创建数据表 : Form
    {
        public 测试自动创建数据表()
        {
            InitializeComponent();
        }

        private void 测试自动创建数据表_Load(object sender, EventArgs e)
        {
            testAutoTable t = new testAutoTable();
            int a = Environment.TickCount;
            for (int i = 0; i < 10000000; i++)
            {
                double c = Convert.ToDouble(t.Foo(999));
            }
            a = Environment.TickCount - a;
            MessageBox.Show(a.ToString());
            Application.Exit();
        }
    }
    //[Serializable]
    public class testAutoTable : IAutoTable
    {
        [All.Data.Column(AutoAdd = true, Length = new int[] { 1 })]
        public int ID
        { get; set; }
        public object Foo(object value)
        {
            return value;
        }
    }
}
