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
        public void RefrushData()
        {
            while (true)
            {
                SpeedUnit = UnitList[MainWindow.SpeedUnitSelectMode];
                Thread.Sleep(100);
            }
        }
    }
}
