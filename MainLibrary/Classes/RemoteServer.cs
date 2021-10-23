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

            DirectoryInfo temp = new(Path.GetTempPath()+workName);
            int count = reader.ReadInt32();
            for(int i = 0; i < count; i++)
            {
                string name = reader.ReadString();
                //Console.WriteLine(name);
                FileInfo file = new(temp + name);
                file.Directory.Create();
                using Stream fileStream = file.Create();
                byte[] buffer = new byte[reader.ReadInt32()];
                reader.BaseStream.Read(new byte[4]);
                //Console.WriteLine(buffer.Length);
                await stream.ReadAsync(buffer);
                await fileStream.WriteAsync(buffer);
            }

            return new Work(workName, temp);
        }
    }
}
