using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Patcher.Forms
{
    public partial class OutputWindow : DockContent
    {
        #region Constructor

        public OutputWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        public void WriteLine(string Text)
        {
            this.richTextBoxOutput.AppendText(Text + Environment.NewLine);
        }

        public void Clear()
        {
            this.richTextBoxOutput.Clear();
        }

        #endregion
    }
}