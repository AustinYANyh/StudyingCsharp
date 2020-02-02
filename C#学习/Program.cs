using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace chatroom_server
{
    class Client
    {
        public Socket clientSocket;
        private Thread mesManageTherad;
        private byte[] bufffer = new byte[1024*1024*4];

        public Client(Socket soc)
        {
            clientSocket = soc;
            //由于消息是不断发送的，需要多次进行处理。这里开一个线程，专门用来处理消息。
            mesManageTherad = new Thread(MessageSendFromClient);
            mesManageTherad.Start();
        }

        private void MessageSendFromClient()
        {
            //开启的线程一直检测客户端客户端发过来的消息
            while (true)
            {
                //判断连接是否断开，  SelectMode.SelectRead读状态模式。
                //poll已断开返回true
                if (clientSocket.Poll(10, SelectMode.SelectRead) == true)
                {
                    clientSocket.Close();
                    break;//终止本线程
                }
                string localip = clientSocket.RemoteEndPoint.ToString();

                try
                {
                    int byteNum = clientSocket.Receive(bufffer);
                    string mes = "";

                    mes = Encoding.UTF8.GetString(bufffer, 0, byteNum);
                    string tmp = mes;
                    string strlength = mes.Substring(0, mes.IndexOf("-"));
                    mes = mes.Substring(mes.IndexOf("-") + 1);
		    int strLength = tmp.Length;

                    if (mes != "")
                    {

                        while (byteNum > 0)
                        {
                            if (strLength >= Convert.ToInt32(strlength))
                            {
                                break;
                            }
                            byteNum = clientSocket.Receive(bufffer);
                            tmp += Encoding.UTF8.GetString(bufffer, 0, byteNum);
			    strLength = tmp.Length;
                        }
                    }

                    Console.WriteLine("客户端发送过来的消息：" + tmp);
                    //广播消息出去给每个客户端
                    Program.CastMessageTOAllConnetedClients(tmp);//对CastMessage的一层封装
                }
                catch (System.Net.Sockets.SocketException)
                {
                    Console.WriteLine(localip + " 客户端已断开连接...");
                    Program.CastMessageTOAllConnetedClients("System message: " + localip + " 已断开连接...");
                }
            }
        }

        /// <summary>
        /// Send messages to Clients
        /// </summary>
        public void CastMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }

        /// <summary>
        /// 判断是否断开连接
        /// </summary>
        public bool Conneted
        {
            get
            {
                return clientSocket.Connected;
            }
        }

    }

    class Program
    {
        static List<Client> clients = new List<Client>();
        static List<Client> notClients = new List<Client>();

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="message"></param>
        public static void CastMessageTOAllConnetedClients(string message)
        {
            foreach (var client in clients)
            {
                if (client.Conneted)
                {
                    client.CastMessage(message);
                }
                else
                {
                    notClients.Add(client);
                }
            }
            foreach (var temp in notClients)
            {
                clients.Remove(temp);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Socket tcpSever = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tcpSever.Bind(new IPEndPoint(IPAddress.Parse("172.17.0.4"), 2000));
                tcpSever.Listen(100);//监听是否有客户端发起连接
                Console.WriteLine("开始等待连接...");

                while (true)
                {
                    Socket clientSocket = tcpSever.Accept();
                    if (clientSocket != null)
                    {
                        Console.WriteLine(clientSocket.RemoteEndPoint.ToString() + " 客户端已成功连接...");
                        Client client = new Client(clientSocket);//将每个新创建的连接通信放于client类做通信
                        Program.CastMessageTOAllConnetedClients("System message:" +
                            clientSocket.RemoteEndPoint.ToString() +
                            " 客户端已成功连接...");
                        clients.Add(client);
                    }
                }
                Console.ReadKey();
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
