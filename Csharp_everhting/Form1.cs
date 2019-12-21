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
using System.IO;

namespace Csharp_myeverything
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //public string txt
        //{
        //    get { return this.EDI_PATH.Text; }
        //    set { this.EDI_PATH.Text = value; }
        //}

        private void BTN_SHOW_Click(object sender, EventArgs e)
        {
            updataListView();
        }

        private void BTN_CLEAR_Click(object sender, EventArgs e)
        {
            LISTDATA.Items.Clear();
            EDI_NAME.Text = "";
        }

        private void EDI_NAME_TextChanged(object sender, EventArgs e)
        {
            updataListView();
        }

        private void 打开所在路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LISTDATA.SelectedItems.Count > 0)
            {
                string path = LISTDATA.FocusedItem.SubItems[0].Text;
                string name = LISTDATA.FocusedItem.SubItems[1].Text;

                //OpenFileDialog ofd = new OpenFileDialog();

                //打开指定路径
                System.Diagnostics.Process.Start(path);

                //打开指定文件夹并选中文件
                //System.Diagnostics.Process.Start("Explorer", "/select," + path + "\\" + name);
            }
            else
            {
                MessageBox.Show("请选中文件名!", "错误提示");
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LISTDATA.SelectedItems.Count > 0)
            {
                string path = LISTDATA.FocusedItem.SubItems[0].Text;
                string name = LISTDATA.FocusedItem.SubItems[1].Text;

                //打开文件夹中某个文件
                System.Diagnostics.Process.Start(path + "/" + name);
            }
            else
            {
                MessageBox.Show("请选中文件名!", "错误提示");
            }
        }

        //这个按钮必须为public
        public void BTN_SEARCH_Click(object sender, EventArgs e)
        {
            search se = new search();
            se.StartPosition = FormStartPosition.CenterScreen;
            se.Show(this);
        }

        public void updataListView()
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

            this.LISTDATA.BeginUpdate();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    string path = dr["path"].ToString();
                    string name = dr["name"].ToString();
                    string key = EDI_NAME.Text.Trim();
                    //name = name.Replace(key, "<font color=green>" + key + "</font>");
                    ListViewItem item = new ListViewItem(path.Trim());
                    item.SubItems.Add(name.Trim());
                    item.UseItemStyleForSubItems = false;
                    //item.SubItems[1].ForeColor = Color.Green;
                    LISTDATA.Items.Add(item);
                    //LISTDATA.Items[itemIdx].SubItems[0].Text = path;
                    //LISTDATA.Items[itemIdx].SubItems[1].Text = name;

                    //高亮显示
                    //TO DO
                }
            }
            this.LISTDATA.EndUpdate();
            mysql.Close();
        }
    }

    class scan
    {
        public static void GetFolderAndFile(string[] path, List<string> FilePath, string FileName)
        {
            //链接数据库
            MySqlConnection mysql = new MySqlConnection
                ("server=111.229.13.33;User Id=luzihan;password=124152;Database=myeverything");

            foreach (string str in path)
            {
                string[] Next = Directory.GetDirectories(str); //获取当前路径下的所有文件夹
                string[] Files = Directory.GetFiles(str, FileName);

                string NAME = "";
                string PATH = "";
                string temp = "";

                foreach (string Nowpath in Next)
                {    
                    temp = Nowpath;
                    PATH = temp.Substring(0, temp.LastIndexOf('\\'));
                    NAME = temp.Substring(temp.LastIndexOf('\\') + 1);
                    Data.insertSql(PATH, NAME);
                    FilePath.Add(Nowpath);
                }

                foreach (string file in Files)
                {
                    temp = file;
                    PATH = temp.Substring(0, temp.LastIndexOf('\\'));
                    NAME = temp.Substring(temp.LastIndexOf('\\') + 1);
                    Data.insertSql(PATH, NAME);
                    FilePath.Add(file);
                }

                GetFolderAndFile(Next, FilePath, FileName);
            }
        }

    }

    class Data
    {
        static public void insertSql(string path, string name)
        {
            //链接数据库
            MySqlConnection mysql = new MySqlConnection
                ("server=111.229.13.33;User Id=luzihan;password=124152;Database=myeverything");

            string sql = "select * from everything;";

            //打开数据库执行sql语句
            mysql.Open();
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            int lineNo = dataTable.Rows.Count;

            sql = string.Format("insert into everything values({0},'{1}','{2}');", lineNo + 1, path, name);
            cmd = new MySqlCommand(sql, mysql);
            if(cmd.ExecuteNonQuery() < 0)
            {
                MessageBox.Show("插入数据库失败!");
            }
            mysql.Close();
        }
    }
}
