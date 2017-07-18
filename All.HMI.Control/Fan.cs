using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace All.Control.HMI
{
    [ToolboxBitmap("Fan.bmp")]
    public partial class Fan : UserControl, IControl
    {
        Color borderColor = Color.Black;
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色")]
        [Category("Shuai")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; this.Invalidate(); }
        }
        int borderThickness = 0;
        /// <summary>
        /// 边框粗细
        /// </summary>
        [Description("边框粗细")]
        [Category("Shuai")]
        public int BorderThickness
        {
            get { return borderThickness; }
            set { borderThickness = value; this.Invalidate(); }
        }
        bool halo = true;
        /// <summary>
        /// 是否有光晕
        /// </summary>
        [Description("是否有光晕")]
        [Category("Shuai")]
        public bool Halo
        {
            get { return halo; }
            set { halo = value; this.Invalidate(); }
        }
        System.Windows.Forms.Orientation orientation = System.Windows.Forms.Orientation.Horizontal;
        /// <summary>
        /// 控件方向
        /// </summary>
        [Description("控件方向")]
        [Category("Shuai")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public System.Windows.Forms.Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; this.Invalidate(); }
        }
        bool isRegion = true;
        /// <summary>
        /// 是否切割图形
        /// </summary>
        [Description("是否切割图形")]
        [Category("Shuai")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsRegion
        {
            get { return isRegion; }
            set { isRegion = true; SetRegion(); this.Invalidate(); }
        }
        int fans = 3;
        /// <summary>
        /// 设置扇叶的数量
        /// </summary>
        [Description("设置扇叶的数量")]
        [Category("Shuai")]
        public int Fans
        {
            get { return fans; }
            set
            {
                if (value > 1)
                {
                    fans = value; SetRegion(); this.Invalidate();
                }
            }
        }
        
        int local = 0;
        Bitmap back;
        public Fan()
        {
            InitializeComponent();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.Width < borderThickness)
            {
                this.Width = borderThickness * 2;
            }
            if (this.Height < borderThickness)
            {
                this.Height = borderThickness * 2;
            }
            InitBack();
            this.SetRegion();
            base.OnSizeChanged(e);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (back == null)
            {
                InitBack();
            }
            using (Graphics g = Graphics.FromImage(back))
            {
                if (this.isRegion)
                {
                    g.Clear(this.BackColor);
                }
                else
                {
                    g.Clear(this.Parent.BackColor);
                }
                if (Halo)
                {
                    using (PathGradientBrush pgb = new PathGradientBrush(GetThisPath()))
                    {
                        pgb.SurroundColors = new Color[] { this.BackColor };
                        pgb.CenterColor = Color.White;
                        pgb.CenterPoint = new PointF(Width / 2, Height / 2);
                        g.FillEllipse(pgb, this.ClientRectangle);
                    }
                }
                if (this.borderThickness > 0)
                {
                    All.Class.GDIHelp.InitAntiAlias(g);
                    g.DrawEllipse(new Pen(borderColor, borderThickness), borderThickness / 2, borderThickness / 2, Width - borderThickness, Height - borderThickness);
                }
            }
            e.Graphics.DrawImageUnscaled(back, 0, 0);
            base.OnPaint(e);
        }
        private void InitBack()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                back = new Bitmap(this.Width, this.Height);
            }
        }
        private void SetRegion()
        {
            //if (this.isRegion)
            //{
                this.Region = new Region(GetThisPath());
            //}
            //else
            //{
            //    this.Region = new Region(new Rectangle(0, 0, Width, Height));
            //}
        }
        private GraphicsPath GetThisPath()
        {
            GraphicsPath result = new GraphicsPath();
            for (int i = 0; i < fans; i++)
            {
                result.AddPie(new Rectangle(0, 0, Width, Height), (i * 3 + local) * 120 / fans - 60 / fans, 120 / fans);
            }
            return result;
        }
        /// <summary>
        /// 转动风扇
        /// </summary>
        public new void Move()
        {
            this.CrossThreadDo(() =>
                {
                    local++;
                    local = local % 3;
                    SetRegion();
                });
        }
    }
}
