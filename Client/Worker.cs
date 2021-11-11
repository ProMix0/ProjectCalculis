using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Worker : IHostedService
    {
        private PathOptions path;
        public Worker(IOptions<PathOptions> options)
        {
            path = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IRemoteServer server = new RemoteServer();
            server.ConnectTo(new(IPAddress.Loopback, 8008));
            List<IWorkMetadata> worksMetas = await server.GetWorksListAsync();
            List<IWork> works = Work.CreateWorksFrom(path.WorksDirectory);
            foreach(var meta in worksMetas)
            {
                IWork inlistWork = works.Find(work => work.Name.Equals(meta.Name));
                if (inlistWork == null)
                {
                    works.Add(await server.GetWorkAsync(meta));
                    continue;
                }
                if (!inlistWork.Metadata.Equals(meta))
                {
                    works.Remove(inlistWork);
                    works.Add(await server.GetWorkAsync(meta));
                }
            }
            IWork work = await server.GetWorkAsync(worksMetas.First());
            await work.Execute(new object[] { Array.Empty<string>() });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
