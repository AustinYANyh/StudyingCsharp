using System;
using System.Collections.Generic;
using System.Collections;
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
        [System.Runtime.InteropServices.ComVisibleAttribute(true)]
        private void Form1_Load(object sender, EventArgs e)
        {
            //将该控件的 IsWebBrowserContextMenuEnabled 属性设置为 false，以防止 WebBrowser 控件在用户右击它时显示其快捷菜单
            webKitBrowser1.IsWebBrowserContextMenuEnabled = false;
            REDI_SHOWMESSAGE.Visible = false;

            string sb = "";
//            sb=@"<html>
//<head>
//<style>
//  /* bubble style */
//        .sender{
//            clear:both;
//        }
//        .sender div:nth-of-type(1){
//            float: left;
//        }
//        .sender div:nth-of-type(2){
//            background-color: aquamarine;
//            float: left;
//            margin: 0 20px 10px 15px;
//            padding: 10px 10px 10px 0px;
//            border-radius:7px;
//        }
// 
//        .receiver div:first-child img,
//        .sender div:first-child img{
//            width:50px;
//            height: 50px;
//        }
// 
//        .receiver{
//            clear:both;
//        }
//        .receiver div:nth-child(1){
//            float: right;
//        }
//        .receiver div:nth-of-type(2){
//            float:right;
//            background-color: gold;
//            margin: 0 10px 10px 20px;
//            padding: 10px 0px 10px 10px;
//            border-radius:7px;
//        }
// 
//        .left_triangle{
//            height:0px;  
//            width:0px;  
//            border-width:8px;  
//            border-style:solid;  
//            border-color:transparent aquamarine transparent transparent;  
//            position: relative;
//            left:-16px;
//            top:3px;
//        }
// 
//        .right_triangle{
//            height:0px;  
//            width:0px;  
//            border-width:8px;  
//            border-style:solid;  
//            border-color:transparent transparent transparent gold;  
//            position: relative;
//            right:-16px;
//            top:3px;
//        }
//
//        .BTN_SEND{
//            position:absolute;
//            bottom: 0;
//        }
//
//        .footer {
//            position: fixed;
//            left: 0px;
//            bottom: 0px;
//            width: 100%;
//            height: 50px;
//            background-color: #eee;
//            z-index: 9999;
//        }
//  </style>
//</head>
//<body>
//<!-- Left -->
//<div class=""sender"">
//      <div>
//          <img src=""./鹿.jpg"">
//      </div>
//  <div>
//      <div class=""left_triangle""></div>
//      <span> 夫君君! </span>
//   </div>
//  </div>
//<!-- Right -->
//  <div class=""receiver"">
//      <div>
//          <img src=""./兔.jpg"">
//      </div>
//   <div>
//        <div class=""right_triangle""></div>
//        <span> 鹿宝宝~ </span>
//   </div>
//  </div>  
//</body>
//</html>
//    ";
            sb = @"<html><head>
<script type=""text/javascript"">window.location.hash = ""#ok"";</script>
<style type=""text/css"">

/*滚动条宽度*/  
::-webkit-scrollbar {  
    width: 8px;  
}  
   
/* 轨道样式 */  
::-webkit-scrollbar-track {  
  
}  
   
/* Handle样式 */  
::-webkit-scrollbar-thumb {  
    border-radius: 10px;  
    background: rgba(0,0,0,0.2);   
}  
  
/*当前窗口未激活的情况下*/  
::-webkit-scrollbar-thumb:window-inactive {  
    background: rgba(0,0,0,0.1);   
}  
  
/*hover到滚动条上*/  
::-webkit-scrollbar-thumb:vertical:hover{  
    background-color: rgba(0,0,0,0.3);  
}  
/*滚动条按下*/  
::-webkit-scrollbar-thumb:vertical:active{  
    background-color: rgba(0,0,0,0.7);  
}  
  
textarea{width: 500px;height: 300px;border: none;padding: 0px;}  

	.chat_content_group.self {
text-align: right;
}
.chat_content_group {
padding: 0px;
}
.chat_content_group.self>.chat_content {
text-align: left;
}
.chat_content_group.self>.chat_content {
background: #7ccb6b;
color:#fff;
/*background: -webkit-gradient(linear,left top,left bottom,from(white,#e1e1e1));
background: -webkit-linear-gradient(white,#e1e1e1);
background: -moz-linear-gradient(white,#e1e1e1);
background: -ms-linear-gradient(white,#e1e1e1);
background: -o-linear-gradient(white,#e1e1e1);
background: linear-gradient(#fff,#e1e1e1);*/
}
.chat_content {
display: inline-block;
min-height: 16px;
max-width: 50%;
color:#292929;
background: #f0f4f6;
/*background: -webkit-gradient(linear,left top,left bottom,from(#cf9),to(#9c3));
background: -webkit-linear-gradient(#cf9,#9c3);
background: -moz-linear-gradient(#cf9,#9c3);
background: -ms-linear-gradient(#cf9,#9c3);
background: -o-linear-gradient(#cf9,#9c3);
background: linear-gradient(#cf9,#9c3);*/
-webkit-border-radius: 5px;
-moz-border-radius: 5px;
border-radius: 5px;
padding: 5px 10px;
word-break: break-all;
/*box-shadow: 1px 1px 5px #000;*/
line-height: 1.4;
}

.chat_content_group.self>.chat_nick {
text-align: right;
}
.chat_nick {
font-size: 14px;
margin: 0 0 10px;
color:#8b8b8b;
}

.chat_content_group.self>.chat_content_avatar {
float: right;
margin: 0 0 0 10px;
}

.chat_content_group.buddy {
text-align: left;
}
.chat_content_group {
padding: 0px;
}
.chat_content_avatar {
float: left;
width: 40px;
height: 40px;
margin-right: 10px;
}
.imgtest{margin:10px 5px;
overflow:hidden;}
.list_ul figcaption p{
font-size:12px;
color:#aaa;
}
.imgtest figure div{
display:inline-block;
margin:5px auto;
width:100px;
height:100px;
border-radius:100px;
border:2px solid #fff;
overflow:hidden;
-webkit-box-shadow:0 0 3px #ccc;
box-shadow:0 0 3px #ccc;
}
.imgtest img{width:100%;
min-height:100%; text-align:center;}
	</style>
</head><body>  
";


            webKitBrowser1.DocumentText = sb;
        }

        private void BTN_SEND_Click(object sender, EventArgs e)
        {
            string ip = user.GetLocalIp();
            string message = REDI_MESSAGE.Text.Length + "-" + ip + "-" + REDI_MESSAGE.Text;

            if (REDI_MESSAGE.Rtf.IndexOf(@"{\pict\") > -1)
            {
                message = REDI_MESSAGE.Rtf.Length + "-" + ip + "-" + REDI_MESSAGE.Rtf;
                
                chatm.SendMessage(message);
            }
            else
            {
                //等于0是直接点发送,等于1是使用快捷键输入框有一个\n
                if (REDI_MESSAGE.Text == "\n" || REDI_MESSAGE.Text.Length == 0)
                {
                    GBOX_WARNING.Visible = true;
                    REDI_MESSAGE.Clear();
                    timer1.Enabled = true;
                    return;
                }

                //信息长度超过150字节,不允许发送
                if (UnicodeEncoding.Default.GetBytes(REDI_MESSAGE.Text).Length > 200)
                {
                    REDI_SHOWMESSAGE.SelectionAlignment = HorizontalAlignment.Center;
                    REDI_SHOWMESSAGE.AppendText("信息长度过长,请分段发送!\r\n");
                    return;
                }

                //chatm.SendMessage(username);
                chatm.SendMessage(message);
            }

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

        /// <summary>
        /// 窗口抖动
        /// </summary>
        public void windowRock()
        {
            //获取当前窗体的坐标
            Point point = this.Location;
            //反复给窗体坐标复制一百次，达到震动的效果
            for (int i = 0; i < 300; i++)
            {
                this.Location = new Point(point.X - 7, point.Y - 7);
                this.Location = new Point(point.X + 7, point.Y + 7);
            }
            this.Location = point;
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

        private void BTN_P_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                contextMenuStrip2.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void 文本模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REDI_SHOWMESSAGE.Visible = true;
            webKitBrowser1.Visible = false;
            richTextBox1.Visible = false;
        }

        private void 气泡模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REDI_SHOWMESSAGE.Visible = false;
            //webKitBrowser1.Visible = true;
            richTextBox1.Visible = true;
            webKitBrowser1.Visible = false;
        }

        private void BTN_ROCK_Click(object sender, EventArgs e)
        {
            chatm.SendMessage("System message:Rock");
        }
    }

    public class ChatManager
    {
        private string _ipAdress = "111.229.13.33";
        //private string _ipAdress = "127.0.0.1";
        private int _port = 2000;
        EndPoint remotPoint;
        public Socket clientSocket;
        public string message;
        bool isInputing = false;
        bool ischeckEDI = false;
        bool iskeepalive = false;

        Thread receiveThread;
        Thread stateThread;

        //缓冲区大小分配为4M
        public byte[] bufferReceive = new byte[1024*1024*4];

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
            receiveThread.IsBackground = true;
            receiveThread.Start();

            stateThread = new Thread(checkEDIState);
            stateThread.Start();

            //防止断开连接,每5分钟发送一次keep alive
            Thread keepAlive = new Thread(KeepAlive);
            keepAlive.Start();
            
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
                        object lockobj = new object();
                        lock(lockobj)
                        {
                            int length = clientSocket.Receive(bufferReceive, bufferReceive.Length, 0);
                            message = Encoding.UTF8.GetString(bufferReceive, 0, length);
                            string strlength = "";
                            if (length != 0)
                            {
                                strlength = message.Substring(0, message.IndexOf("-"));
                                message = message.Substring(message.IndexOf("-")+1);
                            }

                            while(length > 0)
                            {
                                if(message.Length >= Convert.ToInt32(strlength))
                                {
                                    break;
                                }
                                
                                length = clientSocket.Receive(bufferReceive, bufferReceive.Length, 0);
                                message += Encoding.UTF8.GetString(bufferReceive, 0, length);
                            }
 
                            //登录或断线消息---系统消息
                            //System message:223.167.169.200:31966客户端已成功连接...
                            //System message:223.167.169.200:31966已断开连接...
                            if (message.IndexOf("System message:") != -1 )
                            {
                                if (message.IndexOf("Rock") != -1)
                                {
                                    Form1.form1.windowRock();
                                }
                                else if(message.IndexOf("连接") != -1)
                                {
                                    if (Form1.form1.REDI_MESSAGE.InvokeRequired)
                                    {
                                        Form1.form1.REDI_MESSAGE.Invoke(new MethodInvoker(() => { _update(message); }));
                                    }
                                    else
                                    {
                                        _update(message);
                                    }
                                }
                                //客户端收到心跳信息不做任何处理
                                else
                                {
                                    //do nothing
                                }
                            }
                            //发送带图片的信息,直接发送rtf
                            else if (message.IndexOf(@"{\pict\") > -1)
                            {
                                //message = message.Substring(message.IndexOf("-") + 1);
                                if (Form1.form1.REDI_SHOWMESSAGE.InvokeRequired)
                                {
                                    Form1.form1.REDI_SHOWMESSAGE.Invoke(new MethodInvoker(() => { updateMessageBox(message); }));
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

                                if (message.IndexOf("-") != -1)
                                {
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
                                        if (Form1.form1.REDI_MESSAGE.InvokeRequired)
                                        {
                                            Form1.form1.REDI_MESSAGE.Invoke(new MethodInvoker(() => { updateMessageBox(message); }));
                                        }
                                        else
                                        {
                                            updateMessageBox(message);
                                        }
                                    }
                                }
                            }//end of system message's if
                        }//end of lockobj
                    }//end of clientSocket.connectedd's if
                    else
                    {
                        break;
                    }
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    MessageBox.Show(ex.ToString());
                    MessageBox.Show(ex.ErrorCode.ToString());
                }
            }
        }

        public void onReceive(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndReceive(ar);

            }
            catch (ObjectDisposedException)
            { 
            }
        }

        private void KeepAlive()
        {
            while (iskeepalive == true)
            {
                Thread.Sleep(300000);
                try
                {
                    byte[] buffer = Encoding.UTF8.GetBytes("System message:" + DateTime.Now.ToString() + " " + user.dic[user.GetLocalIp()] + " 客户端发送过来的心跳!~");
                    clientSocket.SendTo(buffer, remotPoint);
                }
                catch(SocketException e)
                {
                    MessageBox.Show(e.ErrorCode.ToString());
                }
            }
        }

        /// <summary>
        /// 检测用户输入状态
        /// <summary>
        private void checkEDIState()
        {
            while (ischeckEDI == true)
            {
                func();

                if (isInputing == true)
                {
                    object lockobj = new object();
                    lock (lockobj)
                    {
                        try
                        {
                            string ip = user.GetLocalIp();;
                            string tmp = "-" + "isInputing...";
                            int length = ip.Length + tmp.Length;
                            byte[] buffer = Encoding.UTF8.GetBytes(length + "-" + ip + tmp);
                            clientSocket.SendTo(buffer, remotPoint);
                        }
                        catch(SocketException e)
                        {
                            MessageBox.Show(e.ErrorCode.ToString());
                        }
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
            if (Form1.form1.REDI_MESSAGE.InvokeRequired)
            {
                Form1.form1.REDI_MESSAGE.Invoke(new MethodInvoker(() => { strLengthBefore = getLength(); }));
            }

            int strLengthAfter = 0;
            Thread.Sleep(500);
            if (Form1.form1.REDI_MESSAGE.InvokeRequired)
            {
                Form1.form1.REDI_MESSAGE.Invoke(new MethodInvoker(() => { strLengthAfter = getLength(); }));
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
                Form1.form1.REDI_SHOWMESSAGE.AppendText(user.mesdic[netip] + "已登录!!!" + "\r\n");
            }
            else
            {
                message = message.Substring(message.IndexOf(":") + 1);
                string netip = message.Substring(0, message.IndexOf(":"));
                Form1.form1.REDI_SHOWMESSAGE.SelectionAlignment = HorizontalAlignment.Center;
                Form1.form1.REDI_SHOWMESSAGE.AppendText(user.mesdic[netip] + "已退出..." + "\r\n");
            }
        }

        public void updateMessageBoxPic(string message)
        {
            Clipboard.SetData(DataFormats.Rtf, message);
            Form1.form1.REDI_SHOWMESSAGE.Paste();
        }
     
        public void updateMessageBox(string message)
        {
            object lockObj = new object();

            lock (lockObj)
            {
                //192.168.1.10-1
                int index = message.IndexOf("-");

                Dictionary<string,string> imgdic = new Dictionary<string,string>();

                //imgdic.Add("闫云皓", "http://111.229.13.33:9094/image/show/6");
                //imgdic.Add("陆子涵", "http://111.229.13.33:9094/image/show/5");
                imgdic.Add("闫云皓", @"./兔.jpg");
                imgdic.Add("陆子涵", @"./鹿.jpg");

                //自己的消息
                string name = user.dic[message.Substring(0, index)] + ":" + DateTime.Now.ToString();
          
                if(message.Substring(0, index) == user.GetLocalIp())
                {
                    Form1.form1.REDI_SHOWMESSAGE.SelectionAlignment = HorizontalAlignment.Right;
                    Form1.form1.REDI_SHOWMESSAGE.AppendText(name + "\r\n");
                    changeColor(name, Color.Green);

                    message = message.Substring(index + 1);
                    if (message.IndexOf(@"{\pict\") > -1)
                    {
                        Form1.form1.richTextBox1.AppendText(name + "\r\n");
                        Clipboard.SetData(DataFormats.Rtf, message);
                        Form1.form1.richTextBox1.Paste();
                        Form1.form1.REDI_SHOWMESSAGE.Paste();
                        Form1.form1.REDI_SHOWMESSAGE.Paste();
                    }
                    else
                    {
                        Form1.form1.REDI_SHOWMESSAGE.AppendText(message);

                        //方法学习自https://www.cnblogs.com/tuzhiyuan/p/4518076.html
                        string str = @"<script type=""text/javascript"">window.location.hash = ""#ok"";</script>
                                                <div class=""chat_content_group self"">
                                                    <img class=""chat_content_avatar"" src="" " + imgdic[name.Substring(0, name.IndexOf(":"))] + @""" width=""40px"" height=""40px"">
                                                        <p class=""chat_nick"">" + name + @"</p>
                                                    <p class=""chat_content"">" + message + @"</p>
                                                </div>
                                                <a id='ok'></a>
                                                ";
                        //string str = "<div class=receiver><div><img src=\"\.\/兔.jpg\"></div><div><div class=\"right_triangle"></div><span>" + EDIMESSAGE.value + "</span></div></div>;";
                        Form1.form1.webKitBrowser1.DocumentText = Form1.form1.webKitBrowser1.DocumentText.Replace("<a id='ok'></a>", "") + str;
                    }
                }
                else
                {
                    Form1.form1.REDI_SHOWMESSAGE.SelectionAlignment = HorizontalAlignment.Left;
                    Form1.form1.REDI_SHOWMESSAGE.AppendText(name + "\r\n");
                    changeColor(name, Color.Blue);

                    message = message.Substring(index + 1);
                    Form1.form1.REDI_SHOWMESSAGE.AppendText(message);

                    string str = @"<script type=""text/javascript"">window.location.hash = ""#ok"";</script>                         
                                <div class=""chat_content_group buddy"">
                                        <img class=""chat_content_avatar"" src=""http://face6.web.qq.com/cgi/svr/face/getface?cache=1&amp;type=1&amp;f=40&amp;uin=1286679566&amp;t=1432111720&amp;vfwebqq=5c3a30b487c67c5d37c2415dd32df3ffe3bc5b464d930ddd027d36911fc8d26a4cd23fffce868928"" width=""40px"" height=""40px"">
                                        <p class=""chat_nick"">" + name +@"</p>
                                        <p class=""chat_content"">" + message + @"</p>
                                    </div>
                                <a id='ok'></a>
                                ";
                    Form1.form1.webKitBrowser1.DocumentText = Form1.form1.webKitBrowser1.DocumentText.Replace("<a id='ok'></a>", "") + str;
                }

                Form1.form1.REDI_SHOWMESSAGE.Select(Form1.form1.REDI_SHOWMESSAGE.TextLength, 0);
                Form1.form1.REDI_SHOWMESSAGE.ScrollToCaret();
                //message = message.Substring(index + 1); 

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
            }
        }

        /// <summary>
        /// 改变richTextBox中指定字符串的颜色
        /// </summary>
        public void changeColor(string str,Color color)
        {
            ArrayList list = getIndexArray(Form1.form1.REDI_SHOWMESSAGE.Text, str);
            for (int i = 0; i < list.Count; i++)
            {
                int index = (int)list[i];
                Form1.form1.REDI_SHOWMESSAGE.Select(index, str.Length);
                Form1.form1.REDI_SHOWMESSAGE.SelectionColor = color;
            }
        }
 
        public ArrayList getIndexArray(String inputStr, String findStr)
        {
            ArrayList list = new ArrayList();
            int start = 0;
            while (start < inputStr.Length)
            {
                int index = inputStr.IndexOf(findStr, start);
                if (index >= 0)
                {
                    list.Add(index);
                    start = index + findStr.Length;
                }
                else
                {
                    break;
                }
            }
            return list;
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
