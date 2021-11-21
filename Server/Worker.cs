using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Worker : IHostedService
    {
        private PathOptions path;
        private List<ServerWork> works = new();

        public Worker(IOptions<PathOptions> path)
        {
            this.path = path.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var directory in path.WorksDirectories.Select(fullPath => new DirectoryInfo(fullPath)))
            {
                foreach (var work in directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    ServerWork tempWork = ServerWork.TryCreate(work);
                    if (tempWork != null) works.Add(tempWork);
                }
            }
            //works.Sort((x, y) => x.Name.CompareTo(y.Name));

            TcpListener listener = new(IPAddress.Loopback, 8008);

            listener.Start();
            while (true)
            {
                RemoteClient client = new(await listener.AcceptTcpClientAsync());
                client.GetWorksList = () => works.Select(work => work.Metadata).ToList();
                client.GetWork = name =>
                {
                    return works.Find(work => work.Name.Equals(name)).Work;
                };
            };

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
