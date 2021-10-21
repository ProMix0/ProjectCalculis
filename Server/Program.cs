using MainLibrary.Classes;
using System;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerWork work = new("TestWork", "TestWork.exe");

            TcpListener listener = new(System.Net.IPAddress.Loopback, 8008);
            listener.Start();
            RemoteClient client = new(listener.AcceptTcpClient());
            client.OnWorkRequest = name =>
            {
                Console.WriteLine("Returned work");
                return new ClientWork(work);
            };

            Console.WriteLine("End program");
            Console.ReadLine();
        }
    }
}
