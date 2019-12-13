using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Csharp_myeverything
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BTN_SHOW_Click(object sender, EventArgs e)
        {
            if (EDI_PATH.Text.Trim() == "")
            { //链接数据库
                MySqlConnection mysql = new MySqlConnection
                    ("server=111.229.13.33;User Id=luzihan;password=124152;Database=myeverything");

                string sql = "select * from everything;";

                //打开数据库执行sql语句
                mysql.Open();
                MySqlCommand cmd = new MySqlCommand(sql, mysql);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                DataTable dataTable = new DataTable();

                //填充结果集
                dataTable.Clear();
                adapter.Fill(dataTable);

                this.LISTDATA.BeginUpdate();

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        string path = dr["path"].ToString();
                        string name = dr["name"].ToString();
                        ListViewItem item = new ListViewItem(path.Trim());
                        item.SubItems.Add(name.Trim());
                        LISTDATA.Items.Add(item);
                        //LISTDATA.Items[itemIdx].SubItems[0].Text = path;
                        //LISTDATA.Items[itemIdx].SubItems[1].Text = name;
                    }
                }

                this.LISTDATA.EndUpdate();
                mysql.Close();
            }
            else
            {
                MessageBox.Show("扫描功能暂未开放...", "提示信息");
            }
        }

        private void BTN_CLEAR_Click(object sender, EventArgs e)
        {
            LISTDATA.Items.Clear();
            EDI_PATH.Text = "";
        }

        private void BTN_CLEAR2_Click(object sender, EventArgs e)
        {
            LISTDATA.Items.Clear();
            EDI_NAME.Text = "";
        }

        private void EDI_NAME_TextChanged(object sender, EventArgs e)
        {
            //链接数据库
            MySqlConnection mysql = new MySqlConnection
                ("server=111.229.13.33;User Id=luzihan;password=124152;Database=myeverything");

            string sql = "select * from everything where name like '%";
            sql += EDI_NAME.Text.Trim();
            sql += "%';";

            //打开数据库执行sql语句
            mysql.Open();
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            DataTable dataTable = new DataTable();

            //填充结果集
            dataTable.Clear();
            adapter.Fill(dataTable);
            LISTDATA.Items.Clear();

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    string path = dr["path"].ToString();
                    string name = dr["name"].ToString();
                    ListViewItem item = new ListViewItem(path.Trim());
                    item.SubItems.Add(name.Trim());
                    LISTDATA.Items.Add(item);
                    //LISTDATA.Items[itemIdx].SubItems[0].Text = path;
                    //LISTDATA.Items[itemIdx].SubItems[1].Text = name;
                }
            }

            this.LISTDATA.EndUpdate();
            mysql.Close();
        }

        private void 打开所在路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = LISTDATA.FocusedItem.SubItems[0].Text;
            string name = LISTDATA.FocusedItem.SubItems[1].Text;

            //OpenFileDialog ofd = new OpenFileDialog();

            //打开指定路径
            System.Diagnostics.Process.Start(path);

            //打开指定文件夹并选中文件
            //System.Diagnostics.Process.Start("Explorer", "/select," + path + "\\" + name);
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = LISTDATA.FocusedItem.SubItems[0].Text;
            string name = LISTDATA.FocusedItem.SubItems[1].Text;

            //打开文件夹中某个文件
            System.Diagnostics.Process.Start(path + "/" + name);
        }
    }
}
