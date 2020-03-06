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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int ellipseWidth = 50;
            int ellipseHeight = 40;
            int triangleHeight = 75;

            float x = (this.panel1.Width / 2) - (ellipseWidth / 2) ;
            float y = (this.panel1.Height / 2) - (ellipseWidth / 2) - (ellipseHeight / 2);

            Point pointLeft = new Point((int)x + 1, (int)(y + ellipseHeight / 2));
            Point pointMid = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));
            Point pointRight = new Point((int)(x + ellipseWidth - 1), (int)(y + ellipseHeight / 2));

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
            Image image = panel1.BackgroundImage;
            Bitmap bitmap = getnew(image, 2.0);
            Graphics graphics = Graphics.FromImage(bitmap);
            Bitmap newBitmap = new Bitmap(image.Width, image.Height);
            graphics.DrawImage(newBitmap, new Point((bitmap.Width - image.Width) / 2, (bitmap.Height - image.Height) / 2));
            panel1.BackgroundImage = newBitmap;
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
