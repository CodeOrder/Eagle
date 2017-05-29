using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace Eagle.Core.Notepad
{
    class PictureNote : Grid
    {
        public string name = null;

        public BitmapImage picture;
        public Button imagebox;             //点击这个按钮更换图片
        public Button savebutton = new Button();
        public Button deletebutton = new Button();
        public TextBox contentbox;
        private Grid parentGrid;
        private string path;
        public PictureNote(Grid _parentGrid)
        {
            picture = new BitmapImage(new Uri(Environment.CurrentDirectory + "//Image//WaitPicture.jpg"));
            this.parentGrid = _parentGrid;
            this.Width = picture.Width % 400;
            this.Height = picture.Height % 300;
            this.Background = new SolidColorBrush { Color = Color.FromRgb(235, 235, 109) };
            imagebox = new Button();
            contentbox = new TextBox();

            imagebox.Background = new ImageBrush { ImageSource = picture };
            contentbox.Background = new SolidColorBrush { Color = Color.FromArgb(8, 255, 255, 255) };
            contentbox.Text = "文字...";

            imagebox.Click += imagebox_Click;
            contentbox.TextChanged += contentbox_TextChanged;

            UpdateChildrenLocation();

            this.MouseDown += BoardPicture_MouseDown;
            this.MouseUp += BoardPicture_MouseUp;
            this.MouseMove += BoardPicture_MouseMove;
            this.savebutton.Click += savebutton_Click;
            this.deletebutton.Click += deletebutton_Click;

            this.Children.Add(imagebox);
            this.Children.Add(contentbox);
            this.Children.Add(savebutton);
            this.Children.Add(deletebutton);
            this.HorizontalAlignment = HorizontalAlignment.Left;                    //设置中心为左上角，
            this.VerticalAlignment = VerticalAlignment.Top;                         //Margin即为这个点的坐标

            BitmapImage saveimage = new BitmapImage(new Uri(Environment.CurrentDirectory + "/Icon/DownArrow.png", UriKind.Relative));
            savebutton.Background = new ImageBrush { ImageSource = saveimage };
            savebutton.BorderBrush = null;
            BitmapImage binimage = new BitmapImage(new Uri(Environment.CurrentDirectory + "/Icon/Bin.png", UriKind.Relative));
            deletebutton.Background = new ImageBrush { ImageSource = binimage };
            deletebutton.BorderBrush = null;

            Random nameram = new Random();                                       //使得笔记的名称基本是随机的
            int ramvalue = nameram.Next(0, Int32.MaxValue);
            this.name = DateTime.Now.ToString("yyyyMMddhhmmss") + ramvalue + "_PICNote";
        }

        void deletebutton_Click(object sender, RoutedEventArgs e)
        {
            parentGrid.Children.Remove(this);
        }

        void savebutton_Click(object sender, RoutedEventArgs e)
        {

        }

        void BoardPicture_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ismove)
            {
                Point mousepos = e.GetPosition(null);
                Thickness margin = this.Margin;
                margin.Left = beforepos.X + (mousepos.X - beforepos.X);
                margin.Top = beforepos.Y + (mousepos.Y - beforepos.Y);
                this.Margin = margin;
            }
        }

        void BoardPicture_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ismove == true && e.LeftButton == System.Windows.Input.MouseButtonState.Released)
            {
                PictureNote note = (PictureNote)e.OriginalSource;
                ismove = false;
                note.ReleaseMouseCapture();
            }
        }

        bool ismove = false;
        Point beforepos;
        void BoardPicture_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point mousepos = e.GetPosition(null);
            beforepos = mousepos;
            //beforepos = new Point(this.Margin.Left, this.Margin.Top);
            PictureNote note = (PictureNote)e.OriginalSource;
            ismove = true;
            note.CaptureMouse();
        }

        void contentbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        void imagebox_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "JPEG文件 (*.jpg),(*.JPG)|*.jpg;*.JPG|PNG文件（*.png),（*.PNG)|*.png;*.PNG";
            dialog.FilterIndex = 0;
            dialog.InitialDirectory = Environment.CurrentDirectory;
            if (dialog.ShowDialog() == true)
            {
                this.path = dialog.FileName;
                BitmapImage newpicture = new BitmapImage(new Uri(path, UriKind.Absolute));
                this.imagebox.Background = new ImageBrush { ImageSource = newpicture };
                this.picture = newpicture;
            }
        }

        private void UpdateChildrenLocation()
        {
            ImageBrush background = (ImageBrush)this.imagebox.Background;
            double WHratio = background.ImageSource.Height / background.ImageSource.Width;
            this.Width = 300;
            this.Height = this.Width * WHratio;
            this.imagebox.Margin = new Thickness(30, 10, 10, Height * 0.15);
            this.contentbox.Margin = new Thickness(90, Height - Height * 0.15, 10, 10);
            this.deletebutton.Margin = new Thickness(60, Height - Height * 0.15, 10 +
                (this.Width - contentbox.Margin.Left - contentbox.Margin.Right), 2);
            this.savebutton.Margin = new Thickness(30, Height - Height * 0.15, 10 + (this.Width - contentbox.Margin.Left - contentbox.Margin.Right) +
                (this.Width - deletebutton.Margin.Left - deletebutton.Margin.Right), 2);
        }
    }
}
