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
    public class Program : IWorkCode
    {
        static void Main(string[] args)
        {
            Console.WriteLine("It's Main(), dumb -_-");

            SymmetricAlgorithm cipher = Rijndael.Create();
            cipher.GenerateIV();
            cipher.GenerateKey();

            Server(cipher.CreateDecryptor());

            using TcpClient server = new();
            server.Connect(IPAddress.Loopback, 3004);
            CryptoStream stream = new(server.GetStream(), cipher.CreateEncryptor(), CryptoStreamMode.Write);
            using BinaryWriter writer = new(stream,Encoding.UTF8, true);
            for (int i = 0; i < 30; i++)
                writer.Write((byte)(10 + i));
            stream.Dispose();
        }

        private static void Server(ICryptoTransform transform)
        {
            Task.Run(() =>
            {

                TcpListener listener = new(IPAddress.Any, 3004);
                listener.Start();
                using TcpClient client = listener.AcceptTcpClient();
                using CryptoStream stream = new(client.GetStream(), transform, CryptoStreamMode.Read);
                using BinaryReader reader = new(stream);
                Console.WriteLine(reader.ReadByte());
                Console.WriteLine(reader.ReadByte());
            });
        }

        public void Entrypoint(object argsObject)
        {
            Console.WriteLine("TestWork World!");
            Console.WriteLine("It's really work?");
            Console.WriteLine(argsObject);
        }
    }
}
