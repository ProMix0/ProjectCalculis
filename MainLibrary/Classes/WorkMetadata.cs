using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    public class WorkMetadata : IWorkMetadata
    {
        public string Name { get; private set; }

        public byte[] AssemblyHash { get; private set; }

        internal WorkMetadata(string name,byte[] assemblyHash)
        {
            Name = name;
            AssemblyHash = assemblyHash;
        }

        public WorkMetadata(IWork work)
        {
            Name = work.Name;
            List<byte> hash = new();
            SHA256Managed sha = new();
            foreach(var file in work.AssemblyDirectory.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                using Stream stream = file.OpenRead();
                hash.AddRange(sha.ComputeHash(stream));
            }
            AssemblyHash = hash.ToArray();
        }
    }
}
