using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTest
{
    static class Program
    {
        static byte[] buffer = new byte[1024];
        static List<Socket> ClientSocketsList = new List<Socket>();
        static Socket MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static void Main(string[] args)
        {
            Console.Title = "Host";
            StartServer();
            Console.ReadLine();
        }

        static void StartServer()
        {
            Console.WriteLine("starting server...");
            MainSocket.Bind(new IPEndPoint(IPAddress.Any, 100) );

            MainSocket.Listen(5);
            MainSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }
        static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket = MainSocket.EndAccept(AR);
            ClientSocketsList.Add(socket);
            Console.Clear();
            Console.WriteLine("client connected");
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
            MainSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        static void RecieveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int recieved = socket.EndReceive(AR);
            byte[] DataBuffer = new byte[recieved];
            Array.Copy(buffer, DataBuffer, recieved);

            string text = Encoding.ASCII.GetString(DataBuffer);
            Console.WriteLine("text recieved: " + text);

            byte[] DataToSend = Encoding.ASCII.GetBytes("message recieved");
            socket.BeginSend(DataToSend, 0, DataToSend.Length, SocketFlags.None, new AsyncCallback(SendCallBack), socket);

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
        }

        static void SendCallBack(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }
    }
}
