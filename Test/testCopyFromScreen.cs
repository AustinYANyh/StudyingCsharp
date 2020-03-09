using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyFromScreen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile("./1.jpg");
            pictureBox2.Image = Image.FromFile("./2.jpg");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap catchBmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(catchBmp);

            Point screenPoint = PointToScreen(pictureBox2.Location);

            g.CopyFromScreen(screenPoint, new Point(0, 0), new Size(pictureBox1.Width, pictureBox1.Height));

            pictureBox1.Image = catchBmp;
        }

    }
}
