using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Label = System.Windows.Controls.Label;

namespace testUnit
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
            index = 0;
            SpeedUnitSelectMode = 0;
        }
        public int index;
        public static int preindex = 0;
        public static int SpeedUnitSelectMode;
        public static bool SpeedUnitChanged = false;
        public static int speedPreindex = 0;
        public static int speedUnitMode = 1;
        public static int inch_s_speed_index = 0;
        public static int inch_min_speed_index = 0;
        public static List<double> inch_s_speed_list = new List<double>();
        public static List<double> inch_min_speed_list = new List<double>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Task速度单位转HMI速度单位
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static double ConvertSpeedUnit(double speed)
        {
            double realSpeed = 0;
            switch (SpeedUnitSelectMode)
            {
                case (int)SpeedUnitMode.MmPerSecond:
                    //默认单位
                    realSpeed = speed;
                    break;
                case (int)SpeedUnitMode.MmPerMin:
                    realSpeed = speed * 60;
                    break;
                case (int)SpeedUnitMode.MPerSecond:
                    realSpeed = speed * 1000;
                    break;
                case (int)SpeedUnitMode.MPerMin:
                    realSpeed = Convert.ToDouble((decimal)speed * ((decimal)60 / 1000));
                    break;
                case (int)SpeedUnitMode.IncrPerSecond:
                    inch_s_speed_list.Add(Convert.ToDouble((decimal)speed * (decimal)(0.0393700787402)));
                    realSpeed = Convert.ToDouble(inch_s_speed_list[inch_s_speed_index].ToString("f3"));
                    inch_s_speed_index++;
                    break;
                case (int)SpeedUnitMode.IncrPerMin:
                    inch_min_speed_list.Add(Convert.ToDouble((decimal)speed * (decimal)(0.0393700787402) * 60));
                    realSpeed = Convert.ToDouble(inch_min_speed_list[inch_min_speed_index].ToString("f3"));
                    inch_min_speed_index++;
                    break;
            }
            return realSpeed;
        }

        /// <summary>
        /// HMI上的单位转换,全部统一转为MM/s,在根据界面选择的单位进行转换
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static string HMIUnitConvert(string speed)
        {
            string realSpeed = string.Empty;
            switch (preindex)
            {
                case (int)SpeedUnitMode.MmPerSecond:
                    realSpeed = speed;
                    break;
                case (int)SpeedUnitMode.MmPerMin:
                    realSpeed = (Convert.ToDouble(speed) / 60).ToString();
                    break;
                case (int)SpeedUnitMode.MPerSecond:
                    realSpeed = (Convert.ToDouble(speed) / 1000).ToString();
                    break;
                case (int)SpeedUnitMode.MPerMin:
                    realSpeed = (Convert.ToDecimal(speed) * ((decimal)1000 / 60)).ToString();
                    break;
                case (int)SpeedUnitMode.IncrPerSecond:
                    speed = inch_s_speed_list[inch_s_speed_index].ToString();
                    realSpeed = (Convert.ToDecimal(speed) / (decimal)(0.0393700787402)).ToString("f3");
                    inch_s_speed_index++;
                    break;
                case (int)SpeedUnitMode.IncrPerMin:
                    speed = inch_min_speed_list[inch_min_speed_index].ToString();
                    realSpeed = (Convert.ToDecimal(speed) / (decimal)(0.0393700787402) / 60).ToString("f3");
                    inch_min_speed_index++;
                    break;
            }
            return ConvertSpeedUnit(Convert.ToDouble(realSpeed)).ToString();
        }
        private void UnitCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //转换前准备工作
            inch_s_speed_index = 0;
            inch_min_speed_index = 0;
            if (UnitCombox.SelectedIndex == 4)
                inch_s_speed_list.Clear();
            if (UnitCombox.SelectedIndex == 5)
                inch_min_speed_list.Clear();
            //单位换算
            SpeedUnitSelectMode = UnitCombox.SelectedIndex;
            for (int i = 0; i < 4; ++i)
            {
                string name = "Input" + (i + 1).ToString();
                TextBox box = Application.Current.MainWindow.FindName(name) as TextBox;

                if (box != null)
                    box.Text = HMIUnitConvert(box.Text);
            }
            preindex = UnitCombox.SelectedIndex;
        }
    }
    public enum SpeedUnitMode
    {
        MmPerSecond,
        MmPerMin,
        MPerSecond,
        MPerMin,
        IncrPerSecond,
        IncrPerMin
    }
}
