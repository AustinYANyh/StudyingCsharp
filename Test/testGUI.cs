using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            Graphics graphics = panel1.CreateGraphics();

            Rectangle rectangle = new Rectangle(50, 50, 30, 20);
            graphics.DrawRectangle(new Pen(Brushes.Red), rectangle);

            //graphics = richTextBox1.CreateGraphics();

            rectangle = new Rectangle(2, 0, 20, 25);
            graphics.DrawRectangle(new Pen(Brushes.Red), rectangle);
            */
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitList();
        }

        private void LAB_GROUP1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Visible == false)
            {
                LAB_GRP1_NAME1.Visible = true;
                LAB_GRP1_NAME2.Visible = true;
                pictureBox1.Visible = true;
                pictureBox2.Visible = true;

                LAB_GROUP2.Location = new Point(LAB_GROUP1.Location.X, pictureBox2.Location.Y + 30);
            }
            else
            {
                InitList();
            }
        }

        private void InitList()
        {
            LAB_GRP1_NAME1.Visible = false;
            LAB_GRP1_NAME2.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;

            LAB_GROUP2.Location = new Point(LAB_GROUP1.Location.X, LAB_GROUP1.Location.Y + 30);
        }
    }
}
