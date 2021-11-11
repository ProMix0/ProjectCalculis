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

        internal WorkMetadata(string name, byte[] assemblyHash)
        {
            Name = name;
            AssemblyHash = assemblyHash;
        }

        public WorkMetadata(IWork work)
        {
            Name = work.Name;
            AssemblyHash = work.CalculateHash();
        }

        public override bool Equals(object obj)
        {
            if(obj is IWorkMetadata metadata)
            {
                if (AssemblyHash.Length != metadata.AssemblyHash.Length) return false;
                for(int i = 0; i < AssemblyHash.Length; i++)
                {
                    if (AssemblyHash[i] != metadata.AssemblyHash[i]) return false;
                }
                return true;
            }
            return base.Equals(obj);
        }
    }
}
