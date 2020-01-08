using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace chatroom
{
    public partial class login : Form
    {
        private static MySqlConnection mysql = new MySqlConnection
                ("server=111.229.13.33;User Id=luzihan;password=124152;Database=chat_login");
        public login()
        {
            InitializeComponent();
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
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("用户名或密码错误，请重试！");
                EDI_PASSWD.Clear();
            }
        }

        private bool checkLogin(string name,string passwd)
        {
            mysql.Open();
            string sql = "select * from loginfo where log_name = '";
            sql += name;
            sql += "';";

            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable datatable = new DataTable();
            datatable.Clear();
            adapter.Fill(datatable);

            if (datatable.Rows.Count == 0)
            {
                mysql.Close();
                return false;
            }
            else
            {
                if(passwd == datatable.Rows[0]["log_passwd"].ToString())
                {
                    mysql.Close();
                    return true;
                }
            }
            mysql.Close();
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
    }

    public partial class user : Form
    {
        public static string username = "";   //注意全局变量要使用static 
    }
}
