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
    public class Program : IClientCode,IServerCode
    {
        static void Main(string[] args)
        {
            Console.WriteLine("It's Main(), dumb -_-");

            SymmetricAlgorithm cipher = Rijndael.Create();
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
        public void Entrypoint(object argsObject)
        {
            using Stream stream = File.Create(@"C:\Users\Ученик\AppData\Roaming\ProjectCalculis\Works\TestWork\file.txt");
            using StreamWriter writer = new(stream);
            writer.WriteLine("TestWork World!");
            writer.WriteLine("It's really work?");
            writer.WriteLine(argsObject);
            //Console.WriteLine("TestWork World!");
            //Console.WriteLine("It's really work?");
            //Console.WriteLine(argsObject);
        }

        public object GetArguments()
        {
            return "123";
        }
    }
}
