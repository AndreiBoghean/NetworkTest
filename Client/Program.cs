using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    static class Program
    {
        static Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static void Main(string[] args)
        {
            Console.Title = "Client";
            LoopConnect();
            LoopSend();
        }

        static void LoopConnect()
        {
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    ClientSocket.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException)
                {
                    Console.Clear();
                    Console.WriteLine($"attempt number {attempts} has failed");
                }
            }

            Console.Clear();
            Console.WriteLine("connected at attempt number " + attempts);
        }

        static void LoopSend()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("enter a message");
                ClientSocket.Send(Encoding.ASCII.GetBytes(Console.ReadLine()));

                /*
                byte[] RecievedBuffer = new byte[1024];

                int rec = ClientSocket.Receive(RecievedBuffer);

                byte[] Recieved = new byte[rec];
                Array.Copy(RecievedBuffer, Recieved, rec);
                */
            }
        }

        static void LoopRecieve()
        {
            //ClientSocket.BeginReceive();
        }
    }
}
