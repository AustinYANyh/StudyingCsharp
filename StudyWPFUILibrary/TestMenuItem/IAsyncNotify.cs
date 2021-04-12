/*============================================================================================
   *   Copyright (C) 2021 All rights reserved.
   *   
   *   文件名称：IAsyncNotify.cs
   *   创 建 者：yanyunhao
   *   创建日期：2021/4/12 16:14:05
   *   描    述：
==============================================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;


namespace TestMenuItem {
    public interface IAsyncNotify {

        double Percent { get; set; }
        bool IsCompeleted { get; set; }

        void Start();
        void Stop();

        [MethodImpl(MethodImplOptions.Synchronized)]
        void Advance(double work);

        void Compeleted();

        AsynNotifyAdvanceEventHandler OnAdvance { get; set; }
        Action OnCompeleted { get; set; }
    }

    public delegate void AsynNotifyAdvanceEventHandler(double work);
}
