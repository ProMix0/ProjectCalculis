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
        private List<IWorkMetadata> worksMetas;
        private List<IWork> works;
        private IRemoteServer server;
        public Worker(IOptions<PathOptions> options,IRemoteServer server)
        {
            path = options.Value;
            this.server = server;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            DirectoryInfo worksDirectory = new(path.WorksDirectory);
            server.ConnectTo(new(IPAddress.Loopback, 8008));

            worksMetas = await server.GetWorksListAsync();
            works = Work.CreateWorksFrom(worksDirectory);

            foreach(var meta in worksMetas)
            {
                IWork inlistWork = works.Find(work => work.Name.Equals(meta.Name));
                if (inlistWork == null)
                {
                    works.Add(await server.DownloadWorkAsync(meta,worksDirectory));
                    continue;
                }
                if (!inlistWork.Metadata.Equals(meta))
                {
                    works.Remove(inlistWork);
                    works.Add(await server.DownloadWorkAsync(meta,worksDirectory));
                }
            }
            IWork work = await server.DownloadWorkAsync(worksMetas.First(),worksDirectory);
            await work.Execute(new object[] { Array.Empty<string>() });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
