using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurveEdit
{
    public class CurveEditViewModel : NotificationObject
    {
        public CurveEditViewModel()
        {
            _DutyCycleCurveDotList = new ObservableCollection<CurveDot>();
            _FrequencyCurveDotList = new ObservableCollection<CurveDot>();
        }

        public class CurveDot : INotifyPropertyChanged
        {
            private int index;
            public int Index
            {
                get { return index; }
                set
                {
                    index = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Index"));
                    }
                }
            }
            private string speed;
            public string Speed
            {
                get { return speed; }
                set
                {
                    speed = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Speed"));
                    }
                }
            }

            private string dutycycle;
            public string Dutycycle
            {
                get { return dutycycle; }
                set
                {
                    dutycycle = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Dutycycle"));
                    }
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }

        private ObservableCollection<CurveDot> _DutyCycleCurveDotList;
        public ObservableCollection<CurveDot> DutyCycleCurveDotList
        {
            get { return _DutyCycleCurveDotList; }
            set
            {
                _DutyCycleCurveDotList = value;
                RaisePropertyChanged("DutyCycleCurveDotList");
            }
        }

        private ObservableCollection<CurveDot> _FrequencyCurveDotList;
        public ObservableCollection<CurveDot> FrequencyCurveDotList
        {
            get { return _FrequencyCurveDotList; }
            set
            {
                _FrequencyCurveDotList = value;
                RaisePropertyChanged("FrequencyCurveDotList");
            }
        }
    }
}
