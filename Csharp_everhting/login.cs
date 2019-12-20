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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void BTN_LOGIN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void login_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.F1:
                    MessageBox.Show("好玩吗?快捷键!", "提示");
                    break;
            }
        }

        private void BTN_LOGCANCEL_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
