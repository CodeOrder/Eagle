using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using Eagle.Core;
using Eagle.Core.EagleApplication;

namespace Eagle
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer clock = new Timer();
        public MainWindow()
        {
            InitializeComponent();
            WindowManager.CurrentWindow = this;
            WindowManager.RunningWindows.Add("main", this);
            WindowManager.MainWindows.Add(this);
            WindowManager.MainWindows.Add(new NotePadWindow());
            WindowManager.CurrentID = 0;
            foreach (Window window in WindowManager.MainWindows)
            {
                if (window != this)
                {
                    window.Topmost = false;
                    window.Show();
                }
            }

            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Left = this.Top = 0;
            this.KeyDown += MainWindow_KeyDown;
            this.MouseRightButtonDown += MainWindow_MouseRightButtonDown;
            this.Closing += MainWindow_Closing;
            clock.Enabled = true;
            clock.Interval = 900;
            clock.Elapsed += new System.Timers.ElapsedEventHandler(Update);
            

            ApplicationManager.LoadApplications();
            ApplicationManager.LoadIconInfo();
            foreach (ApplicationIcon icon in ApplicationManager.ApplicationIconList)
            {
                icon.SetParent(this.MainGrid);
                this.MainGrid.Children.Add(icon);
            }

            WindowManager.WindowChanged += WindowManager_WindowChanged;
        }


        void MainWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Topmost = false;
        }

        void WindowManager_WindowChanged(object sender, Window before_win, Window after_win)
        {
            this.Topmost = false;
            System.Diagnostics.Debug.WriteLine("before_win = " + before_win.Name + "   after_win = " + after_win.Name);
            if (after_win == this)                           //检查切换后的窗口是否是主窗口
            {
                System.Diagnostics.Debug.WriteLine("Get!");
                foreach (Core.EagleApplication.Application app in ApplicationManager.RunningApplicationList)
                    app.SendCommand(ApplicationCMDKind.Topmost);        //置顶所有正在运行的Eagle程序
            }
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (ApplicationIcon icon in ApplicationManager.ApplicationIconList)
                ApplicationManager.SaveIconInfo(icon);
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
                WindowManager.NextMainWindow();
            else if (e.Key == Key.Left)
                WindowManager.LastMainWindow();
            foreach (Core.EagleApplication.Application app in ApplicationManager.RunningApplicationList)
                app.SendCommand(ApplicationCMDKind.Untopmost);
        }

        private void Update(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                this.TimeLabel.Content = DateTime.Now.ToString("hh:mm:ss");
                this.DateLabel.Content = DateTime.Now.ToString("yyyy年MM月dd日");
            }));
        }

        private void AddAppButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "eag文件 (*.eag)|*.eag";
            dialog.InitialDirectory = Environment.CurrentDirectory;
            if (dialog.ShowDialog() == true)
            {
                Eagle.Core.EagleApplication.Application app = ApplicationManager.InstallApplication(dialog.FileName);
                ApplicationIcon icon = ApplicationManager.FitApplicationIcon(app);
                icon.SetParent(this.MainGrid);
                this.MainGrid.Children.Add(icon);
                ApplicationManager.ApplicationIconList.Add(icon);
                System.Diagnostics.Debug.WriteLine("Send CMD!");
                app.SendCommand(ApplicationCMDKind.Topmost);
            }
        }
    }
}
