using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ClassLibraryWS;

namespace WinService
{
    public partial class Service1 : ServiceBase
    {
       Logger logger;

        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            logger=new Logger();
            try
            {
                Thread loggerThread = new Thread(new ThreadStart(logger.Start));
                loggerThread.Start();
            }
            catch (Exception)
            {
                using (StreamWriter sw = new StreamWriter(@"D:\Study\LabsCSharp\logg.txt", true, System.Text.Encoding.Default))
                {
                    sw.WriteLine("Logger started with error,emergency mode is on");
                }
            }
        }

        protected override void OnStop()
        {
            logger.Stop(); 
            Thread.Sleep(1000);
        }
    }

    public class Logger
    {

            FileSystemWatcher watcher;
            EtlXmlJsonOption options;
            ParsOptions pmanager;
            bool enabled = true;
            public Logger()
            {
               
                    if (File.Exists(@"C:\Users\dende\source\repos\LabsCSharp\bin\Debug\netcoreapp3.1\appsettings.json"))
                    {
                        pmanager = new ParsOptions(@"C:\Users\dende\source\repos\LabsCSharp\bin\Debug\netcoreapp3.1\appsettings.json");
                    }
                    else
                    {
                        pmanager = new ParsOptions(
                            @"C:\Users\dende\source\repos\LabsCSharp\bin\Debug\netcoreapp3.1\cfg.xml");
                    }

                    options = pmanager.GetModel<EtlXmlJsonOption>();
                    if (options == null) return;

                    var path = options.pathes.Source1;
                    watcher = new FileSystemWatcher(path);
                    watcher.Created += Watcher_Created;
                    watcher.Filter = "*"+options.cryptOptions.Extension;
            }
          
            public bool IsEmp()
            {
                if (options == null) return true;
                else return false;
            }

            public void Start()
            {
                if (!IsEmp())
                {
                    watcher.EnableRaisingEvents = true;
                    while (enabled)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }

            public void Stop()
            {
                watcher.EnableRaisingEvents = false;
                enabled = false;
            }

            private void Watcher_Created(object sender, FileSystemEventArgs e)
            {
                var filePath = e.FullPath;
                RecordEntry(filePath);
            }


            public void RecordEntry(string filePath)
            {
                options.Do(filePath);
            }
    }
}