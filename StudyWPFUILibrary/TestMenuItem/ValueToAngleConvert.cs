/*============================================================================================
   *   Copyright (C) 2021 All rights reserved.
   *   
   *   文件名称：ValueToAngleConvert.cs
   *   创 建 者：yanyunhao
   *   创建日期：2021/4/12 15:24:47
   *   描    述：
==============================================================================================*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ConvertHelper {

    public class ValueToAngleConvert : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var percent = System.Convert.ToDouble(value.ToString());
            if (percent >= 1) return 360.0D;
            return percent * 360;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
