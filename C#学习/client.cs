using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace chatroom
{
    public partial class Form1 : Form
    {
        public static Form1 form1;
        public static ChatManager chatm = new ChatManager();
        public Form1()
        {
            InitializeComponent();
            form1 = this;
            chatm.Start();        
        }

        private void BTN_SEND_Click(object sender, EventArgs e)
        {
            string ip = chatm.GetIP();
            string username = ip + "-" + user.username + ":" + DateTime.Now.ToString();
            string message = ip + "-" + REDI_MESSAGE.Text;

            //等于0是直接点发送,等于1是使用快捷键输入框有一个\n
            if (REDI_MESSAGE.Text.Length == 1 || REDI_MESSAGE.Text.Length == 0)
            {
                GBOX_WARNING.Visible = true;
                REDI_MESSAGE.Clear();
                timer1.Enabled = true;
                return;
            }

            chatm.SendMessage(username);
            chatm.SendMessage(message);

            REDI_MESSAGE.Clear();
        }

        private void REDI_MESSAGE_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                BTN_SEND_Click(sender, e);
                e.Handled = true;
            }
        }

        private void LISTBOX_MESSAGE_Leave(object sender, EventArgs e)
        {
            LISTBOX_MESSAGE.ClearSelected();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = LISTBOX_MESSAGE.SelectedItem.ToString();
            Clipboard.SetText(value);
        }
        private void LISTBOX_MESSAGE_RIGHT_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            System.Drawing.StringFormat strFmt = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.NoClip);
            strFmt.Alignment = System.Drawing.StringAlignment.Far;

            RectangleF rf = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            //if (Form1.form1.LISTBOX_MESSAGE_RIGHT.Items.Count > 0)
            {
                //e.Graphics.DrawString(Form1.form1.LISTBOX_MESSAGE.Items[e.Index].ToString(), e.Font, new System.Drawing.SolidBrush(e.ForeColor), rf, strFmt);
            }
        }

        private void LISTBOX_MESSAGE_RIGHT_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 30;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GBOX_WARNING.Visible = false;
            timer1.Enabled = false;
        }

        private void BTN_CLOSE_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //if (Form1.form1.LAB_STATE.InvokeRequired)
            //{
            //    Form1.form1.LAB_STATE.Invoke(new MethodInvoker(() =>
            //    {
            //        Form1.form1.LAB_STATE.Text = " ";
            //        Application.DoEvents();
            //    }));
            //}
            Form1.form1.LAB_STATE.Text = " ";
            timer2.Enabled = false;
        }

        [DllImport("user32.dll")]//拖动无窗体的控件
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //拖动窗体
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }


        //窗口关闭,不管什么线程都被强制退出
        //调试用
        //private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    System.Environment.Exit(0);
        //}
    }

    public class ChatManager
    {
        private string _ipAdress = "111.229.13.33";
        private int _port = 2000;
        EndPoint remotPoint;
        public Socket clientSocket;
        public string message;
        bool isInputing = false;

        Thread receiveThread;
        Thread stateThread;
        byte[] bufferReceive = new byte[4096];

        public void Start()
        {
            ConnetToSever(_ipAdress, _port);
        }

        public void ConnetToSever(string ipadress, int port)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            remotPoint = new IPEndPoint(IPAddress.Parse(ipadress), port);

            //建立连接
            //clientSocket.BeginConnect(remotPoint, ConnectCallBack, clientSocket);
            try
            {
                clientSocket.Connect(remotPoint); 
            }
            catch(SocketException)
            {
                MessageBox.Show("服务器未开启...请先开启...");
                Form1.form1.Close();
            }
            //因为是一直在准备接受的状态，所以开启一个线程来负责处理接受消息 
            receiveThread = new Thread(ReceiveMessageFormSever);
            receiveThread.Start();

            stateThread = new Thread(checkEDIState);
            stateThread.Start();
        }

        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            clientSocket.SendTo(buffer, remotPoint);
        }

        public void ReceiveMessageFormSever()
        {
            while (true)
            {
                try
                {
                    if (clientSocket.Connected)
                    {
                        int length = clientSocket.Receive(bufferReceive);
                        message = Encoding.UTF8.GetString(bufferReceive, 0, length);

                        //string clientip = clientSocket.LocalEndPoint.ToString();
                        //int index = clientip.IndexOf(":");
                        //clientip = clientip.Substring(0,index);

                        int index = message.IndexOf("-");
                        string mesIp = message.Substring(0,index);
                        string mes = message.Substring(index + 1);
                        string ip = GetIP();
                        if (mesIp != ip && mes == "isInputing...")
                        {
                            if (Form1.form1.LAB_STATE.InvokeRequired)
                            {
                                Form1.form1.LAB_STATE.Invoke(new MethodInvoker(() => { updateLab(); }));
                            }
                        }
                        else if (mesIp == ip && mes == "isInputing...")
                        {
                            //do nothing
                        }
                        //客户端发来的消息,更新到listbox
                        else
                        {
                            if (Form1.form1.LISTBOX_MESSAGE.InvokeRequired)
                            {
                                Form1.form1.LISTBOX_MESSAGE.Invoke(new MethodInvoker(() => { updateListBox(message); }));
                            }
                            else
                            {
                                updateListBox(message);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        /// <summary>
        /// 获取ip地址
        /// </summary>
        /// <returns></returns>
        public string GetLocalIp()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }

        /// <summary>
        /// 获取外网ip地址
        /// </summary>
        /// <returns></returns>
        public string GetIP()
        {
            using (var webClient = new WebClient())
            {
               try{
                    webClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageDate = webClient.DownloadData("http://pv.sohu.com/cityjson?ie=utf-8");
                    String ip = Encoding.UTF8.GetString(pageDate);
                    webClient.Dispose();

                    Match rebool = Regex.Match(ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    return rebool.Value;
               }
               catch (Exception e)
               {
                   return "";
               }

            }
        }

        /// <summary>
        /// 检测用户输入状态
        /// <summary>
        private void checkEDIState()
        {
            while (true)
            {
                func();

                if (isInputing == true)
                {
                    string ip = GetIP();
                    byte[] buffer = Encoding.UTF8.GetBytes(ip + "-" + "isInputing...");
                    clientSocket.SendTo(buffer, remotPoint);
                }
                else
                {
                    //byte[] buffer = Encoding.UTF8.GetBytes("finishInputing...");
                    //clientSocket.SendTo(buffer, remotPoint);
                }
            }
        }

        public bool func()
        {
            isInputing = false;
            int strLengthBefore = 0;
            if (Form1.form1.LISTBOX_MESSAGE.InvokeRequired)
            {
                Form1.form1.LISTBOX_MESSAGE.Invoke(new MethodInvoker(() => { strLengthBefore = getLength(); }));
            }

            int strLengthAfter = 0;
            Thread.Sleep(500);
            if (Form1.form1.LISTBOX_MESSAGE.InvokeRequired)
            {
                Form1.form1.LISTBOX_MESSAGE.Invoke(new MethodInvoker(() => { strLengthAfter = getLength(); }));
            }

            if (strLengthBefore != strLengthAfter)
            {
                isInputing = true;
            }

            return isInputing;
        }
        public int getLength()
        {
            return Form1.form1.REDI_MESSAGE.Text.Length;
        }

        public void updateLab()
        {
            Form1.form1.timer2.Enabled = true;
            Form1.form1.LAB_STATE.Text = "对方正在输入...";
            Application.DoEvents();
        }

        /// <summary>
        /// 判断窗口是否在最前
        /// </summary>
        private const int WS_EX_TOPMOST = 0x00000008;
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        public static bool CheckIsTopMost(IntPtr hWnd)
        {
            int style = GetWindowLong(hWnd, WS_EX_TOPMOST);
            style = style & WS_EX_TOPMOST;
            if (style == WS_EX_TOPMOST)
            {
                return true;
            }
            return false;
        }

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //窗体任务栏图标闪烁
        [DllImport("user32.dll")]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, EntryPoint = "FlashWindow")]
        static extern bool FlashWindow(IntPtr handle, bool invert);

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeOut;
        }

        public const UInt32 FLASHW_STOP = 0x00000000;
        public const UInt32 FLASHW_CAPTION = 0x00000001;
        public const UInt32 FLASHW_TRAY = 0x00000002;
        public const UInt32 FLASHW_ALL = 0x00000003;
        public const UInt32 FLASHW_TIMER = 0x00000004;
        public const UInt32 FLASHW_TIMERNOFG = 0x0000000C;

        public void updateListBox(string message)
        {
            Form1.form1.LISTBOX_MESSAGE.ClearSelected();

            int index = message.IndexOf("-");
            message = message.Substring(index + 1);

            //焦点不在软件界面,接受消息前播放声音提醒
            IntPtr hwnd = FindWindow(null, "兔夫君和鹿夫人");
            //if (CheckIsTopMost(hwnd) == true)
            if (Form1.form1.REDI_MESSAGE.Focused == false)
            {
                //Thread.Sleep(500);

                //闪烁
                FLASHWINFO fi = new FLASHWINFO();

                fi.cbSize = (uint)Marshal.SizeOf(fi);
                fi.hwnd = hwnd;
                fi.dwFlags = FLASHW_TIMER | FLASHW_ALL;
                fi.uCount = 5;
                fi.dwTimeOut = 75;

                FlashWindowEx(ref fi);

                //任务栏消息提示
                FlashWindow(hwnd, true);
                System.Media.SystemSounds.Hand.Play();
            }

            Form1.form1.LISTBOX_MESSAGE.Items.Add(message);
            Form1.form1.LISTBOX_MESSAGE.TopIndex = Form1.form1.LISTBOX_MESSAGE.Items.Count - (int)(Form1.form1.LISTBOX_MESSAGE.Height / Form1.form1.LISTBOX_MESSAGE.ItemHeight);
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            uint dummy = 0;
            byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];

            //启用Keep-Alive
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
            BitConverter.GetBytes((uint)1000).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);
        }
    }
}
