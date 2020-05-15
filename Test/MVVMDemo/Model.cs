using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radio
{
    public class Model : NotificationObject
    {
        private string _Name = "Rabit";
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public void AddName(object obj)
        {
            Name += "Rabit";
        }
    }
}
