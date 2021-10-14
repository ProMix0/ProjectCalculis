using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    [Serializable]
    public class ClientWork : IWork
    {
        public string Name { get; }

        public byte[] WorkCode { get; }

        internal ClientWork(ServerWork work)
        {
            Name = work.Name;
            WorkCode = work.WorkCode;
        }
    }
}
