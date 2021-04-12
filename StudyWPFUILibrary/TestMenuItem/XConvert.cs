/*============================================================================================
   *   Copyright (C) 2021 All rights reserved.
   *   
   *   文件名称：XConvert.cs
   *   创 建 者：yanyunhao
   *   创建日期：2021/4/12 15:44:03
   *   描    述：
==============================================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConvertHelper {
   public sealed class XConvert {
        public static ValueToAngleConvert ValueToAngleConvert { get { return Singleton<ValueToAngleConvert>.GetInstance(); } }
    }
}
