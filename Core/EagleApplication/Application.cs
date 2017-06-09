using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Eagle.Core.EagleApplication
{
    class Application
    {
        public string Path;
        public string ExePath;
        public string Name;
        public string PipePath;
        public BitmapImage Icon;
        internal List<ApplicationCMDKind> CmdList = new List<ApplicationCMDKind>();
        private bool is_set = false;

        public Application(string _name, string _fullpath, string _exepath, string _iconpath,string _pipepath, int passwd)
        {
            if (passwd == _name.Length + _fullpath.Length + _exepath.Length + _iconpath.Length + _pipepath.Length || passwd == 123)
            {
                is_set = true;
                this.Path = _fullpath;
                this.ExePath = _exepath;
                this.Name = _name;
                this.PipePath = _pipepath;
                System.Diagnostics.Debug.WriteLine(this.PipePath);
                this.Icon = new BitmapImage(new Uri(_iconpath));
            }
        }

        internal void Run()
        {
            if (is_set)
            {
                System.Diagnostics.Process.Start(ExePath);
                ApplicationManager.RunningApplicationList.Add(this);
                ApplicationManager.RunningApplicationDictionary.Add(this.Name, this);
            }
        }

        internal void SendCommand(ApplicationCMDKind cmdkind)
        {
            System.Diagnostics.Debug.WriteLine("发出命令：" + cmdkind);
            StreamWriter writer;
            switch (cmdkind)
            {
                case ApplicationCMDKind.Stop:
                    if (!File.Exists(this.PipePath + @"\Stop"))
                    {
                        writer = new StreamWriter(this.PipePath + @"\Stop");
                        writer.WriteLine("");
                        writer.Close();
                        writer.Dispose();
                    }
                    break;
                case ApplicationCMDKind.Topmost:
                    if (!File.Exists(this.PipePath + @"\Topmost"))
                    {
                        writer = new StreamWriter(this.PipePath + @"\Topmost");
                        writer.WriteLine("");
                        writer.Close();
                        writer.Dispose();
                    }
                    break;
                case ApplicationCMDKind.Untopmost:
                    if (!File.Exists(this.PipePath + @"\Untopmost"))
                    {
                        writer = new StreamWriter(this.PipePath + @"\Untopmost");
                        writer.WriteLine("");
                        writer.Close();
                        writer.Dispose();
                    }
                    break;
            }
        }
    }
}
