using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Timers;
using System.Windows.Threading;
using System.Diagnostics;

namespace Eagle.Core
{
    class SmoothWindowEffect
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        Window mask = new Window();
        Color mid_color;
        private double increaseblock;
        private int times;
        internal event Action AtMid;
        internal event Action AtEnd;

        internal SmoothWindowEffect(Color _mid_color,double _times)
        {
            Int32.TryParse(_times + "", out times);
            this.mid_color = _mid_color;

            mask.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            mask.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            mask.Topmost = true;
            mask.Background = new SolidColorBrush(Color.FromRgb( _mid_color.R, _mid_color.G, _mid_color.B));
            mask.Opacity = 0;
            mask.WindowStyle = WindowStyle.None;
            mask.AllowsTransparency = true;
            mask.Left = 0;
            mask.Top = 0;
            mask.Show();
            increaseblock = 1 / _times;
        }

        byte ctime = 0, mode = 0;
        internal void Start()
        {
            timer.Interval = 20;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Update);
            mode = 1;
        }

        private void Update(object sender, EventArgs e)
        {
            mask.Dispatcher.Invoke(new Action(delegate
            {
                mask.Topmost = true;
                if (mode == 1)
                {
                    if (ctime < times)
                        ctime += 1;
                    else if (ctime >= times)
                    {
                        AtMid();
                        mode = 2;
                    }
                }
                else if (mode == 2)
                {
                    if (ctime > 0)
                        ctime -= 1;
                    else if (ctime <= 0)
                    {
                        //if(this.AtEnd.Method != null)
                            this.AtEnd();
                        this.mask.Close();
                        this.timer.Enabled = false;
                    }
                }
                this.mask.Opacity = ctime * increaseblock;
            }));
        }
    }
}
