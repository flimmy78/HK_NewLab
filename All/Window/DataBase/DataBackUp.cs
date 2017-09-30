using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace All.Window
{
    public partial class DataBackUp : All.Window.BestWindow
    {
        All.Data.DataReadAndWrite conn;
        public DataBackUp(All.Data.DataReadAndWrite conn)
        {
            this.conn = conn;
            InitializeComponent();
        }

        private void frmReportBackup_Load(object sender, EventArgs e)
        {

        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            if (conn.Conn.State != System.Data.ConnectionState.Open)
            {
                this.Show("当前数据库不是处于连接状态,不能清理日志", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (conn is All.Data.SqlServer)
            {
                if (this.conn.Conn.ServerVersion.Split('.')[0].ToInt() >= 10)//从Sql2008开始命令不一样了
                {
                    conn.Write(string.Format("ALTER DATABASE {0} SET RECOVERY SIMPLE", this.conn.Conn.Database));
                    conn.Write(string.Format("DBCC SHRINKDATABASE({0}, 0)", this.conn.Conn.Database));

                }
                else
                {
                    conn.Write(string.Format("DUMP TRANSACTION {0} WITH NO_LOG ", this.conn.Conn.Database));
                    conn.Write(string.Format("DBCC SHRINKDATABASE({0}, 0)", this.conn.Conn.Database));
                }
                return;
            }
        }
    }
}
