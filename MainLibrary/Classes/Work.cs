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

        public Task Execute(object[] args)
        {
            return Task.Run(() =>
            {
                string fullName = AssemblyDirectory.EnumerateFiles($"{Name}.dll").First().FullName;
                Assembly assembly = Assembly.LoadFile(fullName);
                assembly.EntryPoint.Invoke(null, args);
            });
        }
    }
}
