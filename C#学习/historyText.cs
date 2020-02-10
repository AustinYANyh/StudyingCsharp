using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace test
{
    public partial class Form1 : Form
    {
        public BinaryWriter bw;
        public BinaryReader br;

        public Form1()
        {
            InitializeComponent();
            richTextBox1.LoadFile("./history.txt", RichTextBoxStreamType.PlainText);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            richTextBox1.AppendText(DateTime.Now.ToString()+"\r\n");
            richTextBox1.AppendText(richTextBox2.Text+"\r\n");
            richTextBox2.Clear();

            /*
            bw = new BinaryWriter(new FileStream("./history.txt", FileMode.Append));
            bw.Write(richTextBox1.Text);
            bw.Close();
            */

            richTextBox1.SaveFile("./history.txt", RichTextBoxStreamType.PlainText);
        }
    }
}
