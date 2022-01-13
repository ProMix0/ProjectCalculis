using MainLibrary.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestWork
{
    public class Program : IClientCode, IServerCode
    {
        
        public async Task<byte[]> Entrypoint(byte[] args)
        {
            await Task.Delay(10000);

            return new byte[] { 0, 9, 8 };
        }

        public void SetResult(byte[] result)
        {
            Console.Write("Receive result:");
            foreach (var n in result.Concat("\n\r".Cast<byte>()))
                Console.Write($" {n}");
        }

        public byte[] GetArgument()
        {
            return new byte[1];
        }
    }
}
