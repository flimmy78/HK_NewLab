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
    public partial class DelUser : BestWindow
    {
        All.Class.cUser allUser;
        public DelUser(All.Class.cUser user)
        {
            allUser = user;
            InitializeComponent();
        }

        private void DelUser_Load(object sender, EventArgs e)
        {
            cbbName.Items.Clear();
            if (allUser != null && allUser.AllUser.Count > 0)
            {
                allUser.AllUser.ForEach(user => cbbName.Items.Add(user.UserName));
                cbbName.SelectedIndex = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cbbName.Text == "Administrator")
            {
                this.Show("对不起,[Administratro]用户是默认管理员账户,不能删除", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cbbName.SelectedIndex < 0)
            {
                this.Show("当前没有选择需要删除的用户,或没有用户可以删除", "不能进行删除", MessageBoxButtons.OK);
                return;
            }
            if (this.Show(string.Format("是否删除指定的用户[{0}]?", cbbName.Text), "是否删除", MessageBoxButtons.OK, MessageBoxIcon.Question) != DialogResult.OK)
            {
                return;
            }
            allUser.AllUser.RemoveAt(allUser.AllUser.FindIndex(tmp => tmp.UserName == cbbName.Text));
            All.Class.cUser.Delete(cbbName.Text);
            this.Show("当前选中用户已成功删除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DelUser_Load(null, null);
        }
    }
}
