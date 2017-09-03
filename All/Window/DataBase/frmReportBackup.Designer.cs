namespace All.Window
{
    partial class frmReportBackup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReportBackup));
            this.groupBox1 = new All.Control.Metro.GroupBox(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new All.Control.Metro.Button();
            this.label1 = new All.Control.Metro.Label();
            this.btnSave = new All.Control.Metro.Button();
            this.btnClose = new All.Control.Metro.Button();
            this.btnClearLog = new All.Control.Metro.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox1.Location = new System.Drawing.Point(15, 47);
            this.groupBox1.MinimumSize = new System.Drawing.Size(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(563, 87);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox1.Location = new System.Drawing.Point(20, 41);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(461, 26);
            this.textBox1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.Boarder = true;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button2.Location = new System.Drawing.Point(480, 40);
            this.button2.MinimumSize = new System.Drawing.Size(10, 10);
            this.button2.Name = "button2";
            this.button2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.button2.Size = new System.Drawing.Size(68, 28);
            this.button2.TabIndex = 102;
            this.button2.Text = "打开";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(21, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "备份文件名称:";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.Boarder = true;
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnSave.Location = new System.Drawing.Point(408, 149);
            this.btnSave.MinimumSize = new System.Drawing.Size(10, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.btnSave.Size = new System.Drawing.Size(76, 27);
            this.btnSave.TabIndex = 102;
            this.btnSave.Text = "保存";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.Boarder = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnClose.Location = new System.Drawing.Point(488, 149);
            this.btnClose.MinimumSize = new System.Drawing.Size(10, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.btnClose.Size = new System.Drawing.Size(76, 27);
            this.btnClose.TabIndex = 101;
            this.btnClose.Text = "关闭";
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnClearLog.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClearLog.BackgroundImage")));
            this.btnClearLog.Boarder = true;
            this.btnClearLog.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClearLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnClearLog.Location = new System.Drawing.Point(317, 149);
            this.btnClearLog.MinimumSize = new System.Drawing.Size(10, 10);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.btnClearLog.Size = new System.Drawing.Size(87, 27);
            this.btnClearLog.TabIndex = 103;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // frmReportBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 192);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1440, 860);
            this.Name = "frmReportBackup";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据备份";
            this.Load += new System.EventHandler(this.frmReportBackup_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private All.Control.Metro.GroupBox groupBox1;
        private All.Control.Metro.Button btnSave;
        private All.Control.Metro.Button btnClose;
        private All.Control.Metro.Button btnClearLog;
        private System.Windows.Forms.TextBox textBox1;
        private All.Control.Metro.Label label1;
        private All.Control.Metro.Button button2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}