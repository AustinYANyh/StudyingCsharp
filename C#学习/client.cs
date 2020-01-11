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
using System.Drawing;
using System.Drawing.Drawing2D;

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
            string ip = user.GetLocalIp();
            string message = ip + "-" + REDI_MESSAGE.Text;

            //等于0是直接点发送,等于1是使用快捷键输入框有一个\n
            if (REDI_MESSAGE.Text.Length == 1 || REDI_MESSAGE.Text.Length == 0)
            {
                GBOX_WARNING.Visible = true;
                REDI_MESSAGE.Clear();
                timer1.Enabled = true;
                return;
            }

            //chatm.SendMessage(username);
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
            try
            {
                this.Close();
            }
            catch(System.Net.Sockets.SocketException)
            {
                MessageBox.Show("对方已关闭连接...");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Form1.form1.LAB_STATE.Text = " ";
            timer2.Enabled = false;
        }

        /// <summary>
        /// 拖动无窗体的控件
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private void PANEL_TITLE_MouseDown(object sender, MouseEventArgs e)
        {
            //拖动窗体
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        /// <summary>
        /// 最小化和关闭按钮鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_CLOSE_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                BTN_CLOSE.Image = Image.FromFile(@"./选中关闭.png");
            }
            catch(System.IO.FileNotFoundException)
            {
                MessageBox.Show("资源路径不正确或资源缺失...");
            }
        }

        private void BTN_CLOSE_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                BTN_CLOSE.Image = Image.FromFile(@"./关闭.png");
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("资源路径不正确或资源缺失...");
            }
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                button1.Image = Image.FromFile(@"./选中最小化.png");
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("资源路径不正确或资源缺失...");
            }
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                button1.Image = Image.FromFile(@"./最小化.png");
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("资源路径不正确或资源缺失...");
            }
        }


        //窗口关闭,不管什么线程都被强制退出
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }
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
                //MessageBox.Show("服务器未开启...请先开启...");
                MessageBox.Show("The server is outline...Please open first...");
                Form1.form1.Close();
            }

            //创建线程执行接收和检测用户输入状态
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

                        //登录或断线消息---系统消息
                        //System message:223.167.169.200:31966客户端已成功连接...
                        //System message:223.167.169.200:31966已断开连接...
                        if (message.IndexOf("System message:") != -1)
                        {
                            if (Form1.form1.LISTBOX_MESSAGE.InvokeRequired)
                            {
                                Form1.form1.LISTBOX_MESSAGE.Invoke(new MethodInvoker(() => { _update(message); }));
                            }
                            else
                            {
                                _update(message);
                            }
                        }
                        else
                        {
                            //处理tcp粘包问题
                            //1.输入状态信息在前
                            int pos = message.IndexOf("isInputing...");
                            if (message.IndexOf("isInputing...") != -1 && pos != message.Length - 13)
                            {
                                message = message.Substring(pos + 13);
                            }
                            //2.输入状态信息在后
                            pos = message.IndexOf("\n");
                            if (message.IndexOf("\n") != -1 && pos != message.Length - 1)
                            {
                                message = message.Substring(0, pos);
                            }


                            int index = message.IndexOf("-");
                            string mesIp = message.Substring(0, index);
                            string mes = message.Substring(index + 1);
                            string ip = user.GetLocalIp();

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
        /// 检测用户输入状态
        /// <summary>
        private void checkEDIState()
        {
            while (true)
            {
                func();

                if (isInputing == true)
                {
                    object lockobj = new object();
                    lock (lockobj)
                    {
                        string ip = user.GetLocalIp();
                        byte[] buffer = Encoding.UTF8.GetBytes(ip + "-" + "isInputing...");
                        clientSocket.SendTo(buffer, remotPoint);
                    }
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

        public void _update(string message)
        {
            //System message:223.167.169.200:31966客户端已成功连接...
            //System message:223.167.169.200:31966已断开连接...
            if(message.IndexOf("客户端已成功连接...") != -1)
            {
                message = message.Substring(message.IndexOf(":") + 1);
                string netip = message.Substring(0, message.IndexOf(":"));
                Form1.form1.REDI_SHOWMESSAGE.SelectionAlignment = HorizontalAlignment.Center;
                Form1.form1.REDI_SHOWMESSAGE.AppendText(user.mesdic[netip] + "已登录!!!");
                //Form1.form1.LISTBOX_MESSAGE.Items.Add((user.mesdic[netip] + "已登录!!!").PadLeft(53));
            }
            else
            {
                message = message.Substring(message.IndexOf(":") + 1);
                string netip = message.Substring(0, message.IndexOf(":"));
                Form1.form1.REDI_SHOWMESSAGE.SelectionAlignment = HorizontalAlignment.Center;
                Form1.form1.REDI_SHOWMESSAGE.AppendText(user.mesdic[netip] + "已退出...");
                //Form1.form1.LISTBOX_MESSAGE.Items.Add((user.mesdic[netip] + "已退出...").PadLeft(53));
            }
        }

        public void updateListBox(string message)
        {
            object lockObj = new object();

            lock (lockObj)
            {
                Form1.form1.LISTBOX_MESSAGE.ClearSelected();

                //192.168.1.10-1
                int index = message.IndexOf("-");

                //自己的消息
                string name = user.dic[message.Substring(0, index)] + ":" + DateTime.Now.ToString();
                
                if(message.Substring(0, index) == user.GetLocalIp())
                {
                    Form1.form1.REDI_SHOWMESSAGE.SelectionAlignment = HorizontalAlignment.Right;
                    Form1.form1.REDI_SHOWMESSAGE.AppendText(name + "\r\n");

                    message = message.Substring(index + 1);
                    Form1.form1.REDI_SHOWMESSAGE.AppendText(message);        
                }
                else
                {
                    Form1.form1.REDI_SHOWMESSAGE.SelectionAlignment = HorizontalAlignment.Left;
                    Form1.form1.REDI_SHOWMESSAGE.AppendText(name + "\r\n");

                    message = message.Substring(index + 1);
                    Form1.form1.REDI_SHOWMESSAGE.AppendText(message); 
                }

                Form1.form1.REDI_SHOWMESSAGE.Select(Form1.form1.REDI_SHOWMESSAGE.TextLength, 0);
                Form1.form1.REDI_SHOWMESSAGE.ScrollToCaret();
                //message = message.Substring(index + 1); 

                Form1.form1.LISTBOX_MESSAGE.Items.Add(name);

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

                //object lockObj = new object();

                //lock (lockObj)
                {
                    Form1.form1.LISTBOX_MESSAGE.Items.Add(message);
                    Form1.form1.LISTBOX_MESSAGE.TopIndex = Form1.form1.LISTBOX_MESSAGE.Items.Count - (int)(Form1.form1.LISTBOX_MESSAGE.Height / Form1.form1.LISTBOX_MESSAGE.ItemHeight);
                }
            }
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
