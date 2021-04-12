/*============================================================================================
   *   Copyright (C) 2021 All rights reserved.
   *   
   *   文件名称：DeafultAsynNotify.cs
   *   创 建 者：yanyunhao
   *   创建日期：2021/4/12 16:30:47
   *   描    述：
==============================================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace TestMenuItem {
    public class DeafultAsynNotify : BaseNotifyPropertyChanged, IAsyncNotify {
        public DeafultAsynNotify() {
            this.Percent = 0;
            this.IsCompeleted = false;
        }

        private double _Percent;
        public double Percent {
            get { return _Percent; }
            set {
                _Percent = value;
                base.OnPropertyChanged("Percent");
            }
        }
        private bool _IsCompeleted;
        public bool IsCompeleted {
            get { return _IsCompeleted; }
            set {
                _IsCompeleted = value;
                base.OnPropertyChanged("IsCompeleted");
            }
        }

        public AsynNotifyAdvanceEventHandler OnAdvance { get; set; }
        public Action OnCompeleted { get; set; }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Advance(double work) {
            Notify(work);
        }

        public void Compeleted() {
            if (this.IsCompeleted) return;
            this.Percent = 0;
            this.IsCompeleted = true;

            if (this.OnCompeleted != null)
                this.OnCompeleted();
        }

        public void Start() {
            this.IsCompeleted = false;
        }

        public void Stop() {
            this.Percent = 0;
            this.IsCompeleted = true;
        }


        private void Notify(double work) {
            this.Percent = work / 100;
            if (this.Percent >= 100 && !this.IsCompeleted)
                this.Compeleted();
            if (this.OnAdvance != null && !this.IsCompeleted)
                this.OnAdvance(work);
        }
    }
}
