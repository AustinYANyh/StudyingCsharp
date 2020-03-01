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

            for (int i=0;i<4;++i)
            {
                listPanel[i].MouseEnter += Panel_MouseEnter;
                listPanel[i].MouseLeave += Panel_MouseLeave;
            }
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.Size = new Size(panel.Width, panel.Height - 40);
            panel.Location = new Point(panel.Location.X, panel.Location.Y + 20);
        }

        private void Panel_MouseEnter(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.Size = new Size(panel.Width, panel.Height + 40);
            panel.Location = new Point(panel.Location.X, panel.Location.Y - 20);
        }

        
    }
}
