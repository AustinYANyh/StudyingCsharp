using System;
using System.Threading;

//数据绑定写法
public class StatusBarPageViewModel : NotificationObject
{
    public StatusBarPageViewModel()
    {
        Thread thread = new Thread(UpdateNowTime);
        thread.IsBackground = true;
        thread.Start();
    }

    void UpdateNowTime()
    {
        while (true)
        {
            NowTime = DateTime.Now.ToString();
            Thread.Sleep(1000);
        }
    }

    private string _NowTime;
    public string NowTime
    {
        get { return _NowTime; }
        set
        {
            _NowTime = value;
            RaisePropertyChanged("NowTime");
        }
    }
}

//计时器修改控件Text写法
public partial class CommonLog : UserControl
{
    CommonLogViewModel viewModel;

    public CommonLog()
    {
        InitializeComponent();
        viewModel = new CommonLogViewModel();
        this.DataContext = viewModel;

        System.Timers.Timer timer = new System.Timers.Timer(1000);
        timer.AutoReset = true;
        timer.Elapsed += Timer_Elapsed;
        timer.Enabled = true;

        viewModel.InitTime(status_info);
    }

    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        NowTime.Dispatcher.BeginInvoke(new Action(delegate
        {
            NowTime.Text = DateTime.Now.ToString();
        }));
    }
}