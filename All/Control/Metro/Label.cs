﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace All.Control.Metro
{
    public partial class Label : System.Windows.Forms.Label,All.Class.Style.ChangeTheme
    {
        public Label()
        {
            InitializeComponent();
        }
        public void ChangeFront(All.Class.Style.FrontColors color)
        {
            //this.Invalidate();
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
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);
        }
    }
}
