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
        private static XmlSerializer serializer = new(typeof(IWork));

        public void ConnectTo(IPEndPoint endPoint)
        {
            TcpClient client = new();
            client.Connect(endPoint);
            stream = client.GetStream();
        } 

        public async Task<IWork> GetWorkAsync(string workName)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(workName);
            await stream.WriteAsync(BitConverter.GetBytes(buffer.Length));
            await stream.WriteAsync(buffer);

            buffer = new byte[4];
            await stream.ReadAsync(buffer);
            int size = BitConverter.ToInt32(buffer);
            buffer = new byte[size];
            await stream.ReadAsync(buffer);

            return MessagePackSerializer.Deserialize<ClientWork>(buffer, ContractlessStandardResolver.Options);

            /*return MessagePackSerializer.Deserialize<ClientWork>(stream, ContractlessStandardResolver.Options);*/
        }
    }
}
