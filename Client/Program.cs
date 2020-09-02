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
        static Socket HostSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Console.Title = "Client";
            LoopConnect();
            LoopSend();
        }

        static void LoopConnect()
        {
            int attempts = 0;
            Console.WriteLine("enter IP to connect to (enter \"auto\" to go start at 0 and keep trying)");
            string inp = Console.ReadLine();
            string GoalIp = "";
            while (!HostSocket.Connected)
            {
                attempts++;
                switch (inp)
                {
                    case "auto":
                        GoalIp = "192.168.1." + attempts;
                        break;
                    default:
                        GoalIp = inp;
                        break;
                }
                try
                {
                    HostSocket.Connect(GoalIp, 23000);
                }
                catch (SocketException)
                {
                    Console.Clear();
                    Console.WriteLine($"attempt number {attempts} to connect to {GoalIp} has failed");
                }
            }

            Console.Clear();
            Console.WriteLine($"connected at attempt number {attempts}");
        }


        static void LoopSend()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("enter a message");
                HostSocket.Send(Encoding.ASCII.GetBytes(Console.ReadLine()));

                byte[] RecievedBuffer = new byte[10240];
                HostSocket.Receive(RecievedBuffer, SocketFlags.None);
                Console.WriteLine(Encoding.ASCII.GetString(RecievedBuffer) );
            }
        }
    }
}
