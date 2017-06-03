using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Eagle.Core.Notepad
{
    class NotepadIO
    {
        /// <summary>
        /// 文件：name.tnot 位置：./Data/Notepad/Text 格式：1）title 2）text 3）left,top 4）Last change time
        /// </summary>
        public static void SaveTextNote(TextNote note)
        {
            string title = note.titlebox.Text;
            string text = note.contentbox.Text;

            FileStream file = new FileStream(Environment.CurrentDirectory + "\\Data\\Notepad\\Text\\" + note.name, FileMode.Create);
            StreamWriter writer = new StreamWriter(file,Encoding.UTF8);
            writer.WriteLine(title);
            writer.WriteLine(text);
            writer.WriteLine(note.Margin.Left + "," + note.Margin.Top);
            writer.WriteLine(note.last_change_time);
            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// 文件：name.pnot 位置：./Data/Notepad/Picture 格式：1）图像完整路径 2）text 3)left,top）
        /// </summary>
        public static void SavePictureNote(PictureNote note)
        {
            BitmapImage image = note.picture;
            string text = note.contentbox.Text;

            FileStream file = new FileStream(Environment.CurrentDirectory + "\\Data\\Notepad\\Picture\\" + note.name, FileMode.Create);
            StreamWriter writer = new StreamWriter(file, Encoding.UTF8);
            writer.WriteLine(image.UriSource.ToString());
            writer.WriteLine(text);
            writer.WriteLine(note.Margin.Left + "," + note.Margin.Top);
            writer.Close();
            writer.Dispose();
        }

        public static Dictionary<string, TextNote> LoadAllTextNote()
        {
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "\\Data\\Notepad\\Text\\");
            FileInfo[] files = dir.GetFiles();
            Dictionary<string, TextNote> result = new Dictionary<string, TextNote>();

            double[] margin = new double[2];
            string[] lines = null;
            TextNote note = null;
            foreach (FileInfo file in files)
            {
                lines = File.ReadAllLines(file.FullName);
                string[] smargin = lines[2].Split(',');
                Double.TryParse(smargin[0],out margin[0]);
                Double.TryParse(smargin[1], out margin[1]);
                note = new TextNote(file.Name, lines[0], lines[1], margin);
                result.Add(file.Name, note);
            }
            return result;
        }

        public static Dictionary<string, PictureNote> LoadAllPictureNote()
        {
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "\\Data\\Notepad\\Picture");
            FileInfo[] files = dir.GetFiles();
            Dictionary<string, PictureNote> result = new Dictionary<string, PictureNote>();

            double[] margin = new double[2];
            string[] lines = null;
            BitmapImage image = new BitmapImage();
            PictureNote note = null;
            foreach (FileInfo file in files)
            {
                lines = File.ReadAllLines(file.FullName);
                string[] smargin = lines[2].Split(',');
                Double.TryParse(smargin[0], out margin[0]);
                Double.TryParse(smargin[1], out margin[1]);
                image = new BitmapImage(new Uri(lines[0]));
                note = new PictureNote(image, lines[1], margin);
                note.name = file.Name;
                result.Add(file.Name, note);
            }
            return result;
        }

        public static void DeleteRubbish()
        {
            List<TextNote> TsavingList = new List<TextNote>();
            foreach (TextNote note in RunningNoteManager.runningTextnote.Values)
                TsavingList.Add(note);

            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "\\Data\\Notepad\\Text\\");
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                bool is_match = false;
                for (int i = 0; i < TsavingList.Count; i++)
                {
                    if (file.Name.Split('.')[0] == TsavingList[i].name.Split('.')[0])
                        is_match = true;
                }
                if (is_match == false)
                    File.Delete(file.FullName);
            }

            List<PictureNote> PsavingList = new List<PictureNote>();
            foreach (PictureNote note in RunningNoteManager.runningPicturenote.Values)
                PsavingList.Add(note);

            dir = new DirectoryInfo(Environment.CurrentDirectory + "\\Data\\Notepad\\Picture\\");
            files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                bool is_match = false;
                for (int i = 0; i < PsavingList.Count; i++)
                {
                    if (file.Name.Split('.')[0] == PsavingList[i].name.Split('.')[0])
                        is_match = true;
                }
                if (is_match == false)
                    File.Delete(file.FullName);
            }
        }
    }
}
