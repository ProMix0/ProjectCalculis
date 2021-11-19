using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;

namespace Client
{
    public class ClientModel : IClientModel
    {
        private DirectoryInfo worksDirectory;
        private IReadOnlyCollection<IWorkMetadata> worksMetas;
        private List<IWork> works;
        private IRemoteServer server;

        public IReadOnlyCollection<IWorkMetadata> WorksMetas
        {
            get
            {
                if (worksMetas == null)
                    worksMetas = server.GetWorksListAsync().Result.AsReadOnly();
                return worksMetas;
            }
        }

        public async Task ExecuteAsync(IWorkMetadata metadata)
        {
            IWork work = await GetWorkAsync(metadata);
            await work.Execute(null);
        }

        private async Task<IWork> GetWorkAsync(IWorkMetadata metadata)
        {
            if (works == null) works = Work.CreateWorksFrom(worksDirectory);
            IWork work = works.Find(work => work.Name.Equals(metadata.Name));
            if (work == null)
            {
                work = await server.DownloadWorkAsync(metadata, worksDirectory);
                works.Add(work);
            }
            return work;
        }

        public ClientModel(IOptions<PathOptions> options, IRemoteServer server)
        {
            this.server = server;
            worksDirectory = new(options.Value.WorksDirectory);
            server.ConnectTo(new(IPAddress.Loopback, 8008));
        }
    }
}
