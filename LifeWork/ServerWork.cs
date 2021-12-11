using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeWork
{
    public class ServerWork : IServerCode
    {
        private Random random = new();
        public byte[] GetArgument()
        {
            byte[] cells = new byte[25];
            for (int i = 0; i < cells.Length; i++)
                cells[i] = (byte)random.Next(0, 1);
            return cells;
        }

        public void SetResult(byte[] result)
        {
            foreach (var item in result)
                Console.WriteLine($"{item} ");
        }
    }
}
