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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testScale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Outside_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point p = e.GetPosition(outside);
            TransformGroup tg = inside.RenderTransform as TransformGroup;
            if (tg == null)
                tg = new TransformGroup();

            if (e.Delta > 0)
            {
                tg.Children.Add(new ScaleTransform(1.4, 1.4, p.X, p.Y));  //centerX和centerY用外部包装元素的坐标，不能用内部被变换的Canvas元素的坐标
            }
            else
            {
                tg.Children.Add(new ScaleTransform(0.6, 0.6, p.X, p.Y));  //centerX和centerY用外部包装元素的坐标，不能用内部被变换的Canvas元素的坐标
            }

            inside.RenderTransform = tg;
        }
    }
}
