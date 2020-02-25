using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FriendLIst
{
    public partial class messageBox : Form
    {
        public static Image picimageMe;
        public static Image picimageYou;
        public static Panel panelBase = new Panel();
        public messageBox()
        {
            InitializeComponent();
            createPicYou();
            createPicMe();
            panelBase.AutoScroll = false;
            panelBase.HorizontalScroll.Enabled = false;
            this.panel1.HorizontalScroll.Enabled = false;
            panelBase.Height = 0;
            panelBase.Width = this.Width-35;
        }

        public void createPicMe()
        {
            Image image = Image.FromFile("./头像.png");
            Point pic = new Point(3, 3 + (MessMa.messCount) * 70);
            Rectangle picRec = new Rectangle(pic.X, pic.Y, 64, 64);
            picimageMe = CutEllipse(image, picRec, new Size(64, 64));
        }

        public void createPicYou()
        {
            Image image = Image.FromFile("./鹿.png");
            Point pic = new Point(3, 3 + (MessMa.messCount) * 70);
            Rectangle picRec = new Rectangle(pic.X, pic.Y, 64, 64);
            picimageYou = CutEllipse(image, picRec, new Size(64, 64));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            /*
            if(MessMa.messCount == 0)
            {
                PictureBox pic = new PictureBox();
                pic.Image = Image.FromFile("./头像.png");
                pic.Location = new Point(3, 3);
                pic.Size = new Size(64, 64);
                MessMa.pointOfPic = pic.Location;
                MessMa.picHeight = pic.Height;
                this.panel1.Controls.Add(pic);
                pic.BringToFront();

                Label lab = new Label();
                lab.Text = "闫云皓";
                lab.Font = new System.Drawing.Font("宋体", 12, FontStyle.Regular);
                lab.Location = new Point(pic.Location.X + pic.Width + 3, pic.Location.Y);
                MessMa.pointOfLab = lab.Location;
                this.panel1.Controls.Add(lab);
                lab.BringToFront();

                //toolTip1.SetToolTip(lab, "我爱你呀,鹿宝宝!~");
                //toolTip1.ShowAlways = true;
                //toolTip1.Show("我爱你呀,鹿宝宝!~", lab, 0, 25); 

                string mess = richTextBox1.Text;
                richTextBox1.Clear();
                Graphics g = this.panel1.CreateGraphics();
                SizeF sizeF = g.MeasureString(mess, new Font("宋体", 17));
                Point point = new Point(lab.Location.X +5 ,lab.Location.Y + 25);
                Rectangle rec = new Rectangle(point,new Size((int)sizeF.Width,(int)sizeF.Height + 3));
                MessMa.stringRecPoint = point;
                Pen pen = new Pen(Color.Blue,3);

                SolidBrush br = new SolidBrush(Color.Blue);
                g.DrawRectangle(pen, rec);
                //g.FillRectangle(br, rec);
                //br = new SolidBrush(Color.Black);   
                TextureBrush tbr = new TextureBrush(Image.FromFile("./文本填充.png"));
                //g.DrawString(mess,new Font("宋体",17,FontStyle.Regular,GraphicsUnit.Pixel),br,point.X+5,point.Y+5);
                g.DrawString(mess, new Font("宋体", 17, FontStyle.Regular), br, point.X, point.Y + 5);
            
                //FillRound(rec, g, br, 5);
            }
            else
            {
                Graphics g = this.panel1.CreateGraphics();
                //g.DrawImage(Image.FromFile("./头像.png"), new Point(3, 3 + (MessMa.messCount * 35)));//
                g.DrawImage(Image.FromFile("./头像.png"), 3, 10 + ((MessMa.pointOfPic.Y + MessMa.picHeight) * MessMa.messCount) +3,64,64);
                SolidBrush br = new SolidBrush(Color.Black);
                g.DrawString("闫云皓", new Font("宋体", 12, FontStyle.Regular), br,MessMa.pointOfLab.X, MessMa.pointOfLab.Y + (MessMa.messCount * 70));

                string mess = richTextBox1.Text;
                richTextBox1.Clear();
                SizeF sizeF = g.MeasureString(mess, new Font("宋体", 17));
                Point point = new Point(MessMa.stringRecPoint.X, MessMa.stringRecPoint.Y + (MessMa.messCount * 73));
                Rectangle rec = new Rectangle(point, new Size((int)sizeF.Width, (int)sizeF.Height + 3));
                Pen pen = new Pen(Color.Blue, 3);

                br = new SolidBrush(Color.Blue);
                g.DrawRectangle(pen, rec);
                //g.FillRectangle(br, rec);
                //br = new SolidBrush(Color.Black);   
                TextureBrush tbr = new TextureBrush(Image.FromFile("./文本填充.png"));
                //g.DrawString(mess,new Font("宋体",17,FontStyle.Regular,GraphicsUnit.Pixel),br,point.X+5,point.Y+5);
                g.DrawString(mess, new Font("宋体", 17, FontStyle.Regular), br, point.X, point.Y + 5);
            }
            MessMa.messCount += 1;
        */
            panelBase.Height += 70;

            Point newPoint = new Point(0, 0);
            this.panel1.AutoScrollPosition = newPoint;

            PictureBox pic = new PictureBox();
            pic.Image = Image.FromFile("./头像.png");
            
            pic.Location = new Point(3, 3 + (MessMa.messCount)*70);
            Rectangle picRec = new Rectangle(pic.Location.X,pic.Location.Y,64,64);
            
            pic.Image = picimageMe;
            pic.Size = new Size(64, 64);
            MessMa.pointOfPic = pic.Location;
            MessMa.picHeight = pic.Height;
            //this.panel1.Controls.Add(pic);
            panelBase.Controls.Add(pic);
            pic.BringToFront();

            Label lab = new Label();
            
            string temp = "闫云皓" + ":" + DateTime.Now.ToString();
            lab.Text = temp;
            lab.Font = new System.Drawing.Font("宋体", 12, FontStyle.Regular);
            lab.Location = new Point(pic.Location.X + pic.Width + 3, pic.Location.Y);
            MessMa.pointOfLab = lab.Location;
            //this.panel1.Controls.Add(lab);
            panelBase.Controls.Add(lab);
            lab.BringToFront();
            lab.Visible = false;
            //toolTip1.SetToolTip(lab, "我爱你呀,鹿宝宝!~");
            //toolTip1.ShowAlways = true;
            //toolTip1.Show("我爱你呀,鹿宝宝!~", lab, 0, 25); 

            string mess = richTextBox1.Text;
            MessMa.messList.Add(mess);
            richTextBox1.Clear();
            //Graphics g = this.panel1.CreateGraphics();
            Graphics g = panelBase.CreateGraphics();
            SizeF sizeF = g.MeasureString(mess, new Font("宋体", 17));
            MessMa.stringWidth.Add((int)sizeF.Width);
            MessMa.stringHeight.Add((int)sizeF.Height);
            //要画的气泡框的point
            Point point = new Point(lab.Location.X + 5, lab.Location.Y + 25);
            MessMa.pointList.Add(point);
            Rectangle rec = new Rectangle(point, new Size((int)sizeF.Width, (int)sizeF.Height + 3));
            MessMa.stringRecPoint = point;
            Pen pen = new Pen(Color.Blue, 3);

            SolidBrush br = new SolidBrush(Color.Blue);  
            TextureBrush tbr = new TextureBrush(Image.FromFile("./文本填充.png"));
            //g.DrawString(mess,new Font("宋体",17,FontStyle.Regular,GraphicsUnit.Pixel),br,point.X+5,point.Y+5);
            //g.DrawString(mess, new Font("宋体", 17, FontStyle.Regular), br, point.X, point.Y + 5);
            MessMa.messCount += 1;

            RichTextBoxEx ricBox = new RichTextBoxEx();
            ricBox.Font = new System.Drawing.Font("宋体", 17, FontStyle.Regular);
            ricBox.Text = "  " + mess;
            ricBox.Location = new Point(20, 10);
            ricBox.WordWrap = true;
            ricBox.ReadOnly = true;
            ricBox.BorderStyle = BorderStyle.None;
            ricBox.ScrollBars = RichTextBoxScrollBars.None;
            ricBox.Location = new Point(10, 10);
            ricBox.ForeColor = Color.FromArgb(255, 255, 255);
            ricBox.BackColor = Color.FromArgb(90, 143, 0);
            ricBox.Dock = DockStyle.Fill;

            Panel BBB = new Panel();
            BBB.BackColor = Color.FromArgb(90, 143, 0);
            BBB.Width = (int)sizeF.Width + 25;
            BBB.Height = (int)sizeF.Height + 10;
            BBB.Location = new Point(lab.Location.X,lab.Location.Y+7);       
            Bitmap localBitmap = new Bitmap(BBB.Width, BBB.Height);
            Graphics bitmapGraphics = Graphics.FromImage(localBitmap);
            bitmapGraphics.Clear(BackColor);
            bitmapGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            Draw(BBB.ClientRectangle, bitmapGraphics, 18, true, 1, Color.FromArgb(90, 143, 0), Color.FromArgb(90, 143, 0));
            BBB.BackgroundImage = localBitmap;
            //this.panel1.Controls.Add(BBB);
            panelBase.Controls.Add(BBB);
            BBB.Controls.Add(ricBox);
            //BBB.BringToFront();

            this.panel1.Controls.Add(panelBase);

            //panel1.Refresh();
            //newPoint = new Point(0, this.panel1.Height - panel1.AutoScrollPosition.Y);
            newPoint = new Point(0, panelBase.Height);
            panel1.AutoScrollPosition = newPoint;

            g.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Point newPoint = new Point(0, 0);
            //this.panel1.AutoScrollPosition = newPoint;

            panelBase.Height += 70;

            PictureBox pic = new PictureBox();
            pic.Image = Image.FromFile("./头像.png");

            pic.Location = new Point(this.panel1.Width - 64 -18, 3 + (MessMa.messCount) * 70);
            Rectangle picRec = new Rectangle(pic.Location.X, pic.Location.Y, 64, 64);

            pic.Image = picimageYou;
            pic.Size = new Size(64, 64);
            MessMa.pointOfPic = pic.Location;
            MessMa.picHeight = pic.Height;
            //this.panel1.Controls.Add(pic);
            panelBase.Controls.Add(pic);
            pic.BringToFront();

            Label lab = new Label();

            string temp = "鹿宝宝" + ":" + DateTime.Now.ToString();
            lab.Text = temp;
            lab.Font = new System.Drawing.Font("宋体", 12, FontStyle.Regular);
            lab.Location = new Point(pic.Location.X + pic.Width + 3, pic.Location.Y);
            MessMa.pointOfLab = lab.Location;
            this.panel1.Controls.Add(lab);
            panelBase.Controls.Add(lab);
            lab.BringToFront();
            lab.Visible = false;
            //toolTip1.SetToolTip(lab, "我爱你呀,鹿宝宝!~");
            //toolTip1.ShowAlways = true;
            //toolTip1.Show("我爱你呀,鹿宝宝!~", lab, 0, 25); 

            string mess = richTextBox1.Text;
            MessMa.messList.Add(mess);
            richTextBox1.Clear();
            Graphics g = panelBase.CreateGraphics();
            SizeF sizeF = g.MeasureString(mess, new Font("宋体", 17));
            MessMa.stringWidth.Add((int)sizeF.Width);
            MessMa.stringHeight.Add((int)sizeF.Height);
            //要画的气泡框的point
            Point point = new Point(lab.Location.X + 5, lab.Location.Y + 25);
            MessMa.pointList.Add(point);
            Rectangle rec = new Rectangle(point, new Size((int)sizeF.Width, (int)sizeF.Height + 3));
            MessMa.stringRecPoint = point;
            Pen pen = new Pen(Color.Blue, 3);

            SolidBrush br = new SolidBrush(Color.Blue);
            TextureBrush tbr = new TextureBrush(Image.FromFile("./文本填充.png"));
            //g.DrawString(mess,new Font("宋体",17,FontStyle.Regular,GraphicsUnit.Pixel),br,point.X+5,point.Y+5);
            //g.DrawString(mess, new Font("宋体", 17, FontStyle.Regular), br, point.X, point.Y + 5);
            MessMa.messCount += 1;

            RichTextBoxEx ricBox = new RichTextBoxEx();
            ricBox.Font = new System.Drawing.Font("宋体", 17, FontStyle.Regular);
            ricBox.Text = " " + mess;
            ricBox.Location = new Point(panel1.Width-64, 10);
            ricBox.WordWrap = true;
            ricBox.ReadOnly = true;
            ricBox.BorderStyle = BorderStyle.None;
            ricBox.ScrollBars = RichTextBoxScrollBars.None;
            ricBox.Location = new Point(10, 10);
            ricBox.ForeColor = Color.FromArgb(255, 255, 255);
            ricBox.BackColor = Color.FromArgb(90, 143, 0);
            ricBox.Dock = DockStyle.Fill;

            Panel BBB = new Panel();
            BBB.BackColor = Color.FromArgb(90, 143, 0);
            BBB.Width = (int)sizeF.Width + 25;
            BBB.Height = (int)sizeF.Height + 10;
            BBB.Location = new Point(panel1.Width -BBB.Width -64 -18, lab.Location.Y + 7);
            Bitmap localBitmap = new Bitmap(BBB.Width, BBB.Height);
            Graphics bitmapGraphics = Graphics.FromImage(localBitmap);
            bitmapGraphics.Clear(BackColor);
            bitmapGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            Draw(BBB.ClientRectangle, bitmapGraphics, 18, true, 0, Color.FromArgb(90, 143, 0), Color.FromArgb(90, 143, 0));
            BBB.BackgroundImage = localBitmap;
            //this.panel1.Controls.Add(BBB);
            panelBase.Controls.Add(BBB);
            BBB.Controls.Add(ricBox);
            //BBB.BringToFront();

            this.panel1.Controls.Add(panelBase);

            //panel1.Refresh();
            //newPoint = new Point(0, this.panel1.Height - panel1.AutoScrollPosition.Y);
            Point newPoint = new Point(0, panelBase.Height);
            panel1.AutoScrollPosition = newPoint;

            g.Dispose();
        }

        private void Draw(Rectangle rectangle, Graphics g, int _radius, bool cusp, int orientation, Color begin_color, Color end_color)
        {
            int span = 2;
            //抗锯齿
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //渐变填充
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush(rectangle, begin_color, end_color, LinearGradientMode.Vertical);
            //画尖角
            if (cusp)
            {
                if (orientation == 0)
                {
                    span = 10;
                    PointF p1 = new PointF(rectangle.Width - 12, rectangle.Y + 10);
                    PointF p2 = new PointF(rectangle.Width - 12, rectangle.Y + 30);
                    PointF p3 = new PointF(rectangle.Width, rectangle.Y + 20);
                    PointF[] ptsArray = { p1, p2, p3 };
                    g.FillPolygon(myLinearGradientBrush, ptsArray);
                    g.FillPath(myLinearGradientBrush, DrawRoundRect(rectangle.X, rectangle.Y, rectangle.Width - span, rectangle.Height - 1, _radius));
                }
                else
                    if (orientation == 1)
                    {
                        span = 10;
                        PointF p1 = new PointF(12, rectangle.Y + 10);
                        PointF p2 = new PointF(12, rectangle.Y + 30);
                        PointF p3 = new PointF(0, rectangle.Y + 20);
                        PointF[] ptsArray = { p1, p2, p3 };
                        g.FillPolygon(myLinearGradientBrush, ptsArray);
                        g.FillPath(myLinearGradientBrush, DrawRoundRect(rectangle.X + span, rectangle.Y, rectangle.Width - span, rectangle.Height - 1, _radius));
                    }
                    else
                    {
                        g.FillPath(myLinearGradientBrush, DrawRoundRect(rectangle.X, rectangle.Y, rectangle.Width - span, rectangle.Height - 1, _radius));
                    }
            }
        }

        public static GraphicsPath DrawRoundRect(int x, int y, int width, int height, int radius)
        {
            //四边圆角
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(x, y, radius, radius, 180, 90);
            gp.AddArc(width - radius, y, radius, radius, 270, 90);
            gp.AddArc(width - radius, height - radius, radius, radius, 0, 90);
            gp.AddArc(x, height - radius, radius, radius, 90, 90);
            gp.CloseAllFigures();
            return gp;
        }      

        //绘制圆图片
        private Image CutEllipse(Image img, Rectangle rec, Size size)
        {
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                using (TextureBrush br = new TextureBrush(img, System.Drawing.Drawing2D.WrapMode.Clamp, rec))
                {
                    br.ScaleTransform(bitmap.Width / (float)rec.Width, bitmap.Height / (float)rec.Height);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.FillEllipse(br, new Rectangle(Point.Empty, size));
                }
            }     
            return bitmap;
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            //panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {          
            Panel p = (Panel)sender;
            Graphics g = p.CreateGraphics();

            List<Point> nowLabList = new List<Point>();
            for(int i=0;i<this.panel1.Controls.Count;++i)
            {
                Label lab = new Label(); 
                if(this.panel1.Controls[i].GetType() == lab.GetType())
                {
                    nowLabList.Add(this.panel1.Controls[i].Location);
                }
            }

            nowLabList.Reverse(0, nowLabList.Count);
            /*
            Pen pen = new Pen(Color.Blue, 3);
            for(int i=0;i<nowLabList.Count;++i)
            {
                Point point = new Point(nowLabList[i].X + 5, nowLabList[i].Y + 25);
                Rectangle rec = new Rectangle(point,new Size(MessMa.stringWidth[i],MessMa.stringHeight[i]));
                SolidBrush br = new SolidBrush(Color.Black);
                g.DrawString(MessMa.messList[i], new Font("宋体", 17, FontStyle.Regular), br, point.X, point.Y + 5);
                g.DrawRectangle(pen, rec);
            }
            */
            g.Dispose();
        }

        private void messageBox_Load(object sender, EventArgs e)
        {
            this.panel1.HorizontalScroll.Enabled = false;
        }
    }

    //PicRichTextBox背景透明
    public class RichTextBoxEx : PicRichTextBox
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }
    }

    public class MessMa
    {
        public static int messCount = 0;
        public static Point pointOfLab = new Point();
        public static Point pointOfPic = new Point();
        public static Point stringRecPoint = new Point();
        public static int picHeight = 0;
        public static List<Point> pointList = new List<Point>();
        public static List<int> stringWidth = new List<int>();
        public static List<int> stringHeight = new List<int>();
        public static List<string> messList = new List<string>(); 
    }
}
