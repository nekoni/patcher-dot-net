public static class PatcherHelper
{
    public const string STR_CONNECTIONSTRING = "";
    public const int INT_COMMANDTIMEOUT = 0;
    public const string STR_SERVICESNAMES = "";
    public const int INT_PREVIOUSPATCHVERSIONREQUIRED = 0;
    public const int INT_PATCHVERSION = 0;
    public const string STR_REGISTRYPATHPATCHVERSION = "";
    public const string STR_REGISTRYKEYPATCHVERSION = "";
    public const bool BOOL_SILENTMODULEEXECUTION = false;
    public const string STR_REGISTRYINSTALLPATH = "";
    public const string STR_REGISTRYINSTALLKEY = "";

    public static string GetHostedFileString(string ResourceName)
    {
        string str = string.Empty;
        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourceName))
        {
            StreamReader reader = new StreamReader(stream);
            str = reader.ReadToEnd();
            stream.Close();
        }
        return str;
    }

    public static Stream GetHostedFileStream(string ResourceName)
    {
        return Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourceName);
    }

    public static void CopyHostedFile(string ResourceName, string TargetPath)
    {
        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourceName))
        {
            BinaryReader reader = new BinaryReader(stream);
            if (File.Exists(TargetPath))
                File.Delete(TargetPath);
            FileStream fs = System.IO.File.Create(TargetPath);
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
    }

    public static int ExecuteHostedFile(string ResourceName, string DestinationName, string Arguments, string WorkingDirectory)
    {
        string tmpPath = Path.GetTempPath();
        string resourcePath = Path.Combine(tmpPath, DestinationName);
        int ExitCode = -1;
        try
        {
            CopyHostedFile(ResourceName, resourcePath);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            Process process = new Process();
            startInfo.Arguments = Arguments;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = BOOL_SILENTMODULEEXECUTION;
            startInfo.FileName = resourcePath;
            startInfo.WorkingDirectory = WorkingDirectory;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            ExitCode = process.ExitCode;
        }
        finally
        {
            if (File.Exists(resourcePath))
                File.Delete(resourcePath);
        }
        return ExitCode;
    }

    public static void BeginSQLTransaction(out SqlConnection sqlCon, out SqlTransaction sqlTran)
    {
        sqlCon = new SqlConnection(STR_CONNECTIONSTRING);
        sqlTran = null;

        if (sqlCon.State != ConnectionState.Closed)
            sqlCon.Close();

        sqlCon.Open();
        sqlTran = sqlCon.BeginTransaction();
    }

    public static void CommitSQLTransaction(SqlConnection sqlCon, SqlTransaction sqlTran)
    {
        if (sqlCon.State == ConnectionState.Open)
        {
            if (sqlTran != null)
            {
                sqlTran.Commit();
                if (sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();
            }
        }
    }

    public static void RollbackSQLTransaction(SqlConnection sqlCon, SqlTransaction sqlTran)
    {
        if (sqlCon.State == ConnectionState.Open)
        {
            if (sqlTran != null)
            {
                sqlTran.Rollback();
                if (sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();
            }
        }
    }

    public static void ExecuteSctipInTransaction(SqlConnection sqlCon, SqlTransaction sqlTran, string ScriptResourceName)
    {
        string Script = GetHostedFileString(ScriptResourceName);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandTimeout = INT_COMMANDTIMEOUT;
        cmd.Connection = sqlCon;
        cmd.Transaction = sqlTran;

        string[] SqlLine;
        Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        SqlLine = regex.Split(Script);

        int LineCount = 0;
        foreach (string line in SqlLine)
        {
            if (line.Length > 0)
            {
                cmd.CommandText = line;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error executing line: " + LineCount.ToString() + " - " + line, ex);
                }
            }
            LineCount++;
        }
    }

    public static void ExecuteScript(string ScriptResourceName)
    {
        SqlConnection sqlCon = new SqlConnection(STR_CONNECTIONSTRING);
        SqlTransaction sqlTran = null;
        try
        {
            string Script = GetHostedFileString(ScriptResourceName);

            if (sqlCon.State != ConnectionState.Closed)
                sqlCon.Close();

            sqlCon.Open();
            sqlTran = sqlCon.BeginTransaction();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = INT_COMMANDTIMEOUT;
            cmd.Connection = sqlCon;
            cmd.Transaction = sqlTran;

            string[] SqlLine;
            Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            SqlLine = regex.Split(Script);

            int LineCount = 0;
            foreach (string line in SqlLine)
            {
                if (line.Length > 0)
                {
                    cmd.CommandText = line;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Error executing line: " + LineCount.ToString() + " - " + line, ex);
                    }
                }
                LineCount++;
            }
            sqlTran.Commit();
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
                sqlCon.Close();
        }
    }

    public static List<ServiceController> SetupServices()
    {
        string[] strServices = STR_SERVICESNAMES.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        List<ServiceController> Services = new List<ServiceController>();
        foreach (string strService in strServices)
            Services.Add(new ServiceController(strService.TrimStart(" ".ToCharArray()).Trim()));

        return Services;
    }

    public static void StopServices(List<ServiceController> Services)
    {
        foreach (ServiceController Service in Services)
            if (Service.Status == ServiceControllerStatus.Running)
            {
                Service.Stop();
                Service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 1, 0));
            }
    }

    public static void StartServices(List<ServiceController> Services)
    {
        foreach (ServiceController Service in Services)
            if (Service.Status == ServiceControllerStatus.Stopped)
            {
                Service.Start();
                Service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
            }
    }

    public static void RestartServices()
    {
        List<ServiceController> Services = SetupServices();
        StopServices(Services);
        StartServices(Services);
    }

    public static void StartServices()
    {
        List<ServiceController> Services = SetupServices();
        StartServices(Services);
    }

    public static void StopServices()
    {
        List<ServiceController> Services = SetupServices();
        StopServices(Services);
    }

    public static string GetRegistryInstallationPath()
    {
        RegistryKey key = Registry.LocalMachine.OpenSubKey(STR_REGISTRYINSTALLPATH, true);
        if (key == null)
            throw new Exception("Registry path for install path not found! (" + STR_REGISTRYINSTALLPATH + ")");
        return (string)key.GetValue(STR_REGISTRYINSTALLKEY);
    }

    public static int GetRegistryPatchVersion()
    {
        RegistryKey key = Registry.LocalMachine.OpenSubKey(STR_REGISTRYPATHPATCHVERSION, true);
        if (key == null)
            throw new Exception("Registry path for patch version not found! (" + STR_REGISTRYPATHPATCHVERSION + ")");
        return (int)key.GetValue(STR_REGISTRYKEYPATCHVERSION);
    }

    public static void SetRegistryPatchVersion()
    {
        RegistryKey key = Registry.LocalMachine.OpenSubKey(STR_REGISTRYPATHPATCHVERSION, true);
        if (key == null)
            key = Registry.LocalMachine.CreateSubKey(STR_REGISTRYPATHPATCHVERSION);
        key.SetValue(STR_REGISTRYKEYPATCHVERSION, INT_PATCHVERSION);
    }
}
