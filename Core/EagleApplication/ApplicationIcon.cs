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

namespace Eagle.Core.EagleApplication
{
    class ApplicationIcon : Grid
    {
        public Application ContentApp;
        public Button ImgButton;
        public Label NameLabel;
        public string name;
        private Grid ParentGrid;
        private bool is_ParentSet = false;
        private bool is_AppSet = false;

        public ApplicationIcon(Grid ParentGrid,Application _contentapp)
        {
            this.ParentGrid = ParentGrid;
            this.ContentApp = _contentapp;
            this.is_ParentSet = true;
            this.is_AppSet = true;
            this.Width = ParentGrid.Width * 0.05;
            this.Height = ParentGrid.Height * 0.05;

            this.Background = new SolidColorBrush(Color.FromArgb(30, 30, 144, 255));
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            setchildren();
            this.Children.Add(ImgButton);
            this.Children.Add(NameLabel);
            this.ImgButton.Click += ImgButton_Click;

            this.MouseLeftButtonDown += ApplicationIcon_MouseDown;
            this.MouseMove += ApplicationIcon_MouseMove;
            this.MouseUp += ApplicationIcon_MouseUp;
            this.NameLabel.MouseLeftButtonDown += ApplicationIcon_MouseDown;
            this.NameLabel.MouseMove += ApplicationIcon_MouseMove;
            this.NameLabel.MouseUp += ApplicationIcon_MouseUp;
        }

        public ApplicationIcon(Application _contentapp,double[] _margin)
        {
            this.ContentApp = _contentapp;
            this.is_ParentSet = false;
            this.is_AppSet = true;
            //this.Width = ParentGrid.Width * 0.05;
            //this.Height = ParentGrid.Height * 0.05;

            this.Background = new SolidColorBrush(Color.FromArgb(30, 30, 144, 255));
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            setchildren();
            this.Margin = new Thickness(_margin[0], _margin[1], 0, 0);
            this.Children.Add(ImgButton);
            this.Children.Add(NameLabel);
            this.ImgButton.Click += ImgButton_Click;

            this.MouseLeftButtonDown += ApplicationIcon_MouseDown;
            this.MouseMove += ApplicationIcon_MouseMove;
            this.MouseUp += ApplicationIcon_MouseUp;
            this.NameLabel.MouseLeftButtonDown += ApplicationIcon_MouseDown;
            this.NameLabel.MouseMove += ApplicationIcon_MouseMove;
            this.NameLabel.MouseUp += ApplicationIcon_MouseUp;
        }

        public void SetParent(Grid _parentGrid)
        {
            this.ParentGrid = _parentGrid;
            this.Width = ParentGrid.Width * 0.05;
            this.Height = ParentGrid.Height * 0.05;
            this.is_ParentSet = true;
        }

        bool ismove = false;
        Point beforepos;
        void ApplicationIcon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                beforepos = e.GetPosition(null);
                ApplicationIcon icon = (ApplicationIcon)e.OriginalSource;
                ismove = true;
                icon.CaptureMouse();
            }
            catch (Exception ex){}
        }

        void ApplicationIcon_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ismove)
            {
                Point mousePOS = e.GetPosition(null);
                Thickness margin = new Thickness(mousePOS.X, mousePOS.Y, 0, 0);
                this.Margin = margin;
            }
        }

        void ApplicationIcon_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ismove == true && e.ButtonState == System.Windows.Input.MouseButtonState.Released)
            {
                ismove = false;
                ApplicationIcon icon = (ApplicationIcon)e.OriginalSource;
                icon.ReleaseMouseCapture();
            }
        }

        void ImgButton_Click(object sender, RoutedEventArgs e)
        {
            this.ContentApp.Run();
        }

        private void setchildren()
        {
            this.Width = 100;
            this.Height = 100;

            ImgButton = new Button();
            ImgButton.Background = new ImageBrush(ContentApp.Icon);
            ImgButton.OpacityMask = new ImageBrush(ContentApp.Icon);
            ImgButton.BorderBrush = null;
            ImgButton.Margin = new Thickness(this.Width * 0.1, this.Height * 0.1, this.Width * 0.1, this.Height * 0.3);

            NameLabel = new Label();
            NameLabel.Content = ContentApp.Name;
            NameLabel.Margin = new Thickness(this.Width * 0.1, this.Height * 0.7, this.Width * 0.1, this.Height * 0.1);
        }
    }
}
