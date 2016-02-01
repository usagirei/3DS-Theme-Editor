// --------------------------------------------------
// 3DS Theme Editor - Ticker.cs
// --------------------------------------------------

using System;
using System.ComponentModel;
using System.Timers;

using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF
{
    public class Ticker : INotifyPropertyChanged
    {
        //private const string FORMAT_DATE = "M/dd (ddd)";
        //private const string FORMAT_HOUR = "HH";
        //private const string FORMAT_MIN = "mm";
        public string DateString => Now.ToString(MainResources.Preview_DateFormat);
        public string HourString => Now.ToString(MainResources.Preview_HourFormat);
        public string MinString => Now.ToString(MainResources.Preview_MinFormat);
        public DateTime Now => DateTime.Now;

        public bool SecModTwo => Now.Second % 2 == 0;

        public string SepString => SecModTwo ? " " : ":";

        public Ticker()
        {
            var timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            // 1 second updates
            timer.Interval = 1000;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(String.Empty));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
