using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace All.Control.Mine
{
    public partial class TextBox : System.Windows.Forms.Control
    {
        //
        // 摘要: 
        //     获取控件中文本的长度。
        //
        // 返回结果: 
        //     控件的文本中包含的字符数。
        [Browsable(false)]
        public virtual int TextLength { get { return txt.TextLength; } }
        /// <summary>
        /// 获取或设置一个值，该值指示控件中当前选定的文本。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string SelectedText { 
            get { return txt.SelectedText; }
            set { txt.SelectedText = value; }
        }
        /// <summary>
        /// 获取或设置文本框中选定的字符数。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int SelectionLength {
            get { return txt.SelectionLength; }
            set { txt.SelectionLength = value; }
        }
        /// <summary>
        /// 获取或设置文本框中选定的文本起始点
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart {
            get { return txt.SelectionStart; }
            set { txt.SelectionStart = value; }
        }
        /// <summary>
        /// 在控件中显示的文本
        /// </summary>
        [Category("外观")]
        [Description("获取或设置 System.Windows.Forms.TextBox 中的当前文本。")]
        public override string Text
        {
            get { return txt.Text; }
            set { txt.Text = value; }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示文本框中的文本是否为只读。
        /// </summary>
        [Category("行为")]
        [Description("如果文本框是只读的，则为 true，否则为 false。默认值为 false。")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public bool ReadOnly
        {
            get { return txt.ReadOnly; }
            set { txt.ReadOnly = value; }
        }
        /// <summary>
        /// 用于显示控件中文本字体
        /// </summary>
        [Category("外观")]
        [Description("用于显示控件中文本字体")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                OnSizeChanged(null);
            }
        }
        /// <summary>
        /// 如果用户可以使用 Tab 键在多行文本框控件中输入 Tab 字符，则为 true；如果按 Tab 键移动焦点，则为 false。默认值为 false。
        /// </summary>
        [Category("行为")]
        [Description("获取或设置一个值，该值指示在多行文本框控件中按 Tab 键时，是否在控件中键入一个 Tab 字符，而不是按选项卡的顺序将焦点移动到下一个控件。")]
        [DefaultValue(false)]
        public bool AcceptsTab
        {
            get { return txt.AcceptsTab; }
            set { txt.AcceptsTab = value; }
        }
        /// <summary>
        /// 如果按 Enter 键时在多行版本的控件中创建一行新文本，则为 true；如果按 Enter 键时激活窗体的默认按钮，则为 false。默认为 false。
        /// </summary>
        [Category("行为")]
        [Description("获取或设置一个值，该值指示在多行 System.Windows.Forms.TextBox 控件中按 Enter 键时，是在控件中创建一行新文本还是激活窗体的默认按钮。")]
        [DefaultValue(false)]
        public bool AcceptsReturn
        {
            get { return txt.AcceptsReturn; }
            set { txt.AcceptsReturn = value; }
        }
        /// <summary>
        /// 获取或设置字符，该字符用于屏蔽单行 System.Windows.Forms.TextBox 控件中的密码字符。
        /// </summary>
        [Category("行为")]
        [Description("用于屏蔽在单行 System.Windows.Forms.TextBox 控件中输入的字符的字符。如果不想让控件在字符键入时将它们屏蔽，请将此属性值设置为0（字符值）。默认值等于 0（字符值）。")]
        [DefaultValue('\0')]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public char PasswordChar 
        {
            get { return txt.PasswordChar; }
            set { txt.PasswordChar = value; }
        }

        /// <summary>
        /// 获取或设置 System.Windows.Forms.TextBox 控件中文本的对齐方式。
        /// </summary>
        [Category("行为")]
        [Description("System.Windows.Forms.HorizontalAlignment 枚举值之一，指定控件中文本的对齐方式。默认为 HorizontalAlignment.Left。")]
        [Localizable(true)]
        public HorizontalAlignment TextAlign
        {
            get { return txt.TextAlign; }
            set { txt.TextAlign = value; }
        }
        /// <summary>
        /// 如果该控件是多行 System.Windows.Forms.TextBox 控件，则为 true；否则为 false。默认为 false。
        /// </summary>
        [Category("行为")]
        [Description("获取或设置一个值，该值指示此控件是否为多行 System.Windows.Forms.TextBox 控件")]
        public bool Multiline
        {
            get { return txt.Multiline; }
            set { txt.Multiline = value; }
        }
        public TextBox()
        {
            InitializeComponent();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            ChangeSize();
            base.OnSizeChanged(e);
        }
        private void ChangeSize()
        {
            if (!Multiline)
            {
                this.Height = txt.Height + 7;
            }
            else
            {
                txt.Height = this.Height - 7;
            }
            txt.Width = this.Width - 4;
            txt.Top = (this.Height - txt.Height) / 2;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(All.Class.Style.BoardColor, 1), 0, 0, this.Width - 1, this.Height - 1);
            base.OnPaint(e);
        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
        }
        private void txt_Enter(object sender, EventArgs e)
        {
            OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
        }
        private void txt_Leave(object sender, EventArgs e)
        {
            OnLeave(e);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
        }
        private void txt_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }
        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }
        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            OnKeyUp(e);
        }
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }
        private void txt_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            OnPreviewKeyDown(e);
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }
        private void txt_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            if (this.Enabled)
            {
                this.BackColor = All.Class.Style.BackColor;
                this.txt.Enabled = true;
                this.txt.Visible = true;
            }
            else
            {
                this.BackColor = All.Class.Style.UEnableColor;
                this.txt.Enabled = false;
                this.txt.Visible = false;
            }
            base.OnEnabledChanged(e);
        }
        /// <summary>
        /// 将选定文本设置为指定文本，但不清除撤消缓冲区。
        /// </summary>
        /// <param name="value">要替换的文本。</param>
        public void Paste(string value)
        {
            txt.Paste(value);
        }
        /// <summary>
        /// 用“剪贴板”的内容替换文本框中的当前选定内容。
        /// </summary>
        public void Paste()
        {
            txt.Paste();
        }
        /// <summary>
        /// 选择文本框中的文本范围。
        /// </summary>
        /// <param name="start">文本框中当前选定文本的第一个字符的位置。</param>
        /// <param name="length">要选择的字符数。</param>
        public void Select(int start, int length)
        {
            txt.Select(start, length);
        }
        /// <summary>
        /// 选定文本框中的所有文本。
        /// </summary>
        public void SelectAll()
        {
            txt.SelectAll();
        }
        /// <summary>
        /// 将控件的内容滚动到当前插入符号位置。
        /// </summary>
        public void ScrollToCaret()
        {
            txt.ScrollToCaret();
        }
        /// <summary>
        /// 向文本框的当前文本追加文本
        /// </summary>
        /// <param name="text">要向文本框的当前内容追加的文本</param>
        public void AppendText(string text)
        {
            txt.AppendText(text);
        }
        /// <summary>
        /// 从文本框控件中清除所有文本
        /// </summary>
        public void Clear()
        {
            txt.Clear();
        }
        /// <summary>
        /// 从该文本框的撤消缓冲区中清除关于最近操作的信息。
        /// </summary>
        public void ClearUndo()
        {
            txt.ClearUndo();
        }
        /// <summary>
        /// 将文本框中的当前选定内容复制到“剪贴板”。
        /// </summary>
        public void Copy()
        {
            txt.Copy();
        }
        /// <summary>
        /// 为控件设置输入焦点。
        /// </summary>
        /// <returns>如果输入焦点请求成功，则为 true；否则为 false。</returns>
        public new bool Focus()
        {
            return txt.Focus();
        }
    }
}
