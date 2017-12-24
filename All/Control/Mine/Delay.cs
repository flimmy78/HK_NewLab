using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
namespace All.Control.Mine
{
    public partial class Delay : System.Windows.Forms.Control, All.Class.Style.ChangeTheme
    {
        Bitmap backImage;
        int index = 0;
        const int Count = 16;
        public Delay()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
            InitializeComponent();
        }
        public void ChangeFront(All.Class.Style.FrontColors color)
        { 
        }
        public void ChangeBack(All.Class.Style.BackColors color)
        {
            this.Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.Width <= 0)
            {
                return;
            }
            this.Height = this.Width;
            backImage = new Bitmap(this.Width, this.Height);
            base.OnSizeChanged(e);
        }
        /// <summary>
        /// 下一个画面
        /// </summary>
        public void Next()
        {
            index++;
            index = index % 16;
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (backImage == null && this.Width > 0)
            {
                backImage = new Bitmap(this.Width, this.Height);
            }
            if (backImage != null)
            {
                using (Graphics g = Graphics.FromImage(backImage))
                {
                    g.Clear(All.Class.Style.BackColor);
                    All.Class.GDIHelp.InitAntiAlias(g);
                    for (int i = 0; i < Count; i++)
                    {
                        g.FillPie(GetFill(i), 0, 0, this.Width - 1, this.Height - 1, i * 360 / Count, -360 / Count - 2);
                    }
                    g.FillEllipse(new SolidBrush(All.Class.Style.BackColor), this.Width/10, this.Height/10, this.Width * 4 / 5, this.Height * 4 / 5);
                }
                e.Graphics.DrawImageUnscaled(backImage, 0, 0);
            }
            base.OnPaint(e);
        }
        private SolidBrush GetFill(int i)
        {
            int r = (255 - All.Class.Style.BoardColor.R) / Count;
            int g = (255 - All.Class.Style.BoardColor.G) / Count;
            int b = (255 - All.Class.Style.BoardColor.B) / Count;
            r *= 1 + ((Count - i + index) % Count);
            g *= 1 + ((Count - i + index) % Count);
            b *= 1 + ((Count - i + index) % Count);
            r += All.Class.Style.BoardColor.R;
            g += All.Class.Style.BoardColor.G;
            b += All.Class.Style.BoardColor.B;
            return new SolidBrush(Color.FromArgb(r, g, b));
        }
    }
}
