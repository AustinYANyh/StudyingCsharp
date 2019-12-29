using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace processBar
{
    public partial class save : Form
    {
        public save()
        {
            InitializeComponent();
        }

        static long GetHttpLength(string url)
        {
            var length = 0l;
            try
            {
                var req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                req.Method = "HEAD";
                req.Timeout = 5000;
                var res = (HttpWebResponse)req.GetResponse();
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    length = res.ContentLength;
                }

                res.Close();
                return length;
            }
            catch (WebException wex)
            {
                return 0;
            }
        }
        public static string savePath()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请指定文件保存路径";

            string foldPath = "";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foldPath = dialog.SelectedPath;
            }
            return foldPath;
        }

        void func(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            Stream responseStream = response.GetResponseStream();
            try
            {
                string path = savePath();
                Form1 frm = (Form1)this.Owner;
                string filename=path+"\\";
                filename+=EDI_SAVENAME.Text;

                Stream stream = new FileStream(filename, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, bArr.Length);
                frm.PRO_PROCESS.Maximum = (int)GetHttpLength(url);
                double i = 0;

                while (size > 0)
                {
                    i += size;
                    frm.PRO_PROCESS.Value = (int)i;
                    Application.DoEvents();
                    Thread.Sleep(100);
                    frm.LAB_PERCENT.Text = Math.Round((i / frm.PRO_PROCESS.Maximum) * 100, 2).ToString() + "%";
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, bArr.Length);
                }
                stream.Close();
                responseStream.Close();
            }
            catch (AccessViolationException)
            {
                MessageBox.Show("没有访问权限!", "提示信息");
            } 
        }

        private void EDI_SAVENAME_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Enter:
                    this.Hide();
                    Form1 frm = (Form1)this.Owner;
                    string url = frm.EDI_PATH.Text.Trim();
                    func(url);
                    this.Close();
                    break;
            }
        }
    }
}
