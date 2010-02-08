namespace Patcher.Forms
{
    partial class ProjectExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectExplorer));
            this.tvProjectExplorer = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NewFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewResourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteResourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvProjectExplorer
            // 
            this.tvProjectExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvProjectExplorer.HideSelection = false;
            this.tvProjectExplorer.ImageIndex = 0;
            this.tvProjectExplorer.ImageList = this.imageList;
            this.tvProjectExplorer.Location = new System.Drawing.Point(0, 0);
            this.tvProjectExplorer.Name = "tvProjectExplorer";
            this.tvProjectExplorer.SelectedImageIndex = 0;
            this.tvProjectExplorer.Size = new System.Drawing.Size(292, 273);
            this.tvProjectExplorer.TabIndex = 0;
            this.tvProjectExplorer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvProjectExplorer_MouseUp);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewFolderToolStripMenuItem,
            this.NewResourceToolStripMenuItem,
            this.DeleteFolderToolStripMenuItem,
            this.DeleteResourceToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(165, 92);
            // 
            // NewFolderToolStripMenuItem
            // 
            this.NewFolderToolStripMenuItem.Image = global::Patcher.Properties.Resources.NewFolderHS;
            this.NewFolderToolStripMenuItem.Name = "NewFolderToolStripMenuItem";
            this.NewFolderToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.NewFolderToolStripMenuItem.Text = "New Folder";
            this.NewFolderToolStripMenuItem.Click += new System.EventHandler(this.NewFolderToolStripMenuItem_Click);
            // 
            // NewResourceToolStripMenuItem
            // 
            this.NewResourceToolStripMenuItem.Image = global::Patcher.Properties.Resources.AddToFavoritesHS;
            this.NewResourceToolStripMenuItem.Name = "NewResourceToolStripMenuItem";
            this.NewResourceToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.NewResourceToolStripMenuItem.Text = "New Resource";
            this.NewResourceToolStripMenuItem.Click += new System.EventHandler(this.NewResourceToolStripMenuItem_Click);
            // 
            // DeleteFolderToolStripMenuItem
            // 
            this.DeleteFolderToolStripMenuItem.Image = global::Patcher.Properties.Resources.DeleteFolderHS;
            this.DeleteFolderToolStripMenuItem.Name = "DeleteFolderToolStripMenuItem";
            this.DeleteFolderToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.DeleteFolderToolStripMenuItem.Text = "Delete Folder";
            this.DeleteFolderToolStripMenuItem.Click += new System.EventHandler(this.DeleteFolderToolStripMenuItem_Click);
            // 
            // DeleteResourceToolStripMenuItem
            // 
            this.DeleteResourceToolStripMenuItem.Image = global::Patcher.Properties.Resources.DeleteHS;
            this.DeleteResourceToolStripMenuItem.Name = "DeleteResourceToolStripMenuItem";
            this.DeleteResourceToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.DeleteResourceToolStripMenuItem.Text = "Delete Resource";
            this.DeleteResourceToolStripMenuItem.Click += new System.EventHandler(this.DeleteResourceToolStripMenuItem_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "PROJECT");
            this.imageList.Images.SetKeyName(1, "FOLDER");
            this.imageList.Images.SetKeyName(2, "RESOURCE");
            // 
            // ProjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.tvProjectExplorer);
            this.Name = "ProjectExplorer";
            this.TabText = "ProjectExplorer";
            this.Text = "ProjectExplorer";
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvProjectExplorer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem NewFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewResourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteResourceToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList;
    }
}