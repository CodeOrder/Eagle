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
        public BitmapImage Icon;
        private bool is_set = false;

        public Application(string _name, string _fullpath, string _exepath, string _iconpath, int passwd)
        {
            if (passwd == _name.Length + _fullpath.Length + _exepath.Length + _iconpath.Length || passwd == 123)
            {
                is_set = true;
                this.Path = _fullpath;
                this.ExePath = _exepath;
                this.Name = _name;
                this.Icon = new BitmapImage(new Uri(_iconpath));
            }
        }

        internal void Run()
        {
            if (is_set)
            {
                System.Diagnostics.Process.Start(ExePath);
            }
        }


    }
}
