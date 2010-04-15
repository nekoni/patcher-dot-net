using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace Patcher.Forms
{
    public partial class ProjectExplorer : DockContent
    {
        #region Fields

        private ProjectForm m_CurrentProject = null;

        private TreeNode m_CurrentNode = null;

        #endregion

        #region Constructor

        public ProjectExplorer()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public void AppendNodes(string[] NodesKeys, TreeNode currentNode, TreeNode newNode)
        {
            foreach (string strKey in NodesKeys)
            {
                if (strKey == tvProjectExplorer.Nodes[0].Text)
                    continue;

                if (currentNode.Nodes[strKey] == null)
                    currentNode.Nodes.Add(newNode);
                else
                    currentNode = currentNode.Nodes[strKey];
            }
        }

        public void SelectProject(ProjectForm projectForm)
        {
            m_CurrentProject = projectForm;

            //populate the tree
            tvProjectExplorer.Nodes.Clear();
            if (projectForm == null)
                return;

            TreeNode rootNode = new TreeNode();
            rootNode.Text = rootNode.Name = projectForm.Text;
            rootNode.ImageKey = "PROJECT";
            rootNode.SelectedImageKey = "PROJECT";
            rootNode.Tag = new PatcherProjectNode(projectForm.FileName);
            tvProjectExplorer.Nodes.Add(rootNode);
            projectForm.OnProjectFileNameChanged += new ProjectForm.ProjectFileNameChangedEventHandeler(projectForm_OnProjectFileNameChanged);

            //add folders
            foreach (FolderNode fn in projectForm.FolderNodes)
            {
                TreeNode currentNode = rootNode;
                TreeNode tn = CreateNewFolderNode(fn.FolderName, fn);
                string[] NodeKeys = fn.NodePath.Split(@"\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                AppendNodes(NodeKeys, currentNode, tn);
                tn.EnsureVisible();
            }

            //add resources
            foreach (ResourceNode rn in projectForm.ResourceNodes)
            {
                TreeNode currentNode = rootNode;
                TreeNode tn = CreateNewResourceNode(rn.FileName, rn);
                string[] NodeKeys = rn.NodePath.Split(@"\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                AppendNodes(NodeKeys, currentNode, tn);
                tn.EnsureVisible();
            }
        }

        public void SetProjectName(string Name)
        {
            tvProjectExplorer.Nodes[0].Name = tvProjectExplorer.Nodes[0].Text = Name;
        }

        TreeNode CreateNewFolderNode(string Text, object TAG)
        {
            TreeNode tn = new TreeNode();
            tn.Name = tn.Text = Text;
            tn.SelectedImageKey = tn.ImageKey = "FOLDER";
            tn.Tag = TAG;
            return tn;
        }

        TreeNode CreateNewResourceNode(string Text, object TAG)
        {
            TreeNode tn = new TreeNode();
            tn.Name = tn.Text = Text;
            ResourceNode rn = (ResourceNode) TAG;
            string strIcon = "RESOURCE";
            if (!rn.ResourceFound)
                strIcon = "UNAVAILABLE";
            tn.SelectedImageKey = tn.ImageKey = strIcon;
            tn.Tag = TAG;
            return tn;
        }

        string GetNewFolderName()
        {
            int i = 0;
            string OutName = "NewFolder";
            TreeNode tn = tvProjectExplorer.SelectedNode;
            while(true)
            {
                if(tn.Nodes.ContainsKey(OutName + i.ToString()))
                    i++;
                else
                    break;
            }
            return OutName + i.ToString();

        }

        void DeleteNode(TreeNode tn)
        {
            foreach (TreeNode t in tn.Nodes)
            {
                if(t.Tag.GetType() == typeof(FolderNode))
                    m_CurrentProject.DelFolder((FolderNode)t.Tag);
                else if (t.Tag.GetType() == typeof(ResourceNode))
                    m_CurrentProject.DelResource((ResourceNode)t.Tag);
                DeleteNode(t);
            }
            tn.Nodes.Clear();
        }

        #endregion

        #region Local Events

        private void NewFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderNode fn = new FolderNode();
            fn.FolderName = GetNewFolderName();
            fn.OnFolderNodeChanged +=new FolderNode.FolderNodeChangedEventHandeler(fn_OnFolderNodeChanged);
            TreeNode tn = CreateNewFolderNode(fn.FolderName, fn);
            tvProjectExplorer.SelectedNode.Nodes.Add(tn);
            fn.NodePath = tn.FullPath;
            tn.EnsureVisible();
            m_CurrentProject.AddFolder(fn);
        }

        private void fn_OnFolderNodeChanged(FolderNode fn)
        {
            if (!tvProjectExplorer.SelectedNode.Parent.Nodes.ContainsKey(fn.Name))
            {
                tvProjectExplorer.SelectedNode.Name = tvProjectExplorer.SelectedNode.Text = fn.Name;
                fn.NodePath = tvProjectExplorer.SelectedNode.FullPath;
            }
            else
            {
                fn.FolderName = tvProjectExplorer.SelectedNode.Name;
                MainForm.Instance.propertyWindow.SelectObject(m_CurrentNode.Tag);
            }
        }

        private void NewResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Selecte Resources";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string FileName in ofd.FileNames)
                {
                    ResourceNode rn = new ResourceNode();
                    rn.ResourceFullPath = FileName;
                    TreeNode tn = CreateNewResourceNode(rn.FileName, rn);
                    tvProjectExplorer.SelectedNode.Nodes.Add(tn);
                    rn.NodePath = tn.FullPath;
                    tn.EnsureVisible();
                    m_CurrentProject.AddResource(rn);
                }
            }
        }

        private void DeleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode tn = tvProjectExplorer.SelectedNode;
            DeleteNode(tn);
            m_CurrentProject.DelFolder((FolderNode)tn.Tag);
            tn.Remove();
            
        }

        private void DeleteResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode tn = tvProjectExplorer.SelectedNode;
            m_CurrentProject.DelResource((ResourceNode)tn.Tag);
            tn.Remove();
        }

        private void tvProjectExplorer_MouseUp(object sender, MouseEventArgs e)
        {
            Point clickPoint = new Point(e.X, e.Y);
            m_CurrentNode = tvProjectExplorer.GetNodeAt(clickPoint);
            if (m_CurrentNode == null) return;

            tvProjectExplorer.SelectedNode = m_CurrentNode;

            if (e.Button == MouseButtons.Right)
            {

                Point screenPoint = tvProjectExplorer.PointToScreen(clickPoint);
                Point formPoint = tvProjectExplorer.PointToClient(screenPoint);

                if (m_CurrentNode.Tag.GetType() == typeof(PatcherProjectNode))
                {
                    NewFolderToolStripMenuItem.Visible = true;
                    NewResourceToolStripMenuItem.Visible = true;
                    DeleteFolderToolStripMenuItem.Visible = false;
                    DeleteResourceToolStripMenuItem.Visible = false;
                }
                else if (m_CurrentNode.Tag.GetType() == typeof(FolderNode))
                {
                    NewFolderToolStripMenuItem.Visible = true;
                    NewResourceToolStripMenuItem.Visible = true;
                    DeleteFolderToolStripMenuItem.Visible = true;
                    DeleteResourceToolStripMenuItem.Visible = false;
                }
                else if (m_CurrentNode.Tag.GetType() == typeof(ResourceNode))
                {
                    NewFolderToolStripMenuItem.Visible = false;
                    NewResourceToolStripMenuItem.Visible = false;
                    DeleteFolderToolStripMenuItem.Visible = false;
                    DeleteResourceToolStripMenuItem.Visible = true;
                }

                contextMenuStrip.Show(this, formPoint);
            }

            MainForm.Instance.propertyWindow.SelectObject(m_CurrentNode.Tag);
        }

        private void projectForm_OnProjectFileNameChanged(string filename)
        {
            tvProjectExplorer.Nodes[0].Tag = new PatcherProjectNode(filename);
        }
        #endregion

        
    }
}