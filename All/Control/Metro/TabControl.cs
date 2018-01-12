using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace All.Control.Metro
{
    public partial class TabControl : System.Windows.Forms.TabControl,All.Class.Style.ChangeTheme
    {

        bool normal = false;
        /// <summary>
        /// 是否常规控件
        /// </summary>
        [Category("Shuai")]
        [Description("是否常规控件")]
        public bool Normal
        {
            get { return normal; }
            set
            {
                normal = value;
                this.Invalidate();
            }
        }
        StringFormat sf;
        Font tabOneFont = null;
        Font tabOtherFont = null;
        Font tabSelectFont = null;
        Bitmap backImage = null;
        protected override void OnFontChanged(EventArgs e)
        {
            Init();
            base.OnFontChanged(e);
        }
        public TabControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint, true);
            //this.SizeMode = TabSizeMode.Normal;
            //this.ItemSize = new Size(58, 22);
            this.MinimumSize = new Size(10, 10);
        }
        public TabControl(IContainer container)
            :this()
        {
            container.Add(this);
            if (!normal && this.SelectedIndex == 0 && this.TabCount > 1)
            {
                this.SelectedIndex = 1;
            }
        }
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
        protected override void OnSizeChanged(EventArgs e)
        {
            Init();
            base.OnSizeChanged(e);
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            All.Class.Style.AllStyle.Remove(this);
            base.OnHandleDestroyed(e);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (e.Control is TabPage)
            {
                e.Control.PaddingChanged += Control_PaddingChanged;
            }
            base.OnControlAdded(e);
        }
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (e.Control is TabPage)
            {
                e.Control.PaddingChanged -= Control_PaddingChanged;
            }
            base.OnControlRemoved(e);
        }

        private void Control_PaddingChanged(object sender, EventArgs e)
        {
            TabPage t = sender as TabPage;
            if (t != null)
            {
                if (t.Padding.All != 0)
                {
                    t.Padding = new Padding(0);
                }
            }
        }
        ContextMenuStrip tmpContextMenuStrip = null;
        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return tmpContextMenuStrip;
            }
            set
            {
                tmpContextMenuStrip = value;
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!normal && this.GetTabRect(0).Contains(e.Location) && tmpContextMenuStrip != null && !All.Class.Environment.IsDesignMode)
            {
                tmpContextMenuStrip.Show(this.PointToScreen(e.Location));
                return;
            }
            base.OnMouseDown(e);
        }
        
        protected override void OnSelecting(TabControlCancelEventArgs e)
        {
            if (!normal && e.TabPageIndex == 0)
            {
                e.Cancel = true;
            }
            base.OnSelecting(e);
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.Invalidate();
            base.OnMouseMove(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Color tmpfontColor;
            Font tmpFont;
            Rectangle rect;
            if (this.TabCount <= 0)
            {
                return;
            }
            if (this.Width <= 5 || this.Height <= 5)
            {
                return;
            }
            //if (this.SelectedIndex == 0 && this.TabCount > 1)
            //{
            //    this.SelectedIndex = 1;
            //}
            if (sf == null || tabOneFont==null || tabOtherFont==null || backImage==null)
            {
                Init();
            }
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.Clear(All.Class.Style.BackColor);
                for (int i = 0; i < this.TabCount; i++)
                {
                    rect = new Rectangle(this.GetTabRect(i).X + 1, this.GetTabRect(i).Y, this.GetTabRect(i).Width - 2, this.GetTabRect(i).Height);
                    tmpfontColor = (this.SelectedIndex == i ? (
                        All.Class.Style.Back == All.Class.Style.BackColors.Black ?
                        All.Class.Style.TitleColor : All.Class.Style.BoardColor) : All.Class.Style.FontColor);
                    tmpFont = (this.SelectedIndex == i ? tabSelectFont : tabOtherFont);
                    switch (i)
                    {
                        case 0:
                            if (normal)
                            {
                                g.FillRectangle(new SolidBrush(All.Class.Style.BackColor), rect);
                                if (this.SelectedIndex == i)
                                {
                                    g.DrawLine(All.Class.Style.BoardPen, rect.X, rect.Y, rect.X + rect.Width - 1, rect.Y);
                                    g.DrawLine(All.Class.Style.BoardPen, rect.X, rect.Y, rect.X, rect.Y + rect.Height);
                                    g.DrawLine(All.Class.Style.BoardPen, rect.X + rect.Width - 1, rect.Y, rect.X + rect.Width - 1, rect.Y + rect.Height);
                                }
                                g.DrawString(this.TabPages[i].Text, tmpFont, new SolidBrush(tmpfontColor), rect, sf);
                            }
                            else
                            {
                                g.FillRectangle(new SolidBrush(All.Class.Style.BoardColor), rect);
                                g.DrawString(this.TabPages[i].Text, tabOneFont, new SolidBrush(All.Class.Style.BackColor), rect, sf);
                            }
                            break;
                        default:
                            g.FillRectangle(new SolidBrush(All.Class.Style.BackColor), rect);
                            if (this.SelectedIndex == i)
                            {
                                g.DrawLine(All.Class.Style.BoardPen, rect.X, rect.Y, rect.X + rect.Width - 1, rect.Y);
                                g.DrawLine(All.Class.Style.BoardPen, rect.X, rect.Y, rect.X, rect.Y + rect.Height);
                                g.DrawLine(All.Class.Style.BoardPen, rect.X + rect.Width - 1, rect.Y, rect.X + rect.Width - 1, rect.Y + rect.Height);
                            }
                            g.DrawString(this.TabPages[i].Text, tmpFont, new SolidBrush(tmpfontColor), rect, sf);
                            break;
                    }
                    if (((!normal && i > 0) || normal) && i != this.SelectedIndex && rect.Contains(this.PointToClient(MousePosition)))
                    {
                        g.FillRectangle(new SolidBrush(All.Class.Style.BoardColor), new Rectangle(rect.X + (int)(rect.Width * 0.1f), rect.Y + rect.Height - 3, (int)(rect.Width * 0.8f), 2));
                    }
                }
                //画标签的框
                Rectangle rect0 = new Rectangle(this.GetTabRect(0).X, this.GetTabRect(0).Y, this.GetTabRect(0).Width - 1, this.GetTabRect(0).Height);
                if (this.SelectedIndex >= 0)
                {
                    rect = new Rectangle(this.GetTabRect(this.SelectedIndex).X + 1, this.GetTabRect(this.SelectedIndex).Y, this.GetTabRect(this.SelectedIndex).Width - 2, this.GetTabRect(this.SelectedIndex).Height);
                    //if (normal)
                    //{
                    //    if (this.TabCount > 0)
                    //    {
                    g.DrawLine(All.Class.Style.BoardPen, rect0.X + 1, rect0.Y + rect.Height, rect.X, rect.Y + rect.Height);
                    g.DrawLine(All.Class.Style.BoardPen, rect.X + rect.Width, rect.Height + rect.Y, this.Width - rect0.X * 2, rect.Height + rect.Y);
                    //    }
                    //}
                    //else
                    //{
                    //    if (this.TabCount > 1)
                    //    {
                    //g.DrawLine(All.Class.Style.BoardPen, rect0.X + rect0.Width, rect0.Y + rect0.Height, rect.X, rect.Y + rect.Height);
                    //g.DrawLine(All.Class.Style.BoardPen, rect.X + rect.Width, rect.Height + rect.Y, this.Width - rect0.X * 2, rect.Height + rect.Y);
                    //    }
                    //}
                    g.DrawLine(All.Class.Style.BoardPen, rect0.X + 1, rect0.Y + rect.Height,
                        rect0.X + 1, this.Height - rect0.Y * 2);
                    g.DrawLine(All.Class.Style.BoardPen, this.Width - rect0.X * 2, rect0.Y + rect0.Height,
                        this.Width - rect0.X * 2, this.Height - rect.Y * 2);
                    g.DrawLine(All.Class.Style.BoardPen, rect0.X + 1, this.Height - rect0.Y - 1, this.Width - rect0.X * 2, this.Height - rect.Y - 1);
                }
            }
            e.Graphics.DrawImageUnscaled(backImage, 0, 0);
            base.OnPaint(e);
        }
        private void Init()
        {
            if (!normal && this.SelectedIndex == 0 && this.TabCount > 0)
            {
                this.SelectedIndex = 1;
            }
            if (this.Width > 0 && this.Height > 0)
            {
                backImage = new Bitmap(this.Width, this.Height);
            }
            sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            tabSelectFont = new System.Drawing.Font(this.Font.FontFamily, this.Font.Size, FontStyle.Regular);
            tabOneFont = new Font(this.Font.FontFamily, this.Font.Size + 1, FontStyle.Bold);
            tabOtherFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Regular);
        }
    }
}
