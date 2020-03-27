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
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.Text.RegularExpressions;

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

            
            if (checkLogin(username.Text, userpasswd.Password.ToString()) == true)
            {
                loginInfo.Content = "登录中";

                timertmp.Elapsed += Timertmp_Elapsed;
                timertmp.Enabled = true;
            }
            else
            {
                //loginInfo.Content = "密码错误...";   
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

        private bool checkLogin(string name, string passwd)
        {
            string sql = "select * from loginfo where log_name = '";
            sql += name;
            sql += "';";

            DataTable datatable = MySQL.selectSql(sql);

            if (datatable.Rows.Count == 0)
            {
                //MessageBox.Show("用户名不存在，请重试！");
                //MessageBox.Show("用户名不存在，请重试！\r\nUser name doesnot exist，please try again！");
                loginInfo.Content = "用户名不存在，请重试！";
                return false;
            }
            else
            {
                if (passwd != datatable.Rows[0]["log_passwd"].ToString())
                {
                    //MessageBox.Show("密码错误，请重试！");
                    //MessageBox.Show("密码错误，请重试！\r\nWrong password,please try again");
                    loginInfo.Content = "密码错误，请重试！";
                    return false;
                }
                else
                {
                    if (user.GetLocalIp() != datatable.Rows[0]["log_ip"].ToString())
                    {
                        //MessageBox.Show("IP地址与用户名绑定的不一致，请重试...");
                        //MessageBox.Show("IP地址与用户名绑定的不一致，请重试...\r\nThe IP address is inconsistent with the username binding,please try again...\n\n注册界面点击重新绑定即可解决!~");
                        loginInfo.Content = "IP地址与用户名绑定的不一致，请重试...\r\nThe IP address is inconsistent with the username binding,please try again...\n\n注册界面点击重新绑定即可解决!~";
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public partial class user : Window
    {
        public static string username = "";   //注意全局变量要使用static
        public static int mesLength = 0;
        public static Dictionary<string, string> dic = new Dictionary<string, string>();
        public static Dictionary<string, string> mesdic = new Dictionary<string, string>();

        /// <summary>
        /// 获取外网ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageDate = webClient.DownloadData("http://pv.sohu.com/cityjson?ie=utf-8");
                    String ip = Encoding.UTF8.GetString(pageDate);
                    webClient.Dispose();

                    Match rebool = Regex.Match(ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    return rebool.Value;
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 获取ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
    }

    public partial class MySQL : Window
    {
        private static MySqlConnection mysql = new MySqlConnection
                ("server=111.229.13.33;User Id=luzihan;password=124152;Database=chat_login");
        public static DataTable selectSql(string sql)
        {
            DataTable datatable = new DataTable();
            try
            {
                mysql.Open();
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                datatable.Clear();
                adapter.Fill(datatable);

                mysql.Close();
                return datatable;
            }
            catch (MySqlException err)
            {
                MessageBox.Show(err.ToString());
                return datatable;
            }
        }

        public static bool executeSql(string sql)
        {
            try
            {
                mysql.Open();
                MySqlCommand cmd = new MySqlCommand(sql, mysql);

                if (cmd.ExecuteNonQuery() < 0)
                {
                    return false;
                }
                mysql.Close();
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException err)
            {
                MessageBox.Show(err.ToString());
                return false;
            }
        }
    }
}
