namespace All.Control.HMI
{
    partial class 调试
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
            this.fan1 = new All.Control.HMI.Fan();
            this.SuspendLayout();
            // 
            // fan1
            // 
            this.fan1.BackColor = System.Drawing.Color.Blue;
            this.fan1.BorderColor = System.Drawing.Color.Black;
            this.fan1.BorderThickness = 2;
            this.fan1.Fans = 4;
            this.fan1.Halo = false;
            this.fan1.IsRegion = true;
            this.fan1.Location = new System.Drawing.Point(49, 173);
            this.fan1.Name = "fan1";
            this.fan1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.fan1.Size = new System.Drawing.Size(70, 70);
            this.fan1.TabIndex = 0;
            // 
            // 调试
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.fan1);
            this.Name = "调试";
            this.Text = "调试";
            this.ResumeLayout(false);

        }

        #endregion

        private Fan fan1;
    }
}