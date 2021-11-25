using MainLibrary.Classes;
using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerWork
    {
        public IWork Work { get; }
        public IWorkMetadata Metadata => Work.Metadata;
        public string Name => Work.Name;
        public IServerCode Server { get; }

        private ServerWork(IWork work, IServerCode server)
        {
            Work = work;
            Server = server;
        }

        public static ServerWork TryCreate(DirectoryInfo directory)
        {
            if (!directory.Exists) return null;

            IWork work = new Work(directory.Name, directory);
            if (work == null) return null;

            string fullName = directory.EnumerateFiles($"{directory.Name}.dll").First().FullName;
            Assembly assembly = Assembly.LoadFile(fullName);
            IServerCode server = (IServerCode)assembly
                .CreateInstance(
                assembly
                .GetExportedTypes()
                .Where(type => type.IsAssignableTo(typeof(IServerCode)))
                .First().FullName);
            if (server == null) return null;

            return new(work, server);
        }
    }
}
