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
    [ToolboxBitmap("Circle.bmp")]
    public partial class Circle : System.Windows.Forms.UserControl, IControl
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
        public bool IsRegion
        {
            get { return isRegion; }
            set { isRegion = value; SetRegion(); this.Invalidate(); }
        }

        Bitmap back;
        public Circle()
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
                    g.DrawEllipse(new Pen(borderColor, borderThickness), borderThickness/2, borderThickness/2, Width - borderThickness, Height - borderThickness);
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
            if (this.isRegion)
            {
                this.Region = new Region(GetThisPath());
            }
            else
            {
                this.Region = new Region(new Rectangle(0, 0, Width, Height));
            }
        }
        private GraphicsPath GetThisPath()
        {
            GraphicsPath result = new GraphicsPath();
            result.AddEllipse(0, 0, this.Width, this.Height);
            return result;
        }
    }
}
