using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chatroom
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void EDI_PASSWD_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Enter:
                    if (user.username == EDI_USERNAME.Text && user.passwd == EDI_PASSWD.Text)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    break;
            }
        }
    }

    //懒,所以用户名和密码写死
    public partial class user : Form
    {
        public static string username = "luzihan";   //注意全局变量要使用static 
        public static string passwd = "zhu";
    }
}
