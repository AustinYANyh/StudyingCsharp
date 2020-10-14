using System;
using System.Collections.Generic;
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

namespace testAnimation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Storyboard closeStoryboard;
        public MainWindow()
        {
            InitializeComponent();
            closeStoryboard = (Storyboard)this.FindResource("closeStoryboard");
            closeStoryboard.Completed += (c, d) => Application.Current.Shutdown(); ;
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closeStoryboard.Begin();
            e.Cancel = true;
        }

        private void row1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }// end for class
}// end for namespace
