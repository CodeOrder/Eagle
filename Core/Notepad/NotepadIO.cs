using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;

namespace Eagle.Core.Notepad
{
    class NotepadIO
    {
        /// <summary>
        /// 文件：name.tnot 位置：./Data/Notepad/ 格式：1）title 2）text 3）left,top 4）Last change time
        /// </summary>
        /// <param name="note"></param>
        public static void SaveTextNote(TextNote note)
        {

            string title = note.titlebox.Text;
            string text = note.contentbox.Text;

            FileStream file = new FileStream(Environment.CurrentDirectory + "\\Data\\Notepad\\" + note.name, FileMode.Create);
            StreamWriter writer = new StreamWriter(file,Encoding.UTF8);
            writer.WriteLine(title);
            writer.WriteLine(text);
            writer.WriteLine(note.Margin.Left + "," + note.Margin.Top);
            writer.WriteLine(note.last_change_time);
            writer.Close();
            writer.Dispose();
        }

        public static Dictionary<string, TextNote> LoadAllTextNote()
        {
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "\\Data\\Notepad\\");
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

        public static void DeleteTextRubbish()
        {
            List<TextNote> savingList = new List<TextNote>();
            foreach (TextNote note in RunningNoteManager.runningTextnote.Values)
                savingList.Add(note);

            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "\\Data\\Notepad\\");
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                bool is_match = false;
                System.Diagnostics.Debug.WriteLine("File's name = " + file.Name.Split('.')[0]);
                for (int i = 0; i < savingList.Count; i++)
                {
                    System.Diagnostics.Debug.WriteLine("listname = " + savingList[i].name);
                    if (file.Name.Split('.')[0] == savingList[i].name.Split('.')[0])
                        is_match = true;
                }
                if (is_match == false)
                    File.Delete(file.FullName);
            }
        }
    }
}
