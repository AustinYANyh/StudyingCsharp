/*============================================================================================
   *   Copyright (C) 2021 All rights reserved.
   *   
   *   文件名称：BaseNotifyPropertyChanged.cs
   *   创 建 者：yanyunhao
   *   创建日期：2021/4/12 16:24:59
   *   描    述：
==============================================================================================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestMenuItem {
    public class BaseNotifyPropertyChanged : INotifyPropertyChanged {

        protected virtual void OnPropertyChanged(string PropertyName) {
            if (PropertyChanged != null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;
    }
}
