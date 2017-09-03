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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            All.Data.SqlServer sql = new All.Data.SqlServer();
            sql.Login("192.168.0.100", "HaiXinIndoor", "sa", "");
            All.Window.frmReportBackup back = new All.Window.frmReportBackup(sql);
            back.Show();
        }
    }
}
