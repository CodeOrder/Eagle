using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace Eagle.Core.EagleApplication
{
    class ApplicationManager
    {
        //internal static Dictionary<string, Application> RunningApplicationDictionary = new Dictionary<string,Application>();
        //internal static List<Application> RunningApplicationList = new List<Application>();
        internal static Dictionary<string, Application> ApplicationDictionary = new Dictionary<string,Application>();
        internal static List<Application> ApplicationList = new List<Application>();
        internal static Dictionary<Application, ApplicationIcon> ApplicationIconDictionary = new Dictionary<Application, ApplicationIcon>();
        internal static List<ApplicationIcon> ApplicationIconList = new List<ApplicationIcon>();

        internal static Application GetAppByName(string name)
        {
            return ApplicationDictionary[name];
        }

        internal static ApplicationIcon GetIconByApp(Application app)
        {
            return ApplicationIconDictionary[app];
        }

        // Name Fullpath Exename Iconpath
        internal static void LoadApplications()
        {
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "\\Data\\Application");
            FileInfo[] files = dir.GetFiles();

            string[] lines;
            Application app;
            int passwd = 0;
            foreach (FileInfo file in files)
            {
                lines = File.ReadAllLines(file.FullName, Encoding.UTF8);
                app = new Application(lines[0], lines[1], lines[2], lines[3],123);
                ApplicationList.Add(app);
                ApplicationDictionary.Add(lines[0], app);
            }

            foreach(Application a in ApplicationList)
                Debug.WriteLine(a.Name + " In list");
        }
        /// <summary>
        /// 安装并注册一个标准的Eagle Application，返回这个Application
        /// name.eag：程序名，可执行文件名（无后缀），图标文件名（加后缀）
        /// </summary>
        /// <param name="INSFilepath">是一个后缀为.eag的文件夹</param>
        /// <returns></returns>
        internal static EagleApplication.Application InstallApplication(string INSFilepath)
        {
            FileInfo INSFile = new FileInfo(INSFilepath);
            Directory.CreateDirectory(Environment.CurrentDirectory + @"\Application\" + INSFile.Name);   //在Eagle目录创建程序文件夹
            DirectoryInfo INSdir = new DirectoryInfo(INSFile.DirectoryName);
            CopyDirectory(INSdir.FullName, Environment.CurrentDirectory + @"\Application\" + INSFile.Name);


            string[] infolines = File.ReadAllLines(INSFile.FullName);                                 //注册程序信息
            FileStream filestream = new FileStream(Environment.CurrentDirectory + "\\Data\\Application\\" + INSFile.Name + ".dat", FileMode.Create);
            StreamWriter writer = new StreamWriter(filestream);
            writer.WriteLine(infolines[0]);
            writer.WriteLine(Environment.CurrentDirectory + @"\Application\" + INSFile.Name);
            writer.WriteLine(Environment.CurrentDirectory + @"\Application\" + INSFile.Name + @"\" + infolines[1] + ".exe");
            writer.WriteLine(Environment.CurrentDirectory + @"\Application\" + INSFile.Name + @"\" + infolines[2]);
            writer.Close();
            writer.Dispose();

            EagleApplication.Application app = new EagleApplication.Application(infolines[0],
                Environment.CurrentDirectory + @"\Application\" + INSFile.Name,
                Environment.CurrentDirectory + @"\Application\" + INSFile.Name + @"\" + infolines[1] + ".exe",
                Environment.CurrentDirectory + @"\Application\" + INSFile.Name + @"\" + infolines[2], 
                123);
            ApplicationManager.ApplicationList.Add(app);
            ApplicationManager.ApplicationDictionary.Add(infolines[0], app);
            return app;
        }

        private static void CopyDirectory(string sourceDirName, string destDirName)
        {
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
            }
            if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                destDirName = destDirName + Path.DirectorySeparatorChar;

            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                if (File.Exists(destDirName + Path.GetFileName(file)))
                    continue;
                File.Copy(file, destDirName + Path.GetFileName(file), true);
                File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
            }

            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, destDirName + Path.GetFileName(dir));
            }
        }         

        internal static void UninstallApplication(string appname)
        {
            Directory.Delete(Environment.CurrentDirectory + "\\Application\\" + appname);
            ApplicationList.Remove(GetAppByName(appname));
            ApplicationDictionary.Remove(appname);
            File.Delete(Environment.CurrentDirectory + "\\Data\\Application\\" + appname + ".dat");
        }

        /// <summary>
        /// 储存的文件格式：ContentApp.name Margin(left,top)
        /// </summary>
        /// <param name="icon">要储存的图标</param>
        internal static void SaveIconInfo(ApplicationIcon icon)
        {
            FileStream file = new FileStream(Environment.CurrentDirectory + @"\Data\Application\Icon\" + icon.ContentApp.Name + ".ion", FileMode.Create);
            StreamWriter writer = new StreamWriter(file, Encoding.UTF8);
            writer.WriteLine(icon.ContentApp.Name);
            writer.WriteLine(icon.Margin.Left + "," + icon.Margin.Top);
            writer.Close();
            writer.Dispose();
        }

        internal static void LoadIconInfo()
        {
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + @"\Data\Application\Icon");
            FileInfo[] files = dir.GetFiles();
            double[] margin = new double[2];
            foreach (FileInfo file in files)
            {
                string[] lines = File.ReadAllLines(file.FullName);
                string[] smargin = lines[1].Split(',');
                Double.TryParse(smargin[0], out margin[0]);
                Double.TryParse(smargin[1], out margin[1]);
                ApplicationIcon icon = new ApplicationIcon(ApplicationManager.GetAppByName(lines[0]), margin);
                ApplicationManager.ApplicationIconList.Add(icon);
            }
        }

        internal static ApplicationIcon FitApplicationIcon(Application app)
        {
            return new ApplicationIcon(app, new double[] { 0, 0, 0, 0 });
        }

        internal static void RegistIcon(ApplicationIcon icon)
        {
            
        }
    }
}
