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
    public partial class AboutWindow : BestWindow
    {
        int closeCount = 10;
		bool isNeedRegion = true;
        bool autoClose = false;
        delegate void GetLocalCodeHandle();
        public AboutWindow(string systemName, bool isNeedRegion,bool autoClose)
        {
            InitializeComponent();
            lblName.Text = systemName;
            this.isNeedRegion = isNeedRegion;
            this.autoClose = autoClose;
        }
        public AboutWindow(string systemName, bool isNeedRegion)
            : this(systemName, isNeedRegion, false)
        {
        }
        private void GetLocalCode()
        {
            lblID.Text = All.Class.Code.Region.Encryption();
            if (isNeedRegion)
            {
                lblCode.Text = All.Class.Region.ReadValue(All.Class.Code.Region.RegionCode);
            }
            else
            {
                lblCode.Text = "无须单独授权";
            }
            txtCode.Text = lblCode.Text;
        }
        string guid = All.Class.Num.CreateGUID();
        private void AboutWindow_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new GetLocalCodeHandle(GetLocalCode));
            if (autoClose)
            {
                All.Class.Thread.CreateOrOpen(guid, () => FlushAutoClose(), 1000);
            }
        }
        private void FlushAutoClose()
        {
            this.CrossThreadDo(() =>
                {
                    if (closeCount <= 0)
                    {
                        this.Close();
                    }
                    this.Text = string.Format("{0}  自动关闭倒计时{1}秒", this.Text.Split(new string[] { "自动关闭倒计时" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim(), closeCount);
                    closeCount--;
                });
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (guid != "")
            {
                All.Class.Thread.Kill(guid);
            }
            base.OnClosing(e);
        }
        private void lblCode_Click(object sender, EventArgs e)
        {
            txtCode.Left = lblCode.Location.X;
            txtCode.Top = lblCode.Location.Y - 4;
            txtCode.Size = lblCode.Size;
            txtCode.Text = lblCode.Text;
            txtCode.Visible = true;
            lblCode.Visible = false;
        }
        private void txtCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    lblCode.Visible = true;
                    txtCode.Visible = false;
                    break;
                case Keys.Enter:
                    lblCode.Visible = true;
                    txtCode.Visible = false;
                    lblCode.Text = txtCode.Text;
                    string error = "";
                    if (Class.Code.Region.isRegion(lblID.Text, lblCode.Text,out error)==All.Class.Code.Region.ErrorList.注册码合格)
                    {
                        Class.Code.Region.WriteInfoToReion(lblID.Text, lblCode.Text);
                        MessageBox.Show("程序已成功注册,十分感谢您的使用", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("对不起,您输入的注册不正确,请输入正确的注册码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }

        }
    }
}
