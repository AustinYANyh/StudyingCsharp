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
            chatm.Start();
        }

        private void BTN_SEND_Click(object sender, EventArgs e)
        {
            string username = user.username + ":\n";
            string message = username + REDI_MESSAGE.Text;

            //chatm.SendMessage(username);
            chatm.SendMessage(message);

            REDI_MESSAGE.Clear();
        }
    }

    public class ChatManager
    {
        private string _ipAdress = "127.0.0.1";
        private int _port = 9000;
        EndPoint remotPoint;
        Socket clientSocket;
        public string message;

        Thread receiveThread;
        byte[] bufferReceive = new byte[4096];

        public void Start()
        {
            ConnetToSever(_ipAdress, _port);
        }

        public void Update(string message)
        {
            
        }

        public void ConnetToSever(string ipadress, int port)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            remotPoint = new IPEndPoint(IPAddress.Parse(ipadress), port);
            //建立连接
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
                            Form1.form1.LISTBOX_MESSAGE.Invoke(new MethodInvoker(() => { Form1.form1.LISTBOX_MESSAGE.Items.Add(message); }));
                        }
                        else
                        {
                            Form1.form1.LISTBOX_MESSAGE.Items.Add(message);
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
    }
}
