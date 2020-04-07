using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace studyWPF_1.viewMode
{
    class MainWindowViewMode : BindableBase
    {
        ChatManager chatm = new ChatManager();

        public DelegateCommand<object> SelectItemChangedCommand { get; set; }
        public DelegateCommand<object> closeCommand { get; set; }
        public DelegateCommand<object> send_Click { get; set; }

        public MainWindowViewMode()
        {
            chatm.Start();

            friends = new ObservableCollection<Friend>();
            friends.Add(new Friend() { NickName = "染墨灬若流云", Head = new BitmapImage(new Uri("pack://application:,,,/Resources/head1.jpg")) });
            friends.Add(new Friend() { NickName = "执笔灬绘浮沉", Head = new BitmapImage(new Uri("pack://application:,,,/Resources/head2.jpg")) });
            friends.Add(new Friend() { NickName = "素手灬挽秋风", Head = new BitmapImage(new Uri("pack://application:,,,/Resources/github.png")) });

            values = new ObservableCollection<itemValue>();
            values.Add(new itemValue() { Value = "Orange" });
            values.Add(new itemValue() { Value = "Apple" });
            values.Add(new itemValue() { Value = "Pear" });

            SelectItemChangedCommand = new DelegateCommand<object>((p) =>
            {
                ListView lv = p as ListView;
                Friend friend = lv.SelectedItem as Friend;
                Head = friend.Head;
                NickName = friend.NickName;
            });

            closeCommand = new DelegateCommand<object>((p) =>
            {
                Application.Current.Shutdown();
            });

            messages = new ObservableCollection<Message>();

            send_Click = new DelegateCommand<object>((p) =>
            {
                RichTextBox messageBox = p as RichTextBox;
                string message = new TextRange(messageBox.Document.ContentStart, messageBox.Document.ContentEnd).Text;
                messageBox.Document.Blocks.Clear();

                string ip = user.GetLocalIp();
                message = message.Length + "-" + ip + "-" + message;
                //messages.Add(new Message() { SendMessage = message });
                chatm.SendMessage(message);
            });
        }

        public class ChatManager
        {
            private string _ipAdress = "111.229.13.33";
            //private string _ipAdress = "127.0.0.1";
            private int _port = 2000;
            EndPoint remotPoint;
            public Socket clientSocket;
            public string message;
            public bool isInputing = true;
            public bool ischeckEDI = true;
            public bool iskeepalive = true;
            public int PY = 19;

            Thread receiveThread;
            Thread stateThread;

            //缓冲区大小分配为4M
            public byte[] bufferReceive = new byte[1024 * 1024 * 4];

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
                catch (SocketException)
                {
                    //MessageBox.Show("服务器未开启...请先开启...");
                    MessageBox.Show("服务器未开启...请先开启...\r\nThe server is outline...Please open first...");

                }

                //创建线程执行接收和检测用户输入状态
                receiveThread = new Thread(ReceiveMessageFormSever);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                //stateThread = new Thread(checkEDIState);
                //stateThread.Start();

                //防止断开连接,每5分钟发送一次keep alive
                //Thread keepAlive = new Thread(KeepAlive);
                //keepAlive.Start();

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
                            lock (lockobj)
                            {
                                int length = clientSocket.Receive(bufferReceive, bufferReceive.Length, 0);
                                message = Encoding.UTF8.GetString(bufferReceive, 0, length);
                                string strlength = "";
                                if (length != 0)
                                {
                                    strlength = message.Substring(0, message.IndexOf("-"));
                                    message = message.Substring(message.IndexOf("-") + 1);
                                }

                                while (length > 0)
                                {
                                    if (message.Length >= Convert.ToInt32(strlength))
                                    {
                                        break;
                                    }

                                    length = clientSocket.Receive(bufferReceive, bufferReceive.Length, 0);
                                    message += Encoding.UTF8.GetString(bufferReceive, 0, length);
                                }

                                //登录或断线消息---系统消息
                                //System message:223.167.169.200:31966客户端已成功连接...
                                //System message:223.167.169.200:31966已断开连接...
                                if (message.IndexOf("System message:") != -1)
                                {
                                    if (message.IndexOf("Rock") != -1)
                                    {
                                        //Form1.form1.windowRock();
                                    }
                                    else if (message.IndexOf("连接") != -1)
                                    {
                                        //if (Form1.form1.REDI_MESSAGE.InvokeRequired)
                                        {
                                           // Form1.form1.REDI_MESSAGE.Invoke(new MethodInvoker(() => { _update(message); }));
                                        }
                                        //else
                                        {
                                            //_update(message);
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
                                    //if (Form1.form1.REDI_SHOWMESSAGE.InvokeRequired)
                                    {
                                        //Form1.form1.REDI_SHOWMESSAGE.Invoke(new MethodInvoker(() => { updateMessageBox(message); }));
                                    }
                                    isInputing = true;
                                    ischeckEDI = true;
                                    iskeepalive = true;
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
                                            //if (Form1.form1.LAB_STATE.InvokeRequired)
                                            {
                                                //Form1.form1.LAB_STATE.Invoke(new MethodInvoker(() => { updateLab(); }));
                                            }
                                        }
                                        else if (mesIp == ip && mes == "isInputing...")
                                        {
                                            //do nothing
                                        }
                                        //客户端发来的消息,更新到listbox
                                        else
                                        {
                                            //if (Form1.form1.REDI_MESSAGE.InvokeRequired)
                                            {
                                                //Form1.form1.REDI_MESSAGE.Invoke(new MethodInvoker(() => { updateMessageBox(message); }));
                                            }
                                            //else
                                            {
                                                //updateMessageBox(message);
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
        }

        #region 数据结构和访问器
        public class Friend
        {
            public string NickName { get; set; }
            public BitmapImage Head { get; set; }
        }

        public class itemValue
        {
            public string Value { get; set; }
        }

        public class Message
        {
            public string SendMessage { get; set; }
        }

        private ObservableCollection<Friend> friends;
        private ObservableCollection<itemValue> values;

        public ObservableCollection<Friend> Friends
        {
            get
            {
                return friends;
            }
            set
            {
                friends = value;
            }
        }

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
            }
        }
        public ObservableCollection<itemValue> itemValues
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
            }
        }

        private BitmapImage headPic;

        public BitmapImage Head
        {
            get
            {
                return headPic;
            }
            set
            {
                SetProperty(ref headPic, value);
            }
        }

        private string nickname;

        public string NickName
        {
            get
            {
                return nickname;
            }
            set
            {
                SetProperty(ref nickname, value);
            }
        }

        private string sendMessage;

        public string SendMessage
        {
            get
            {
                return sendMessage;
            }
            set
            {
                sendMessage = value;
            }
        }

        public string itemvalue;

        public string Value
        {
            get
            {
                return itemvalue;
            }
            set
            {
                SetProperty(ref itemvalue, value);
            }
        }
        #endregion
    }
}
