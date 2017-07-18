using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace All.Control.HMI
{
    [ToolboxBitmap("Square.bmp")]
    public partial class Square : System.Windows.Forms.UserControl,IControl
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
        public System.Windows.Forms.Orientation Orientation
        {
            get { return orientation; } 
            set { orientation = value; this.Invalidate(); }
        }
        int radius = 0;
        /// <summary>
        /// 圆角半径
        /// </summary>
        [Description("圆角半径")]
        [Category("Shuai")]
        public int Radius
        {
            get { return radius; }
            set { radius = value; SetRegion(); this.Invalidate(); }
        }
        bool isRegion = false;
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
            set { isRegion = value; SetRegion(); this.Invalidate(); }
        }
        bool borderSeal = true;
        /// <summary>
        /// 边框是否封口
        /// </summary>
        [Description("边框是否封口")]
        [Category("Shuai")]
        public bool BorderSeal
        {
            get { return borderSeal; }
            set { borderSeal = value; this.Invalidate(); }
        }
        Bitmap back;
        public Square()
        {
            InitializeComponent();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
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
                g.Clear(this.BackColor);
                if (Halo)
                {
                    LinearGradientMode mode = LinearGradientMode.Horizontal;
                    if (System.Windows.Forms.Orientation.Horizontal == this.Orientation)
                    {
                        mode = LinearGradientMode.Vertical;
                    }
                    using (LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, BackColor, System.Windows.Forms.ControlPaint.LightLight(System.Windows.Forms.ControlPaint.LightLight(BackColor)), mode))
                    {
                        lgb.SetSigmaBellShape(0.5f);
                        g.FillRectangle(lgb, this.ClientRectangle);
                    }
                }
                if (this.borderThickness > 0)
                {
                    if (borderSeal)
                    {
                        g.DrawPath(new Pen(borderColor, borderThickness), GetGraphicsPath());
                    }
                    else 
                    {
                        switch (this.orientation)
                        {
                            case System.Windows.Forms.Orientation.Horizontal:
                                g.DrawLine(new Pen(borderColor, borderThickness), 0, borderThickness / 2, Width, borderThickness / 2);
                                g.DrawLine(new Pen(borderColor, borderThickness), 0, Height - borderThickness / 2, Width, Height - borderThickness / 2);
                                break;
                            case System.Windows.Forms.Orientation.Vertical:
                                g.DrawLine(new Pen(borderColor, borderThickness), Width - borderThickness / 2, 0, Width - borderThickness / 2, Height);
                                g.DrawLine(new Pen(borderColor, borderThickness), borderThickness / 2, 0, borderThickness / 2, Height);
                                break;
                        }
                    }
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
            using (GraphicsPath gp = new GraphicsPath())
            {
                if (this.Radius > 0)
                {
                    gp.AddArc(0, 0, this.Radius, this.Radius, 180, 90);

                    gp.AddArc(this.Width - this.radius-1, 0, this.radius, this.radius, 270, 90);

                    gp.AddArc(this.Width - this.radius, this.Height - this.radius , this.radius, this.radius, -1, 92);

                    gp.AddArc(0, this.Height - this.radius - 1, this.radius, this.radius, 90, 90);
                }
                else
                {
                    gp.AddRectangle(this.ClientRectangle);
                }
                if (this.isRegion)
                {
                    this.Region = new Region(gp);
                }
                else
                {
                    this.Region = new Region(new Rectangle(0, 0, Width, Height));

                }
            }
        }
        private GraphicsPath GetGraphicsPath()
        {
            GraphicsPath gp = new GraphicsPath();
            if (this.Radius > 0)
            {
                gp.AddArc(0, 0, this.Radius, this.Radius, 180, 90);

                gp.AddArc(this.Width - this.radius - 1, 0, this.radius, this.radius, 270, 90);

                gp.AddArc(this.Width - this.radius - 1, this.Height - this.radius - 1, this.radius, this.radius, 0, 90);

                gp.AddArc(0, this.Height - this.radius - 1, this.radius, this.radius, 90, 90);

            }
            gp.AddRectangle(new RectangleF(this.borderThickness / 2, this.borderThickness / 2, this.Width - this.borderThickness, this.Height - this.borderThickness));
            return gp;
        }
    }
}
