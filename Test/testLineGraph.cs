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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reactive.Linq;

namespace stduyCurve
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = new CancellationToken();
        public MainWindow()
        {
            InitializeComponent();
            token = source.Token;
            //线程中更新曲线
            Task task = new Task(() =>
            {
                int nPointNum = 100;
                Random randm = new Random();
                double[] dArray = new double[nPointNum];
                double[] dX = new double[nPointNum];
                double[] dY = new double[nPointNum];
                double dRandomtTmp = 0;

                while (true)
                {
                    if (token.IsCancellationRequested)
                        return;
                    for (int n = 0; n < dArray.Length; n++)
                    {
                        dRandomtTmp = randm.NextDouble();
                        dArray[n] = (dRandomtTmp < 0.5) ? -dRandomtTmp * dArray.Length : dRandomtTmp * dArray.Length;
                    }
                    for (int n = 0; n < dX.Length; n++)
                    {
                        dX[n] = n;
                        dY[n] = randm.Next(dX.Length);
                    }

                    //更新UI
                    Dispatcher.Invoke(new Action(delegate
                    {
                        //this.BarChart.PlotBars(dArray);
                        this.LineChart.Plot(dX, dY);
                    }));
                    Thread.Sleep(1000);//每秒刷新一次
                }
            }, token);
            task.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            source.Cancel();
        }
    }
}
