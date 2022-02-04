using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ClientModel> logger;

        public ClientModel(IOptions<PathOptions> options, IRemoteServer server, ILogger<ClientModel> logger)
        {
            this.server = server;
            this.logger = logger;
            worksDirectory = new(options.Value.WorksDirectory);
            server.ConnectTo(new(IPAddress.Loopback, 8008));
        }

        public IReadOnlyCollection<IWorkMetadata> WorksMetas
        {
            get
            {
                logger.LogDebug($"Calling {nameof(WorksMetas)} property");
                worksMetas ??= server.GetWorksList().Result.AsReadOnly();
                return worksMetas;
            }
        }

        public async Task Execute(IWorkMetadata metadata)
        {
            logger.LogDebug($"Calling {nameof(Execute)}()");

            Task<IWork> workTask= GetWork(metadata);
            Task<byte[]> argsTask = server.GetArgs(metadata);

            Task<byte[]> resultTask = (await workTask).Execute(await argsTask);

            await server.SendWorkResult(metadata, await resultTask);
        }

        private async Task<IWork> GetWork(IWorkMetadata metadata)
        {
            logger.LogDebug($"Getting {nameof(GetWork)}()");
            if (works == null)
                works = Work.CreateWorksFrom(worksDirectory);
            IWork work = works.Find(work => work.Name.Equals(metadata.Name));
            if (work == null || !work.Metadata.Equals(metadata))
            {
                worksDirectory.CreateSubdirectory(metadata.Name).Delete(true);
                works.Remove(work);
                work = await server.DownloadWork(metadata);
                works.Add(work);
            }
            return work;
        }
    }
}
