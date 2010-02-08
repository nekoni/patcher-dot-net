using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Patcher.Forms
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void AboutDialog_Load(object sender, EventArgs e)
        {
            Version Product = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            lblVersion.Text = "Patcher Version " + Product.Major + "." + Product.Minor + "." + Product.Revision + " Build " + Product.Build;
        }
    }
}