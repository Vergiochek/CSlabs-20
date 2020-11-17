using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryMonitorService
{
    public partial class DirectoryMonitorService : ServiceBase
    {
        FileSystemWatcher fileSystemWatcher;
        XORCipher cipher = new XORCipher();
        private string compressFile;
        private string sourceFile;
        private string targetPath;
        private string fileText;

        public DirectoryMonitorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //EventLog.WriteEntry("Directory Monitor Service Started");
            fileSystemWatcher = new FileSystemWatcher("E:\\SourceDirectory")
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true
            };

            fileSystemWatcher.Created += DirectoryChanged;
            fileSystemWatcher.Deleted += DirectoryChanged;
            fileSystemWatcher.Renamed += FileRenamed;
            fileSystemWatcher.Changed += DirectoryChanged;
        }

        private void FileRenamed(object sender, RenamedEventArgs e)
        {
            sourceFile = e.FullPath;
            compressFile = e.FullPath.Substring(0, e.FullPath.IndexOf('.')) + ".rar"; // E:\SourceDirectory\text.rar

            EncryptFileText(sourceFile);

            Archive.Compress(sourceFile, compressFile);
            File.Delete(sourceFile);

            targetPath = CreateDataArchieve() + compressFile.Substring(compressFile.IndexOf('y') + 2);
            File.Copy(compressFile, targetPath);
            Archive.Decompress(targetPath, targetPath.Substring(0, targetPath.IndexOf(".rar")) + ".txt");
            DecryptFileText(targetPath.Substring(0, targetPath.IndexOf(".rar")) + ".txt");
            File.Delete(targetPath);
        }

        public string CreateDataArchieve()
        {
            DateTime date = DateTime.Now;
            string filepath = "E:\\TargetDirectory\\" +  "archive " + date.ToString("D");

            if(!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            return filepath + "\\";
        }

        public void EncryptFileText(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                fileText = reader.ReadToEnd();
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                fileText = cipher.Encrypt(fileText, "hey");
                writer.WriteLine(fileText);
            }
        }

        public void DecryptFileText(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                fileText = cipher.Decrypt(fileText, "hey");
                writer.WriteLine(fileText);
            }
        }

        private void DirectoryChanged(object sender, FileSystemEventArgs e)
        {           
            var msg = $"{e.ChangeType} - {e.FullPath} - {DateTime.Now}{Environment.NewLine}";
            File.AppendAllText(@"E:\TargetDirectory\log.txt", msg);
        }

        protected override void OnStop()
        {
        }
    }
}
