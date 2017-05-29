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
using System.Windows.Shapes;
using Eagle.Core;
using Eagle.Core.Notepad;

namespace Eagle
{
    /// <summary>
    /// NotePadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NotePadWindow : Window
    {
        private int mode = 0;
        public NotePadWindow()
        {
            InitializeComponent();
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Left = this.Top = 0;
            this.KeyDown += NotePadWindow_KeyDown;
            this.MouseRightButtonDown += NotePadWindow_MouseRightButtonDown;
            this.TextButton.IsEnabled = false;
        }

        void NotePadWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePOS = e.GetPosition(this.PanelGrid);
            if (this.mode == 0)
            {
                TextNote note = new TextNote(this.PanelGrid);
                note.Margin = new Thickness(mousePOS.X,mousePOS.Y,0,0);
                this.PanelGrid.Children.Add(note);
            }
            if(this.mode == 1)
            {
                PictureNote note = new PictureNote(this.PanelGrid);
                note.Margin = new Thickness(mousePOS.X, mousePOS.Y, 0, 0);
                this.PanelGrid.Children.Add(note);
            }
        }

        void NotePadWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
                WindowManager.NextMainWindow();
            else if (e.Key == Key.Left)
                WindowManager.LastMainWindow();
        }

        private void TextButton_Click(object sender, RoutedEventArgs e)
        {
            this.TextButton.IsEnabled = false;
            this.PictureButton.IsEnabled = true;
            this.mode = 0;
        }

        private void PictureButton_Click(object sender, RoutedEventArgs e)
        {
            this.PictureButton.IsEnabled = false;
            this.TextButton.IsEnabled = true;
            this.mode = 1;
        }


    }
}
