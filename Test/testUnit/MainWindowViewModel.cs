using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace testUnit
{
    class MainWindowViewModel : NotificationObject
    {
        public MainWindowViewModel()
        {
            Task task = new Task(() =>
            {
                RefrushData();
            });
            task.Start();
        }
        List<string> UnitList = new List<string>() { "mm/s", "mm/min", "m/s", "m/min", "in/s", "in/min" };
        private string _SpeedUnit;
        public string SpeedUnit
        {
            get { return _SpeedUnit; }
            set
            {
                _SpeedUnit = value;
                RaisePropertyChanged("SpeedUnit");
            }
        }

        private int _PreSpeedMode = 0;
        public int PreSpeedMode
        {
            get { return _PreSpeedMode; }
            set
            {
                _PreSpeedMode = value;
                RaisePropertyChanged("PreSpeedMode");
            }
        }

        private int _CurSpeedMode = 0;
        public int CurSpeedMode
        {
            get { return _CurSpeedMode; }
            set
            {
                _CurSpeedMode = value;
                RaisePropertyChanged("CurSpeedMode");
            }
        }

        private double _Speed1 = 100;
        public double Speed1
        {
            get { return _Speed1; }
            set
            {
                _Speed1 = value;
                RaisePropertyChanged("Speed1");
            }
        }

        private double _Speed2 = 200;
        public double Speed2
        {
            get { return _Speed2; }
            set
            {
                _Speed2 = value;
                RaisePropertyChanged("Speed2");
            }
        }

        private double _Speed3 = 300;
        public double Speed3
        {
            get { return _Speed3; }
            set
            {
                _Speed3 = value;
                RaisePropertyChanged("Speed3");
            }
        }

        private double _Speed4 = 50;
        public double Speed4
        {
            get { return _Speed4; }
            set
            {
                _Speed4 = value;
                RaisePropertyChanged("Speed4");
            }
        }

        public void RefrushData()
        {
            while (true)
            {
                SpeedUnit = UnitList[CurSpeedMode];
                Thread.Sleep(100);
            }
        }
    }
}
