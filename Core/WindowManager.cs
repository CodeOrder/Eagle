using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;

namespace Eagle.Core
{
    class WindowManager
    {
        internal static Dictionary<string, Window> RunningWindows = new Dictionary<string, Window>();
        internal static List<Window> MainWindows = new List<Window>();
        internal static Window CurrentWindow = new Window();
        internal static int CurrentID = 0;
        internal delegate void WindowChangedHandler(object sender, Window before_win, Window after_win);
        internal static event WindowChangedHandler WindowChanged;
        static SmoothWindowEffect effect;
        static string mode;

        internal static void NextMainWindow()
        {
            Debug.WriteLine("Before currentID = " + CurrentID);
            mode = "next";
            effect = new SmoothWindowEffect(System.Windows.Media.Color.FromRgb(255, 255, 255), 20);
            effect.AtMid += ChangeWindow;
            effect.AtEnd += SetWindowChanged;
            if (CurrentID < MainWindows.Count - 1)
                CurrentID += 1;
            else
                CurrentID = 0;
            effect.Start();
            Debug.WriteLine("After currentID = " + CurrentID);
        }

        internal static void LastMainWindow()
        {
            mode = "last";
            effect = new SmoothWindowEffect(System.Windows.Media.Color.FromRgb(255, 255, 255), 20);
            effect.AtMid += ChangeWindow;
            effect.AtEnd += SetWindowChanged;
            effect.Start();
            if (CurrentID > 0)
                CurrentID -= 1;
            else
                CurrentID = MainWindows.Count - 1;
        }

        private static void ChangeWindow()
        {
            Debug.WriteLine("mode = "+mode);
            Window before = new Window(), after;
            if (mode == "next")
            {
                if (CurrentID == 0)
                    before = MainWindows[MainWindows.Count - 1];
                else
                    before = MainWindows[CurrentID - 1];
            }
            else if (mode == "last")
            {
                if (CurrentID == MainWindows.Count - 1)
                    before = MainWindows[0];
                else
                    before = MainWindows[CurrentID + 1];
            }
            after = MainWindows[CurrentID];
            before.Topmost = false;
            after.Topmost = false;
            WindowChanged(null, before, after);
            after.Focus();                  //使得切换后的窗口在渐变层与切换前窗口之间
        }

        private static void SetWindowChanged()
        {
            Window after = MainWindows[CurrentID];
            if(after != WindowManager.RunningWindows["main"])
                after.Topmost = true;
        }
    }
}
