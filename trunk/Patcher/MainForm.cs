using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Patcher.Forms;
using System.IO;
using Patcher.Helper;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.CodeDom;


namespace Patcher
{
    public partial class MainForm : Form
    {
        #region Fields

        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent = null;
        private ProjectExplorer m_projectExplorer = null;
        private PropertyWindow m_propertyWindow = null;
        private OutputWindow m_outputWindow = null;
        private ResourcesHelper m_resourcesHelper = new ResourcesHelper();
        private static MainForm m_Instance;

        public static MainForm Instance
        {
            get { return m_Instance; }
            set { m_Instance = value; }
        }

        #endregion

        #region Constructor

        public MainForm()
        {
            m_Instance = this;

            InitializeComponent();

            InizializeDockWindows();
        }

        private void InizializeDockWindows()
        {
            m_projectExplorer = new ProjectExplorer();
            m_propertyWindow = new PropertyWindow();
            m_outputWindow = new OutputWindow();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            dockPanel.ActiveDocumentChanged += new EventHandler(dockPanel_ActiveDocumentChanged);
        }

        #endregion

        #region Methods

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(ProjectExplorer).ToString())
                return m_projectExplorer;
            else if (persistString == typeof(PropertyWindow).ToString())
                return m_propertyWindow;
            else if (persistString == typeof(OutputWindow).ToString())
                return m_outputWindow;
            else
            {
                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                if (parsedStrings[0] != typeof(ProjectForm).ToString())
                    return null;

                ProjectForm projectForm = new ProjectForm();
                if (parsedStrings[1] != string.Empty)
                    projectForm.FileName = parsedStrings[1];
                if (parsedStrings[2] != string.Empty)
                    projectForm.Text = parsedStrings[2];

                return projectForm;
            }
        }

        private void CreateDefaultLayout()
        {
            dockPanel.SuspendLayout(true);

            m_projectExplorer.Show(dockPanel, DockState.DockLeft);
            m_propertyWindow.Show(dockPanel, DockState.DockRight);
            m_outputWindow.Show(dockPanel, DockState.DockBottom);

            dockPanel.ResumeLayout(true, true);
        }

        private void CloseAllProjects()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                IDockContent[] documents = dockPanel.DocumentsToArray();
                foreach (IDockContent content in documents)
                    content.DockHandler.Close();
            }
        }

        private IDockContent FindProject(string text)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    if (form.Text == text)
                        return form as IDockContent;

                return null;
            }
            else
            {
                foreach (IDockContent content in dockPanel.Documents)
                    if (content.DockHandler.TabText == text)
                        return content;

                return null;
            }
        }

        private bool ProjectAlreadyOpen(string FileName)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    if (((ProjectForm)(form)).FileName == FileName)
                        return true;

                return false;
            }
            else
            {
                foreach (IDockContent content in dockPanel.Documents)
                    if (((ProjectForm)(content.DockHandler.Form)).FileName == FileName)
                        return true;

                return false;
            }
        }

        private ProjectForm CreateNewProject()
        {
            ProjectForm projectForm = new ProjectForm();
            projectForm.OnProjectNameChanged += new ProjectForm.ProjectNameChangedEventHandeler(projectForm_OnProjectNameChanged);

            int count = 1;
            string text = "NewProject" + count.ToString();
            while (FindProject(text) != null)
            {
                count++;
                text = "NewProject" + count.ToString();
            }
            projectForm.Text = text;
            return projectForm;
        }
       
        private void OpenProject(string FileName)
        {
            ProjectForm projectForm = new ProjectForm(FileName);
            projectForm.OnProjectNameChanged += new ProjectForm.ProjectNameChangedEventHandeler(projectForm_OnProjectNameChanged);
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                projectForm.MdiParent = this;
                projectForm.Show();
            }
            else
                projectForm.Show(dockPanel);
        }

        private void CommandCreateNewProject()
        {
            ProjectForm projectForm = CreateNewProject();
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                projectForm.MdiParent = this;
                projectForm.Show();
            }
            else
                projectForm.Show(dockPanel);
        }

        private void CommandSaveProject()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                ((ProjectForm)(ActiveMdiChild)).SaveProject();
            else if (dockPanel.ActiveDocument != null)
                ((ProjectForm)(dockPanel.ActiveDocument.DockHandler.Form)).SaveProject();
        }

        private void CommandSaveAsProject()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                ((ProjectForm)(ActiveMdiChild)).SaveAsProject();
            else if (dockPanel.ActiveDocument != null)
                ((ProjectForm)(dockPanel.ActiveDocument.DockHandler.Form)).SaveAsProject();
        }

        private void CommandSaveAllProjects()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    ((ProjectForm)form).SaveProject();
            }
            else
            {
                IDockContent[] documents = dockPanel.DocumentsToArray();
                foreach (IDockContent content in documents)
                    ((ProjectForm)(content.DockHandler.Form)).SaveProject();
            }
        }

        private void CommandOpenProject()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = ProjectForm.Filter;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (!ProjectAlreadyOpen(ofd.FileName))
                    {
                        OpenProject(ofd.FileName);
                    }
                    else
                    {
                        MessageBox.Show(m_resourcesHelper.GetResourceString("err_item_already_exist"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void CompileCode(string sourceFile, List<string> ReferencedAssemblies, string CompilerOptions, string MainClass, List<string> LinkedResources, List<string> EmbeddedResources, string exeFile)
        {
            CodeDomProvider provider = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();

            // Generate an executable instead of 
            // a class library.
            cp.GenerateExecutable = true;

            // Set the assembly file name to generate.
            cp.OutputAssembly = exeFile;

            // Generate debug information.
            cp.IncludeDebugInformation = false;

            // Add assembly reference.
            foreach(string ReferencedAssemby in ReferencedAssemblies)
                cp.ReferencedAssemblies.Add(ReferencedAssemby);

            // Save the assembly as a physical file.
            cp.GenerateInMemory = false;

            // Set the level at which the compiler 
            // should start displaying warnings.
            cp.WarningLevel = 3;

            // Set whether to treat all warnings as errors.
            cp.TreatWarningsAsErrors = false;

            // Set compiler argument to optimize output.
            cp.CompilerOptions = CompilerOptions;

            // Set a temporary files collection.
            // The TempFileCollection stores the temporary files
            // generated during a build in the current directory,
            // and does not delete them after compilation.
            cp.TempFiles = new TempFileCollection(Path.GetTempPath(), true);

            if (provider.Supports(GeneratorSupport.EntryPointMethod))
            {
                // Specify the class that contains 
                // the main method of the executable.
                cp.MainClass = MainClass;
            }

            if (provider.Supports(GeneratorSupport.Resources))
            {
                // Set the embedded resource file of the assembly.
                // This is useful for culture-neutral resources, 
                // or default (fallback) resources.
                foreach (string EmbeddedResource in EmbeddedResources)
                    cp.EmbeddedResources.Add(EmbeddedResource);
                // Set the linked resource reference files of the assembly.
                // These resources are included in separate assembly files,
                // typically localized for a specific language and culture.
                foreach (string LinkedResource in LinkedResources)
                    cp.LinkedResources.Add(LinkedResource);
            }

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);
            TextWriter OutputMessage = new StringWriter();
            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                OutputMessage.WriteLine("Errors building {0} into {1}",
                    sourceFile, cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    OutputMessage.WriteLine("  {0}", ce.ToString());
                    OutputMessage.WriteLine();
                }
            }
            else
            {
                OutputMessage.WriteLine("Source {0} built into {1} successfully.",
                    sourceFile, cr.PathToAssembly);
            }

            m_outputWindow.WriteLine(OutputMessage.ToString());
        }

        List<string> Split(string InputString, string Separator)
        {
            List<string> StrList = new List<string>();
            if(InputString != null)
            {
                string[] Items = InputString.Split(Separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string Item in Items)
                    StrList.Add(Item.TrimStart(" ".ToCharArray()).Trim());
            }
            return StrList;
        }

        string CopyHostedFile(string ResourceName, string TargetPath)
        {
            string OutputFile = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + "." + ResourceName))
            {
                BinaryReader reader = new BinaryReader(stream);
                OutputFile = Path.Combine(TargetPath, ResourceName);
                if (File.Exists(OutputFile))
                    File.Delete(OutputFile);
                FileStream fs = System.IO.File.Create(OutputFile);
                BinaryWriter writer = new BinaryWriter(fs);
                byte b;
                do
                {
                    b = reader.ReadByte();
                    writer.Write(b);
                }
                while (reader.BaseStream.Position != reader.BaseStream.Length);

                writer.Flush();
                writer.Close();
                reader.Close();
                fs.Close();
                stream.Close();
            }
            return OutputFile;
        }

        private string UpdateSourceCodeFile(string SouceCodeFile, PatcherProject CurrentProject)
        {
            string strFileName = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());

            string Namespaces =
            "using Microsoft.Win32;" + Environment.NewLine +
            "using System.IO;" + Environment.NewLine +
            "using System.Reflection;" + Environment.NewLine +
            "using System.Diagnostics;" + Environment.NewLine +
            "using System.Data.SqlClient;" + Environment.NewLine +
            "using System.Data;" + Environment.NewLine +
            "using System.Text.RegularExpressions;" + Environment.NewLine +
            "using System.ServiceProcess;" + Environment.NewLine +
            "using System.Threading;" + Environment.NewLine; 

            string Content = string.Empty;
            int iCount = 0;
            using (TextReader tr = new StreamReader(SouceCodeFile))
            {
                Content = tr.ReadToEnd().Trim();
                string[] nps = Namespaces.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string np in nps)
                    Content = Content.Replace(np, "");
                Content = Namespaces + Content;
                Content = Content.TrimEnd("}".ToCharArray());
            }

            string strPatcherHelper = CopyHostedFile("Resources.PatcherHelper.cs", Path.GetTempPath());
            using (TextReader tr = new StreamReader(strPatcherHelper))
            {
                Content+=tr.ReadToEnd() + Environment.NewLine + "}";
            }
            if (File.Exists(strPatcherHelper))
                File.Delete(strPatcherHelper);

            int iTotalOperations = 0;
            //genero la funzione principale con:
            using (TextWriter tw = new StringWriter())
            {
                PatcherOptions po = CurrentProject.PO;

                tw.WriteLine("List<string> ListBackupFiles = new List<string>();");
                tw.WriteLine("SqlConnection sqlCon = null;");
                tw.WriteLine("SqlTransaction sqlTran = null;");
                tw.WriteLine("LogMessage(Color.Green, \"Installation started..\");");
                tw.WriteLine("try");
                tw.WriteLine("{");
                if (po.SavePatchVersion)
                {
                    tw.WriteLine("int iCurrentInstalledPatch = {0};", po.PatchVersion);
                    tw.WriteLine("try");
                    tw.WriteLine("{");
                    tw.WriteLine("iCurrentInstalledPatch = PatcherHelper.GetRegistryPatchVersion();");
                    tw.WriteLine("}");
                    tw.WriteLine("catch { iCurrentInstalledPatch = -1; }");
                    tw.WriteLine("if (iCurrentInstalledPatch == {0}) throw new Exception(\"Patch n {0} was already installed\");", po.PatchVersion);
                }

                if(po.PreviousPatchRequired)
                {
                    tw.WriteLine("int iPreviousPatchVersion = 0;");
                    tw.WriteLine("try");
                    tw.WriteLine("{");
                    tw.WriteLine("iPreviousPatchVersion = PatcherHelper.GetRegistryPatchVersion();");
                    tw.WriteLine("}");
                    tw.WriteLine("catch { iPreviousPatchVersion = -1; }");
                    tw.WriteLine("if(iPreviousPatchVersion != {0}) throw new Exception(\"Cannot install this patch because patch version {0} was not installed\");", po.PreviousPatchVersionRequired.ToString());
                }

                if (po.RestartServices)
                {
                    tw.WriteLine("LogMessage(Color.Blue, \"Stopping services..\");");
                    tw.WriteLine("PatcherHelper.StopServices();");
                    tw.WriteLine("Thread.Sleep(" + po.ServiceWaitAfterStop.ToString() + ");");
                    tw.WriteLine("ReportProgress();");
                    iTotalOperations++;
                }

                if ((CurrentProject.FolderNodes.Count + CurrentProject.ResourceNodes.Count) > 0)
                {
                    if (po.InstallationPath == null)
                        tw.WriteLine("string strInstallationRoot = PatcherHelper.GetRegistryInstallationPath();");
                    else
                        tw.WriteLine("string strInstallationRoot = @\"{0}\".TrimEnd(@\"\\\".ToCharArray());", po.InstallationPath);

                    foreach (FolderNode fn in CurrentProject.FolderNodes)
                    {
                        if (fn.UseRegistryInstallKey)
                            tw.WriteLine("string strDir{0} = @\"{1}\".Replace(\"{2}\", strInstallationRoot);", iCount.ToString(), fn.NodePath, CurrentProject.ProjectName);
                        else
                        {
                            string strDestinationRoot = (fn.DestinationRoot == null) ? po.InstallationPath : fn.DestinationRoot;
                            tw.WriteLine("string strDir{0} = @\"{1}\".Replace(\"{2}\", @\"{3}\");", iCount.ToString(), fn.NodePath, CurrentProject.ProjectName, strDestinationRoot.TrimEnd(new char[] { Path.DirectorySeparatorChar }));
                        }
                        tw.WriteLine("LogMessage(Color.Blue, \"Creating \" + strDir{0});", iCount.ToString());
                        tw.WriteLine("if(!Directory.Exists(strDir{0})) Directory.CreateDirectory(strDir{0});", iCount.ToString());
                        tw.WriteLine("ReportProgress();");
                        iCount++;
                    }
                    iTotalOperations += iCount;

                    iCount = 0;
                    foreach (ResourceNode rn in CurrentProject.ResourceNodes)
                    {
                        tw.WriteLine("string strRes{0} = @\"{1}\".Replace(\"{2}\", strInstallationRoot);", iCount.ToString(), rn.NodePath, CurrentProject.ProjectName);
                        tw.WriteLine("LogMessage(Color.Blue, \"Coping \" + strRes{0});", iCount.ToString());
                        tw.WriteLine("if(File.Exists(strRes{0}))", iCount.ToString());
                        tw.WriteLine("{");
                        tw.WriteLine("File.Copy(strRes{0}, strRes{0} + \".bkp\", true);", iCount.ToString());
                        tw.WriteLine("ListBackupFiles.Add(strRes{0} + \".bkp\");", iCount.ToString());
                        tw.WriteLine("}");
                        tw.WriteLine("PatcherHelper.CopyHostedFile(\"{0}\", strRes{1});", Path.GetFileName(rn.ResourceTempFullPath), iCount.ToString());
                        tw.WriteLine("ReportProgress();");
                        iCount++;
                    }
                    iTotalOperations += iCount;
                }

                foreach (CommandEntry ce in CurrentProject.CommandEntries)
                {
                    if (ce.CmdType == CmdTypes.SQLScript)
                    {
                        tw.WriteLine("LogMessage(Color.Blue, \"Starting SQL transaction..\");");
                        tw.WriteLine("PatcherHelper.BeginSQLTransaction(out sqlCon, out sqlTran);");
                        tw.WriteLine("ReportProgress();");
                        iTotalOperations++;
                        iTotalOperations++;
                        break;   
                    }
                }

                iCount = 0;
                foreach (CommandEntry ce in CurrentProject.CommandEntries)
                {
                    tw.WriteLine("LogMessage(Color.Blue, \"Executing \" + \"{0}\");", Path.GetFileName(ce.ResourceFullPath));
                    if (ce.CmdType == CmdTypes.SQLScript)
                        tw.WriteLine("PatcherHelper.ExecuteSctipInTransaction(sqlCon, sqlTran, \"{0}\");", Path.GetFileName(ce.ResourceTempFullPath));
                    else if(ce.CmdType == CmdTypes.ExecutableModule || ce.CmdType == CmdTypes.BATScript)
                        tw.WriteLine("if(PatcherHelper.ExecuteHostedFile(\"{0}\", \"{1}\",@\"{2}\", @\"{3}\") != 0) throw new Exception(\"Error executing module {1}\");", Path.GetFileName(ce.ResourceTempFullPath), Path.GetFileName(ce.ResourceFullPath), ce.CommandArguments, ce.WorkingFolder);
                    tw.WriteLine("ReportProgress();");
                    iCount++;
                }
                iTotalOperations+= iCount;

                if (po.RestartServices)
                {
                    tw.WriteLine("LogMessage(Color.Blue, \"Starting services..\");");
                    tw.WriteLine("PatcherHelper.StartServices();");
                    tw.WriteLine("ReportProgress();");
                    iTotalOperations++;
                }
                tw.WriteLine("if(sqlCon != null)");
                tw.WriteLine("{");
                tw.WriteLine("PatcherHelper.CommitSQLTransaction(sqlCon, sqlTran);");
                tw.WriteLine("LogMessage(Color.Blue, \"SQL transaction committed.\");");
                tw.WriteLine("ReportProgress();");
                tw.WriteLine("}");
                
                if(po.SavePatchVersion)
                {
                    tw.WriteLine("PatcherHelper.SetRegistryPatchVersion();");
                }

                tw.WriteLine("LogMessage(Color.Green, \"Installation completed successfully.\");");
                
                tw.WriteLine("}");
                tw.WriteLine("catch(Exception ex)");
                tw.WriteLine("{");
                tw.WriteLine("LogMessage(Color.Red, \"An error has occurred.\");");
                tw.WriteLine("LogMessage(Color.Brown, \"Rollback installation..\");");
                tw.WriteLine("foreach(string BackupFile in ListBackupFiles)");
                tw.WriteLine("File.Copy(BackupFile, BackupFile.TrimEnd(\".bkp\".ToCharArray()), true);");
                tw.WriteLine("if(sqlCon != null)");
                tw.WriteLine("{");
                tw.WriteLine("PatcherHelper.RollbackSQLTransaction(sqlCon, sqlTran);");
                tw.WriteLine("LogMessage(Color.Brown, \"SQL transaction rollback executed.\");");
                tw.WriteLine("}");
                if (po.RestartServices)
                {
                    tw.WriteLine("LogMessage(Color.Brown, \"Starting services..\");");
                    tw.WriteLine("PatcherHelper.StartServices();");
                }
                tw.WriteLine("LogMessage(Color.Brown, \"Rollback installation complete..\");");
                tw.WriteLine("LogMessage(Color.Red, \"Installation failed.\");");
                tw.WriteLine("throw ex;");
                tw.WriteLine("}");
                tw.WriteLine("finally");
                tw.WriteLine("{");
                tw.WriteLine("foreach(string BackupFile in ListBackupFiles)");
                tw.WriteLine("File.Delete(BackupFile);");
                tw.WriteLine("}");
                tw.Flush(); 
                              
                Content = Content.Replace("ENTRYPOINT", tw.ToString());
                Content = Content.Replace("const string STR_CONNECTIONSTRING = \"\"", "const string STR_CONNECTIONSTRING = @\"" + po.ConnectionString + "\"");
                Content = Content.Replace("const int INT_COMMANDTIMEOUT = 0", "const int INT_COMMANDTIMEOUT = " + po.CommandTimeout.ToString());
                Content = Content.Replace("const string STR_SERVICESNAMES = \"\"", "const string STR_SERVICESNAMES = \"" + po.ServicesNames + "\"");
                Content = Content.Replace("const bool BOOL_SILENTMODULEEXECUTION = false", "const bool BOOL_SILENTMODULEEXECUTION = " + po.SilentModuleExecution.ToString().ToLower());
                Content = Content.Replace("const string STR_REGISTRYINSTALLPATH = \"\"", "const string STR_REGISTRYINSTALLPATH = @\"" + po.RegistryInstallPath + "\"");
                Content = Content.Replace("const string STR_REGISTRYINSTALLKEY = \"\"", "const string STR_REGISTRYINSTALLKEY = @\"" + po.RegistryInstallKey + "\"");
                Content = Content.Replace("string STR_TEXT = \"\"", "string STR_TEXT = \"" + po.Text + "\"");
                Content = Content.Replace("string STR_TITLE = \"\"", "string STR_TITLE = \"" + po.Title + "\"");
                Content = Content.Replace("int INT_STEPSNUMBER = 0", "int INT_STEPSNUMBER = " + iTotalOperations.ToString());
                string LogoFile = (po.LogoFile.ToUpper() == "DEFAULT" ? "DefaultLogo.jpg" : Path.GetFileName(po.LogoFile));
                Content = Content.Replace("Bitmap LOGO_FILE = null", "Bitmap LOGO_FILE = new Bitmap(PatcherHelper.GetHostedFileStream(\"" + LogoFile + "\"))");
                string IconFile = (po.IconFile.ToUpper() == "DEFAULT" ? "DefaultIcon.ico" : Path.GetFileName(po.IconFile));
                Content = Content.Replace("Icon ICON_FILE = null", "Icon ICON_FILE = new Icon(PatcherHelper.GetHostedFileStream(\"" + IconFile + "\"))");
                //gestione della versione
                Content = Content.Replace("const int INT_PREVIOUSPATCHVERSIONREQUIRED = 0", "const int INT_PREVIOUSPATCHVERSIONREQUIRED = " + po.PreviousPatchVersionRequired.ToString());
                Content = Content.Replace("const int INT_PATCHVERSION = 0", "const int INT_PATCHVERSION = " + po.PatchVersion.ToString());
                Content = Content.Replace("const string STR_REGISTRYPATHPATCHVERSION = \"\"", "const string STR_REGISTRYPATHPATCHVERSION = @\"" + po.RegistryPathPatchVersion + "\"");
                Content = Content.Replace("const string STR_REGISTRYKEYPATCHVERSION = \"\"", "const string STR_REGISTRYKEYPATCHVERSION = @\"" + po.RegistryKeyPatchVersion + "\"");       
            }

            using (TextWriter tw = new StreamWriter(strFileName))
            {
                tw.Write(Content);
            }

            return strFileName;
        }

        #endregion

        #region Public Properties

        public ResourcesHelper resourceHelper
        {
            get { return m_resourcesHelper; }
        }

        public PropertyWindow propertyWindow
        {
            get { return m_propertyWindow; }
        }

        public ProjectExplorer projectExplorer
        {
            get { return m_projectExplorer; }
        }

        public OutputWindow outputWindow
        {
            get { return m_outputWindow; }
        }

        #endregion

        #region Local Events

        private void frmMain_Load(object sender, EventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");

            if (File.Exists(configFile))
                dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
            else
            {
                CreateDefaultLayout();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (m_bSaveLayout)
                dockPanel.SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
        }

        private void menuItemProjectExplorer_Click(object sender, EventArgs e)
        {
            m_projectExplorer.Show();
        }

        private void menuItemPropertyWindow_Click(object sender, EventArgs e)
        {
            m_propertyWindow.Show();
        }

        private void menuItemOutputWindow_Click(object sender, EventArgs e)
        {
            m_outputWindow.Show();
        }

        void dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if(dockPanel.ActiveDocument != null)
                m_projectExplorer.SelectProject(dockPanel.ActiveDocument.DockHandler.Form as ProjectForm);
        }

        private void menuItemNew_Click(object sender, EventArgs e)
        {
            CommandCreateNewProject();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            CommandCreateNewProject();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            m_bSaveLayout = true;
            this.Close();
        }

        private void exitWithoutSavingLayout_Click(object sender, EventArgs e)
        {
            m_bSaveLayout = false;
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSaveProject();
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSaveAllProjects();
        }

        private void menuItemClose_Click(object sender, EventArgs e)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                ActiveMdiChild.Close();
            else if (dockPanel.ActiveDocument != null)
                dockPanel.ActiveDocument.DockHandler.Close();
            m_projectExplorer.SelectProject(null);
        }

        private void menuItemCloseAll_Click(object sender, EventArgs e)
        {
            CloseAllProjects();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandSaveAsProject();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            CommandSaveProject();
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            CommandOpenProject();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            CommandOpenProject();
        }

        void projectForm_OnProjectNameChanged(string Name)
        {
            m_projectExplorer.SetProjectName(Name);
        }

        private void BuildToolStripButton_Click(object sender, EventArgs e)
        {
            ProjectForm CurrentProjectForm = null;
            PatcherProject CurrentProject = null;

            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                CurrentProjectForm = ((ProjectForm)(ActiveMdiChild));
            else if (dockPanel.ActiveDocument != null)
                CurrentProjectForm = ((ProjectForm)(dockPanel.ActiveDocument.DockHandler.Form));

            m_outputWindow.Clear();
            CurrentProjectForm.UpdateCommandEntries();
            CurrentProject = CurrentProjectForm.patcherProject;
            Directory.SetCurrentDirectory(Path.GetDirectoryName(CurrentProjectForm.FileName));

            if (CurrentProject != null)
            {
                string strLogoFile = string.Empty;
                string strIconFile = string.Empty;
                string strSourceFile = string.Empty;
                PatcherOptions po = CurrentProject.PO;
                try
                {
                    List<string> ReferencedAssemblies = Split(po.AdditionalReferencedAssemblies, ";");
                    List<string> EmbeddedResources = Split(po.AdditionalEmbeddedResources, ";");
                    List<string> LinkedResources = Split(po.AdditionalLinkedResources, ";");

                    if (po.IconFile.ToUpper() != "DEFAULT")
                        EmbeddedResources.Add(po.IconFile);
                    else
                    {
                        strIconFile = Path.Combine(Path.GetTempPath(), "DefaultIcon.ico");
                        using (FileStream fs = new FileStream(strIconFile, FileMode.Create, FileAccess.Write))
                        {
                            global::Patcher.Properties.Resources.install.Save(fs);
                            EmbeddedResources.Add(strIconFile);
                        }
                    }

                    if (po.LogoFile.ToUpper() != "DEFAULT")
                        EmbeddedResources.Add(po.LogoFile);
                    else
                    {
                        strLogoFile = Path.Combine(Path.GetTempPath(), "DefaultLogo.jpg");
                        global::Patcher.Properties.Resources.Patcher.Save(strLogoFile);
                        EmbeddedResources.Add(strLogoFile);
                    }

                    if (po.SourceFile.ToUpper() != "DEFAULT")
                        strSourceFile = UpdateSourceCodeFile(po.SourceFile, CurrentProject);
                    else
                    {
                        strSourceFile = CopyHostedFile("Resources.DefaultGUI.cs", Path.GetTempPath());
                        string strFileName = strSourceFile;
                        strSourceFile = UpdateSourceCodeFile(strSourceFile, CurrentProject);
                        if(File.Exists(strFileName))
                            File.Delete(strFileName);
                    }

                    foreach (ResourceNode rn in CurrentProject.ResourceNodes)
                        EmbeddedResources.Add(rn.ResourceTempFullPath);

                    foreach (CommandEntry ce in CurrentProject.CommandEntries)
                        EmbeddedResources.Add(ce.ResourceTempFullPath);

                    CompileCode(strSourceFile, ReferencedAssemblies, po.CompilerOptions, po.MainClass, LinkedResources, EmbeddedResources, Path.Combine(po.OutputFolder, po.OutputFileName));
                }
                catch (Exception ex)
                {
                    m_outputWindow.WriteLine(ex.ToString());
                }
                finally
                {
                    if (strIconFile != string.Empty)
                        if (File.Exists(strIconFile))
                            File.Delete(strIconFile);

                    if (strLogoFile != string.Empty)
                        if (File.Exists(strLogoFile))
                            File.Delete(strLogoFile);

                    if (strSourceFile != string.Empty)
                        if (File.Exists(strSourceFile))
                        {
                            if (po.SaveSourceCode)
                                File.Copy(strSourceFile, Path.Combine(po.OutputFolder, Path.GetFileNameWithoutExtension(po.OutputFileName) + ".cs"), true);
                            File.Delete(strSourceFile);
                        }
                }
            }
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            AboutDialog ad = new AboutDialog();
            ad.ShowDialog();
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            AboutDialog ad = new AboutDialog();
            ad.ShowDialog();
        }

        private void toolStripMenuItemGenSQL_Click(object sender, EventArgs e)
        {
            GenSQLDialog dlg = new GenSQLDialog();
            dlg.ShowDialog();
        }

        #endregion         
    }
}