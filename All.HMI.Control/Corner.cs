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
    [ToolboxBitmap("Corner.bmp")]
    public partial class Corner : UserControl,IControl
    {
        /// <summary>
        /// 弯管方向
        /// </summary>
        public enum CornerList : int
        {
            上 = 4,
            下 = 8,
            左 = 16,
            右 = 32
        }
        CornerList conrners = CornerList.上;
        /// <summary>
        /// 弯管方向
        /// </summary>
        [Description("弯管方向")]
        [Category("Shuai")]
        public CornerList Conrners
        {
            get { return conrners; }
            set { conrners = value; SetRegion(); this.Invalidate(); }
        }
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
        bool isRegion = false;
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
        public Corner()
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
                All.Class.GDIHelp.InitAntiAlias(g);
                g.Clear(this.Parent.BackColor);
                Point center = GetCenter();
                if (Halo)
                {
                    using (GraphicsPath gp = new GraphicsPath())
                    {
                        Rectangle tmp = new Rectangle(center.X - Width, center.Y - Height, Width * 2, Height * 2);
                        gp.AddEllipse(tmp);
                        ColorBlend cb = new ColorBlend();
                        cb.Colors = new Color[] { this.BackColor, System.Windows.Forms.ControlPaint.LightLight(System.Windows.Forms.ControlPaint.LightLight(BackColor)), this.BackColor, this.BackColor };
                        cb.Positions = new float[] { 0, 0.25f, 0.5f, 1f };
                        using (PathGradientBrush pgb = new PathGradientBrush(gp))
                        {
                            pgb.InterpolationColors = cb;
                            g.FillEllipse(pgb, tmp);
                        }
                        g.FillEllipse(new SolidBrush(this.Parent.BackColor), center.X - Width / 2.0f, center.Y - Height / 2.0f, Width, Height);
                    }
                }
                if (this.borderThickness > 0)
                {
                    g.DrawEllipse(new Pen(this.borderColor, this.borderThickness), center.X - Width / 2.0f - this.borderThickness / 2.0f, center.Y - Height / 2.0f - this.borderThickness / 2.0f, Width + this.borderThickness, Height + this.borderThickness);
                    g.DrawEllipse(new Pen(this.borderColor, this.borderThickness), center.X - Width + this.borderThickness / 2.0f, center.Y - Height + this.borderThickness / 2.0f, Width * 2 - this.borderThickness, Height * 2 - this.borderThickness);
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
        private Point GetCenter()
        {
            Point result = Point.Empty;
            switch (conrners)
            {
                case CornerList.上:
                    result = new Point(Width, 0);
                    break;
                case CornerList.下:
                    result = new Point(0, Height);
                    break;
                case CornerList.左:
                    result = new Point(0, 0);
                    break;
                case CornerList.右:
                    result = new Point(Width, Height);
                    break;
            }
            return result;
        }
        private GraphicsPath GetThisPath()
        {
            GraphicsPath result = new GraphicsPath();

            switch (conrners)
            {
                case CornerList.上:
                    result.AddPie(Width / 2, -Height / 2, Width, Height, 89, 92);
                    result.AddPie(0, -Height, Width * 2, Height * 2, 181, -92);
                    break;
                case CornerList.下:
                    result.AddPie(-Width, 0, Width * 2, Height * 2, 269, 92);
                    result.AddPie(-Width / 2, Height / 2, Width, Height, 1, -92);
                    break;
                case CornerList.左:
                    result.AddPie(-Width / 2, -Height / 2, Width, Height, -1, 92);
                    result.AddPie(-Width, -Height, Width * 2, Height * 2, 91, -92);
                    break;
                case CornerList.右:
                    result.AddPie(0, 0, Width * 2, Height * 2, 179, 92);
                    result.AddPie(Width / 2, Height / 2, Width, Height, 271, -92);
                    break;
            }
            result.CloseAllFigures();
            return result;
        }
        //private GraphicsPath GetThisPath()
        //{
        //    Point result;
        //    return GetThisPath(out result);
        //}
    }
}
