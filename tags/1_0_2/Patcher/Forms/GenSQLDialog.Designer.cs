namespace Patcher.Forms
{
    partial class GenSQLDialog
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
            this.tabOptions = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkBoxEncode64 = new System.Windows.Forms.CheckBox();
            this.btnBuild = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbCondition = new System.Windows.Forms.TextBox();
            this.tbField = new System.Windows.Forms.TextBox();
            this.lblEqual = new System.Windows.Forms.Label();
            this.lblWhere = new System.Windows.Forms.Label();
            this.lblSet = new System.Windows.Forms.Label();
            this.tbDbName = new System.Windows.Forms.TextBox();
            this.tbTable = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbDbName1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chBoxEncode2 = new System.Windows.Forms.CheckBox();
            this.chBoxEncode1 = new System.Windows.Forms.CheckBox();
            this.btnBuild1 = new System.Windows.Forms.Button();
            this.btnBrowse1 = new System.Windows.Forms.Button();
            this.btnBrowse2 = new System.Windows.Forms.Button();
            this.tbInsertValues = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbInsertItems = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabOptions.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.tabPage1);
            this.tabOptions.Controls.Add(this.tabPage2);
            this.tabOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabOptions.Location = new System.Drawing.Point(0, 0);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new System.Drawing.Size(278, 276);
            this.tabOptions.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkBoxEncode64);
            this.tabPage1.Controls.Add(this.btnBuild);
            this.tabPage1.Controls.Add(this.btnBrowse);
            this.tabPage1.Controls.Add(this.tbCondition);
            this.tabPage1.Controls.Add(this.tbField);
            this.tabPage1.Controls.Add(this.lblEqual);
            this.tabPage1.Controls.Add(this.lblWhere);
            this.tabPage1.Controls.Add(this.lblSet);
            this.tabPage1.Controls.Add(this.tbDbName);
            this.tabPage1.Controls.Add(this.tbTable);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.lblUpdate);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(270, 250);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Update";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkBoxEncode64
            // 
            this.chkBoxEncode64.AutoSize = true;
            this.chkBoxEncode64.Checked = true;
            this.chkBoxEncode64.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxEncode64.Location = new System.Drawing.Point(57, 124);
            this.chkBoxEncode64.Name = "chkBoxEncode64";
            this.chkBoxEncode64.Size = new System.Drawing.Size(85, 17);
            this.chkBoxEncode64.TabIndex = 23;
            this.chkBoxEncode64.Text = "B64 Encode";
            this.chkBoxEncode64.UseVisualStyleBackColor = true;
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(183, 120);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(75, 23);
            this.btnBuild.TabIndex = 22;
            this.btnBuild.Text = "build";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.tbBuild_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(183, 63);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 21;
            this.btnBrowse.Text = "browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbCondition
            // 
            this.tbCondition.Location = new System.Drawing.Point(57, 94);
            this.tbCondition.Name = "tbCondition";
            this.tbCondition.Size = new System.Drawing.Size(100, 20);
            this.tbCondition.TabIndex = 20;
            // 
            // tbField
            // 
            this.tbField.Location = new System.Drawing.Point(57, 67);
            this.tbField.Name = "tbField";
            this.tbField.Size = new System.Drawing.Size(100, 20);
            this.tbField.TabIndex = 18;
            // 
            // lblEqual
            // 
            this.lblEqual.AutoSize = true;
            this.lblEqual.Location = new System.Drawing.Point(163, 70);
            this.lblEqual.Name = "lblEqual";
            this.lblEqual.Size = new System.Drawing.Size(13, 13);
            this.lblEqual.TabIndex = 17;
            this.lblEqual.Text = "=";
            // 
            // lblWhere
            // 
            this.lblWhere.AutoSize = true;
            this.lblWhere.Location = new System.Drawing.Point(8, 97);
            this.lblWhere.Name = "lblWhere";
            this.lblWhere.Size = new System.Drawing.Size(36, 13);
            this.lblWhere.TabIndex = 15;
            this.lblWhere.Text = "where";
            // 
            // lblSet
            // 
            this.lblSet.AutoSize = true;
            this.lblSet.Location = new System.Drawing.Point(8, 67);
            this.lblSet.Name = "lblSet";
            this.lblSet.Size = new System.Drawing.Size(23, 13);
            this.lblSet.TabIndex = 16;
            this.lblSet.Text = "Set";
            // 
            // tbDbName
            // 
            this.tbDbName.Location = new System.Drawing.Point(56, 11);
            this.tbDbName.Name = "tbDbName";
            this.tbDbName.Size = new System.Drawing.Size(100, 20);
            this.tbDbName.TabIndex = 19;
            this.tbDbName.Text = "ATDb";
            // 
            // tbTable
            // 
            this.tbTable.Location = new System.Drawing.Point(57, 41);
            this.tbTable.Name = "tbTable";
            this.tbTable.Size = new System.Drawing.Size(100, 20);
            this.tbTable.TabIndex = 19;
            this.tbTable.Text = "asm.release_cfg";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Use";
            // 
            // lblUpdate
            // 
            this.lblUpdate.AutoSize = true;
            this.lblUpdate.Location = new System.Drawing.Point(8, 41);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(42, 13);
            this.lblUpdate.TabIndex = 14;
            this.lblUpdate.Text = "Update";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbDbName1);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.chBoxEncode2);
            this.tabPage2.Controls.Add(this.chBoxEncode1);
            this.tabPage2.Controls.Add(this.btnBuild1);
            this.tabPage2.Controls.Add(this.btnBrowse1);
            this.tabPage2.Controls.Add(this.btnBrowse2);
            this.tabPage2.Controls.Add(this.tbInsertValues);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.tbInsertItems);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(270, 250);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Insert";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbDbName1
            // 
            this.tbDbName1.Location = new System.Drawing.Point(66, 6);
            this.tbDbName1.Name = "tbDbName1";
            this.tbDbName1.Size = new System.Drawing.Size(100, 20);
            this.tbDbName1.TabIndex = 29;
            this.tbDbName1.Text = "ATDb";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Use";
            // 
            // chBoxEncode2
            // 
            this.chBoxEncode2.AutoSize = true;
            this.chBoxEncode2.Checked = true;
            this.chBoxEncode2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chBoxEncode2.Location = new System.Drawing.Point(21, 166);
            this.chBoxEncode2.Name = "chBoxEncode2";
            this.chBoxEncode2.Size = new System.Drawing.Size(85, 17);
            this.chBoxEncode2.TabIndex = 27;
            this.chBoxEncode2.Text = "B64 Encode";
            this.chBoxEncode2.UseVisualStyleBackColor = true;
            // 
            // chBoxEncode1
            // 
            this.chBoxEncode1.AutoSize = true;
            this.chBoxEncode1.Checked = true;
            this.chBoxEncode1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chBoxEncode1.Location = new System.Drawing.Point(21, 142);
            this.chBoxEncode1.Name = "chBoxEncode1";
            this.chBoxEncode1.Size = new System.Drawing.Size(85, 17);
            this.chBoxEncode1.TabIndex = 27;
            this.chBoxEncode1.Text = "B64 Encode";
            this.chBoxEncode1.UseVisualStyleBackColor = true;
            // 
            // btnBuild1
            // 
            this.btnBuild1.Location = new System.Drawing.Point(186, 203);
            this.btnBuild1.Name = "btnBuild1";
            this.btnBuild1.Size = new System.Drawing.Size(75, 23);
            this.btnBuild1.TabIndex = 26;
            this.btnBuild1.Text = "build";
            this.btnBuild1.UseVisualStyleBackColor = true;
            this.btnBuild1.Click += new System.EventHandler(this.btnBuild1_Click);
            // 
            // btnBrowse1
            // 
            this.btnBrowse1.Location = new System.Drawing.Point(112, 136);
            this.btnBrowse1.Name = "btnBrowse1";
            this.btnBrowse1.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse1.TabIndex = 25;
            this.btnBrowse1.Text = "browse";
            this.btnBrowse1.UseVisualStyleBackColor = true;
            this.btnBrowse1.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnBrowse2
            // 
            this.btnBrowse2.Location = new System.Drawing.Point(112, 165);
            this.btnBrowse2.Name = "btnBrowse2";
            this.btnBrowse2.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse2.TabIndex = 25;
            this.btnBrowse2.Text = "browse";
            this.btnBrowse2.UseVisualStyleBackColor = true;
            this.btnBrowse2.Click += new System.EventHandler(this.btnBrowse2_Click);
            // 
            // tbInsertValues
            // 
            this.tbInsertValues.Location = new System.Drawing.Point(63, 103);
            this.tbInsertValues.Name = "tbInsertValues";
            this.tbInsertValues.Size = new System.Drawing.Size(200, 20);
            this.tbInsertValues.TabIndex = 23;
            this.tbInsertValues.Text = "1, 1, \'AMS 6.0\',\'6.0.0\',GETDATE(),";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(193, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = ",";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(193, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = ")";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Values (";
            // 
            // tbInsertItems
            // 
            this.tbInsertItems.Location = new System.Drawing.Point(66, 32);
            this.tbInsertItems.Multiline = true;
            this.tbInsertItems.Name = "tbInsertItems";
            this.tbInsertItems.Size = new System.Drawing.Size(195, 65);
            this.tbInsertItems.TabIndex = 21;
            this.tbInsertItems.Text = "ASM.RELEASE_CFG ([ID_RELEASE],[ID_CPU_TYPE],[NAME],[VERSION],[DATETIME],[CONFIGUR" +
                "ATION],[RELEASE_DATA])";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Insert Into ";
            // 
            // GenSQLDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 276);
            this.Controls.Add(this.tabOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenSQLDialog";
            this.ShowIcon = false;
            this.Text = "Generate SQL";
            this.tabOptions.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabOptions;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox chkBoxEncode64;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbCondition;
        private System.Windows.Forms.TextBox tbField;
        private System.Windows.Forms.Label lblEqual;
        private System.Windows.Forms.Label lblWhere;
        private System.Windows.Forms.Label lblSet;
        private System.Windows.Forms.TextBox tbTable;
        private System.Windows.Forms.Label lblUpdate;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox tbInsertItems;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chBoxEncode1;
        private System.Windows.Forms.Button btnBuild1;
        private System.Windows.Forms.Button btnBrowse2;
        private System.Windows.Forms.TextBox tbInsertValues;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chBoxEncode2;
        private System.Windows.Forms.Button btnBrowse1;
        private System.Windows.Forms.TextBox tbDbName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDbName1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;

    }
}