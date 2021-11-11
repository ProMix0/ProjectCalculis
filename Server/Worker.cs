using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Worker : IHostedService
    {
        private PathOptions path;
        private List<IWork> works = new();

        public Worker(IOptions<PathOptions> path)
        {
            this.path = path.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var directory in path.WorksDirectories.Select(fullPath => new DirectoryInfo(fullPath)))
            {
                if (directory.Exists)
                    foreach (var work in directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                        works.Add(new Work(work.Name, work));
            }
            works.Sort((x, y) => x.Name.CompareTo(y.Name));

            TcpListener listener = new(System.Net.IPAddress.Loopback, 8008);
            listener.Start();
            return Task.Run(() =>
                {
                    while (true)
                    {
                        RemoteClient client = new(listener.AcceptTcpClient());
                        client.GetWorksList = () => works;
                        client.GetWork = name =>
                        {
                            Console.WriteLine("Returned work");
                            return works.Find(work => work.Name.Equals(name));
                        };
                    }
                }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
