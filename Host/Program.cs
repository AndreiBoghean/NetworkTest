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
        static int msgs = 0;
        static byte[] buffer = new byte[102400];
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
            IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Single(addresses => addresses.AddressFamily == AddressFamily.InterNetwork);
            
            MainSocket.Bind(new IPEndPoint(ip, 23000) );

            MainSocket.Listen(5);
            MainSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            Console.Clear();
            Console.WriteLine($"began server at {ip} 23000");
        }
        static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket = MainSocket.EndAccept(AR);
            ClientSocketsList.Add(socket);
            Console.Clear();
            Console.WriteLine($"client {ClientSocketsList.Count} connected");
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
            MainSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        static void RecieveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int recieved = socket.EndReceive(AR);
            byte[] DataBuffer = new byte[recieved];
            Array.Copy(buffer, DataBuffer, recieved);

            Console.WriteLine($"text recieved from {socket.RemoteEndPoint}: {Encoding.ASCII.GetString(DataBuffer)}");

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);

            Console.WriteLine();
            Console.WriteLine("enter reply");
            byte[] DataToSend = Encoding.ASCII.GetBytes($"reply from host: " + Console.ReadLine() );
            socket.BeginSend(DataToSend, 0, DataToSend.Length, SocketFlags.None, new AsyncCallback(SendCallBack), socket);
        }

        static void SendCallBack(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }
    }
}