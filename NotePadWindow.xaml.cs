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
using System.Diagnostics;
using Eagle.Core;
using Eagle.Core.Notepad;

namespace Eagle
{
    internal class RunningNoteManager
    {
        internal static Dictionary<string, TextNote> runningTextnote = new Dictionary<string, TextNote>();
        internal static Dictionary<string, PictureNote> runningPicturenote = new Dictionary<string, PictureNote>();
    }

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
            this.Loaded += NotePadWindow_Loaded;
            this.Closing += NotePadWindow_Closing;
            this.TextButton.IsEnabled = false;
        }

        void NotePadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<string, TextNote> LoadedTextNotes = NotepadIO.LoadAllTextNote();
            foreach (TextNote note in LoadedTextNotes.Values)
            {
                note.SetParentGrid(this.PanelGrid);
                this.PanelGrid.Children.Add(note);
                RunningNoteManager.runningTextnote.Add(note.name, note);
            }

            Dictionary<string, PictureNote> LoadedPictureNotes = NotepadIO.LoadAllPictureNote();
            foreach (PictureNote note in LoadedPictureNotes.Values)
            {
                note.SetParentGrid(this.PanelGrid);
                this.PanelGrid.Children.Add(note);
                RunningNoteManager.runningPicturenote.Add(note.name, note);
                Debug.WriteLine("图像笔记："+note.name + "已保存至RunningNoteManager");
            }
        }

        void NotePadWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Topmost = false;
            foreach (TextNote note in RunningNoteManager.runningTextnote.Values)
                NotepadIO.SaveTextNote(note);
            foreach (PictureNote note in RunningNoteManager.runningPicturenote.Values)
                NotepadIO.SavePictureNote(note);
            NotepadIO.DeleteRubbish();
        }

        void NotePadWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePOS = e.GetPosition(this.PanelGrid);
            if (this.mode == 0)
            {
                TextNote note = new TextNote(this.PanelGrid);
                RunningNoteManager.runningTextnote.Add(note.name, note);
                note.Margin = new Thickness(mousePOS.X,mousePOS.Y,0,0);
                this.PanelGrid.Children.Add(note);
            }
            if(this.mode == 1)
            {
                PictureNote note = new PictureNote(this.PanelGrid);
                RunningNoteManager.runningPicturenote.Add(note.name, note);
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

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in WindowManager.RunningWindows.Values)
                window.Close();
            foreach (Window window in WindowManager.MainWindows)
                window.Close();
        }


    }
}
