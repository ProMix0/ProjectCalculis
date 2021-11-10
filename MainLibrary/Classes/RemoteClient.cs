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
        public IRemoteClient.WorkRequest GetWork { set; private get; }
        public IRemoteClient.WorksListRequest GetWorksList { set; private get; }
        private Task listenTask;
        private readonly Stream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        public RemoteClient(TcpClient client)
        {
            stream = client.GetStream();
            reader = new(stream);
            writer = new(stream);
            listenTask = Task.Run(Listen);
        }

        private void Listen()
        {
            try
            {
                while (true)
                {
                    string name = reader.ReadString();

                    if(name.Equals("GET works"))
                    {
                        List<IWork> list = GetWorksList?.Invoke();
                        writer.Write(list.Count);
                        foreach (var workToSend in list)
                        {
                            IWorkMetadata metadata = workToSend.Metadata;
                            writer.Write(metadata.Name);
                            writer.Write((int)metadata.AssemblyHash.Length);
                            writer.Write(metadata.AssemblyHash);
                        }
                        continue;
                    }

                    IWork work = GetWork?.Invoke(name);

                    List<FileInfo> files = work.AssemblyDirectory.EnumerateFiles("*", SearchOption.AllDirectories).ToList();
                    writer.Write(files.Count);
                    foreach (var file in files)
                    {
                        writer.Write(file.FullName[work.AssemblyDirectory.FullName.Length..]);

                        writer.Write((int)file.Length);

                        byte[] data = File.ReadAllBytes(file.FullName);
                        writer.Write(data);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
