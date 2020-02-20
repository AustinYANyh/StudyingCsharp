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

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            for (int i = 0; i < Gloal.contrlCount; ++i)
            { 
                richTextBox1.AppendText(Gloal.labList[i].Location.Y.ToString() + "\n");
            }

            add ad = new add();
            ad.StartPosition = FormStartPosition.CenterParent;
            ad.ShowDialog();

            Gloal.contrlCount += 1;
            //panel1.BackColor = Color.Red;
            panel1.Height = 80;
            string name = "LAB_GRP1_NAME" + Gloal.contrlCount;
            //Point point = new Point(LAB_GRP1.Location.X + 20, LAB_GRP1.Location.Y + (Gloal.contrlCount * 35));
            Point point = new Point(10, 0 + ((Gloal.contrlCount -1) * 35));
            Label lab = new Label();
            lab.Name = name;
            lab.Location = new Point(point.X, point.Y);
            lab.Text = Gloal.addName;
            //lab.Text = "鹿宝宝";
            Gloal.addName = "";
            this.panel1.Controls.Add(lab);
            lab.BringToFront();
            Gloal.labList.Add(lab);

            //LAB_GRP2.Location = new Point(LAB_GRP1.Location.X, LAB_GRP1.Location.Y + (Gloal.contrlCount * 35) + 29);
            LAB_GRP2.Location = new Point(LAB_GRP1.Location.X, panel1.Location.Y + panel1.Height + 9);

            closeList();
            openList();
        }

        public void refrushList()
        {
            for (int i = 1; i <= Gloal.contrlCount; ++i)
            {
                Point point = new Point(10, 0 + ((i - 1) * 35));
                Gloal.labList[i-1].Location = new Point(point.X, point.Y);
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
            refrushList();
            for (int i = 0; i < Gloal.contrlCount; ++i)
            {
                Gloal.labList[i].Visible = true;
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
            }
            panel1.Visible = false;
            LAB_GRP2.Location = Gloal.pointofLABGRP2;
        }
    }

    public class Gloal
    {
        public static Point pointofLABGRP2 = new Point();
        public static List<Label> labList = new List<Label>();
        public static int contrlCount = 0;
        public static string addName = "";
    }
}
