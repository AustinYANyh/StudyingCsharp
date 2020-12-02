using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using testUnit.Model;

namespace testUnit
{
    class MainWindowViewModel : NotificationObject
    {
        public MainModel Model { get; set; }
        public DelegateCommand<object> SpeedUnitCombox_SelectionChanged { get; set; }
        public DelegateCommand<object> TimeUnitCombox_SelectionChanged { get; set; }
        public DelegateCommand<object> DistanceUnitCombox_SelectionChanged { get; set; }
        public MainWindowViewModel()
        {
            Model = ModelLocator.Locator.Main;

            Task task = new Task(() =>
            {
                RefrushData();
            });
            task.Start();

            SpeedUnitCombox_SelectionChanged = new DelegateCommand<object>((p) =>
            {
                SpeedConvertEvent(p as ComboBox);
            });

            TimeUnitCombox_SelectionChanged = new DelegateCommand<object>((p) =>
            {
                TimeConvertEvent(p as ComboBox);
            });

            DistanceUnitCombox_SelectionChanged = new DelegateCommand<object>((p) =>
            {
                DistanceConvertEvent(p as ComboBox);
            });
        }

        private void SpeedConvertEvent(ComboBox SpeedUnitCombox)
        {
            //转换前准备工作
            UnitConvert.SpeedUnitHelper.speedUnitMode = SpeedUnitCombox.SelectedIndex;

            //速度单位换算
            Model.Speed1 = UnitConvert.SpeedUnitHelper.HMISpeedUnitConvert(Model.Speed1);
            Model.Speed2 = UnitConvert.SpeedUnitHelper.HMISpeedUnitConvert(Model.Speed2);
            Model.Speed3 = UnitConvert.SpeedUnitHelper.HMISpeedUnitConvert(Model.Speed3);
            Model.Speed4 = UnitConvert.SpeedUnitHelper.HMISpeedUnitConvert(Model.Speed4);

            //最后才可以更新上一次速度选择的索引
            if (SpeedUnitCombox.SelectedIndex == -1)
                UnitConvert.SpeedUnitHelper.speedPreindex = 0;
            else
                UnitConvert.SpeedUnitHelper.speedPreindex = SpeedUnitCombox.SelectedIndex;
        }

        private void TimeConvertEvent(ComboBox TimeUnitCombox)
        {
            UnitConvert.TimeUnitHelper.timeUnitMode = TimeUnitCombox.SelectedIndex;

            Model.Time1 = UnitConvert.TimeUnitHelper.HMITimeUnitConvert(Model.Time1);
            Model.Time2 = UnitConvert.TimeUnitHelper.HMITimeUnitConvert(Model.Time2);
            Model.Time3 = UnitConvert.TimeUnitHelper.HMITimeUnitConvert(Model.Time3);
            Model.Time4 = UnitConvert.TimeUnitHelper.HMITimeUnitConvert(Model.Time4);

            if (TimeUnitCombox.SelectedIndex == -1)
                UnitConvert.TimeUnitHelper.timePreindex = 0;
            else
                UnitConvert.TimeUnitHelper.timePreindex = TimeUnitCombox.SelectedIndex;
        }

        private void DistanceConvertEvent(ComboBox DistanceUnitCombox)
        {
            UnitConvert.DistanceUnitHelper.distanceUnitMode = DistanceUnitCombox.SelectedIndex;

            Model.Distance1 = UnitConvert.DistanceUnitHelper.HMIDistanceUnitConvert(Model.Distance1);
            Model.Distance2 = UnitConvert.DistanceUnitHelper.HMIDistanceUnitConvert(Model.Distance2);
            Model.Distance3 = UnitConvert.DistanceUnitHelper.HMIDistanceUnitConvert(Model.Distance3);
            Model.Distance4 = UnitConvert.DistanceUnitHelper.HMIDistanceUnitConvert(Model.Distance4);

            if (DistanceUnitCombox.SelectedIndex == -1)
                UnitConvert.DistanceUnitHelper.distancePreIndex = 0;
            else
                UnitConvert.DistanceUnitHelper.distancePreIndex = DistanceUnitCombox.SelectedIndex;
        }

        public void RefrushData()
        {
            while (true)
            {
                Model.SpeedUnit = UnitConvert.HMI_Unit.SpeedUnitList[Model.CurSpeedMode];
                Model.TimeUnit = UnitConvert.HMI_Unit.TimeUnitList[Model.CurTimeMode];
                Model.DistanceUnit = UnitConvert.HMI_Unit.DistanceUnitList[Model.CurDistanceMode];
                Thread.Sleep(20);
            }
        }
    }
}
