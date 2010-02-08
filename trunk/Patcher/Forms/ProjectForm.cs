using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using Patcher.Helper;
using System.Reflection;

namespace Patcher.Forms
{
    #region Enum

    public enum CmdTypes
    {
        UNKNOWN = 0,
        SQLScript,
        ExecutableModule,
        BATScript,
        WSHScript
    }

    public enum NodeTypes
    {
        Folder,
        Content
    }

    #endregion

    public partial class ProjectForm : DockContent
    {
        #region Fields

        string m_FileName = string.Empty;

        PatcherProject m_PatcherProject;
        
        #endregion

        #region Properties

        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }

        public List<FolderNode> FolderNodes
        {
            get { return m_PatcherProject.FolderNodes; }
        }

        public List<ResourceNode> ResourceNodes
        {
            get { return m_PatcherProject.ResourceNodes; }
        }

        public const string Filter = "Patcher Project File (*.ppj)|*.ppj";

        public PatcherProject patcherProject
        {
            get { return m_PatcherProject; }
        }

        #endregion

        #region Events

        public delegate void ProjectNameChangedEventHandeler(string Name);
        public event ProjectNameChangedEventHandeler OnProjectNameChanged;

        public delegate void ProjectFileNameChangedEventHandeler(string filename);
        public event ProjectFileNameChangedEventHandeler OnProjectFileNameChanged;

        #endregion

        #region Constructor

        public ProjectForm()
        {
            InitializeComponent();

            m_PatcherProject = new PatcherProject(true);
        }

        public ProjectForm(string FileName)
        {
            InitializeComponent();

            m_PatcherProject = XMLSerializationHelper<PatcherProject>.Load(FileName);
            Text = m_PatcherProject.ProjectName;
            m_FileName = FileName;
        }

        #endregion

        #region Methods

        private void SetupProjectForm()
        {
            m_PatcherProject.ProjectName = Text;
            pgGenerateOptions.SelectedObject = m_PatcherProject.PO;
            listViewCommands.Items.Clear();
            foreach (CommandEntry cmd in m_PatcherProject.CommandEntries)
            {
                ListViewItem lvi = new ListViewItem(cmd.CommandName, (int)cmd.CmdType);
                lvi.Tag = cmd;
                listViewCommands.Items.Add(lvi);
            }
        }

        private void MoveItem(bool UpOrDown)
        {
            int dr = UpOrDown ? -1 : 1;
            if (listViewCommands.SelectedIndices.Count > 0)
            {
                int RowIndex = listViewCommands.SelectedIndices[0];
                ListViewItem lvi = listViewCommands.SelectedItems[0];
                CommandEntry cmdTmp = (CommandEntry)lvi.Tag;
                CommandEntry cmdNew = new CommandEntry();
                cmdNew.CmdType = cmdTmp.CmdType;
                cmdNew.CommandArguments = cmdTmp.CommandArguments;
                cmdNew.CommandName = cmdTmp.CommandName;
                cmdNew.ResourceFullPath = cmdTmp.ResourceFullPath;
                cmdNew.WorkingFolder = cmdTmp.WorkingFolder;
                ListViewItem lvid = listViewCommands.Items[RowIndex + dr];
                lvi.Text = lvid.Text;
                lvi.ImageIndex = lvid.ImageIndex;
                lvi.Tag = lvid.Tag;
                lvi.Selected = false;
                lvid.Text = cmdNew.CommandName;
                lvid.ImageIndex = (int)cmdNew.CmdType;
                lvid.Tag = cmdNew;
                lvid.Selected = true;
            }
        }

        private void SaveProjectFile()
        {
            m_PatcherProject.ProjectName = Path.GetFileNameWithoutExtension(m_FileName);
            foreach (FolderNode fn in m_PatcherProject.FolderNodes)
                fn.NodePath = fn.NodePath.Replace(Text, m_PatcherProject.ProjectName);
            foreach (ResourceNode rn in m_PatcherProject.ResourceNodes)
                rn.NodePath = rn.NodePath.Replace(Text, m_PatcherProject.ProjectName);
            Text = m_PatcherProject.ProjectName;
            if (OnProjectNameChanged != null)
                OnProjectNameChanged(Text);
            m_PatcherProject.CommandEntries.Clear();
            foreach (ListViewItem lvi in listViewCommands.Items)
                m_PatcherProject.CommandEntries.Add((CommandEntry)lvi.Tag);
            XMLSerializationHelper<PatcherProject>.Save(m_PatcherProject, m_FileName);
        }
        #endregion

        #region Public Methods

        public void AddFolder(FolderNode fn)
        {
            m_PatcherProject.FolderNodes.Add(fn);
        }

        public void AddResource(ResourceNode rn)
        {
            m_PatcherProject.ResourceNodes.Add(rn);
        }

        public void DelFolder(FolderNode fn)
        {
            m_PatcherProject.FolderNodes.Remove(fn);
        }

        public void DelResource(ResourceNode rn)
        {
            m_PatcherProject.ResourceNodes.Remove(rn);
        }

        public void SaveAsProject()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = Application.ExecutablePath;
                sfd.FileName = m_PatcherProject.ProjectName + ".ppj";
                sfd.Filter = Filter;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    m_FileName = sfd.FileName;
                    if (OnProjectFileNameChanged != null)
                        OnProjectFileNameChanged(m_FileName);
                }
                else
                    return;
             
                SaveProjectFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void SaveProject()
        {
            try
            {
                if (m_FileName == string.Empty)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.InitialDirectory = Application.ExecutablePath;
                    sfd.FileName = m_PatcherProject.ProjectName + ".ppj";
                    sfd.Filter = Filter;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        m_FileName = sfd.FileName;
                        if (OnProjectFileNameChanged != null)
                            OnProjectFileNameChanged(m_FileName);
                    }
                    else
                        return;
                }

                SaveProjectFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void UpdateCommandEntries()
        {
            m_PatcherProject.CommandEntries.Clear();
            foreach (ListViewItem lvi in listViewCommands.Items)
                m_PatcherProject.CommandEntries.Add((CommandEntry) lvi.Tag);
        }

        #endregion

        #region Local Events

        private void ProjectForm_Load(object sender, EventArgs e)
        {
            SetupProjectForm();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CommandEntry cmd = new CommandEntry();
            ListViewItem lvi = new ListViewItem("New Command", (int) CmdTypes.UNKNOWN);
            cmd.CommandName = "New Command";
            cmd.CmdType = CmdTypes.UNKNOWN;
            cmd.OnCommandEntryChanged += new CommandEntry.CommandEntryChangedEventHandeler(cmd_OnCommandEntryChanged);
            lvi.Tag = cmd;
            lvi.Selected = true;
            listViewCommands.Items.Add(lvi);
        }

        void cmd_OnCommandEntryChanged(CommandEntry ce)
        {
            ListViewItem lvi = listViewCommands.SelectedItems[0];
            lvi.Text = ce.CommandName;
            lvi.ImageIndex = (int)ce.CmdType;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listViewCommands.SelectedIndices.Count > 0)
            {
                int RowIndex = listViewCommands.SelectedIndices[0];
                listViewCommands.Items.RemoveAt(RowIndex);
                MainForm.Instance.propertyWindow.SelectObject(null);
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            MoveItem(true);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            MoveItem(false);
        }

        private void listViewCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewCommands.SelectedIndices.Count > 0)
            {
                if (listViewCommands.Items.Count > 1)
                {
                    int RowIndex = listViewCommands.SelectedIndices[0];
                    if (RowIndex == 0)
                    {
                        btnUp.Enabled = false;
                        btnDown.Enabled = true;
                    }
                    else if (RowIndex == listViewCommands.Items.Count - 1)
                    {
                        btnUp.Enabled = true;
                        btnDown.Enabled = false;
                    }
                    else
                    {
                        btnUp.Enabled = true;
                        btnDown.Enabled = true;
                    }
                }
                else
                {
                    btnUp.Enabled = false;
                    btnDown.Enabled = false;
                }
                btnRemove.Enabled = true;
                btnAdd.Enabled = true;

                ListViewItem lvi = listViewCommands.SelectedItems[0];
                MainForm.Instance.propertyWindow.SelectObject(lvi.Tag);
            }
            else
            {
                btnRemove.Enabled = false;
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
        }

        private void ProjectForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.Instance.projectExplorer.SelectProject(null);
            MainForm.Instance.propertyWindow.SelectObject(null);
        }

        #endregion   
    }

    public class PatcherProjectNode
    {
        public PatcherProjectNode(string FileName) { _FileName = FileName; }

        private string _FileName;

        public string FileName
        {
            get { return _FileName; }
        }
    }

    [Serializable]
    [DefaultPropertyAttribute("Appearance")]
    public class PatcherOptions
    {
        public PatcherOptions() { }

        string _Text;
        const string TEXT = "STR_TEXT";
        [CategoryAttribute("Appearance"),
        DescriptionAttribute("The text associated with the patch main window")]
        public string Text { get { return _Text; } set { _Text = value; } }

        string _IconFile;
        [EditorAttribute(typeof(FileNameEditor), typeof(UITypeEditor)),
        CategoryAttribute("Appearance"),
        DescriptionAttribute("Specify the icon displayed in the form's controlbox")]
        public string IconFile { get { return _IconFile; } set { _IconFile = value; } }

        string _LogoFile;
        [EditorAttribute(typeof(FileNameEditor), typeof(UITypeEditor)),
        CategoryAttribute("Appearance"),
        DescriptionAttribute("Specify the logo displayed in the form")]
        public string LogoFile { get { return _LogoFile; } set { _LogoFile = value; } }

        string _Title;
        const string TITLE = "STR_TITLE";
        [CategoryAttribute("Appearance"),
        DescriptionAttribute("The text associated with the patch title")]
        public string Title { get { return _Title; } set { _Title = value; } }

        string _ConnectionString;
        const string CONNECTIONSTRING = "STR_CONNECTIONSTRING";
        [CategoryAttribute("SQL"),
        DescriptionAttribute("The SQL connection string")]
        public string ConnectionString { get { return _ConnectionString; } set { _ConnectionString = value; } }

        int _CommandTimeout;
        const string COMMANDTIMEOUT = "INT_COMMANDTIMEOUT";
        [CategoryAttribute("SQL"),
        DescriptionAttribute("The SQL command timeout (s)")]
        public int CommandTimeout { get { return _CommandTimeout; } set { _CommandTimeout = value; } }

        string _ServicesNames;
        const string SERVICESNAMES = "STR_SERVICESNAMES";
        [CategoryAttribute("Services"),
        DescriptionAttribute("Specify services names to restart after the installation. Names must be separeted by a semicolumn (;) ")]
        public string ServicesNames { get { return _ServicesNames; } set { _ServicesNames = value; } }

        bool _RestartServices;
        const string RESTARTSERVICES = "BOOL_RESTARTSERVICES";
        [CategoryAttribute("Services"),
        DescriptionAttribute("If TRUE stop services before installation and restart after installation")]
        public bool RestartServices { get { return _RestartServices; } set { _RestartServices = value; } }

        int _PreviousPatchVersionRequired;
        const string PREVIOUSPATCHVERSIONREQUIRED = "INT_PREVIOUSPATCHVERSIONREQUIRED";
        [CategoryAttribute("Version"),
        DescriptionAttribute("Specify patch version that has to be installed before apply this patch")]
        public int PreviousPatchVersionRequired { get { return _PreviousPatchVersionRequired; } set { _PreviousPatchVersionRequired = value; } }

        bool _PreviousPatchRequired;
        [CategoryAttribute("Version"),
        DescriptionAttribute("If TRUE check if a patch with PreviousPatchVersionRequired was installed to continue")]
        public bool PreviousPatchRequired { get { return _PreviousPatchRequired; } set { _PreviousPatchRequired = value; } }

        int _PatchVersion;
        const string PATCHVERSION = "INT_PATCHVERSION";
        [CategoryAttribute("Version"),
        DescriptionAttribute("Specify patch version of the output patch")]
        public int PatchVersion { get { return _PatchVersion; } set { _PatchVersion = value; } }

        bool _SavePatchVersion;
        [CategoryAttribute("Version"),
        DescriptionAttribute("If TRUE save in the registry the patch version")]
        public bool SavePatchVersion { get { return _SavePatchVersion; } set { _SavePatchVersion = value; } }

        string _RegistryPathPatchVersion;
        const string REGISTRYPATHPATCHVERSION = "STR_REGISTRYPATHPATCHVERSION";
        [CategoryAttribute("Version"),
        DescriptionAttribute("The registry path where read/write the patch version (inside HKEY_LOCAL_MACHINE)")]
        public string RegistryPathPatchVersion { get { return _RegistryPathPatchVersion; } set { _RegistryPathPatchVersion = value; } }

        string _RegistryKeyPatchVersion;
        const string REGISTRYKEYPATCHVERSION = "STR_REGISTRYKEYPATCHVERSION";
        [CategoryAttribute("Version"),
        DescriptionAttribute("The registry key where read/write the patch version (inside HKEY_LOCAL_MACHINE)")]
        public string RegistryKeyPatchVersion { get { return _RegistryKeyPatchVersion; } set { _RegistryKeyPatchVersion = value; } }

        bool _SilentModuleExecution;
        const string SILENTMODULEEXECUTION = "BOOL_SILENTMODULEEXECUTION";
        [CategoryAttribute("Module Execution"),
        DescriptionAttribute("If TRUE hide the module window when executed")]
        public bool SilentModuleExecution { get { return _SilentModuleExecution; } set { _SilentModuleExecution = value; } }

        string _RegistryInstallPath;
        const string REGISTRYINSTALLPATH = "STR_REGISTRYINSTALLPATH";
        [CategoryAttribute("Install Folder"),
        DescriptionAttribute("The registry path where is specified the installation root folder (inside HKEY_LOCAL_MACHINE)")]
        public string RegistryInstallPath { get { return _RegistryInstallPath; } set { _RegistryInstallPath = value; } }

        string _RegistryInstallKey;
        const string REGISTRYINSTALLKEY = "STR_REGISTRYINSTALLKEY";
        [CategoryAttribute("Install Folder"),
        DescriptionAttribute("The registry key where is specified the installation root folder (inside HKEY_LOCAL_MACHINE)")]
        public string RegistryInstallKey { get { return _RegistryInstallKey; } set { _RegistryInstallKey = value; } }

        string _InstallationPath;
        const string INSTALLATIONPATH = "STR_INSTALLATIONPATH";
        [CategoryAttribute("Install Folder"),
        DescriptionAttribute("Specify the installation root folder. This override the registry install path")]
        public string InstallationPath { get { return _InstallationPath; } set { _InstallationPath = value; } }

        string _SourceFile;
        [EditorAttribute(typeof(FileNameEditor), typeof(UITypeEditor)),
        CategoryAttribute("Build"),
        DescriptionAttribute("Specify source code file (CSharp) which contains the GUI to be build")]
        public string SourceFile { get { return _SourceFile; } set { _SourceFile = value; } }

        string _OutputFileName;
        [CategoryAttribute("Build"),
        DescriptionAttribute("Specify the output file name")]
        public string OutputFileName { get { return _OutputFileName; } set { _OutputFileName = value; } }

        string _OutputFolder;
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor)),
        CategoryAttribute("Build"),
        DescriptionAttribute("Specify the output folder")]
        public string OutputFolder { get { return _OutputFolder; } set { _OutputFolder = value; } }

        string _AdditionalReferencedAssemblies;
        [CategoryAttribute("Build"),
        DescriptionAttribute("Specify additional referenced assemblies to be added during build process. Names must be separeted by a semicolumn (;) ")]
        public string AdditionalReferencedAssemblies { get { return _AdditionalReferencedAssemblies; } set { _AdditionalReferencedAssemblies = value; } }

        string _AdditionalEmbeddedResources;
        [CategoryAttribute("Build"),
        DescriptionAttribute("Specify additional embedded resources with full path which is to be added during build process. Names must be separeted by a semicolumn (;) ")]
        public string AdditionalEmbeddedResources { get { return _AdditionalEmbeddedResources; } set { _AdditionalEmbeddedResources = value; } }

        string _AdditionalLinkedResources;
        [CategoryAttribute("Build"),
        DescriptionAttribute("Specify additional linked resources with full path which is to be added during build process. Names must be separeted by a semicolumn (;) ")]
        public string AdditionalLinkedResources { get { return _AdditionalLinkedResources; } set { _AdditionalLinkedResources = value; } }
     
        string _CompilerOptions;
        [CategoryAttribute("Build"),
        DescriptionAttribute("Specify compiler options")]
        public string CompilerOptions { get { return _CompilerOptions; } set { _CompilerOptions = value; } }

        string _MainClass;
        [CategoryAttribute("Build"),
        DescriptionAttribute("Specify the class that contains the main method of the executable")]
        public string MainClass { get { return _MainClass; } set { _MainClass = value; } }

        bool _SaveSourceCode;
        [CategoryAttribute("Build"),
        DescriptionAttribute("If TRUE save in the output folder a copy of the source code generated")]
        public bool SaveSourceCode { get { return _SaveSourceCode; } set { _SaveSourceCode = value; } }
    }

    [Serializable]
    [DefaultPropertyAttribute("Detail")]
    public class CommandEntry
    {
        public CommandEntry() { }

        public delegate void CommandEntryChangedEventHandeler(CommandEntry ce);
        public event CommandEntryChangedEventHandeler OnCommandEntryChanged;

        private CmdTypes _CmdType;
        [Browsable(false)]
        [XmlAttributeAttribute("CmdType")]
        public CmdTypes CmdType { get { return _CmdType; } set { _CmdType = value; } }

        private string _CommandName;
        [CategoryAttribute("Detail"),
        DescriptionAttribute("Specify the command name visible in the listview")]
        [XmlAttributeAttribute("CommandName")]
        public string CommandName 
        { 
            get { return _CommandName; } 
            set 
            { 
                _CommandName = value;
                if (OnCommandEntryChanged != null)
                    OnCommandEntryChanged(this);
            } 
        }

        private string _ResourceFullPath;
        [EditorAttribute(typeof(FileNameEditor), typeof(UITypeEditor)),
        CategoryAttribute("Detail"),
        DescriptionAttribute("Specify command file with full path")]
        [XmlAttributeAttribute("ResourceFullPath")]
        public string ResourceFullPath 
        { 
            get { return _ResourceFullPath; } 
            set 
            {
                _ResourceTempFullPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
                File.Copy(value, _ResourceTempFullPath, true);
                _ResourceFullPath = value;
                if (value == null)
                {
                    CmdType = CmdTypes.UNKNOWN;
                    return;
                }
                switch (Path.GetExtension(_ResourceFullPath).ToUpper())
                {
                    case ".SQL":
                        CmdType = CmdTypes.SQLScript;
                        break;
                    case ".BAT":
                        CmdType = CmdTypes.BATScript;
                        break;
                    case ".WSH":
                        CmdType = CmdTypes.WSHScript;
                        break;
                    case ".EXE":
                    case ".COM":
                    case ".MSI":
                        CmdType = CmdTypes.ExecutableModule;
                        break;
                    default:
                        CmdType = CmdTypes.UNKNOWN;
                        break;
                }
                if (OnCommandEntryChanged != null)
                    OnCommandEntryChanged(this);
            } 
        }

        private string _ResourceTempFullPath;
        [Browsable(false)]
        [XmlIgnore]
        public string ResourceTempFullPath { get { return _ResourceTempFullPath; } set { _ResourceTempFullPath = value; } }

        private string _CommandArguments;
        [CategoryAttribute("Detail"),
        DescriptionAttribute("Specify the command arguments")]
        [XmlAttributeAttribute("CommandArguments")]
        public string CommandArguments { get { return _CommandArguments; } set { _CommandArguments = value; } }

        private string _WorkingFolder;
        [CategoryAttribute("Detail"),
        DescriptionAttribute("Specify the working folder where the command will be executed")]
        [XmlAttributeAttribute("WorkingFolder")]
        public string WorkingFolder { get { return _WorkingFolder; } set { _WorkingFolder = value; } }
    }

    [Serializable]
    [DefaultPropertyAttribute("Detail")]
    public class ResourceNode
    {
        public ResourceNode() { }

        private string _NodePath;
        [Browsable(false)]
        [XmlAttributeAttribute("NodePath")]
        public string NodePath{ get { return _NodePath; } set { _NodePath = value; }}

        private string _ResourceFullPath;
        [Browsable(false)]
        [XmlAttributeAttribute("ResourceFullPath")]
        public string ResourceFullPath 
        { 
            get { return _ResourceFullPath; } 
            set 
            {
                _ResourceTempFullPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
                File.Copy(value, _ResourceTempFullPath, true);
                _ResourceFullPath = value; 
            } 
        }

        private string _ResourceTempFullPath;
        [Browsable(false)]
        [XmlIgnore]
        public string ResourceTempFullPath { get { return _ResourceTempFullPath; } set { _ResourceTempFullPath = value; } }

        [CategoryAttribute("Detail"),
        DescriptionAttribute("Element File Name")]
        [XmlIgnore]
        public string FileName { get { return Path.GetFileName(_ResourceFullPath); } }

        [CategoryAttribute("Detail"),
        DescriptionAttribute("Element Directory Name")]
        [XmlIgnore]
        public string DirectoryName { get {return Path.GetDirectoryName(_ResourceFullPath); } }
    }

    [Serializable]
    [DefaultPropertyAttribute("Detail")]
    public class FolderNode
    {
        public FolderNode() { }

        public delegate void FolderNodeChangedEventHandeler(FolderNode fn);
        public event FolderNodeChangedEventHandeler OnFolderNodeChanged;

        private string _NodePath;
        [Browsable(false)]
        [XmlAttributeAttribute("NodePath")]
        public string NodePath { get { return _NodePath; } set { _NodePath = value; } }

        private string _FolderName;
        [CategoryAttribute("Detail"),
        DescriptionAttribute("Folder Name")]
        [XmlIgnore]
        public string Name
        {
            get { return _FolderName; }
            set 
            { 
                _FolderName = value;
                if (OnFolderNodeChanged != null)
                    OnFolderNodeChanged(this);
            }
        }
        [Browsable(false)]
        [XmlAttributeAttribute("FolderName")]
        public string FolderName { get { return _FolderName; } set { _FolderName = value; } } 

        private bool _UseRegistryInstallKey;
        [CategoryAttribute("Detail"),
        DescriptionAttribute("If TRUE create this folder iside the folder specified in the RegistryInstallKey Option. If this is a subfolder, this property is inherited from parent folder")]
        [XmlAttributeAttribute("UseRegistryInstallKey")]
        public bool UseRegistryInstallKey { get { return _UseRegistryInstallKey; } set  { _UseRegistryInstallKey = value; } }

        private string _DestinationRoot;
        [CategoryAttribute("Detail"),
        DescriptionAttribute("The destination root where the folder will be create. This parameter is ignored if UseRegistryInstallKey is TRUE. If this is a subfolder, this property is inherited from parent folder")]
        [XmlAttributeAttribute("DestinationRoot")]
        public string DestinationRoot { get { return _DestinationRoot; } set { _DestinationRoot = value; } }
    }

    [Serializable]
    public class PatcherProject
    {
        public PatcherProject() 
        {
        }

        public PatcherProject(bool Inizialize) 
        {
            _PO = new PatcherOptions();         
            _PO.Text = "Patcher";
            _PO.Title = "Patcher Title";
            _PO.IconFile = "Default";
            _PO.LogoFile = "Default";
            _PO.ConnectionString = @"Data Source=LOCALHOST\SQLEXPRESS;initial catalog=MyDB;integrated security=true;Connection Timeout=200;";
            _PO.CommandTimeout = 15;
            _PO.ServicesNames = string.Empty;
            _PO.RestartServices = false;
            _PO.PatchVersion = 0;
            _PO.PreviousPatchRequired = false;
            _PO.PreviousPatchVersionRequired = 0;
            _PO.RegistryInstallPath = @"Software\My Company";
            _PO.RegistryPathPatchVersion = @"Software\My Company";
            _PO.SilentModuleExecution = true;
            _PO.RegistryInstallKey = "InstallationPath";
            _PO.RegistryKeyPatchVersion = @"LatestPatchVersion";
            _PO.SourceFile = "Default";
            _PO.OutputFileName = "Patch.exe";
            _PO.OutputFolder = Application.StartupPath;
            _PO.AdditionalReferencedAssemblies = "System.dll; System.Data.dll; System.Deployment.dll; System.Drawing.dll; System.ServiceProcess.dll; System.Windows.Forms.dll; System.Xml.dll";
            _PO.CompilerOptions = "/optimize /target:winexe";
            _PO.MainClass = "DefaultGUI.Program";
            _PO.SaveSourceCode = false;
            _FolderNodes = new List<FolderNode>();
            _ResourceNodes = new List<ResourceNode>();
            _CommandEntries = new List<CommandEntry>();
        }

        string _ProjectName;
        [XmlAttributeAttribute("ProjectName")]
        public string ProjectName { get { return _ProjectName; } set { _ProjectName = value; } }
       
        PatcherOptions _PO;
        [XmlElement("PatcherOptions")]
        public PatcherOptions PO { get { return _PO; } set { _PO = value; } }

        List<FolderNode> _FolderNodes = new List<FolderNode>();
        [XmlArray("FolderNodes"), XmlArrayItem("FolderNode", typeof(FolderNode))]
        public List<FolderNode> FolderNodes { get { return _FolderNodes; } set { _FolderNodes = value; } }

        List<ResourceNode> _ResourceNodes = new List<ResourceNode>();
        [XmlArray("ResourceNodes"), XmlArrayItem("ResourceNode", typeof(ResourceNode))]
        public List<ResourceNode> ResourceNodes { get { return _ResourceNodes; } set { _ResourceNodes = value; } }

        List<CommandEntry> _CommandEntries = new List<CommandEntry>();
        [XmlArray("CommandEntries"), XmlArrayItem("CommandEntry", typeof(CommandEntry))]
        public List<CommandEntry> CommandEntries { get { return _CommandEntries; } set { _CommandEntries = value; } }
    }
}