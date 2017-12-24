using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace All.Control.Mine
{
    public partial class Num : System.Windows.Forms.Control
    {
        Seven[] seven;
        float value = 8888.88f;
        /// <summary>
        /// 显示值
        /// </summary>
        [Description("显示值")]
        [Category("Shuai")]
        public float Value
        {
            get { return this.value; }
            set { this.value = value; flush(); }
        }
        bool point = true;
        /// <summary>
        /// 是否显示小数点
        /// </summary>
        [Description("是否显示小数点")]
        [Category("Shuai")]
        public bool Point
        {
            get { return point; }
            set { point = value; Init(); }
        }
        bool shadow = true;
        /// <summary>
        /// 是否有影子
        /// </summary>
        [Description("是否有影子")]
        [Category("Shuai")]
        public bool Shadow
        {
            get { return shadow; }
            set
            {
                shadow = value;
                for (int i = 0; i < seven.Length; i++)
                {
                    seven[i].Shardow = value;
                }
            }
        }
        int maxLen = 6;
        /// <summary>
        /// 最长有效数字
        /// </summary>
        [Description("最长有效数字")]
        [Category("Shuai")]
        public int MaxLen
        {
            get { return maxLen; }
            set { maxLen = value; Init(); }
        }
        bool symbol = false;
        /// <summary>
        /// 是否有符号
        /// </summary>
        [Description("是否有符号")]
        [Category("Shuai")]
        public bool Symbol
        {
            get { return symbol; }
            set { symbol = value; Init(); }
        }
        private Color shadowColor
        {
            get
            {
                Color result = Color.FromArgb(20, foreColor.R, foreColor.G, foreColor.B);
                if ((backColor.R + backColor.G + backColor.B) > 128 * 3)
                {
                    result = Color.FromArgb(60, foreColor.R, foreColor.G, foreColor.B);
                }
                return result;
            }
        }
        System.Drawing.Color foreColor = Color.Red;
        /// <summary>
        /// 字体颜色
        /// </summary>
        [Description("字体颜色")]
        [Category("Shuai")]
        public new System.Drawing.Color ForeColor
        {
            get { return foreColor; }
            set
            {
                foreColor = value;
                if (seven != null)
                {
                    for (int i = 0; i < seven.Length; i++)
                    {
                        seven[i].ForeColor = value;
                    }
                }
            }
        }
        System.Drawing.Color backColor = Color.Black;

        /// <summary>
        /// 背景色
        /// </summary>
        [Description("背景色")]
        [Category("Shuai")]
        public new System.Drawing.Color BackColor
        {
            get { return backColor; }
            set
            {
                backColor = value;
                if (seven != null)
                {
                    for (int i = 0; i < seven.Length; i++)
                    {
                        seven[i].BackColor = value;
                    }
                }
            }
        }
        public Num()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
            Init();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            Init();
            base.OnSizeChanged(e);
        }
        private void flush()
        {
            if (seven != null)
            {
                if (value.ToString().Length <= seven.Length)
                {
                    int count = 0;
                    for (int i = value.ToString().Length - 1, j = seven.Length - 1; i >= 0 && j >= 0; i--, j--)
                    {
                        seven[j].ForeColor = foreColor;
                        switch (value.ToString().Substring(i, 1))
                        {
                            case "0":
                                seven[j].Value = 0;
                                seven[j].Simplor = Seven.simplorList.Value;
                                break;
                            case "1":
                                seven[j].Value = 1;
                                seven[j].Simplor = Seven.simplorList.Value;
                                break;
                            case "2":
                                seven[j].Value = 2;
                                seven[j].Simplor = Seven.simplorList.Value;
                                break;
                            case "3":
                                seven[j].Value = 3;
                                seven[j].Simplor = Seven.simplorList.Value;
                                break;
                            case "4":
                                seven[j].Value = 4;
                                seven[j].Simplor = Seven.simplorList.Value;
                                break;
                            case "5":
                                seven[j].Value = 5;
                                seven[j].Simplor = Seven.simplorList.Value;
                                break;
                            case "6":
                                seven[j].Value = 6;
                                seven[j].Simplor = Seven.simplorList.Value;
                                break;
                            case "7":
                                seven[j].Value = 7;
                                seven[j].Simplor = Seven.simplorList.Value;
                                break;
                            case "8":
                                seven[j].Value = 8;
                                seven[j].Simplor = Seven.simplorList.Value;
                                break;
                            case "9":
                                seven[j].Value = 9;
                                seven[j].Simplor = Seven.simplorList.Value;
                                break;
                            case ".":
                                seven[j].Value = 0;
                                seven[j].Simplor = Seven.simplorList.Point;
                                break;
                            case "+":
                                seven[j].Value = 0;
                                seven[j].Simplor = Seven.simplorList.Add;
                                break;
                            case "-":
                                seven[j].Value = 0;
                                seven[j].Simplor = Seven.simplorList.Del;
                                break;
                        }
                        count++;
                    }
                    for (int i = 0; i < seven.Length - count; i++)
                    {
                        seven[i].ForeColor = shadowColor;
                        seven[i].Value = 0;
                        seven[i].Simplor = Seven.simplorList.Value;
                    }
                }
            }
        }
        private void Init()
        {
            if (seven != null)
            {
                for (int i = 0; i < seven.Length; i++)
                {
                    seven[i].Dispose();
                }
            }
            seven = new Seven[maxLen + (symbol ? 1 : 0) + (point ? 1 : 0)];
            for (int i = 0; i < seven.Length; i++)
            {
                seven[i] = new Seven();
                seven[i].Width = this.Width / seven.Length - 8;
                seven[i].Height = this.Height;
                seven[i].Left = this.Width / seven.Length * i;
                seven[i].FontSize = this.Height / 10;
                seven[i].FontSpace = this.Height / 30;
                seven[i].Top = 0;
                seven[i].Shardow = this.shadow;
                seven[i].BackColor = this.BackColor;
                seven[i].ForeColor = this.ForeColor;
                this.Controls.Add(seven[i]);
            }
            flush();
        }

    }
}
