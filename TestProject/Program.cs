using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("It's Main(), dumb -_-");

            SymmetricAlgorithm cipher = Aes.Create();
            //cipher.Mode = CipherMode.CTS;
            //cipher.BlockSize = 8;
            cipher.GenerateIV();
            cipher.GenerateKey();

            Server(cipher);

            using TcpClient server = new();
            server.Connect(IPAddress.Loopback, 3004);
            Stream stream = server.GetStream();
            using BinaryWriter writer = new(new CryptoStream(stream, cipher.CreateDecryptor(), CryptoStreamMode.Write));
            using BinaryReader reader = new(new CryptoStream(stream, cipher.CreateDecryptor(), CryptoStreamMode.Read));
            writer.Write(10);
            Console.WriteLine($"Client receive: {reader.ReadInt32()}");
            stream.Dispose();
        }

        private static void Server(SymmetricAlgorithm cipher)
        {
            Task.Run(() =>
            {

                TcpListener listener = new(IPAddress.Any, 3004);
                listener.Start();
                using TcpClient client = listener.AcceptTcpClient();
                using Stream stream = client.GetStream();
                using BinaryWriter writer = new(new CryptoStream(stream, cipher.CreateDecryptor(), CryptoStreamMode.Write));
                using BinaryReader reader = new(new CryptoStream(stream, cipher.CreateDecryptor(), CryptoStreamMode.Read));
                int temp = reader.ReadInt32();
                Console.WriteLine($"Server receive: {temp}");
                writer.Write(temp + 1);
            });
        }
    }
}
