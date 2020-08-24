using Microsoft.Practices.Prism.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BodorLaserWPFService.BLL;
using BodorThinkerConfig.View;
using X.Config;

namespace x
{
    public class IOListViewModel : NotificationObject
    {
        public IOListViewModel()
        {
            var a = Y.Instance.IOConfig.IList;
            InputIOList = new List<IOData>();
            for (int i = 0; i < a.Count; ++i)
            {
                IOData data = new IOData
                {
                    Index = System.Convert.ToInt32(a[i].P),
                    CanSelected = true
                };
                InputIOList.Add(data);
            }
            InputIOList.Insert(0, new IOData() { Index = 0, CanSelected = true });

            var b = Y.Instance.IOConfig.OList;
            OutputIOList = new List<IOData>();
            for (int i = 0; i < b.Count; ++i)
            {
                IOData data = new IOData
                {
                    Index = System.Convert.ToInt32(b[i].P),
                    CanSelected = true
                };
                OutputIOList.Add(data);
            }
            OutputIOList.Insert(0, new IOData() { Index = 0, CanSelected = true });
        }

        public void SetValue()
        {
            var a = Y.Instance.IOConfig.IList;
            for (int i = 0; i < IOList.InputIOIndexList.Count; ++i)
                a[i].P = (IOList.InputIOIndexList[i].SelectedItem as IOData).Index.ToString();
            var b = Y.Instance.IOConfig.OList;
            for (int i = 0; i < IOList.OutputIOIndexList.Count; ++i)
                b[i].P = (IOList.OutputIOIndexList[i].SelectedItem as IOData).Index.ToString();
            //移除通用输入和输出
            a.RemoveAll((p) => p.L == "GeneralInput");
            b.RemoveAll((p) => p.L == "GeneralOutput");
            //写入通用输入和输出
            for (int i = 1; i < InputIOList.Count; ++i)
            {
                if (InputIOList[i].Index == 0)
                    continue;
                if (InputIOList[i].CanSelected && a.FindIndex((p) => p.P == InputIOList[i].index.ToString()) == -1)
                    a.Add(new I() { L = "GeneralInput", P = InputIOList[i].index.ToString(), Desc = "通用输入" });
            }
            for (int i = 1; i < OutputIOList.Count; ++i)
            {
                if (OutputIOList[i].Index == 0)
                    continue;
                if (OutputIOList[i].CanSelected && b.FindIndex((p) => p.P == OutputIOList[i].index.ToString()) == -1)
                    b.Add(new O() { L = "GeneralOutput", P = OutputIOList[i].index.ToString(), Desc = "通用输出" });
            }
            Y.Instance.SaveIOConfigToLocal();
        }

        public List<IOData> _InputIOList;
        public List<IOData> InputIOList
        {
            get { return _InputIOList; }
            set
            {
                _InputIOList = value;
                RaisePropertyChanged("InputIOList");
            }
        }

        public List<IOData> _OutputIOList;
        public List<IOData> OutputIOList
        {
            get { return _OutputIOList; }
            set
            {
                _OutputIOList = value;
                RaisePropertyChanged("OutputIOList");
            }
        }
    }
    public class IOData : INotifyPropertyChanged
    {
        public int index;
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Index"));
            }
        }

        public bool canSelected;
        public bool CanSelected
        {
            get { return canSelected; }
            set
            {
                canSelected = value;
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("CanSelected"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
