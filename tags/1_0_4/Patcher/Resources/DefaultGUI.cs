using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

namespace DefaultGUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            bool hidewindow = false;
            if (args.Length > 0)
            {
                if (args[0] == "-h")
                    hidewindow = true;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mf = new MainForm(hidewindow);
            Application.Run(mf);
            return mf.ExitCode;
        }
    }

    public class MainForm : Form
    {
        #region Fields

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private bool bFormBusy = false;
        private bool bHideWindow = false;
        private int iExitCode = -1;
        private BackgroundWorker bwWorkerThread = null;

        private string STR_TEXT = "";
        private string STR_TITLE = "";
        private int INT_STEPSNUMBER = 0;
        private Bitmap LOGO_FILE = null;
        private Icon ICON_FILE = null;
        
        #endregion

        #region Constructor

        public MainForm(bool HideWindow) 
        {
            InizializeComponents();

            bHideWindow = HideWindow;
        }

        #endregion

        #region Fields

        public int ExitCode
        {
            get { return iExitCode; }
        }

        #endregion

        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InizializeComponents()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 93);
            this.label1.TabIndex = 0;
            this.label1.Text = STR_TITLE;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(293, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(90, 90);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.Image = LOGO_FILE;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 108);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(371, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 2;
            this.progressBar1.Step = 1;
            this.progressBar1.Maximum = INT_STEPSNUMBER;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 138);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(371, 179);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 329);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ICON_FILE;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
            this.Load += new EventHandler(MainForm_Load);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "Form1";
            this.Text = STR_TEXT;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
        }

        private void SelectLastText(string s)
        {
            string rtbText = (string)CommonInvokeCall.GetControlProperty(richTextBox1, "Text");
            int sel = Math.Max(0, rtbText.Length - s.Length);
            CommonInvokeCall.SetControlProperty(richTextBox1, "SelectionStart", sel);
            CommonInvokeCall.SetControlProperty(richTextBox1, "SelectionLength", s.Length);
            CommonInvokeCall.CallGenericMethod(richTextBox1, "ScrollToCaret", null);
        }

        private void LogMessage(Color col, string msg)
        {
            Color bkcol = (Color)CommonInvokeCall.GetControlProperty(richTextBox1, "BackColor");
            CommonInvokeCall.CallGenericMethod(richTextBox1, "AppendText", new object[] { msg });
            SelectLastText(msg);
            CommonInvokeCall.SetControlProperty(richTextBox1, "SelectionColor", col);
            CommonInvokeCall.SetControlProperty(richTextBox1, "SelectionBackColor", bkcol);
            CommonInvokeCall.SetControlProperty(richTextBox1, "SelectionFont", Control.DefaultFont);
            CommonInvokeCall.CallGenericMethod(richTextBox1, "AppendText", new object[] { Environment.NewLine });
        }

        private void ReportProgress()
        {
            bwWorkerThread.ReportProgress(0);
        }

        #endregion

        #region Local Events

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bFormBusy)
            {
                e.Cancel = true;
                MessageBox.Show("Application busy. Wait the end of the process before close", STR_TEXT, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            bFormBusy = true;
            bwWorkerThread = new BackgroundWorker();
            bwWorkerThread.WorkerReportsProgress = true;
            bwWorkerThread.WorkerSupportsCancellation = false;
            bwWorkerThread.ProgressChanged += new ProgressChangedEventHandler(bwWorkerThread_ProgressChanged);
            bwWorkerThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwWorkerThread_RunWorkerCompleted);
            bwWorkerThread.DoWork += new DoWorkEventHandler(bwWorkerThread_DoWork);
            bwWorkerThread.RunWorkerAsync();

            if (bHideWindow)
            {
                this.Hide();
                ShowInTaskbar = false;
            }
        }

        void bwWorkerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ENTRYPOINT
                iExitCode = 0;
            }
            catch (Exception ex)
            {
                LogMessage(Color.Red, "Error: " + ex.Message);
                CommonInvokeCall.SetControlProperty(progressBar1, "Value", 0);
                iExitCode = -1;
            }
        }

        void bwWorkerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bFormBusy = false;
            if (bHideWindow)
                CommonInvokeCall.CallGenericMethod(this, "Close", null);
        }

        void bwWorkerThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CommonInvokeCall.CallGenericMethod(progressBar1, "PerformStep", null);
        }

        internal class CommonInvokeCall
        {
            private delegate void SetCtrlPropertyDelegate(object obj, object val, object[] index);
            private delegate object GetCtrlPropertyDelegate(object obj, object[] index);
            private delegate object MethodInvokeDelegate(object obj, object[] parms);

            public static object CallGenericMethod(Control cntr, string mName, object[] parms)
            {
                MethodInfo m = cntr.GetType().GetMethod(mName);
                if (cntr.InvokeRequired)
                {
                    Delegate dgMethodCall = new MethodInvokeDelegate(m.Invoke);
                    return cntr.Invoke(dgMethodCall, new object[] { cntr, parms });
                }
                return m.Invoke(cntr, parms);
            }

            public static void SetControlProperty(Control cntr, string propName, object value)
            {
                PropertyInfo p = cntr.GetType().GetProperty(propName);
                if (cntr.InvokeRequired)
                {
                    Delegate dgSetProp = new SetCtrlPropertyDelegate(p.SetValue);
                    cntr.Invoke(dgSetProp, new object[] { cntr, value, null });
                    return;
                }
                p.SetValue(cntr, value, null);
            }

            public static object GetControlProperty(Control cntr, string propName)
            {
                PropertyInfo p = cntr.GetType().GetProperty(propName);
                if (cntr.InvokeRequired)
                {
                    Delegate dgGetProp = new GetCtrlPropertyDelegate(p.GetValue);
                    return cntr.Invoke(dgGetProp, new object[] { cntr, null });
                }
                return p.GetValue(cntr, null);
            }
        }

        #endregion
    }
}