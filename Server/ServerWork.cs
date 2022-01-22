using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ServerWork
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

        private static ILogger<ServerWork> staticLogger;
        internal static void AddLogger(ILogger<ServerWork> logger)
        {
            if (staticLogger == null)
            {
                staticLogger = logger;
                staticLogger.LogInformation("Logger added");
            }
            else
                staticLogger.LogWarning("Attempt to add new logger");
        }

        internal static bool TryCreate(DirectoryInfo directory, out ServerWork outWork)
        {
            outWork = null;

            if (!directory.Exists)
            {
                staticLogger.LogWarning($"Directory of {directory.Name} don't exists");
                return false;
            }

            IWork work = new Work(directory.Name, directory);

            string fullName = directory.EnumerateFiles($"{directory.Name}.dll").First().FullName;
            Assembly assembly = Assembly.LoadFile(fullName);
            IServerCode server = (IServerCode)assembly
                .CreateInstance(
                assembly
                .GetExportedTypes()
                .Where(type => type.IsAssignableTo(typeof(IServerCode)))
                .First().FullName);
            if (server == null)
            {
                staticLogger.LogWarning($"Can't create IServerCode from {fullName}");
                return false;
            }

            outWork = new(work, server);
            return true;
        }
    }
}
