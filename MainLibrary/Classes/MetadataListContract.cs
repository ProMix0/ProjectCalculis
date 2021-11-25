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
    public class MetadataListContract : ITransferContract<List<IWorkMetadata>>
    {
        private string requestTemplate = "GET WORKS";
        private Regex requestRegex;

        public MetadataListContract()
        {
            requestRegex = new(requestTemplate);
        }

        public bool IsRequest(string request, out string[] args)
        {
            args = Array.Empty<string>();
            return requestRegex.IsMatch(request);
        }

        public Task<List<IWorkMetadata>> ReceiveData(Stream stream,string[] args)
        {
            using BinaryWriter writer = new(stream, Encoding.UTF8, true);
            using BinaryReader reader = new(stream, Encoding.UTF8, true);
            writer.Write(requestTemplate);

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

        public Task SendData(Stream stream, List<IWorkMetadata> data)
        {
            using BinaryWriter writer = new(stream, Encoding.UTF8, true);
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
