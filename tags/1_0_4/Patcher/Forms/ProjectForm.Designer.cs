namespace Patcher.Forms
{
    partial class ProjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectForm));
            this.pgGenerateOptions = new System.Windows.Forms.PropertyGrid();
            this.lblCommands = new System.Windows.Forms.Label();
            this.lblGenerationOptions = new System.Windows.Forms.Label();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.imageListCmdType = new System.Windows.Forms.ImageList(this.components);
            this.listViewCommands = new System.Windows.Forms.ListView();
            this.CommandName = new System.Windows.Forms.ColumnHeader();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pgGenerateOptions
            // 
            this.pgGenerateOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgGenerateOptions.Location = new System.Drawing.Point(15, 25);
            this.pgGenerateOptions.Name = "pgGenerateOptions";
            this.pgGenerateOptions.Size = new System.Drawing.Size(548, 448);
            this.pgGenerateOptions.TabIndex = 11;
            this.pgGenerateOptions.ToolbarVisible = false;
            // 
            // lblCommands
            // 
            this.lblCommands.AutoSize = true;
            this.lblCommands.Location = new System.Drawing.Point(12, 476);
            this.lblCommands.Name = "lblCommands";
            this.lblCommands.Size = new System.Drawing.Size(62, 13);
            this.lblCommands.TabIndex = 8;
            this.lblCommands.Text = "Commands:";
            // 
            // lblGenerationOptions
            // 
            this.lblGenerationOptions.AutoSize = true;
            this.lblGenerationOptions.Location = new System.Drawing.Point(12, 9);
            this.lblGenerationOptions.Name = "lblGenerationOptions";
            this.lblGenerationOptions.Size = new System.Drawing.Size(101, 13);
            this.lblGenerationOptions.TabIndex = 9;
            this.lblGenerationOptions.Text = "Generation Options:";
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Enabled = false;
            this.btnDown.Image = global::Patcher.Properties.Resources.down;
            this.btnDown.Location = new System.Drawing.Point(533, 528);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(30, 30);
            this.btnDown.TabIndex = 12;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Enabled = false;
            this.btnUp.Image = global::Patcher.Properties.Resources.up;
            this.btnUp.Location = new System.Drawing.Point(533, 492);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 30);
            this.btnUp.TabIndex = 12;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // imageListCmdType
            // 
            this.imageListCmdType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCmdType.ImageStream")));
            this.imageListCmdType.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListCmdType.Images.SetKeyName(0, "UNKNOWN.ico");
            this.imageListCmdType.Images.SetKeyName(1, "SQLScript.ico");
            this.imageListCmdType.Images.SetKeyName(2, "ExecutableModule.ico");
            this.imageListCmdType.Images.SetKeyName(3, "BATScript.ico");
            this.imageListCmdType.Images.SetKeyName(4, "WSHScript.ico");
            // 
            // listViewCommands
            // 
            this.listViewCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCommands.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CommandName});
            this.listViewCommands.FullRowSelect = true;
            this.listViewCommands.GridLines = true;
            this.listViewCommands.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewCommands.HideSelection = false;
            this.listViewCommands.Location = new System.Drawing.Point(15, 492);
            this.listViewCommands.MultiSelect = false;
            this.listViewCommands.Name = "listViewCommands";
            this.listViewCommands.Size = new System.Drawing.Size(512, 127);
            this.listViewCommands.SmallImageList = this.imageListCmdType;
            this.listViewCommands.TabIndex = 13;
            this.listViewCommands.UseCompatibleStateImageBehavior = false;
            this.listViewCommands.View = System.Windows.Forms.View.Details;
            this.listViewCommands.SelectedIndexChanged += new System.EventHandler(this.listViewCommands_SelectedIndexChanged);
            // 
            // CommandName
            // 
            this.CommandName.Text = "Command Name";
            this.CommandName.Width = 500;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(15, 624);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(96, 624);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 14;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // ProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 659);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.listViewCommands);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.pgGenerateOptions);
            this.Controls.Add(this.lblCommands);
            this.Controls.Add(this.lblGenerationOptions);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectForm";
            this.Load += new System.EventHandler(this.ProjectForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProjectForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgGenerateOptions;
        private System.Windows.Forms.Label lblCommands;
        private System.Windows.Forms.Label lblGenerationOptions;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.ImageList imageListCmdType;
        private System.Windows.Forms.ListView listViewCommands;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ColumnHeader CommandName;
    }
}