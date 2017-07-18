namespace HKLabs
{
    partial class Form6
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
            this.light1 = new All.Control.Light();
            this.square4 = new All.Control.HMI.Square();
            this.square3 = new All.Control.HMI.Square();
            this.square2 = new All.Control.HMI.Square();
            this.square1 = new All.Control.HMI.Square();
            this.SuspendLayout();
            // 
            // light1
            // 
            this.light1.BackColor = System.Drawing.SystemColors.Control;
            this.light1.LedColor = System.Drawing.Color.Red;
            this.light1.Location = new System.Drawing.Point(213, 247);
            this.light1.Name = "light1";
            this.light1.Size = new System.Drawing.Size(50, 50);
            this.light1.TabIndex = 2;
            this.light1.Text = "light1";
            // 
            // square4
            // 
            this.square4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.square4.BorderColor = System.Drawing.Color.Empty;
            this.square4.BorderThickness = 0;
            this.square4.Halo = true;
            this.square4.Location = new System.Drawing.Point(289, 49);
            this.square4.Name = "square4";
            this.square4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.square4.Radius = 0;
            this.square4.Size = new System.Drawing.Size(100, 24);
            this.square4.TabIndex = 4;
            // 
            // square3
            // 
            this.square3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.square3.BorderColor = System.Drawing.Color.Empty;
            this.square3.BorderThickness = 0;
            this.square3.Halo = true;
            this.square3.Location = new System.Drawing.Point(221, 141);
            this.square3.Name = "square3";
            this.square3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.square3.Radius = 30;
            this.square3.Size = new System.Drawing.Size(101, 36);
            this.square3.TabIndex = 3;
            // 
            // square2
            // 
            this.square2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.square2.BorderColor = System.Drawing.Color.Empty;
            this.square2.BorderThickness = 0;
            this.square2.Halo = false;
            this.square2.Location = new System.Drawing.Point(448, 247);
            this.square2.Name = "square2";
            this.square2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.square2.Radius = 20;
            this.square2.Size = new System.Drawing.Size(41, 41);
            this.square2.TabIndex = 1;
            // 
            // square1
            // 
            this.square1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.square1.BorderColor = System.Drawing.Color.Empty;
            this.square1.BorderThickness = 0;
            this.square1.Halo = false;
            this.square1.Location = new System.Drawing.Point(213, 134);
            this.square1.Name = "square1";
            this.square1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.square1.Radius = 40;
            this.square1.Size = new System.Drawing.Size(118, 50);
            this.square1.TabIndex = 0;
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 533);
            this.Controls.Add(this.square4);
            this.Controls.Add(this.square3);
            this.Controls.Add(this.light1);
            this.Controls.Add(this.square2);
            this.Controls.Add(this.square1);
            this.Name = "Form6";
            this.Text = "Form6";
            this.ResumeLayout(false);

        }

        #endregion

        private All.Control.HMI.Square square1;
        private All.Control.HMI.Square square2;
        private All.Control.Light light1;
        private All.Control.HMI.Square square3;
        private All.Control.HMI.Square square4;


    }
}