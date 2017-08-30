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
    public partial class AddUser : BestWindow
    {
        All.Class.cUser User;
        public AddUser(All.Class.cUser user)
        {
            this.User = user;
            InitializeComponent();
            SetLevel(typeof(All.Class.cUser.LevelList));
        }
        /// <summary>
        /// 设置权限选项
        /// </summary>
        /// <param name="values"></param>
        public void SetLevel(string[] values)
        {
            cbbLevel.Items.Clear();
            if (values == null)
            {
                return;
            }
            for (int i = 0; i < values.Length; i++)
            {
                cbbLevel.Items.Add(values[i]);
            }
            if (cbbLevel.Items.Count > 0)
            {
                cbbLevel.SelectedIndex = cbbLevel.Items.Count - 1;
            }
        }
        /// <summary>
        /// 设置权限选项
        /// </summary>
        /// <param name="levelEnumType"></param>
        public void SetLevel(Type levelEnumType)
        {
            cbbLevel.Items.Clear();
            Enum.GetNames(levelEnumType).ToList().ForEach(
                level => cbbLevel.Items.Add(level));
            if (cbbLevel.Items.Count > 0)
            {
                cbbLevel.SelectedIndex = cbbLevel.Items.Count - 1;
            }
        }
        private void AddUser_Load(object sender, EventArgs e)
        {
        }
       
        private void btnOk_Click(object sender, EventArgs e)
        {
            MessageWindow mw;
            if (User.AllUser.FindIndex(user => user.UserName == txtName.Text) >= 0)
            {
                txtName.Focus();
                mw = new MessageWindow("当前要添加的用户已存在,不能添加", "错误!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mw.ShowDialog();
                return;
            }

            if (txtPassword.Text != txtPasswordAgain.Text)
            {
                txtPassword.Focus();
                mw = new All.Window.MessageWindow("两次输入的密码不一致,请重新输入密码!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mw.ShowDialog();
                return;
            }
            All.Class.cUser.UserSet us = new Class.cUser.UserSet();
            us.UserName = txtName.Text;
            us.PassWord = txtPassword.Text;
            us.Level = cbbLevel.Text;
            us.Save();
            User.AllUser.Add(us);
            mw = new MessageWindow("当前用户已成功添加", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mw.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
