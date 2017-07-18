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
    [ToolboxBitmap("Hexagon.bmp")]
    public partial class Hexagon : UserControl, IControl
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
        public Hexagon()
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
                if (isRegion)
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
                    g.DrawPath(new Pen(borderColor, borderThickness), GetThisPath());
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
            result.AddLine(Width * 0.2929f, 0, Width * 0.4142f, 0);
            result.AddLine(Width * 0.7071f, 0, Width, Height / 2);
            result.AddLine(Width, Height / 2, Width * 0.7071f, Height);
            result.AddLine(Width * 0.7071f, Height, Width * 0.2929f, Height);
            result.AddLine(Width * 0.2929f, Height, 0, Height / 2);
            result.CloseAllFigures();
            return result;
        }
    }
}
