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
        public bool is_parentSet = false;

        public BitmapImage picture = new BitmapImage(new Uri(Environment.CurrentDirectory + "//Image//WaitPicture.jpg"));
        public Button imagebox;             //点击这个按钮更换图片
        public Button deletebutton = new Button();
        public TextBox contentbox;
        private Grid parentGrid;

        private string path;
        public PictureNote(Grid _parentGrid)
        {
            is_parentSet = true;
            this.parentGrid = _parentGrid;
            SetChildren();
            UpdateChildrenLocation();

            this.MouseDown += BoardPicture_MouseDown;
            this.MouseUp += BoardPicture_MouseUp;
            this.MouseMove += BoardPicture_MouseMove;

            this.Children.Add(imagebox);
            this.Children.Add(contentbox);
            this.Children.Add(deletebutton);
            this.HorizontalAlignment = HorizontalAlignment.Left;                    //设置中心为左上角，
            this.VerticalAlignment = VerticalAlignment.Top;                         //Margin即为这个点的坐标

            Random nameram = new Random();                                       //使得笔记的名称基本是随机的
            int ramvalue = nameram.Next(0, Int32.MaxValue);
            this.name = DateTime.Now.ToString("yyyyMMddhhmmss") + ramvalue + "_PICNote";
        }

        public PictureNote(BitmapImage _picture, string _text, double[] _margin)
        {
            SetChildren();
            UpdateChildrenLocation();

            this.picture = _picture;
            this.imagebox.Background = new ImageBrush(this.picture);
            this.contentbox.Text = _text;
            this.Margin = new Thickness(_margin[0], _margin[1], 0, 0);
            this.MouseDown += BoardPicture_MouseDown;
            this.MouseUp += BoardPicture_MouseUp;
            this.MouseMove += BoardPicture_MouseMove;

            this.Children.Add(imagebox);
            this.Children.Add(contentbox);
            this.Children.Add(deletebutton);
            this.HorizontalAlignment = HorizontalAlignment.Left;                    //设置中心为左上角，
            this.VerticalAlignment = VerticalAlignment.Top;                         //Margin即为这个点的坐标

            Random nameram = new Random();                                       //使得笔记的名称基本是随机的
            int ramvalue = nameram.Next(0, Int32.MaxValue);
            this.name = DateTime.Now.ToString("yyyyMMddhhmmss") + ramvalue + "_PICNote";
        }

        public void SetParentGrid(Grid _parent)
        {
            this.parentGrid = _parent;
            this.is_parentSet = true;
        }

        void deletebutton_Click(object sender, RoutedEventArgs e)
        {
            parentGrid.Children.Remove(this);
            RunningNoteManager.runningPicturenote.Remove(this.name);
        }

        void BoardPicture_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ismove)
            {
                Point mousepos = e.GetPosition(null);
                Thickness margin = this.Margin;
                margin.Left = mousepos.X ;
                margin.Top = mousepos.Y;
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
            PictureNote note = (PictureNote)e.OriginalSource;
            ismove = true;
            note.CaptureMouse();
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
            this.imagebox.Margin = new Thickness(10, 10, 10, Height * 0.15);
            this.contentbox.Margin = new Thickness(40, Height - Height * 0.15, 10, 2);
            this.deletebutton.Margin = new Thickness(10, Height - Height * 0.15, 10 + (this.Width - this.contentbox.Margin.Left - this.contentbox.Margin.Right), 2);
        }

        private void SetChildren()
        {
            this.Width = picture.Width % 400;
            this.Height = picture.Height % 300;
            this.Background = new SolidColorBrush { Color = Color.FromRgb(235, 235, 109) };
            imagebox = new Button();
            contentbox = new TextBox();

            imagebox.Background = new ImageBrush { ImageSource = picture };
            contentbox.Background = new SolidColorBrush { Color = Color.FromArgb(8, 255, 255, 255) };
            BitmapImage binimage = new BitmapImage(new Uri(Environment.CurrentDirectory + "/Icon/Bin.png", UriKind.Relative));
            deletebutton.Background = new ImageBrush { ImageSource = binimage };
            deletebutton.BorderBrush = null;
            contentbox.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentbox.VerticalContentAlignment = VerticalAlignment.Center;
            contentbox.BorderBrush = null;
            contentbox.Text = "文字...";
            contentbox.KeyDown += contentbox_KeyDown;
            imagebox.Click += imagebox_Click;
            this.deletebutton.Click += deletebutton_Click;
        }

        void contentbox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                this.contentbox.ReleaseAllTouchCaptures();
        }
    }
}
