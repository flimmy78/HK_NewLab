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
    public partial class DelayWindow : BestWindow
    {
        /// <summary>
        /// 显示的内容文本
        /// </summary>
        public string Info
        {
            get { return lblText.Text; }
            set
            {
                this.CrossThreadDo(() =>
                    {
                        lblText.Text = value;
                        this.Width = lblText.Left + delay1.Left + (int)lblText.CreateGraphics().MeasureString(lblText.Text, lblText.Font).Width;
                        this.Left = Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2;
                        btnCancel.Left = this.Width / 2 - btnCancel.Width / 2;
                    });
            }
        }
        /// <summary>
        /// 要延时执行的动作
        /// </summary>
        public Action Action
        {
            get { return action; }
            set { action = value; }
        }
        Action action = null;
        string threadName = "";
        /// <summary>
        /// 后台线程执行耗时操作
        /// </summary>
        /// <param name="info">显示文本</param>
        /// <param name="action">须要执行的操作</param>
        public DelayWindow(string info,Action action) :
            this("", info,action)
        { }
        public DelayWindow()
            : this("", null)
        { }
        /// <summary>
        /// 后台线程执行耗时操作
        /// </summary>
        /// <param name="text">标题</param>
        /// <param name="info">显示文本</param>
        /// <param name="action">须要执行的操作</param>
        public DelayWindow(string text, string info,Action action)
        {
            InitializeComponent();
            this.action = action;
            this.Text = text;
            this.Top = Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2;
            this.Info = info;
        }

        private void DelayWindow_Load(object sender, EventArgs e)
        {
            timer1.Tick += timer1_Tick;
            timer1.Enabled = true;
            threadName = All.Class.Thread.CreateOrOpen(Run);
        }
        private void Run()
        {
            if (action != null)
            {
                action();
            }
            All.Class.Thread.Run(() =>
                {
                    this.CrossThreadDo(() => this.Close());
                });
            All.Class.Thread.Kill(threadName);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (threadName != "")
            {
                All.Class.Thread.Kill(threadName);
            }
            timer1.Tick -= timer1_Tick;
            timer1.Enabled = false;
            timer1.Stop();
            base.OnClosing(e);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            delay1.Next();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
