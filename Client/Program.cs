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
            IWork work = server.GetWorkAsync("Test work").Result;
            IWorkCompiler compiler = new WorkCompiler();
            Assembly assembly=compiler.GetAssembly(work);
            assembly.EntryPoint.Invoke(null, new object[] { null });
        }
    }
}
