using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Csharp_myeverything
{
    public partial class search : Form
    {
        public search()
        {
            InitializeComponent();
        }

        private void BTN_CANCEL_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BTN_SURE_Click(object sender, EventArgs e)
        {
            string path = Search_EDI_PATH.Text.Trim();
            //Form1 frm = new Form1();
            //frm.txt = path;
            //frm.Show();
            //EDI_PATH.Text = path;
            Form1 frm = (Form1)this.Owner;
            //frm.EDI_PATH.Text = path;

            //MessageBox.Show("扫描功能暂未开放...", "提示信息");
            List<string> vector = new List<string>();
            string[] start = { path };
            scan.GetFolderAndFile(start, vector, "*.*");

            //扫描完成,更新listview
            frm.updataListView();
            this.Close();
        }

        private void Search_EDI_PATH_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Enter:
                    BTN_SURE_Click(sender, e);
                    break;
            }
        }
    }
}
