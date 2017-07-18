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
    public partial class Triangle : UserControl, IControl
    {
        /// <summary>
        /// 三角形类型
        /// </summary>
        public enum TriangleList : int
        {
            普通三角形 = 1,
            直角三角形 = 2
        }
        /// <summary>
        /// 三角形朝向
        /// </summary>
        public enum BottomList : int
        {
            上 = 4,
            下 = 8,
            左 = 16,
            右 = 32
        }
        TriangleList triangles = TriangleList.普通三角形;
        /// <summary>
        /// 三角形类型
        /// </summary>
        [Description("三角形类型")]
        [Category("Shuai")]
        public TriangleList Triangles
        {
            get { return triangles; }
            set { triangles = value; SetRegion(); this.Invalidate(); }
        }
        BottomList bottoms = BottomList.下;
        /// <summary>
        /// 三角形朝向
        /// </summary>
        [Description("三角形朝向")]
        [Category("Shuai")]
        public BottomList Bottoms
        {
            get { return bottoms; }
            set { bottoms = value; SetRegion(); this.Invalidate(); }
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
        public Triangle()
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
                g.Clear(this.isRegion ? this.BackColor : this.Parent.BackColor);
                if (Halo)
                {
                    using (PathGradientBrush pgb = new PathGradientBrush(GetThisPath()))
                    {
                        pgb.SurroundColors = new Color[] { this.BackColor };
                        pgb.CenterColor = Color.White;
                        pgb.CenterPoint = new PointF(Width / 2, Height / 2);
                        g.FillRectangle(pgb, this.ClientRectangle);
                    }
                }
                if (this.borderThickness > 0)
                {
                    All.Class.GDIHelp.InitAntiAlias(g);
                    g.DrawPolygon(new Pen(this.borderColor, this.borderThickness), GetThisPath().PathPoints);
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

            switch ((int)triangles | (int)bottoms)
            {
                case 5:
                    result.AddPolygon(new PointF[] { 
                        new PointF(0, 0),
                        new PointF(Width,0),
                        new PointF(Width/2.0f,Height),
                        new PointF(0, 0)
                    });
                    break;
                case 9:
                    result.AddLine(Width / 2, 0, Width, Height);
                    result.AddLine(Width, Height, 0, Height);
                    break;
                case 17:
                    result.AddLine(0, 0, Width, Height / 2);
                    result.AddLine(Width, Height / 2, 0, Height);
                    break;
                case 33:
                    result.AddLine(0, Height / 2, Width, 0);
                    result.AddLine(Width, 0, Width, Height);
                    break;
                case 6:
                    result.AddLine(0, 0, Width, 0);
                    result.AddLine(Width, 0, Width, Height);
                    break;
                case 10:
                    result.AddLine(0, 0, 0, Height);
                    result.AddLine(0, Height, Width, Height);
                    break;
                case 18:
                    result.AddLine(0, Height, 0, 0);
                    result.AddLine(0, 0, Width, 0);
                    break;
                case 34:
                    result.AddLine(Width, 0, Width, Height);
                    result.AddLine(Width, Height, 0, Height);
                    break;
            }
            result.CloseAllFigures();
            return result;
        }
    }
}
