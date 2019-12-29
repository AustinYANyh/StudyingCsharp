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
using System.Net;
using System.IO;

namespace processBar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {

        }

        public void BTN_DOWNLOAD_Click(object sender, EventArgs e)
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

        public void button1_Click(object sender, EventArgs e)
        {
            save s = new save();
            s.Show(this);
        }
    }
}
