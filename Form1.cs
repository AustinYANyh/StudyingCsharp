using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace processBar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BTN_DOWNLOAD_Click(object sender, EventArgs e)
        {
            double i = 0;
            while(i<200)
            {
                i++;
                PRO_PROCESS.Value = (int)i;
                double percent = Math.Round((i / PRO_PROCESS.Maximum) * 100, 2);

                Application.DoEvents(); 
                Thread.Sleep(500);
                this.LAB_PERCENT.Text = percent.ToString() + "%";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            while(i<1000)
            {
                i++;
                LAB_PERCENT.Text = i.ToString();
            }
        }
    }
}
