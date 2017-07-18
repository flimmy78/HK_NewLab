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
    [ToolboxBitmap("Pipe.bmp")]
    public partial class Pipe : UserControl,IControl
    {
        /// <summary>
        /// 缺角类型
        /// </summary>
        public enum CornerList : int
        {
            上 = 2,
            下 = 4,
            左 = 8,
            右 = 16,
            无 = 32,
            上下 = 64,
            下上 = 64 << 8,
            上左 = 128,
            左上 = 128 << 8,
            右左 = 256,
            左右 = 256 << 8,
            右下 = 512,
            下右 = 512 << 8,
            上右 = 1024,
            右上 = 1024 << 8,
            左下 = 2048,
            下左 = 2048 << 8
        }
        CornerList corner = CornerList.无;
        /// <summary>
        /// 缺角类型
        /// </summary>
        [Description("缺角类型")]
        [Category("Shuai")]
        public CornerList Corner
        {
            get { return corner; }
            set { corner = value; SetRegion(); this.Invalidate(); }
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
        PointF[] allCutPoint = new PointF[12];
        public Pipe()
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
                int min = Math.Min(Width, Height);
                allCutPoint[0] = new PointF(0, 0);
                allCutPoint[1] = new PointF(min, 0);
                allCutPoint[2] = new PointF(Width - min, 0);
                allCutPoint[3] = new PointF(Width, 0);
                allCutPoint[4] = new PointF(0, min);
                allCutPoint[5] = new PointF(Width, min);
                allCutPoint[6] = new PointF(0, Height - min);
                allCutPoint[7] = new PointF(Width, Height - min);
                allCutPoint[8] = new PointF(0, Height);
                allCutPoint[9] = new PointF(min, Height);
                allCutPoint[10] = new PointF(Width - min, Height);
                allCutPoint[11] = new PointF(Width, Height);
            }

        }
        private void SetRegion()
        {
            int min = Math.Min(this.Width, this.Height);
            using (GraphicsPath gp = new GraphicsPath())
            {
                switch ((int)this.corner | (int)this.orientation)
                {
                    case (int)CornerList.无 | (int)System.Windows.Forms.Orientation.Horizontal:
                    case (int)CornerList.无 | (int)System.Windows.Forms.Orientation.Vertical:
                        gp.AddRectangle(this.ClientRectangle);
                        break;
                    case (int)CornerList.上 | (int)System.Windows.Forms.Orientation.Horizontal:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[0],allCutPoint[2],allCutPoint[11],allCutPoint[8]
                        });
                        break;
                    case (int)CornerList.上 | (int)System.Windows.Forms.Orientation.Vertical:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[0],allCutPoint[5],allCutPoint[11],allCutPoint[8]
                        });
                        break;
                    case (int)CornerList.右 | (int)System.Windows.Forms.Orientation.Horizontal:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[0],allCutPoint[3],allCutPoint[10],allCutPoint[8]
                        });
                        break;
                    case (int)CornerList.右 | (int)System.Windows.Forms.Orientation.Vertical:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[0],allCutPoint[3],allCutPoint[7],allCutPoint[8]
                        });
                        break;
                    case (int)CornerList.下 | (int)System.Windows.Forms.Orientation.Horizontal:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[0],allCutPoint[3],allCutPoint[11],allCutPoint[9]
                        });
                        break;
                    case (int)CornerList.下 | (int)System.Windows.Forms.Orientation.Vertical:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[0],allCutPoint[3],allCutPoint[11],allCutPoint[6]
                        });
                        break;
                    case (int)CornerList.左 | (int)System.Windows.Forms.Orientation.Horizontal:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[1],allCutPoint[3],allCutPoint[11],allCutPoint[8]
                        });
                        break;
                    case (int)CornerList.左 | (int)System.Windows.Forms.Orientation.Vertical:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[4],allCutPoint[3],allCutPoint[11],allCutPoint[8]
                        });
                        break;
                    case (int)CornerList.上右 | (int)System.Windows.Forms.Orientation.Vertical:
                    case (int)CornerList.右上 | (int)System.Windows.Forms.Orientation.Vertical:
                    case (int)CornerList.上右 | (int)System.Windows.Forms.Orientation.Horizontal:
                    case (int)CornerList.右上 | (int)System.Windows.Forms.Orientation.Horizontal:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[0],allCutPoint[5],allCutPoint[7],allCutPoint[8]
                        });
                        break;
                    case (int)CornerList.上下 | (int)System.Windows.Forms.Orientation.Horizontal:
                    case (int)CornerList.下上 | (int)System.Windows.Forms.Orientation.Horizontal:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[0],allCutPoint[2],allCutPoint[11],allCutPoint[9]
                        });
                        break;
                    case (int)CornerList.上下 | (int)System.Windows.Forms.Orientation.Vertical:
                    case (int)CornerList.下上 | (int)System.Windows.Forms.Orientation.Vertical:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[0],allCutPoint[5],allCutPoint[11],allCutPoint[6]
                        });
                        break;
                    case (int)CornerList.上左 | (int)System.Windows.Forms.Orientation.Horizontal:
                    case (int)CornerList.左上 | (int)System.Windows.Forms.Orientation.Horizontal:
                    case (int)CornerList.上左 | (int)System.Windows.Forms.Orientation.Vertical:
                    case (int)CornerList.左上 | (int)System.Windows.Forms.Orientation.Vertical:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[1],allCutPoint[2],allCutPoint[11],allCutPoint[8]
                        });
                        break;
                    case (int)CornerList.右下 | (int)System.Windows.Forms.Orientation.Horizontal:
                    case (int)CornerList.下右 | (int)System.Windows.Forms.Orientation.Horizontal:
                    case (int)CornerList.右下 | (int)System.Windows.Forms.Orientation.Vertical:
                    case (int)CornerList.下右 | (int)System.Windows.Forms.Orientation.Vertical:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[0],allCutPoint[3],allCutPoint[10],allCutPoint[9]
                        });
                        break;
                    case (int)CornerList.右左 | (int)System.Windows.Forms.Orientation.Horizontal:
                    case (int)CornerList.左右 | (int)System.Windows.Forms.Orientation.Horizontal:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[1],allCutPoint[3],allCutPoint[10],allCutPoint[8]
                        });
                        break;
                    case (int)CornerList.右左 | (int)System.Windows.Forms.Orientation.Vertical:
                    case (int)CornerList.左右 | (int)System.Windows.Forms.Orientation.Vertical:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[4],allCutPoint[3],allCutPoint[7],allCutPoint[8]
                        });
                        break;
                    case (int)CornerList.下左 | (int)System.Windows.Forms.Orientation.Horizontal:
                    case (int)CornerList.左下 | (int)System.Windows.Forms.Orientation.Horizontal:
                    case (int)CornerList.下左 | (int)System.Windows.Forms.Orientation.Vertical:
                    case (int)CornerList.左下 | (int)System.Windows.Forms.Orientation.Vertical:
                        gp.AddPolygon(new PointF[]
                        {
                            allCutPoint[4],allCutPoint[3],allCutPoint[11],allCutPoint[6]
                        });
                        break;       
             
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
            gp.AddRectangle(new RectangleF(this.borderThickness / 2, this.borderThickness / 2, this.Width - this.borderThickness, this.Height - this.borderThickness));
            return gp;
        }
    }
}
