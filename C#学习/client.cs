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

namespace chatroom
{
    public partial class Form1 : Form
    {
        public static Form1 form1;
        public static ChatManager chatm = new ChatManager();
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            form1 = this;
            chatm.Start();        }

        private void BTN_SEND_Click(object sender, EventArgs e)
        {
            //string username = user.username + ":\n";
            string username = user.username + ":" + DateTime.Now.ToString();
            //Graphics vGraphics = CreateGraphics();
            //SizeF vSizeF = vGraphics.MeasureString("事实是事实是事实是事实是事实是事实是事实是事实是事实是事实是事实是事", Font);
            //username = username.PadLeft(91);
            string message = REDI_MESSAGE.Text;

            chatm.SendMessage(username);
            chatm.SendMessage(message);

            REDI_MESSAGE.Clear();
        }

        ~Form1()
        {
            
        }

        private void REDI_MESSAGE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Alt)
            {
                BTN_SEND_Click(sender, e);
            }
        }
    }

    public class ChatManager
    {
        private string _ipAdress = "111.229.13.33";
        private int _port = 2000;
        EndPoint remotPoint;
        Socket clientSocket;
        public string message;

        Thread receiveThread;
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
            clientSocket.Connect(remotPoint);
            //因为是一直在准备接受的状态，所以开启一个线程来负责处理接受消息 
            receiveThread = new Thread(ReceiveMessageFormSever);
            receiveThread.Start();

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
                        //Form1 frm = new Form1();
                        if (Form1.form1.LISTBOX_MESSAGE.InvokeRequired)
                        {
                            Form1.form1.LISTBOX_MESSAGE.Invoke(new MethodInvoker(() => { updateListBox(message); }));
                        }
                        else
                        {
                            updateListBox(message);
                        }
                        //Form1.form1.LISTBOX_MESSAGE.Items.Add(message);
                        //frm.LISTBOX_MESSAGE.Items.Add(message);
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

        public void updateListBox(string message)
        {
            bool scroll = false;
            if (Form1.form1.LISTBOX_MESSAGE.TopIndex == Form1.form1.LISTBOX_MESSAGE.Items.Count - (int)(Form1.form1.LISTBOX_MESSAGE.Height / Form1.form1.LISTBOX_MESSAGE.ItemHeight))
                scroll = true;
            Form1.form1.LISTBOX_MESSAGE.Items.Add(message);
            if (scroll)
                Form1.form1.LISTBOX_MESSAGE.TopIndex = Form1.form1.LISTBOX_MESSAGE.Items.Count - (int)(Form1.form1.LISTBOX_MESSAGE.Height / Form1.form1.LISTBOX_MESSAGE.ItemHeight);
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            uint dummy = 0;
            byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);//启用Keep-Alive
            BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));//在这个时间间隔内没有数据交互，则发探测包 毫秒
            BitConverter.GetBytes((uint)1000).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);//发探测包时间间隔 毫秒
            //client.Client.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
        }
    }
}
