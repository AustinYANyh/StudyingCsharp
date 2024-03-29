﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace chatroom
{
    public partial class login : Form
    {
        public static List<TextBox> textList = new List<TextBox>();
        public static List<Label> labList = new List<Label>();
        public static List<Button> btnList = new List<Button>();

        public login()
        {
            InitializeComponent();
            EDI_PASSWD.AutoSize = false;
            EDI_PASSWD.Height = EDI_USERNAME.Height;
            initDic();
        }

        private void login_Load(object sender, EventArgs e)
        {
            EDI_USERNAME.Visible = false;
            EDI_PASSWD.Visible = false;
            LAB_USERNAME.Visible = false;        
            LAB_PASSWD.Visible = false;
            BTN_LOGON.Visible = false;
            BTN_LOGIN.Visible = false;

            btnList.Add(BTN_LOGON);
            btnList.Add(BTN_LOGIN);
            labList.Add(LAB_USERNAME);
            labList.Add(LAB_PASSWD);
            textList.Add(EDI_USERNAME);
            textList.Add(EDI_PASSWD);

            LAB_USERNAME.Parent = this;
            LAB_PASSWD.Parent = this;
            LAB_USERNAME.BackColor = Color.Transparent;
            LAB_PASSWD.BackColor = Color.Transparent;

            this.Size = new Size(654, 558);
            Bitmap bitmap = new Bitmap(Image.FromFile("./背景.jpg"), this.Size);
            this.BackgroundImage = bitmap;
            this.Width = 467;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {      
            EDI_USERNAME.Visible = true;
            LAB_USERNAME.Visible = true;
            timer2.Enabled = true;
            timer1.Enabled = false;      
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            EDI_PASSWD.Visible = true;
            LAB_PASSWD.Visible = true;
            timer3.Enabled = true;
            timer2.Enabled = false;
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            BTN_LOGIN.Visible = true;
            BTN_LOGON.Visible = true;
            timer3.Enabled = false;
        }

        private void initDic()
        {
            string sql = "select * from loginfo;";
            DataTable datatable = MySQL.selectSql(sql);

            foreach (DataRow dr in datatable.Rows)
            {
                user.dic.Add(dr["log_ip"].ToString(), dr["log_name"].ToString());
                //user.mesdic.Add(dr["log_netip"].ToString(), dr["log_name"].ToString());
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
            user.username = EDI_USERNAME.Text;

            if (checkLogin(EDI_USERNAME.Text, EDI_PASSWD.Text) == true)
            {
                //user.mesdic.Add(datatable.Rows[0]["log_netip"].ToString(), datatable.Rows[0]["log_name"].ToString());
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                EDI_PASSWD.Clear();
                return;
            }

            user.dic.Clear();
            //user.mesdic.Clear();
            initDic();
        }

        private bool checkLogin(string name,string passwd)
        {
            string sql = "select * from loginfo where log_name = '";
            sql += name;
            sql += "';";

            DataTable datatable = MySQL.selectSql(sql);

            if (datatable.Rows.Count == 0)
            {
                //MessageBox.Show("用户名不存在，请重试！");
                MessageBox.Show("用户名不存在，请重试！\r\nUser name doesnot exist，please try again！");
                return false;
            }
            else
            {
                if(passwd != datatable.Rows[0]["log_passwd"].ToString())
                {
                    //MessageBox.Show("密码错误，请重试！");
                    MessageBox.Show("密码错误，请重试！\r\nWrong password,please try again");
                    return false;
                }
                else
                {
                    if(user.GetLocalIp() != datatable.Rows[0]["log_ip"].ToString())
                    {
                        //MessageBox.Show("IP地址与用户名绑定的不一致，请重试...");
                        MessageBox.Show("IP地址与用户名绑定的不一致，请重试...\r\nThe IP address is inconsistent with the username binding,please try again...\n\n注册界面点击重新绑定即可解决!~");
                        return false;
                    }
                }
            }
            return true;
        }

        private void BTN_LOGIN_Click(object sender, EventArgs e)
        {
            Func_login();
        }

        private void BTN_LOGON_Click(object sender, EventArgs e)
        {
            logon log_on = new logon();
            log_on.StartPosition = FormStartPosition.CenterParent;
            log_on.ShowDialog();
        }

        private void EDI_USERNAME_TextChanged(object sender, EventArgs e)
        {
            user.username = EDI_USERNAME.Text;
        }
    }

    public partial class user : Form
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
