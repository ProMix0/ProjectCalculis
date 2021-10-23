using MainLibrary.Classes;
using MainLibrary.Interfaces;
using System;
using System.Net;
using System.Reflection;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IRemoteServer server = new RemoteServer();
            server.ConnectTo(new(IPAddress.Loopback, 8008));
            IWork work = server.GetWorkAsync("TestWork").Result;
            work.Execute(new object[] { Array.Empty<string>() });
        }
    }
}
