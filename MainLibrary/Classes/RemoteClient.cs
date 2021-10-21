using MainLibrary.Interfaces;
using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MainLibrary.Classes
{
    public class RemoteClient : IRemoteClient
    {
        public IRemoteClient.WorkRequest OnWorkRequest { set; private get; }
        private Task listenTask;
        private readonly Stream stream;
        static private XmlSerializer serializer = new(typeof(IWork));

        public RemoteClient(TcpClient client)
        {
            stream = client.GetStream();
            listenTask = Task.Run(Listen);
        }

        private void Listen()
        {
            while (true)
            {
                byte[] buffer = new byte[4];
                stream.Read(buffer);
                buffer = new byte[BitConverter.ToInt32(buffer)];
                stream.Read(buffer);

                string name = Encoding.UTF8.GetString(buffer);
                ClientWork work= (ClientWork)(OnWorkRequest?.Invoke(name));
                //buffer =Encoding.UTF8.GetBytes(JsonSerializer.Serialize(work));
                buffer = MessagePackSerializer.Serialize(work.GetType(), work, ContractlessStandardResolver.Options);
                stream.Write(BitConverter.GetBytes(buffer.Length));
                stream.Write(buffer);

                /*stream.Write(BitConverter.GetBytes(buffer.Length));
                stream.Write(buffer);*/
            }
        }
    }
}
