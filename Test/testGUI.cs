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
        Point pointLABGRP2 = new Point();

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
            this.panel2.HorizontalScroll.Enabled = false;
            this.panel2.VerticalScroll.Enabled = true;
            Add add = new Add();
            add.StartPosition = FormStartPosition.CenterParent;
            add.ShowDialog();
            
            string picPath = "./选中最小化.png";
            string name = Gloal.addListName;

            Gloal.contrlCount += 1;

            PictureBox pic = new PictureBox();
            pic.Name = "pictureBox" + Gloal.contrlCount;
            pic.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + ((Gloal.contrlCount -1)*35));
            pic.Image = Image.FromFile(picPath);
            pic.AutoSize = true;

            Label lab = new Label();
            lab.AutoSize = true;
            lab.Name = "LAB_GRP1_NAME" + Gloal.contrlCount;
            lab.Location = new Point(LAB_GRP1_NAME1.Location.X, LAB_GRP1_NAME1.Location.Y + ((Gloal.contrlCount -1)*35));
            //lab.Location = new Point(LAB_GRP1_NAME1.Location.X, pictureBox1.Location.Y + ((Gloal.contrlCount - 1) * 35)); 
            lab.Text = Gloal.addListName;
            //lab.Font = new Font("宋体", 9, FontStyle.Regular);
            lab.Font = LAB_GRP1_NAME1.Font;
            lab.AutoSize = true;
            this.panel2.Controls.Add(lab);
            this.panel2.Controls.Add(pic);

            Gloal.picList.Add(pic);
            Gloal.labList.Add(lab);

            lab.Visible = pictureBox1.Visible == false ? false : true;
            pic.Visible = pictureBox1.Visible == false ? false : true;

            InitList();
            richTextBox1.Clear();
            richTextBox1.AppendText(LAB_GROUP1.Location.Y.ToString() + "\n");
            for (int i = 0; i < Gloal.contrlCount; ++i)
            {
                richTextBox1.AppendText("   " + Gloal.picList[i].Location.Y.ToString() + "\n");
            }
            richTextBox1.AppendText(LAB_GROUP2.Location.Y.ToString() + "\n");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pointLABGRP2 = LAB_GROUP2.Location;

            Gloal.picList.Add(pictureBox1);
            Gloal.picList.Add(pictureBox2);
            Gloal.labList.Add(LAB_GRP1_NAME2);
            Gloal.labList.Add(LAB_GRP1_NAME1);

            Point temp = LAB_GRP1_NAME2.Location;
            LAB_GRP1_NAME2.Location = LAB_GRP1_NAME1.Location;
            LAB_GRP1_NAME1.Location = new Point(temp.X, temp.Y);

            this.panel2.Controls.Add(LAB_GRP1_NAME1);
            this.panel2.Controls.Add(LAB_GRP1_NAME2);
            this.panel2.Controls.Add(pictureBox1);
            this.panel2.Controls.Add(pictureBox2);

            richTextBox1.AppendText(LAB_GROUP1.Location.Y.ToString() + "\n");
            for (int i = 0; i < Gloal.contrlCount; ++i)
            {
                richTextBox1.AppendText("   " + Gloal.picList[i].Location.Y.ToString() + "\n");
            }
            richTextBox1.AppendText(LAB_GROUP2.Location.Y.ToString() + "\n");
        }

        private void LAB_GROUP1_Click(object sender, EventArgs e)
        {
            InitList();
        }

        private void InitList()
        {
            if (pictureBox1.Visible == false)
            {
                for (int i = 0; i < Gloal.contrlCount; ++i)
                {
                    Gloal.picList[i].Visible = true;
                    Gloal.labList[i].Visible = true;
                    Gloal.labList[i].Refresh();
                    Gloal.picList[i].Refresh();
                }
                LAB_GROUP2.Location = new Point(pointLABGRP2.X, pointLABGRP2.Y);
                //LAB_GROUP2.Location = new Point(LAB_GROUP1.Location.X, Gloal.labList[Gloal.contrlCount - 1].Location.Y + 25);
                //LAB_GROUP2.Location = new Point(38,282);
            }
            else
            {
                for (int i = 0; i < Gloal.contrlCount; ++i)
                {
                    Gloal.picList[i].Visible = false;
                    Gloal.labList[i].Visible = false;
                }
                LAB_GROUP2.Location = new Point(LAB_GROUP1.Location.X,LAB_GROUP1.Location.Y + 25);
            }
        }
    }
}

class Gloal
{
    public static string addListName = "";
    public static int contrlCount = 2;
    public static List<PictureBox> picList = new List<PictureBox>();
    public static List<Label> labList = new List<Label>();
}
