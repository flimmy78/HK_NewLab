namespace All.Window
{
    partial class DelUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DelUser));
            this.btnCancel = new All.Control.Metro.Button();
            this.btnOk = new All.Control.Metro.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new All.Control.Metro.Label();
            this.cbbName = new All.Control.Metro.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222)))));
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.Boarder = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(32)))), ((int)(((byte)(33)))));
            this.btnCancel.Location = new System.Drawing.Point(266, 140);
            this.btnCancel.MinimumSize = new System.Drawing.Size(10, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.btnCancel.Size = new System.Drawing.Size(83, 26);
            this.btnCancel.TabIndex = 37;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222)))));
            this.btnOk.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOk.BackgroundImage")));
            this.btnOk.Boarder = true;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(32)))), ((int)(((byte)(33)))));
            this.btnOk.Location = new System.Drawing.Point(167, 139);
            this.btnOk.MinimumSize = new System.Drawing.Size(10, 10);
            this.btnOk.Name = "btnOk";
            this.btnOk.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.btnOk.Size = new System.Drawing.Size(83, 26);
            this.btnOk.TabIndex = 36;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.ForeColor = System.Drawing.Color.Black;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(17, 47);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(114, 138);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 41;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222)))));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(32)))), ((int)(((byte)(33)))));
            this.label1.Location = new System.Drawing.Point(142, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 39;
            this.label1.Text = "用户名:";
            // 
            // cbbName
            // 
            this.cbbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbName.BackColor = System.Drawing.SystemColors.Control;
            this.cbbName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbName.ForeColor = System.Drawing.Color.Black;
            this.cbbName.Location = new System.Drawing.Point(195, 75);
            this.cbbName.Name = "cbbName";
            this.cbbName.Size = new System.Drawing.Size(172, 20);
            this.cbbName.TabIndex = 38;
            // 
            // DelUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 204);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbbName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DelUser";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "删除指定用户";
            this.Load += new System.EventHandler(this.DelUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Control.Metro.Button btnCancel;
        private Control.Metro.Button btnOk;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Control.Metro.Label label1;
        private Control.Metro.ComboBox cbbName;
    }
}