using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeWork
{
    class ServerWork : IServerCode
    {
        public byte[] GetArgument()
        {
            return new byte[] { 3, 2, 1 };
        }

        public void SetResult(byte[] result)
        {
            foreach (var item in result)
                Console.Write($"{item} ");
        }
    }
}
