using MainLibrary.Classes;
using System;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Work work = new("TestWork", new(@"D:\Projects\ProjectCalculis\TestWork\bin\Debug\net5.0"));*/
            Work work = new("TestWork", new(@"C:\Users\Ученик\source\repos\ProjectCalculis\TestWork\bin\Debug\net5.0"));

            TcpListener listener = new(System.Net.IPAddress.Loopback, 8008);
            listener.Start();
            RemoteClient client = new(listener.AcceptTcpClient());
            client.OnWorkRequest = name =>
            {
                Console.WriteLine("Returned work");
                return work;
            };

            Console.WriteLine("End program");
            Console.ReadLine();
        }
    }
}
