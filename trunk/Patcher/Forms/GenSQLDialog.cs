using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Patcher.Forms
{
    public partial class GenSQLDialog : Form
    {
        byte[] mFile1 = null;
        byte[] mFile2 = null;

        public GenSQLDialog()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream myStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    mFile1 = new byte[myStream.Length];
                    myStream.Read(mFile1, 0, (int)myStream.Length);
                }
            }
        }

        private void tbBuild_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string bin_file = string.Empty;
                if (chkBoxEncode64.Checked)
                    bin_file = Convert.ToBase64String(mFile1);
                else
                    bin_file = Encoding.UTF8.GetString(mFile1);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("use " + tbDbName.Text);
                sb.AppendLine("GO");
                sb.AppendLine("UPDATE " + tbTable.Text + " SET " + tbField.Text + " = '" + bin_file + "' WHERE " + tbCondition.Text);
                sb.AppendLine("GO");

                byte[] bff = Encoding.UTF8.GetBytes(sb.ToString());
                using (FileStream fs = System.IO.File.Create(sfd.FileName))
                {
                    fs.Write(bff, 0, bff.Length);
                    fs.Close();
                }
            }
        }


        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream myStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    mFile2 = new byte[myStream.Length];
                    myStream.Read(mFile2, 0, (int)myStream.Length);
                }
            }
        }

        private void btnBuild1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string bin_file = string.Empty;
                if (chBoxEncode1.Checked)
                    bin_file = Convert.ToBase64String(mFile1);
                else
                    bin_file = Encoding.UTF8.GetString(mFile1);

                string bin_file2 = string.Empty;
                if (chBoxEncode2.Checked)
                    bin_file2 = Convert.ToBase64String(mFile2);
                else
                    bin_file2 = Encoding.UTF8.GetString(mFile2);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("use " + tbDbName1.Text);
                sb.AppendLine("GO");
                sb.AppendLine("INSERT INTO " + tbInsertItems.Text + " VALUES ( " + tbInsertValues.Text + "'" + bin_file + "','" + bin_file2 + "')");
                sb.AppendLine("GO");

                byte[] bff = Encoding.UTF8.GetBytes(sb.ToString());
                using (FileStream fs = System.IO.File.Create(sfd.FileName))
                {
                    fs.Write(bff, 0, bff.Length);
                    fs.Close();
                }
            }
        }

    }
}