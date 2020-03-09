using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace makeBig
{
    public partial class Form1 : Form
    {
        float a = 0.5f;
        double beishu = 1.00;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Point pointLeft = new Point(364, 168);
            Point pointMid = new Point(388,243);
            Point pointRight = new Point(412,168);

            Bitmap bitmap = new Bitmap(panel1.Width, panel1.Height);

            Graphics graphics = Graphics.FromImage(bitmap);

            Pen pen = new Pen(Color.Blue, 1);

            graphics.DrawLine(pen, pointLeft, pointMid);
            graphics.DrawLine(pen, pointMid, pointRight);
            graphics.DrawLine(pen, pointLeft, pointRight);

            panel1.BackgroundImage = bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();

            //graphics.Clear(Panel.DefaultBackColor);

            beishu += 0.5;
            Point pointMid = new Point(388, 243);

            Point pointLeft = new Point(Convert.ToInt32(pointMid.X - beishu * 24), Convert.ToInt32(pointMid.Y - beishu * 75));
            Point pointRight = new Point(Convert.ToInt32(pointMid.X + beishu * 24), Convert.ToInt32(pointMid.Y - beishu * 75));

            Pen pen = new Pen(Color.Red, 1);

            graphics.DrawLine(pen, pointLeft, pointMid);
            graphics.DrawLine(pen, pointMid, pointRight);
            graphics.DrawLine(pen, pointLeft, pointRight);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = this.panel1.CreateGraphics();

            g.DrawRectangle(new Pen(Color.Red, 1), new Rectangle(0, 0, 100, 20));

            g.TranslateTransform(0, 0);

            g.RotateTransform(Convert.ToInt32(textBox1.Text.Trim()));

            g.DrawRectangle(new Pen(Color.BurlyWood, 1), new Rectangle(0, 0, 100, 20));

            textBox1.Clear();
        }


        public Bitmap getnew(Image bit, double beishu)//beishu参数为放大的倍数。放大缩小都可以，0.8即为缩小至原来的0.8倍
        {
            Bitmap destBitmap = new Bitmap(Convert.ToInt32(bit.Width * beishu), Convert.ToInt32(bit.Height * beishu));
            Graphics g = Graphics.FromImage(destBitmap);
            g.Clear(Color.Transparent);
            //设置画布的描绘质量           
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(bit, new Rectangle(0, 0, destBitmap.Width, destBitmap.Height), 0, 0, bit.Width, bit.Height, GraphicsUnit.Pixel);
            g.Dispose();
            return destBitmap;
        }
    }
}
