using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

namespace Axis
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool Ismove = false;

        private double XOffset { get { return ui.Width / 2; } }
        private double YOffset { get { return ui.Height / 2; } }
        private void CanvasInPath_MouseMove(object sender, MouseEventArgs e)
        {
            //PositionSpeed.Text = "Speed : " + " " + (e.GetPosition((UIElement)sender).X / 5).ToString();
            //PositionFreq.Text = "Freq : " + " " + (e.GetPosition((UIElement)sender).Y / 5).ToString();
        }

        private void Uic_MouseMove(object sender, MouseEventArgs e)
        {
            if (Ismove == true)
            {
                double realPointx = e.GetPosition(this).X;
                double realPointy = this.ActualHeight - e.GetPosition(this).Y;

                double xpos = (double)e.GetPosition(CanvasInPath).X;
                double ypos = (double)e.GetPosition(CanvasInPath).Y;

                PositionSpeed.Text = (xpos / 5).ToString() + "%";
                PositionFreq.Text = (ypos / 5).ToString() + "%";

                double currentPointx = Convert.ToDouble(PositionSpeed.Text.ToString().Substring(0, PositionSpeed.Text.ToString().IndexOf("%")));
                double currentPointy = Convert.ToDouble(PositionFreq.Text.ToString().Substring(0, PositionFreq.Text.ToString().IndexOf("%")));

                if ((currentPointx <= 0 || currentPointx >= 100) && (currentPointy > 0 && currentPointy < 100))
                {
                    PositionFreq.Text = (ypos / 5).ToString() + "%";
                    Canvas.SetTop((UIElement)sender, ypos - YOffset);
                }
                else if ((currentPointy <= 0 || currentPointy >= 100) && (currentPointx > 0 && currentPointx < 100))
                {
                    PositionSpeed.Text = (xpos / 5).ToString() + "%";
                    Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                }
                else if ((currentPointx > 0 && currentPointx < 100) && (currentPointy > 0 && currentPointy < 100))
                {
                    PositionSpeed.Text = (xpos / 5).ToString() + "%";
                    PositionFreq.Text = (ypos / 5).ToString() + "%";
                    Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                    Canvas.SetTop((UIElement)sender, ypos - YOffset);
                }
                else
                {
                    return;
                }
            }
        }

        Point originPoint;
        private void Uic_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point originPoint = e.GetPosition(sender as FrameworkElement);
            Ismove = true;
        }

        private void Uic_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Ismove = false;
        }

        private void PositionSpeed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Convert.ToDouble(PositionSpeed.Text.ToString().Substring(0, PositionSpeed.Text.ToString().IndexOf("%"))) >= 100)
            {
                PositionSpeed.Text = "100%";
            }
            else if (Convert.ToDouble(PositionSpeed.Text.ToString().Substring(0, PositionSpeed.Text.ToString().IndexOf("%"))) <= 0)
            {
                PositionSpeed.Text = "0%";
            }
        }

        private void PositionFreq_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Convert.ToDouble(PositionFreq.Text.ToString().Substring(0, PositionFreq.Text.ToString().IndexOf("%"))) >= 100)
            {
                PositionFreq.Text = "100%";
            }
            else if (Convert.ToDouble(PositionFreq.Text.ToString().Substring(0, PositionFreq.Text.ToString().IndexOf("%"))) <= 0)
            {
                PositionFreq.Text = "0%";
            }
        }
    }
}