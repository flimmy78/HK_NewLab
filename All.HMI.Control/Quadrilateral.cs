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
    public partial class Quadrilateral : UserControl, IControl
    {

        /// <summary>
        /// 四边形类型
        /// </summary>
        public enum QuadrilateralList : int
        {
            上 = 4,
            下 = 8,
            左 = 16,
            右 = 32,
            中 = 64
        }
        QuadrilateralList quadrilaterals = QuadrilateralList.中;
        /// <summary>
        /// 四边形类型
        /// </summary>
        [Description("四边形类型")]
        [Category("Shuai")]
        public QuadrilateralList Quadrilaterals
        {
            get { return quadrilaterals; }
            set { quadrilaterals = value; SetRegion(); this.Invalidate(); }
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
        float apex = 0.5f;
        /// <summary>
        /// 四边形顶点所占比例
        /// </summary>
        [Description("四边形顶点所占比例")]
        [Category("Shuai")]
        public float Apex
        {
            get { return apex; }
            set { apex = value; SetRegion(); this.Invalidate(); }
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
        public Quadrilateral()
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
                        g.FillRectangle(pgb, this.ClientRectangle);
                    }
                }
                else
                {
                    g.FillPath(new SolidBrush(this.BackColor), GetThisPath());
                }
                if (this.borderThickness > 0)
                {
                    All.Class.GDIHelp.InitAntiAlias(g);
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
        private GraphicsPath GetThisPath(out Point center)
        {
            center = new Point(this.Width / 2, this.Height / 2);
            GraphicsPath result = new GraphicsPath();

            switch (quadrilaterals)
            {
                case QuadrilateralList.中:
                    result.AddLine(Width * apex, 0, Width, Height * apex);
                    result.AddLine(Width, Height * apex, Width * (1 - apex), Height);
                    result.AddLine(Width * (1 - apex), Height, 0, Height * (1 - apex));
                    break;
                case QuadrilateralList.上:
                    result.AddLine(Width * apex, 0, Width, 0);
                    result.AddLine(Width, 0, Width * (1 - apex), Height);
                    result.AddLine(Width * (1 - apex), Height, 0, Height);
                    break;
                case QuadrilateralList.下:
                    result.AddLine(0, 0, Width * apex, 0);
                    result.AddLine(Width * apex, 0, Width, Height);
                    result.AddLine(Width, Height, Width * (1 - apex), Height);
                    break;
                case QuadrilateralList.左:
                    result.AddLine(0, 0, Width, apex * Height);
                    result.AddLine(Width, apex * Height, Width, Height);
                    result.AddLine(Width, Height, 0, Height * (1 - apex));
                    break;
                case QuadrilateralList.右:
                    result.AddLine(Width, 0, Width, Height * apex);
                    result.AddLine(Width, Height * apex, 0, Height);
                    result.AddLine(0, Height, 0, Height * (1 - apex));
                    break;
            }
            result.CloseAllFigures();
            return result;
        }
        private GraphicsPath GetThisPath()
        {
            Point result;
            return GetThisPath(out result);
        }
    }
}
