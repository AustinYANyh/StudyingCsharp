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
            string sql = "";
            string name = EDI_LOGON_NAME.Text.Trim();
            string passwd = EDI_LOGON_PASSWD.Text.Trim();

            if(name.Length <= 0 || passwd.Length <= 0)
            {
                MessageBox.Show("用户名或密码不合法,请重试...","提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if(EDI_LOGON_IP.Text.Trim().Length <= 0)
            {
                MessageBox.Show("IP地址为必填项!...","提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                sql = string.Format("select * from loginfo where log_name ='{0}';", name);
                DataTable datatable = MySQL.selectSql(sql);

                if (datatable.Rows.Count > 0)
                {
                    MessageBox.Show("用户名已存在,请直接登录...");
                    return;
                }

                sql = string.Format("select * from loginfo where log_ip ='{0}';", EDI_LOGON_IP.Text.Trim());
                datatable.Clear();
                datatable = MySQL.selectSql(sql);

                if (datatable.Rows.Count > 0)
                {
                    MessageBox.Show("ip地址已注册,请直接登录...");
                    return;
                }
            }
            catch(MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("sql 语句语法错误...");
                return;
            }
            
            sql = string.Format("insert into loginfo values('{0}','{1}');", name, passwd);

            if (MySQL.executeSql(sql) == false)
            {
                MessageBox.Show("插入数据库失败!");
                return;
            }
            
            this.Close();
        }

        private void BTN_LOGON_IPCLICK_Click(object sender, EventArgs e)
        {
            EDI_LOGON_IP.Text = user.GetLocalIp();
        }
    }
}
