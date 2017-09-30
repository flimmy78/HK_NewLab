namespace All.Control.Mine
{
    partial class TextBox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt
            // 
            this.txt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt.Location = new System.Drawing.Point(2, 4);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(393, 14);
            this.txt.TabIndex = 1;
            this.txt.Click += new System.EventHandler(this.txt_Click);
            this.txt.TextChanged += new System.EventHandler(this.txt_TextChanged);
            this.txt.Enter += new System.EventHandler(this.txt_Enter);
            this.txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.txt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            this.txt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_KeyUp);
            this.txt.Leave += new System.EventHandler(this.txt_Leave);
            this.txt.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txt_PreviewKeyDown);
            // 
            // TextBox
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txt);
            this.Size = new System.Drawing.Size(399, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt;




    }
}
