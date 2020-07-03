using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testLineAttritube
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly BackgroundWorker backgroundWorker;
        public MainWindow()
        {
            InitializeComponent();
            backgroundWorker = new BackgroundWorker() { WorkerReportsProgress = true };
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ppp.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 10; ++i)
            {
                backgroundWorker.ReportProgress(i * 10);
                Thread.Sleep(500);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation dax = new DoubleAnimation();
            DoubleAnimation day = new DoubleAnimation();
            dax.From = 0;
            day.From = 0;

            //设置反弹
            BounceEase be = new BounceEase();
            //设置反弹次数为3
            be.Bounces = 3;
            be.Bounciness = 3;//弹性程度，值越大反弹越低
            day.EasingFunction = be;

            //设置终点
            dax.To = 300;
            day.To = 150;

            //指定时长
            Duration duration = new Duration(TimeSpan.FromMilliseconds(2000));
            dax.Duration = duration;
            day.Duration = duration;
            //动画主体是TranslatTransform变形，而非Button
            this.tt.BeginAnimation(TranslateTransform.XProperty, dax);
            this.tt.BeginAnimation(TranslateTransform.YProperty, day);

            backgroundWorker.RunWorkerAsync();
        }
    }

    public class LocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - 25;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
