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
    public partial class logon : Form
    {
        private static MySqlConnection mysql = new MySqlConnection
                ("server=111.229.13.33;User Id=luzihan;password=124152;Database=chat_login");
        public logon()
        {
            InitializeComponent();
        }

        private void BTN_NO_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BTN_YES_Click(object sender, EventArgs e)
        {
            string name = EDI_LOGON_NAME.Text.Trim();
            string passwd = EDI_LOGON_PASSWD.Text.Trim();

            if(name.Length <= 0 || passwd.Length <= 0)
            {
                MessageBox.Show("用户名或密码不合法,请重试...");
            }

            mysql.Open();

            string sql = "";
            sql = string.Format("insert into loginfo values('{0}','{1}');", name, passwd);

            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            if (cmd.ExecuteNonQuery() < 0)
            {
                MessageBox.Show("插入数据库失败!");
            }
            
            mysql.Close();
            this.Close();
        }
    }
}
