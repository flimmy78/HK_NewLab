using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;
namespace All.Control.Mine
{
    public partial class GridJiLiang : System.Windows.Forms.UserControl, All.Class.Style.ChangeTheme
    {
        const float Min = 0;
        const float Max = 10;
        /// <summary>
        /// 边框空白区域
        /// </summary>
        const int Space = 8;
        /// <summary>
        /// 文本宽度
        /// </summary>
        int textSpace = 60;
        /// <summary>
        /// 文本宽度
        /// </summary>
        [Description("文本宽度")]
        [Category("Shuai")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        public int TextSpace
        {
            get { return textSpace; }
            set { textSpace = value; this.Invalidate(); }
        }
        /// <summary>
        /// 标题行高度
        /// </summary>
        const int TextHeight = 20;
        private ChannelCollection channels;
        /// <summary>
        /// 计量曲线集合
        /// </summary>
        [Description("计量曲线集合")]
        [Category("Shuai")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        public ChannelCollection Channels
        {
            get
            {
                if (this.channels == null)
                {
                    this.channels = new ChannelCollection(this.Invalidate);
                }
                return this.channels;
            }
        }
        int rowsCount = 10;
        /// <summary>
        /// 行数量
        /// </summary>
        [Description("行数量")]
        [Category("Shuai")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        public int RowsCount
        {
            get { return rowsCount; }
            set
            {
                if (rowsCount <= 0)
                {
                    return;
                }
                rowsCount = value;
                this.Invalidate();
            }
        }
        int columnsCount = 10;
        /// <summary>
        /// 列数量
        /// </summary>
        [Description("列数量")]
        [Category("Shuai")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        public int ColumnsCount
        {
            get { return columnsCount; }
            set
            {
                if (columnsCount <= 0)
                {
                    return;
                }
                columnsCount = value;
                this.Invalidate();
            }
        }

        bool textShow = true;
        /// <summary>
        /// 是否显示计量名称
        /// </summary>
        [Description("是否显示计量名称")]
        [Category("Shuai")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        public bool TextShow
        {
            get { return textShow; }
            set
            {
                textShow = value;
                this.Invalidate();
            }
        }
        bool pointShow = true;
        /// <summary>
        /// 是否显示单个数据点
        /// </summary>
        [Description("是否显示单个数据点")]
        [Category("Shuai")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        public bool PointShow
        {
            get { return pointShow; }
            set
            {
                pointShow = value;
                this.Invalidate();
            }
        }
        bool pointValueShow = true;
        /// <summary>
        /// 是否显示单个数据点的值
        /// </summary>
        [Description("是否显示单个数据点的值")]
        [Category("Shuai")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        public bool PointValueShow
        {
            get { return pointValueShow; }
            set
            {
                pointValueShow = value;
                this.Invalidate();
            }
        }

        Bitmap backImage;
        /// <summary>
        /// 自动刷新委托
        /// </summary>
        public delegate void FlushHandle();
        public GridJiLiang()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
            InitializeComponent();
        }
        #region//接口方法
        public void ChangeFront(All.Class.Style.FrontColors color)
        {
            this.Invalidate();
        }
        public void ChangeBack(All.Class.Style.BackColors color)
        {
            this.BackColor = All.Class.Style.BackColor;
            this.ForeColor = All.Class.Style.FontColor;
            this.Invalidate();
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            All.Class.Style.AllStyle.Add(this);
            ChangeBack(All.Class.Style.Back);
            base.OnHandleCreated(e);
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            All.Class.Style.AllStyle.Remove(this);
            base.OnHandleDestroyed(e);
        }
        #endregion
        #region//重载方法
        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.Width < 100)
            {
                this.Width = 100;
            }
            if (this.Height < 50)
            {
                this.Height = 50;
            }
            Init();
            base.OnSizeChanged(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Init();
            Draw();
            e.Graphics.DrawImageUnscaled(backImage, 0, 0);
            base.OnPaint(e);
        }
        private void Init()
        {
            if (this.channels == null)
            {
                this.channels = new ChannelCollection(this.Invalidate);
            }
            if (backImage == null)
            {
                backImage = new Bitmap(Width, Height);
            }
        }
        private void Draw()
        {
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.Clear(this.BackColor);
                DrawBack(g);
                DrawText(g);
                DrawChannel(g);
            }
        }
        /// <summary>
        /// 绘画背景
        /// </summary>
        /// <param name="g"></param>
        private void DrawBack(Graphics g)
        {
            All.Class.GDIHelp.Init(g);
            using (Pen p = new Pen(All.Class.Style.BoardColor, 3))
            {
                p.EndCap = LineCap.ArrowAnchor;
                Rectangle tmp = DrawArea;
                g.DrawLine(p, tmp.Left, tmp.Top + tmp.Height, tmp.Left, tmp.Top);
                g.DrawLine(p, tmp.Left, tmp.Top + tmp.Height, tmp.Left + tmp.Width, tmp.Top + tmp.Height);
                p.EndCap = LineCap.NoAnchor;
                p.Width = 1;
                p.DashStyle = DashStyle.Dash;
                int space = tmp.Height / rowsCount;
                for (int i = 1; i <= rowsCount; i++)
                {
                    g.DrawLine(p, tmp.Left, tmp.Top + tmp.Height - space * i, tmp.Left + tmp.Width, tmp.Top + tmp.Height - space * i);
                }
                space = tmp.Width / columnsCount;
                for (int i = 1; i <= columnsCount; i++)
                {
                    g.DrawLine(p, tmp.Left + space * i, tmp.Top, tmp.Left + space * i, tmp.Top + tmp.Height);
                }
            }
        }
        /// <summary>
        /// 绘画标题
        /// </summary>
        /// <param name="g"></param>
        private void DrawText(Graphics g)
        {
            if (!textShow)
            {
                return;
            }
            All.Class.GDIHelp.Init(g);
            int lineWidth = 10;
            int index = -1;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            Pen p;
            Rectangle tmp = TextArea;
            for (int i = 0; i < this.channels.Count; i++)
            {
                if (this.channels[i].Show)
                {
                    index++;
                    p = new Pen(this.channels[i].Color, 2);
                    g.DrawLine(p, tmp.Left, tmp.Top + (index + 0.5f) * TextHeight, tmp.Left + lineWidth, tmp.Top + (index + 0.5f) * TextHeight);
                    g.DrawString(this.channels[i].Text, this.Font, All.Class.Style.FontBrush,
                        new Rectangle(tmp.Left + (int)(lineWidth * 1.5f), tmp.Top + index * TextHeight, 2000, TextHeight), sf);
                }
            }
        }
        /// <summary>
        /// 绘画计量
        /// </summary>
        /// <param name="g"></param>
        private void DrawChannel(Graphics g)
        {
            All.Class.GDIHelp.InitAntiAlias(g);
            float[] yMin = new float[this.channels.Count];
            float[] yMax = new float[this.channels.Count];

            float min = 0;
            float max = -9999;

            for (int i = 0; i < this.channels.Count; i++)
            {
                if (!this.channels[i].Show)
                {
                    continue;
                }
                yMax[i] = (float)(Max * this.channels[i].K + this.channels[i].B);
                if (yMax[i] > max)
                {
                    max = yMax[i];
                }
            }
            if (max == -9999)
            {
                return;
            }
            PointF p1 = Point.Empty;
            PointF p2 = Point.Empty;
            //List<PointF> tmp;
            PointF pPoint;
            RectangleF maxValue = new RectangleF(Min, min, Max - Min, max - min);
            for (int i = 0; i < this.channels.Count; i++)
            {
                if (!this.channels[i].Show)
                {
                    continue;
                }
                p1 = ValueToScreen(this.channels[i].StartPoint(maxValue), DrawArea, maxValue);
                p2 = ValueToScreen(this.channels[i].EndPoint(maxValue), DrawArea, maxValue);
                if (p1 == PointF.Empty || p2 == PointF.Empty)
                {
                    continue;
                }
                //画线
                g.DrawLine(new Pen(this.channels[i].Color, 2), p1, p2);
                if (pointShow)
                {
                    //画点
                    //tmp = this.channels[i].ScalePoint(maxValue);
                    for (int j = 0; j < this.channels[i].Count; j++)
                    {
                        pPoint = ValueToScreen(this.channels[i].Scale(maxValue, this.channels[i][j]), DrawArea, maxValue);
                        if (pPoint != PointF.Empty)
                        {
                            g.FillEllipse(new SolidBrush(this.channels[i].Color),
                                PointToPrint(pPoint));
                            if (pointValueShow)
                            {
                                g.DrawString(string.Format("({0},{1})",this.channels[i][j].X, this.channels[i][j].Y),
                                    this.Font, new SolidBrush(this.channels[i].Color),
                                    new PointF(pPoint.X + 2, pPoint.Y - All.Class.GDIHelp.GetFontHeight(this.Font) / 2f));
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 将指定的点放大为可显示的图形
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        private RectangleF PointToPrint(PointF old)
        {
            int scale = 3;
            return new RectangleF(old.X - scale, old.Y - scale, scale * 2, scale * 2);
        }
        /// <summary>
        /// 将从(xMin->xMax)值点转化为界面上显示的点
        /// </summary>
        /// <param name="old"></param>
        /// <param name="screen"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private PointF ValueToScreen(PointF old, RectangleF screen, RectangleF maxValue)
        {
            if (screen == RectangleF.Empty || maxValue == RectangleF.Empty
                || maxValue.Width == 0 || maxValue.Height == 0
                || screen.Width == 0 || screen.Height == 0)
            {
                return PointF.Empty;
            }
            double xScale = (double)screen.Width / (double)maxValue.Width;
            double yScale = (double)screen.Height / (double)maxValue.Height;
            PointF cur = new PointF((float)(old.X * xScale),(float)( old.Y * yScale));
            return new PointF(screen.X + cur.X, screen.Y + screen.Height - cur.Y);
        }
        /// <summary>
        /// 标题区域
        /// </summary>
        internal Rectangle TextArea
        {
            get
            {
                Rectangle result = Rectangle.Empty;
                if (textShow)
                {
                    result = new Rectangle(Width - Space - textSpace, Space, textSpace, Height - Space * 2);
                }
                return result;
            }
        }
        /// <summary>
        /// 图形区域
        /// </summary>
        internal Rectangle DrawArea
        {
            get 
            {
                Rectangle result = new Rectangle(Space, Space, Width - Space * 2, Height - Space * 2);
                if (textShow)
                {
                    result.Width = result.Width - textSpace - Space;
                }
                return result;
            }
        }
        #endregion
        /// <summary>
        /// 计量集合
        /// </summary>
        [ListBindable(false)]
        public class ChannelCollection : BaseCollection, System.Collections.IList
        {
            public ArrayList items = new ArrayList();
            FlushHandle flushHandle;
            /// <summary>
            /// 获取指定计量
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public Channel this[int index]
            {
                get
                {
                    return (Channel)this.items[index];
                }
            }

            public ChannelCollection(FlushHandle flushHandle)
            {
                this.flushHandle = flushHandle;
            }
            /// <summary>
            /// 获取直线中实际包含的元素数。
            /// </summary>
            public override int Count
            { get { return this.items.Count; } }
            /// <summary>
            /// 添加一条计量
            /// </summary>
            /// <param name="channel"></param>
            /// <returns></returns>
            public int Add(Channel channel)
            {
                channel.Invalidate = flushHandle;
                return this.items.Add(channel);
            }
            /// <summary>
            /// 批量添加计量
            /// </summary>
            /// <param name="channels"></param>
            public virtual void AddRange(params Channel[] channels)
            {
                foreach (Channel channel in channels)
                {
                    channel.Invalidate = flushHandle;
                    this.items.Add(channel);
                }
                this.flushHandle();
            }
            /// <summary>
            /// 清除所有计量
            /// </summary>
            public void Clear()
            {
                this.items.Clear();
            }
            /// <summary>
            /// 插入一条计量
            /// </summary>
            /// <param name="index"></param>
            /// <param name="channel"></param>
            public void Insert(int index, Channel channel)
            {
                this.items.Insert(index, channel); 
                this.flushHandle();
            }
            /// <summary>
            /// 移除指定计量
            /// </summary>
            /// <param name="channel"></param>
            public void Remove(Channel channel)
            {
                this.items.Remove(channel);
                this.flushHandle();
            }
            /// <summary>
            /// 移除指定位置计量
            /// </summary>
            /// <param name="index"></param>
            public void RemoveAt(int index)
            {
                this.items.RemoveAt(index);
                this.flushHandle();
            }
            /// <summary>
            /// 获取是否包含指定计量
            /// </summary>
            /// <param name="channel"></param>
            /// <returns></returns>
            public bool Contains(Channel channel)
            {
                return this.items.IndexOf(channel) != -1;
            }
            /// <summary>
            /// 从指定位置开始,将整个计量复制到指定表
            /// </summary>
            /// <param name="array"></param>
            /// <param name="index"></param>
            public void CopyTo(Channel[] array, int index)
            {
                this.items.CopyTo(array, index);
            }
            /// <summary>
            /// 获取指定计量的位置
            /// </summary>
            /// <param name="channel"></param>
            /// <returns></returns>
            public int IndexOf(Channel channel)
            {
                return this.items.IndexOf(channel);
            }
            #region//泛型数据
            bool IList.IsFixedSize
            {
                get { return false; }
            }
            bool IList.IsReadOnly
            {
                get { return false; }
            }
            object IList.this[int index]
            {
                get { return this[index]; }
                set { throw new NotSupportedException(); }
            }
            int IList.Add(object value)
            {
                return this.Add((Channel)value);
            }
            void IList.Clear()
            {
                this.Clear();
                this.flushHandle();
            }
            bool IList.Contains(object value)
            {
                return this.items.Contains(value);
            }
            int IList.IndexOf(object value)
            {
                return this.items.IndexOf(value);
            }
            void IList.Insert(int index, object value)
            {
                this.Insert(index, (Channel)value);
                this.flushHandle();
            }
            void IList.Remove(object value)
            {
                this.Remove((Channel)value);
                this.flushHandle();
            }
            void IList.RemoveAt(int index)
            {
                this.RemoveAt(index);
                this.flushHandle();
            }
            bool ICollection.IsSynchronized
            {
                get
                {
                    return false;
                }
            }
            object ICollection.SyncRoot
            {
                get
                {
                    return this;
                }
            }
            void ICollection.CopyTo(Array array, int index)
            {
                this.items.CopyTo(array, index);
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.items.GetEnumerator();
            }
            #endregion
        }
        /// <summary>
        /// 场景
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Description("单一计量曲线")]
        public class Channel
        {
            string text = "";
            /// <summary>
            /// 计量项目
            /// </summary>
            [Category("Shuai")]
            [Description("计量项目")]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public string Text
            {
                get { return text; }
                set { text = value; if (Invalidate != null)Invalidate(); }
            }

            bool show = true;
            /// <summary>
            /// 是否显示直线
            /// </summary>
            [Category("Shuai")]
            [Description("是否显示直线")]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public bool Show
            {
                get { return show; }
                set { show = value; if (Invalidate != null)Invalidate(); }
            }
            Color color = Color.Red;
            /// <summary>
            /// 直线颜色
            /// </summary>
            [Category("Shuai")]
            [Description("直线颜色")]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public Color Color
            {
                get { return color; }
                set { color = value; if (Invalidate != null)Invalidate(); }
            }
            double k = 1;
            /// <summary>
            /// 计量K值
            /// </summary>
            //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            //[EditorBrowsable(EditorBrowsableState.Never)]
            //[Browsable(false)]
            public double K
            {
                get { return k; }
                set { k = value; if (Invalidate != null)Invalidate(); }
            }
            double b = 1;
            /// <summary>
            /// 计量B值
            /// </summary>
            //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            //[EditorBrowsable(EditorBrowsableState.Never)]
            //[Browsable(false)]
            public double B
            {
                get { return b; }
                set { b = value; if (Invalidate != null)Invalidate(); }
            }
            FlushHandle invalidate = null;
            /// <summary>
            /// 刷新
            /// </summary>
            [Description("刷新")]
            internal FlushHandle Invalidate
            {
                get { return invalidate; }
                set { invalidate = value; }
            }
            List<PointF> point = new List<PointF>();
            /// <summary>
            /// 此处一定不能省
            /// </summary>
            public Channel()
            {
                
            }
            /// <summary>
            /// 当前计量点数量
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            [Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public int Count
            { get { return point.Count; } }
            /// <summary>
            /// 获取指定计量点
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public PointF this[int index]
            {
                get { return point[index]; }
            }
            /// <summary>
            /// 将指定的键和值添加到计量值中
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void Add(float x, float y)
            {
                int index = point.FindIndex(p => p.X == x);
                if (index >= 0)
                {
                    point.RemoveAt(index);
                }
                this.point.Add(new PointF(x, y));
                GetKB();
            }
            /// <summary>
            /// 根据X值返回对应的点
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public PointF GetPointFromX(float x)
            {
                return new PointF(x, (float)(x * k + b));
            }
            /// <summary>
            /// 获取正数区域的值,如果x或者y小于,会自动将取值移动到第一象限去
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            private PointF GetPointFromXPositive(float x)
            {
                if (k == 0)
                {
                    return new PointF(x, (float)b);
                }
                if (k < 0)
                {
                    return GetPointFromX(x);
                }
                float y = (float)(k * x + b);
                if (x >= 0 && y >= 0)
                {
                    return new PointF(x, y);
                }
                else if (b > 0)
                {
                    return new PointF(0, (float)b);
                }
                else
                {
                    return new PointF((float)(-b / k), 0);
                }
            }
            /// <summary>
            /// 清除计量值
            /// </summary>
            public void Clear()
            {
                this.point.Clear();
                GetKB();
            }
            private void GetKB()
            {
                double k, b;
                All.Class.Num.CalcLiner(point, out k, out b);
                this.k = k;
                this.b = b;
                if (Invalidate != null) Invalidate();
            }
            /// <summary>
            /// 获取当前计量区域对显示区域的压缩后的起始点
            /// </summary>
            /// <returns></returns>
            internal PointF StartPoint(RectangleF valueArea)
            {
                if (point == null || point.Count <= 0 || valueArea == RectangleF.Empty ||
                    valueArea.Width <= 0 || valueArea.Height <= 0)
                {
                    return Scale(valueArea, GetPointFromXPositive(Min));
                }
                float minX = 9999999999;
                point.ForEach(p =>
                {
                    if (p.X < minX)
                    {
                        minX = p.X;
                    }
                });
                return Scale(valueArea, GetPointFromXPositive(minX));
            }
            internal PointF EndPoint(RectangleF valueArea)
            {
                if (point == null || point.Count <= 0 || valueArea == RectangleF.Empty ||
                    valueArea.Width <= 0 || valueArea.Height <= 0)
                {
                    return Scale(valueArea, GetPointFromXPositive(Max));
                }
                float maxX = -9999999999;
                point.ForEach(p =>
                {
                    if (p.X > maxX)
                    {
                        maxX = p.X;
                    }
                });
                return Scale(valueArea, GetPointFromXPositive(maxX));
            }
            ///// <summary>
            ///// 将所有点转化为只有界面区域大小的点
            ///// </summary>
            ///// <param name="valueArea"></param>
            ///// <returns></returns>
            //internal List<PointF> ScalePoint(RectangleF valueArea)
            //{
            //    List<PointF> result = new List<PointF>();
            //    PointF tmp;
            //    point.ForEach(p =>
            //        {
            //            tmp = Scale(valueArea, p);
            //            if (tmp != PointF.Empty)
            //            {
            //                result.Add(tmp);
            //            }
            //        });
            //    return result;
            //}
            /// <summary>
            /// 将指定点缩放为只有界面区域值大小的点
            /// </summary>
            /// <param name="valueArea"></param>
            /// <param name="p"></param>
            /// <returns></returns>
            internal PointF Scale(RectangleF valueArea, PointF p)
            {
                List<PointF> allPoint = point;
                if (valueArea == RectangleF.Empty ||
                    valueArea.Width <= 0 || valueArea.Height <= 0)
                {
                    return PointF.Empty;
                }
                if (allPoint == null || allPoint.Count <= 0)
                {
                    allPoint = new List<PointF>();
                    allPoint.Add(new PointF(Min, (float)(k * Min + b)));
                    allPoint.Add(new PointF(Max, (float)(k * Max + b)));
                }
                float maxX = -9999999999;
                float maxY = -9999999999;
                allPoint.ForEach(pp =>
                {
                    if (pp.X > maxX)
                    {
                        maxX = pp.X;
                    }
                    if (pp.Y > maxY)
                    {
                        maxY = pp.Y;
                    }
                });
                if (maxX <= 0 || maxY <= 0)
                {
                    return PointF.Empty;
                }
                PointF tmp = new PointF(p.X / maxX, p.Y / maxY);//缩放到比例1
                tmp = new PointF(tmp.X * valueArea.Width, tmp.Y * valueArea.Height);//按显示比例缩放回来
                tmp = new PointF(tmp.X + valueArea.X, tmp.Y + valueArea.Y);//将图形偏移
                return tmp;
            }
        }
    }
}
