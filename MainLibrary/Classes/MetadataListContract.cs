using MainLibrary.Abstractions;
using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    public class MetadataListContract : GetContract<List<IWorkMetadata>>
    {

        public MetadataListContract(Func<Dictionary<string, string>, List<IWorkMetadata>> onSend) :base("WORKS", new("GET WORKS"),onSend)
        {
            AsServer();
        }

        public MetadataListContract() : base("WORKS", new("GET WORKS"),  null)
        {
            AsClient();
        }

        protected override Task<List<IWorkMetadata>> ReceiveData(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            List<IWorkMetadata> result = new();
            for (int i = 0; i < count; i++)
            {
                string name = reader.ReadString();
                byte[] hash = reader.ReadBytes(reader.ReadInt32());
                result.Add(new WorkMetadata(name, hash));
            }
            return Task.FromResult(result);
        }

        protected override Task SendData(BinaryWriter writer, List<IWorkMetadata> data)
        {
            writer.Write(data.Count);
            foreach (var metadata in data)
            {
                writer.Write(metadata.Name);
                writer.Write((int)metadata.AssemblyHash.Length);
                writer.Write(metadata.AssemblyHash);
            }
            return Task.CompletedTask;
        }
    }
}
