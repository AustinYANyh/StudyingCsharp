using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Documents;

namespace testLineAttritube
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool IsOpen = false;
        public MainWindowViewModel viewModel;
        public readonly BackgroundWorker backgroundWorker;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
            backgroundWorker = new BackgroundWorker() { WorkerReportsProgress = true };
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            OrderInput.AddItem(new AutoCompleteEntry("上海", null));
            OrderInput.AddItem(new AutoCompleteEntry("北京", null));
            OrderInput.AddItem(new AutoCompleteEntry("济南", null));
            OrderInput.AddItem(new AutoCompleteEntry("青岛", null));
            OrderInput.AddItem(new AutoCompleteEntry("天津", null));
            OrderInput.AddItem(new AutoCompleteEntry("黑龙江", null));
            OrderInput.AddItem(new AutoCompleteEntry("聊城", null));
            OrderInput.AddItem(new AutoCompleteEntry("上班", null));
            OrderInput.AddItem(new AutoCompleteEntry("上天", null));
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //ppp.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 10; ++i)
            {
                backgroundWorker.ReportProgress(i * 10);
                Thread.Sleep(100);
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
            //this.tt.BeginAnimation(TranslateTransform.XProperty, dax);
            //this.tt.BeginAnimation(TranslateTransform.YProperty, day);

            backgroundWorker.RunWorkerAsync();
        }

        private void title_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.MainWindow.DragMove();
        }
        int alts, altd;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource hWndSource;
            WindowInteropHelper wih = new WindowInteropHelper(this);
            hWndSource = HwndSource.FromHwnd(wih.Handle);
            //添加处理程序
            hWndSource.AddHook(MainWindowProc);
            alts = HotKey.GlobalAddAtom("Alt-S");
            altd = HotKey.GlobalAddAtom("Alt-D");
            HotKey.RegisterHotKey(wih.Handle, alts, HotKey.KeyModifiers.Alt, (int)Keys.S);
            HotKey.RegisterHotKey(wih.Handle, altd, HotKey.KeyModifiers.Alt, (int)Keys.D);
        }

        void HighLight()
        {
            TextRange textrange = new TextRange(OrderShow.Document.ContentStart, OrderShow.Document.ContentEnd);
            string[] str = textrange.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int index = 0;
            for (int i = 0; i < str.Count(); ++i)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    textrange = new TextRange(OrderShow.Document.ContentStart, OrderShow.Document.ContentEnd);
                    textrange.ApplyPropertyValue(TextElement.ForegroundProperty, System.Windows.Media.Brushes.Black);
                    textrange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Regular);
                }));
                TextPointer p1 = OrderShow.Selection.Start;
                p1 = p1.GetPositionAtOffset(index);
                index += str[i].Length;
                TextPointer p2 = OrderShow.Selection.Start;
                p2 = p2.GetPositionAtOffset(index);
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    textrange = new TextRange(p1, p2);
                    textrange.ApplyPropertyValue(TextElement.ForegroundProperty, System.Windows.Media.Brushes.Blue);
                    textrange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                }));
                index += 4;
                Thread.Sleep(1000);
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                textrange = new TextRange(OrderShow.Document.ContentStart, OrderShow.Document.ContentEnd);
                textrange.ApplyPropertyValue(TextElement.ForegroundProperty, System.Windows.Media.Brushes.Black);
                textrange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Regular);
            }));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OrderShow.CaretPosition = OrderShow.CaretPosition.DocumentStart;
            Thread thread = new Thread(HighLight);
            thread.IsBackground = true;
            thread.Start();
        }

        private IntPtr MainWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case HotKey.WM_HOTKEY:
                    {
                        int sid = wParam.ToInt32();
                        if (sid == alts)
                        {
                            System.Windows.MessageBox.Show("按下Alt+S");
                        }
                        else if (sid == altd)
                        {
                            System.Windows.MessageBox.Show("按下Alt+D");
                        }
                        handled = true;
                        break;
                    }
            }
            return IntPtr.Zero;
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
