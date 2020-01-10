using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace chatroom
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
            initDic();
        }

        private void initDic()
        {
            string sql = "select * from loginfo;";
            DataTable datatable = MySQL.selectSql(sql);

            foreach(DataRow dr in datatable.Rows)
            {
                user.dic.Add(dr["log_ip"].ToString(), dr["log_name"].ToString());
            }
        }

        private void EDI_PASSWD_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Enter:
                    Func_login();
                    break;
            }
        }

        private void Func_login()
        {
            if (checkLogin(EDI_USERNAME.Text, EDI_PASSWD.Text) == true)
            {
                user.username = EDI_USERNAME.Text;

                string sql = string.Format("select * from loginfo where log_name = '{0}';",user.username);
                DataTable datatable = MySQL.selectSql(sql);

                //如果外网IP变化,更新数据库
                if (datatable.Rows[0]["log_netip"].ToString() != user.GetIP())
                {
                    sql = string.Format("update loginfo set log_netip = '{0}' where log_name = '{1}';", user.GetIP(), datatable.Rows[0]["log_name"]);

                    if (MySQL.executeSql(sql) == false)
                    {
                        MessageBox.Show("外网IP地址更新失败...");
                    }
                }
                user.mesdic.Add(datatable.Rows[0]["log_netip"].ToString(), datatable.Rows[0]["log_name"].ToString());

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                EDI_PASSWD.Clear();
            }
        }

        private bool checkLogin(string name,string passwd)
        {
            string sql = "select * from loginfo where log_name = '";
            sql += name;
            sql += "';";

            DataTable datatable = MySQL.selectSql(sql);

            if (datatable.Rows.Count == 0)
            {
                MessageBox.Show("用户名不存在，请重试！");
                return false;
            }
            else
            {
                if(passwd == datatable.Rows[0]["log_passwd"].ToString())
                {
                    if (user.GetLocalIp() == datatable.Rows[0]["log_ip"].ToString())
                    {
                        return true;
                    }
                    else
                        MessageBox.Show("此主机IP地址与用户名绑定的不一致,请重试...");
                        return false;
                }
                else
                {
                    MessageBox.Show("密码错误，请重试！");
                    return false;
                }
            }
            return false;
        }

        private void BTN_LOGIN_Click(object sender, EventArgs e)
        {
            Func_login();
        }

        private void BTN_LOGON_Click(object sender, EventArgs e)
        {
            logon log_on = new logon();
            log_on.ShowDialog();
        }

        private void login_Load(object sender, EventArgs e)
        {
            LAB_USERNAME.Parent = this;
            LAB_PASSWD.Parent = this;
            LAB_USERNAME.BackColor = Color.Transparent;
            LAB_PASSWD.BackColor = Color.Transparent;
        }
    }

    public partial class user : Form
    {
        public static string username = "";   //注意全局变量要使用static
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

    public partial class MySQL : Form
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
