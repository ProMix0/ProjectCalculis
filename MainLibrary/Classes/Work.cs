using MainLibrary.Interfaces;
using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    public class Work : IWork
    {
        public string Name { get; }

        public DirectoryInfo AssemblyDirectory { get; }

        public Work(string name, DirectoryInfo directory)
        {
            Name = name;
            AssemblyDirectory = directory;
        }

        public void Execute(object[] args)
        {
            Assembly assembly = Assembly.Load(AssemblyDirectory.EnumerateFiles($"{Name}.exe").First().FullName);
            assembly.EntryPoint.Invoke(null, args);
        }
    }
}
