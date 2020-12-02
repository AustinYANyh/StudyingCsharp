using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testUnit
{
    public class MainModel:NotificationObject
    {
        public MainModel()
        {

        }

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

        private string _TimeUnit;
        public string TimeUnit
        {
            get { return _TimeUnit; }
            set
            {
                _TimeUnit = value;
                RaisePropertyChanged("TimeUnit");
            }
        }

        private string _DistanceUnit;
        public string DistanceUnit
        {
            get { return _DistanceUnit; }
            set
            {
                _DistanceUnit = value;
                RaisePropertyChanged("DistanceUnit");
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

        private int _PreTimeMode = 0;
        public int PreTimeMode
        {
            get { return _PreTimeMode; }
            set
            {
                _PreTimeMode = value;
                RaisePropertyChanged("PreTimeMode");
            }
        }

        private int _CurTimeMode = 0;
        public int CurTimeMode
        {
            get { return _CurTimeMode; }
            set
            {
                _CurTimeMode = value;
                RaisePropertyChanged("CurTimeMode");
            }
        }

        private int _PreDistanceMode = 0;
        public int PreDistanceMode
        {
            get { return _PreDistanceMode; }
            set
            {
                _PreDistanceMode = value;
                RaisePropertyChanged("PreDistanceMode");
            }
        }

        private int _CurDistanceMode = 0;
        public int CurDistanceMode
        {
            get { return _CurDistanceMode; }
            set
            {
                _CurDistanceMode = value;
                RaisePropertyChanged("CurDistanceMode");
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

        private double _Time1 = 100;
        public double Time1
        {
            get { return _Time1; }
            set
            {
                _Time1 = value;
                RaisePropertyChanged("Time1");
            }
        }

        private double _Time2 = 200;
        public double Time2
        {
            get { return _Time2; }
            set
            {
                _Time2 = value;
                RaisePropertyChanged("Time2");
            }
        }

        private double _Time3 = 300;
        public double Time3
        {
            get { return _Time3; }
            set
            {
                _Time3 = value;
                RaisePropertyChanged("Time3");
            }
        }

        private double _Time4 = 50;
        public double Time4
        {
            get { return _Time4; }
            set
            {
                _Time4 = value;
                RaisePropertyChanged("Time4");
            }
        }

        private double _Distance1 = 100;
        public double Distance1
        {
            get { return _Distance1; }
            set
            {
                _Distance1 = value;
                RaisePropertyChanged("Distance1");
            }
        }

        private double _Distance2 = 200;
        public double Distance2
        {
            get { return _Distance2; }
            set
            {
                _Distance2 = value;
                RaisePropertyChanged("Distance2");
            }
        }

        private double _Distance3 = 300;
        public double Distance3
        {
            get { return _Distance3; }
            set
            {
                _Distance3 = value;
                RaisePropertyChanged("Distance3");
            }
        }

        private double _Distance4 = 50;
        public double Distance4
        {
            get { return _Distance4; }
            set
            {
                _Distance4 = value;
                RaisePropertyChanged("Distance4");
            }
        }
    }
}
