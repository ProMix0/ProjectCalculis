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
                    IWork work = OnWorkRequest?.Invoke(name);

                    List<FileInfo> files = work.AssemblyDirectory.EnumerateFiles("*", SearchOption.AllDirectories).ToList();
                    writer.Write(files.Count);
                    foreach (var file in files)
                    {
                        writer.Write(file.FullName[work.AssemblyDirectory.FullName.Length..]);
                        Console.WriteLine(file.FullName[work.AssemblyDirectory.FullName.Length..]);
                        using Stream fileStream = file.OpenRead();
                        writer.Write(fileStream.Length);
                        Console.WriteLine(fileStream.Length);
                        fileStream.CopyTo(stream);
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
