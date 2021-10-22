using MainLibrary.Classes;
using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface IWork
    {
        string Name { get; }
        DirectoryInfo AssemblyDirectory { get; }
        void Execute(object[] args);
    }
}
