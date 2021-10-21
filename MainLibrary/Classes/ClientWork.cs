using MainLibrary.Interfaces;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class ClientWork : IWork
    {
        public string Name { get; }

        public byte[] WorkCode { get; }

        public ClientWork(ServerWork work)
        {
            Name = work.Name;
            WorkCode = work.WorkCode;
        }

        public ClientWork(string name, byte[] workCode)
        {
            Name = name;
            WorkCode = workCode;
        }
    }
}
