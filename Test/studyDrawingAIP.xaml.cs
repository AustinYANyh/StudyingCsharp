using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
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
        }
    }
}
