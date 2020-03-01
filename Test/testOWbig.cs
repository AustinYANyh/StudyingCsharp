using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testFlashbig
{
    public partial class Form1 : Form
    {
        public static List<Panel> listPanel = new List<Panel>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listPanel.Add(panel1);
            listPanel.Add(panel2);
            listPanel.Add(panel3);
            listPanel.Add(panel4);

            setBackImage();
         
            for (int i=0;i< listPanel.Count; ++i)
            {
                listPanel[i].MouseEnter += Panel_MouseEnter;
                listPanel[i].MouseLeave += Panel_MouseLeave;
                listPanel[i].MouseClick += Form1_MouseClick;
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < listPanel.Count; ++i)
            {
                listPanel[i].Visible = false;
            }
            Label lab = new Label();
            this.Controls.Add(lab);
            lab.Name = "lab";
            lab.Size = new Size(305, 305);
            lab.Location = new Point(295, 235);
            lab.Text = "回家种地吧!~别玩游戏";
            Font font = new Font("楷体", 17, FontStyle.Regular);
            lab.Font = font;

            Button button = new Button();
            this.Controls.Add(button);
            button.Size = new Size(55, 35);
            button.Location = new Point(785,488);
            button.Text = "返回";
            button.Font = new Font("楷体", 13, FontStyle.Regular);
            button.Click += Button_Click;
        }

        private void Button_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < listPanel.Count; ++i)
            {
                listPanel[i].Visible = true;
            }
            Button button = (Button)sender;
            button.Visible = false;
            foreach(Control aaa in this.Controls)
            {
                if(aaa.Name == "lab")
                {
                    aaa.Visible = false;
                }
            }
        }

        public void setBackImage()
        {
            Bitmap bitmap = null;
            for (int i = 0; i < listPanel.Count; ++i)
            {
                string path = "./" + listPanel[i].Name + ".png";
                bitmap = new Bitmap(Image.FromFile(path), new Size(listPanel[i].Width, listPanel[i].Height));
                listPanel[i].BackgroundImage = bitmap;
            }
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.Size = new Size(panel.Width, panel.Height - 40);
            string path = "./" + panel.Name +".png";
            Bitmap bitmap = new Bitmap(Image.FromFile(path), panel.Size);
            panel.BackgroundImage = bitmap;
            panel.Location = new Point(panel.Location.X, panel.Location.Y + 20);
        }

        private void Panel_MouseEnter(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.BackgroundImage = null;
            panel.BackColor = Color.Transparent;

            panel.Size = new Size(panel.Width, panel.Height + 40);
            string path = "./" + panel.Name + ".png";
            Bitmap bitmap = new Bitmap(Image.FromFile(path), panel.Size);
            panel.BackgroundImage = bitmap;
            panel.Location = new Point(panel.Location.X, panel.Location.Y - 20);
        }
    }
}
