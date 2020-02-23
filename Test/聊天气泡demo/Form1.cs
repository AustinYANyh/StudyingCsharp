using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FriendLIst
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Gloal.pointofLABGRP2 = LAB_GRP2.Location;  
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PIC_USER.Image = Image.FromFile("./头像.png");
            LAB_USER.Text = "闫云皓";
            EDI_BELOWUSER.Text = "世间有太多奇迹,教人不能言喻,想这一刻,能够遇见最好的你";
            addListMember("鹿宝宝");
            addListMember("染墨灬若流云");
            addListMember("执笔灬绘浮沉");
            addListMember("素手灬挽秋风");

            closeList();
            LAB_GRP2.BringToFront();
            EDI_BELOWUSER.Select(0, 0);

            Gloal.labList[0].MouseDoubleClick += Form1_MouseDoubleClick;
        }

        void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            messageBox messagebox = new messageBox();
            messagebox.StartPosition = FormStartPosition.CenterScreen;
            messagebox.Show();
        }

        public void addListMember(string s)
        {
            Gloal.contrlCount += 1;
            //panel1.BackColor = Color.Red;
            panel1.Height = 80;
            string name = "LAB_GRP1_NAME" + Gloal.contrlCount;
            //Point point = new Point(LAB_GRP1.Location.X + 20, LAB_GRP1.Location.Y + (Gloal.contrlCount * 35));
            Point point = new Point(25, 0 + ((Gloal.contrlCount - 1) * 35));
            Label lab = new Label();
            lab.Name = name;
            lab.Location = new Point(point.X, point.Y);
            //lab.Text = Gloal.addName;
            lab.Text = s;
            Gloal.addName = "";
            this.panel1.Controls.Add(lab);
            lab.BringToFront();
            Gloal.labList.Add(lab);

            string picname = "pictureBox" + Gloal.contrlCount;
            string picPath = "./头像.png";
            point = new Point(5, 3 + ((Gloal.contrlCount - 1) * 35));
            PictureBox pic = new PictureBox();
            pic.Image = Image.FromFile(picPath);
            pic.Name = picname;
            pic.Location = new Point(point.X, point.Y);
            pic.Size = new System.Drawing.Size(12, 12);
            this.panel1.Controls.Add(pic);
            pic.BringToFront();
            Gloal.picList.Add(pic);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            add ad = new add();
            ad.StartPosition = FormStartPosition.CenterParent;
            ad.ShowDialog();

            Gloal.contrlCount += 1;
            //panel1.BackColor = Color.Red;
            panel1.Height = 80;
            string name = "LAB_GRP1_NAME" + Gloal.contrlCount;
            //Point point = new Point(LAB_GRP1.Location.X + 20, LAB_GRP1.Location.Y + (Gloal.contrlCount * 35));
            Point point = new Point(25, 0 + ((Gloal.contrlCount -1) * 35));
            Label lab = new Label();
            lab.Name = name;
            lab.Location = new Point(point.X, point.Y);
            lab.Text = Gloal.addName;
            //lab.Text = "鹿宝宝";
            Gloal.addName = "";
            this.panel1.Controls.Add(lab);
            lab.BringToFront();
            Gloal.labList.Add(lab);

            string picname = "pictureBox" + Gloal.contrlCount;
            string picPath = "./头像.png";
            point = new Point(5, 3 + ((Gloal.contrlCount - 1) * 35));
            PictureBox pic = new PictureBox();
            pic.Image = Image.FromFile(picPath);
            pic.Name = picname;
            pic.Location = new Point(point.X, point.Y);
            pic.Size = new System.Drawing.Size(12, 12);
            this.panel1.Controls.Add(pic);
            pic.BringToFront();
            Gloal.picList.Add(pic);

            //LAB_GRP2.Location = new Point(LAB_GRP1.Location.X, LAB_GRP1.Location.Y + (Gloal.contrlCount * 35) + 29);
            LAB_GRP2.Location = new Point(LAB_GRP1.Location.X, panel1.Location.Y + panel1.Height + 9);

            closeList();
            openList();
             */
        }

        public void refrushListofName()
        {
            for (int i = 1; i <= Gloal.contrlCount; ++i)
            {
                Point point = new Point(25, 0 + ((i - 1) * 35));
                Gloal.labList[i-1].Location = new Point(point.X, point.Y);
            }
        }

        public void refrushListofPic()
        {
            for (int i = 1; i <= Gloal.contrlCount; ++i)
            {
                Point point = new Point(5, 3 + ((i - 1) * 35));
                Gloal.picList[i - 1].Location = new Point(point.X, point.Y);
            }
        }

        private void LAB_GRP1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Gloal.labList[0].Visible == false)
                {
                    openList();
                }
                else
                {
                    closeList();
                }
            }
            catch (Exception error)
            {
                //列表中无内容
                //do nothing
            }
        }

        public void openList()
        {
            refrushListofName();
            refrushListofPic();
            for (int i = 0; i < Gloal.contrlCount; ++i)
            {
                Gloal.labList[i].Visible = true;
                Gloal.picList[i].Visible = true;
            }
            panel1.Visible = true;
            LAB_GRP2.BringToFront();
            LAB_GRP2.Location = new Point(LAB_GRP1.Location.X, panel1.Location.Y + panel1.Height + 9);
        }
        public void closeList()
        {
            for (int i = 0; i < Gloal.contrlCount; ++i)
            {
                Gloal.labList[i].Visible = false;
                Gloal.picList[i].Visible = false;
            }
            panel1.Visible = false;
            LAB_GRP2.Location = Gloal.pointofLABGRP2;
        }

        private void LAB_GRP1_MouseEnter(object sender, EventArgs e)
        {
            LAB_GRP1.ForeColor = Color.Blue;
        }

        private void LAB_GRP1_MouseLeave(object sender, EventArgs e)
        {
            LAB_GRP1.ForeColor = Color.Empty;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            
            for (int i = 0; i < Gloal.contrlCount; ++i)
            {
                /*
                if (e.Location.X >= Gloal.labList[i].Location.X
                    && e.Location.Y >= Gloal.labList[i].Location.Y
                    && e.Location.X <= Gloal.labList[i].Width + Gloal.labList[i].Location.X
                    && e.Location.Y <= Gloal.labList[i].Height + Gloal.labList[i].Location.Y)
                {
                    Gloal.labList[i].ForeColor = Color.Blue;
                }
                else
                {
                    Gloal.labList[i].ForeColor = Color.Empty;
                }                
                */

                Gloal.labList[i].MouseEnter += new EventHandler(changeColor);
                Gloal.labList[i].MouseLeave += new EventHandler(backColor);
            }
        }

        public void changeColor(object sender, EventArgs e)
        {
            Label lab = (Label)sender;
            lab.ForeColor = Color.Blue;
        }

        public void backColor(object sender, EventArgs e)
        {
            Label lab = (Label)sender;
            lab.ForeColor = Color.Empty;
        }

        private void EDI_BELOWUSER_Click(object sender, EventArgs e)
        {
            EDI_BELOWUSER.SelectAll();
        }

        private void EDI_BELOWUSER_MouseLeave(object sender, EventArgs e)
        {
            EDI_BELOWUSER.Select(0, 0);
            EDI_BELOWUSER.SelectionStart = 0;
            ActiveControl = null;  
        }
    }

    public class Gloal
    {
        public static Point pointofLABGRP2 = new Point();
        public static List<Label> labList = new List<Label>();
        public static List<PictureBox> picList = new List<PictureBox>();
        public static int contrlCount = 0;
        public static string addName = "";
    }
}
