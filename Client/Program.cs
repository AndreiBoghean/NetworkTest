﻿using System;
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
            Console.ReadLine();
        }

        static void LoopConnect()
        {
            int attempts = 0;

            IPAddress addr;
            Console.WriteLine("enter IP address to connect to");
            switch (Console.ReadLine())
            {
                case "DEF": addr = IPAddress.Parse("82.28.37.155"); break;
                default: addr = IPAddress.Parse(Console.ReadLine()); break;
            }

            int port;
            Console.WriteLine("enter port to connect to");
            switch (Console.ReadLine())
            {
                case "DEF": port = 27015; break;
                default: port = Convert.ToInt32(Console.ReadLine()); break;
            }

            while (!ClientSocket.Connected)
            {
                attempts++;
                try { ClientSocket.Connect(addr, port); }
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
                
                byte[] RecievedBuffer = new byte[65536];

                int rec = ClientSocket.Receive(RecievedBuffer);

                byte[] Recieved = new byte[rec];
                Array.Copy(RecievedBuffer, Recieved, rec);

                Console.WriteLine("reply recived: " + Encoding.ASCII.GetString(Recieved) );
            }
        }
    }
}
