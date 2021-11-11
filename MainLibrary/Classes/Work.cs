﻿using MainLibrary.Interfaces;
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

        public IWorkMetadata Metadata
        {
            get
            {
                if (metadata == null)
                    metadata = new WorkMetadata(this);
                return metadata;
            }
        }
        private IWorkMetadata metadata;

        public Work(string name, DirectoryInfo directory)
        {
            Name = name;
            AssemblyDirectory = directory;
        }

        public Task Execute(object argsObject)
        {
            return Task.Run(() =>
            {
                string fullName = AssemblyDirectory.EnumerateFiles($"{Name}.dll").First().FullName;
                Assembly assembly = Assembly.LoadFile(fullName);
                foreach (var work in assembly.GetExportedTypes().Where(type => type.IsAssignableTo(typeof(IWorkCode))))
                {
                    IWorkCode instance= (IWorkCode)assembly.CreateInstance(work.FullName);
                    instance.Entrypoint(argsObject);
                }
            });
        }
    }
}
