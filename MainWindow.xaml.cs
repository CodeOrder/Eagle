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
            WindowManager.RunningWindows.Add("main",this);
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

            this.Topmost = true;
            InitializeComponent();
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Left = this.Top = 0;
            this.KeyDown += MainWindow_KeyDown;
            clock.Enabled = true;
            clock.Interval = 900;
            clock.Elapsed += new System.Timers.ElapsedEventHandler(Update);

            SmoothWindowEffect effect = new SmoothWindowEffect(Color.FromRgb(255, 255, 255), 20);
            effect.AtMid += tmp;
            effect.AtEnd += tmp;
            effect.Start();
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
                WindowManager.NextMainWindow();
            else if (e.Key == Key.Left)
                WindowManager.LastMainWindow();
        }

        private void Update(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(new Action(delegate
                {
                    this.TimeLabel.Content = DateTime.Now.ToString("hh:mm:ss");
                    this.DateLabel.Content = DateTime.Now.ToString("yyyy年MM月dd日");
                }));
        }

        void tmp()
        {
            System.Diagnostics.Debug.WriteLine("Finish!");
        }
    }
}
