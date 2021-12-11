using MainLibrary.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestWork
{
    public class Program : IClientCode, IServerCode
    {
        
        public Task<byte[]> Entrypoint(byte[] args)
        {
            //MessageBox.Show
            //Console.WriteLine("TestWork World!");
            //Console.WriteLine("It's really work?");
            //Console.WriteLine(argsObject);
            return Task.FromResult(new byte[] { 0, 9, 8 });
        }

        public void SetResult(byte[] result)
        {
            Console.WriteLine("Receive result");
        }

        public byte[] GetArgument()
        {
            return null;
        }
    }
}
