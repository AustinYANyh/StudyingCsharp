using System.Windows;
using System.Windows.Controls;
using System;

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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void UnitCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //转换前准备工作
            HMIUnitConvertLib.SpeedUnitHelper.speedUnitMode = UnitCombox.SelectedIndex;

            //速度单位换算
            viewModel.Speed1 = HMIUnitConvertLib.SpeedUnitHelper.HMISpeedUnitConvert(viewModel.Speed1);
            viewModel.Speed2 = HMIUnitConvertLib.SpeedUnitHelper.HMISpeedUnitConvert(viewModel.Speed2);
            viewModel.Speed3 = HMIUnitConvertLib.SpeedUnitHelper.HMISpeedUnitConvert(viewModel.Speed3);
            viewModel.Speed4 = HMIUnitConvertLib.SpeedUnitHelper.HMISpeedUnitConvert(viewModel.Speed4);

            //最后才可以更新上一次速度选择的索引
            if (UnitCombox.SelectedIndex == -1)
                HMIUnitConvertLib.SpeedUnitHelper.speedPreindex = 0;
            else
                HMIUnitConvertLib.SpeedUnitHelper.speedPreindex = UnitCombox.SelectedIndex;
        }
    }
}
