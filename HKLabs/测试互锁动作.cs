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
    public partial class 测试互锁动作 : Form
    {
        public 测试互锁动作()
        {
            InitializeComponent();
        }
        All.Communicate.Udp main = new All.Communicate.Udp();
        All.Communicate.Udp[] client = new All.Communicate.Udp[5];

        All.Meter.SSLockClient[] ssClient = new All.Meter.SSLockClient[5];
        All.Meter.SSLockMain ssMain = new All.Meter.SSLockMain();
        private void 测试互锁动作_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> parm = new Dictionary<string, string>();
            parm.Add("LocalPort", "3000");
            main.Init(parm);
            main.Open();
            main.CommunicateErrorRaise += main_CommunicateErrorRaise;
            ssMain = new All.Meter.SSLockMain();
            ssMain.Parent = main;
            ssMain.Init(new Dictionary<string, string>());
            ssMain.NeedMainReset += ssMain_NeedMainReset;
            ssMain.NeedMainStart += ssMain_NeedMainStart;
            ssMain.NeedMainSwitch += ssMain_NeedMainSwitch;
            ssMain.Queue.QueueItemChange += Queue_QueueItemChange;
            for (int i = 0; i < client.Length; i++)
            {
                parm = new Dictionary<string, string>();
                parm.Add("LocalPort", string.Format("{0}", 3001 + i));
                parm.Add("RemotHost", "127.0.0.1");
                parm.Add("RemotPort", "3000");
                client[i] = new All.Communicate.Udp();
                client[i].Init(parm);
                client[i].Open();
                client[i].CommunicateErrorRaise += main_CommunicateErrorRaise;
                ssClient[i] = new All.Meter.SSLockClient();
                ssClient[i].Parent = client[i];
                ssClient[i].Init(new Dictionary<string, string>());
                ssClient[i].NeedClientReset += 测试互锁动作_NeedClientReset;
                ssClient[i].NeedClientSwitch += 测试互锁动作_NeedClientSwitch;
                ssClient[i].Tag = i+1;
            }
        }

        void Queue_QueueItemChange()
        {
            this.CrossThreadDo(() =>
                {
                    listBox1.Items.Clear();
                    ssMain.Queue.Value(0).ForEach(v =>
                        {
                            listBox1.Items.Add(v);
                        });
                });
        }

        void ssMain_NeedMainSwitch(object sender, EventArgs e)
        {
            this.CrossThreadDo(() =>
                {
                    listBox2.Items.Add(string.Format("主机 须要切换线路", sender));
                    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                });
        }

        void ssMain_NeedMainStart(object sender, EventArgs e)
        {
            this.CrossThreadDo(() =>
                {
                    listBox2.Items.Add(string.Format("主机 须要开始测试互锁动作", sender));
                    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                });
        }

        void ssMain_NeedMainReset(object sender, EventArgs e)
        {
            this.CrossThreadDo(() =>
                {
                    listBox2.Items.Add(string.Format("主机 须要复位线路", sender));
                    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                });
        }

        void 测试互锁动作_NeedClientSwitch(object sender, EventArgs e)
        {
            this.CrossThreadDo(() =>
                {
                    listBox2.Items.Add(string.Format("{0}#  须要切换线路", sender));
                    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                });
        }

        void 测试互锁动作_NeedClientReset(object sender, EventArgs e)
        {
            this.CrossThreadDo(() =>
                {
                    listBox2.Items.Add(string.Format("{0}#  须要删除连接", sender));
                    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                });
        }

        void main_CommunicateErrorRaise(Exception e)
        {
            throw new NotImplementedException();
        }
        private int GetIndex()
        {
            if (radioButton2.Checked)
            { return 1; }
            if (radioButton3.Checked)
            { return 2; }
            if (radioButton4.Checked)
            { return 3; }
            if (radioButton5.Checked)
            { return 4; }
            return 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ssClient[GetIndex()].Write(All.Meter.SSLockClient.SetStatueList.请求测试);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ssClient[GetIndex()].Write(All.Meter.SSLockClient.SetStatueList.开始测试);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ssClient[GetIndex()].Write(All.Meter.SSLockClient.SetStatueList.删除请求);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ssMain.Write(All.Meter.SSLockMain.SetStatueList.互锁切换执行完毕);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ssMain.Write(All.Meter.SSLockMain.SetStatueList.互锁操作执行完毕);
        }
    }
}
