using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace Eagle.Core.Notepad
{
    class TextNote : Grid                           ///软木板笔记对象，实际上是一个Grid，包含两个TextBox子对象
    ///和两格Button子对象，子对象的更新触发CorkIdea的更新
    {
        public string name = null;

        public TextBox titlebox = new TextBox();
        public TextBox contentbox = new TextBox();
        public Button savebutton = new Button();
        public Button deletebutton = new Button();

        private Grid ParentGrid;
        public TextNote(Grid ParentWindow)
        {
            this.ParentGrid = ParentWindow;
            setparent();
            this.Children.Add(titlebox);                //加入子对象
            this.Children.Add(contentbox);
            this.Children.Add(savebutton);
            this.Children.Add(deletebutton);
            setchildren();
            Random nameram = new Random();              //使得笔记的名称基本是随机的
            int ramvalue = nameram.Next(0, Int32.MaxValue);
            this.name = DateTime.Now.ToString("yyyyMMddhhmmss") + ramvalue + "_Note";
        }

        private void setparent()
        {
            SolidColorBrush background = new SolidColorBrush(Color.FromRgb(235, 235, 109));
            this.Background = background;
            this.AllowDrop = true;
            this.Width = 150;
            this.Height = 300;
            this.Margin = new Thickness(100, 100, 0, 0);
            this.MouseDown += BoardNote_MouseDown;
            this.MouseUp += BoardNote_MouseUp;
            this.MouseMove += BoardNote_MouseMove;
            this.HorizontalAlignment = HorizontalAlignment.Left;                    //设置中心为左上角，
            this.VerticalAlignment = VerticalAlignment.Top;                         //Margin即为这个点的坐标
        }

        private void setchildren()
        {
            titlebox.Margin = new Thickness((double)this.Width * 0.5, (double)this.Height * 0.05, (double)this.Width * 0.1, (double)this.Height * 0.85);
            contentbox.Margin = new Thickness((double)this.Width * 0.1, (double)this.Height * 0.15, (double)this.Width * 0.1, (double)this.Height * 0.05);
            titlebox.TextChanged += titlebox_TextChanged;
            contentbox.TextChanged += contentbox_TextChanged;
            SolidColorBrush titleback = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255));
            SolidColorBrush contentback = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255));
            titlebox.Background = titleback;
            contentbox.Background = contentback;
            titlebox.BorderBrush = null;
            contentbox.BorderBrush = null;
            contentbox.AcceptsReturn = true;
            contentbox.TextWrapping = TextWrapping.Wrap;

            savebutton.Margin = new Thickness((double)this.Width * 0.1, (double)this.Height * 0.05, (double)this.Width * 0.7, (double)this.Height * 0.85);
            deletebutton.Margin = new Thickness((double)this.Width * 0.3, (double)this.Height * 0.05, (double)this.Width * 0.5, (double)this.Height * 0.85);
            BitmapImage saveimage = new BitmapImage(new Uri(Environment.CurrentDirectory + "/Icon/DownArrow.png", UriKind.Relative));
            savebutton.Background = new ImageBrush { ImageSource = saveimage };
            savebutton.BorderBrush = null;
            BitmapImage binimage = new BitmapImage(new Uri(Environment.CurrentDirectory + "/Icon/Bin.png", UriKind.Relative));
            deletebutton.Background = new ImageBrush { ImageSource = binimage };
            deletebutton.BorderBrush = null;
            savebutton.Click += savebutton_Click;
            deletebutton.Click += deletebutton_Click;
        }

        void deletebutton_Click(object sender, RoutedEventArgs e)
        {
            ParentGrid.Children.Remove(this);
        }

        void savebutton_Click(object sender, RoutedEventArgs e)
        {

        }

        bool ismove = false;
        Point beforepos;
        void BoardNote_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point mousepos = e.GetPosition(null);
            beforepos = mousepos;
            TextNote note = (TextNote)e.OriginalSource;
            ismove = true;
            note.CaptureMouse();
        }

        void BoardNote_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ismove == true && e.LeftButton == System.Windows.Input.MouseButtonState.Released)
            {
                TextNote note = (TextNote)e.OriginalSource;
                ismove = false;
                note.ReleaseMouseCapture();
            }
        }

        void BoardNote_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
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

        void titlebox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        void contentbox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
