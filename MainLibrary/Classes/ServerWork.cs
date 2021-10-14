using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    public class ServerWork : IWork
    {
        public string Name { get; }

        public byte[] WorkCode => File.ReadAllBytes(fileName);

        private readonly string fileName;
        public ServerWork(string name, string fileName)
        {
            Name = name;
            this.fileName = fileName;
        }
    }
}
