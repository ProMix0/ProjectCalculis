using MainLibrary.Interfaces;
using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MainLibrary.Classes
{
    public class RemoteServer : IRemoteServer
    {
        private Stream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        public void ConnectTo(IPEndPoint endPoint)
        {
            TcpClient client = new();
            client.Connect(endPoint);
            stream = client.GetStream();
            reader = new(stream);
            writer = new(stream);
        }

        public async Task<IWork> GetWorkAsync(string workName)
        {
            writer.Write(workName);

            DirectoryInfo temp = new(Path.GetTempPath() + workName);
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string name = reader.ReadString();
                //Console.WriteLine($"Name: {name}, legth: {name.Length}");

                int size = reader.ReadInt32();
                //Console.WriteLine(size);

                FileInfo file = new(temp.FullName + name);
                file.Directory.Create();
                byte[] data = reader.ReadBytes(size);
                using Stream fileStream = file.Create();
                await fileStream.WriteAsync(data);

                reader.ReadBytes(4);
            }

            return new Work(workName, temp);
        }
    }
}
