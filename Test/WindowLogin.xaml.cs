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
using System.Windows.Shapes;
using System.Threading;
using MaterialDesignThemes.Wpf;

namespace studyWPF_1
{
    /// <summary>
    /// WindowLogin.xaml 的交互逻辑
    /// </summary>
    public partial class WindowLogin : Window
    {
        public System.Timers.Timer timer = new System.Timers.Timer(1000);
        public System.Timers.Timer timertmp = new System.Timers.Timer(1000);

        public WindowLogin()
        {
            InitializeComponent();
        }

        private void BTN_login_Click(object sender, RoutedEventArgs e)
        {
            timer.Elapsed += new System.Timers.ElapsedEventHandler(T_Elapsed);
            timer.Enabled = true;

            if (username.Text == "123" && userpasswd.Password.ToString() == "123")
            {
                loginInfo.Content = "登录中";

                timertmp.Elapsed += Timertmp_Elapsed;
                timertmp.Enabled = true;
            }
            else
            {
                loginInfo.Content = "密码错误...";   
            }
        }

        private void Timertmp_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                MainWindow mainWindow = new MainWindow();
                this.Hide();
                mainWindow.Show();

                timer.Enabled = false;
                timertmp.Enabled = false;
            }));
            
        }

        private void T_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            }));
            timer.Enabled = false;
        }
    }
}
