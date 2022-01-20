using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        private ContractsOptions contracts;
        private List<ServerWork> works = new();
        private IContractsCollection contractsCollection;
        private readonly Logger<Worker> logger;

        public Worker(IOptions<Options> options, Logger<Worker> logger, Logger<ServerWork> serverLogger)
        {
            path = options.Value.Paths;
            contracts = options.Value.Contracts;
            this.logger = logger;

            ServerWork.AddLogger(serverLogger);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var directory in path.WorksDirectories.Select(fullPath => new DirectoryInfo(fullPath)))
            {
                if (directory.Exists)
                {
                    logger.LogInformation($"{directory.FullName} exists");
                    foreach (var work in directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                    {
                        if (ServerWork.TryCreate(work, out ServerWork tempWork))
                        {
                            logger.LogInformation($"{work.Name} work successfuly created");
                            works.Add(tempWork);
                        }
                        else
                            logger.LogInformation($"{work.Name} don't created");
                    }
                }
                else
                    logger.LogInformation($"{directory.FullName} don't exists");
            }
            //works.Sort((x, y) => x.Name.CompareTo(y.Name));

            AddContracts();

            TcpListener listener = new(IPAddress.Any, 8008);

            listener.Start();
            logger.LogInformation("Listener started");
            while (true)
            {
                IRemoteClient client = new RemoteClient(await listener.AcceptTcpClientAsync());
                client.SetContracts(contractsCollection);
            };

        }

        private void AddContracts()
        {
            ContractsCollection collection = new();
            foreach(var contractName in contracts.GET)
            {
                switch (contractName)
                {
                    case nameof(ArgsContract):
                        collection.Add(new ArgsContract(args => works.Find(work => work.Name.Equals(args["name"])).Server.GetArgument()));
                        logger.LogInformation($"{nameof(ArgsContract)} added");
                        break;
                    case nameof(MetadataContract):
                        collection.Add(new MetadataContract(_ => works.Select(work => work.Metadata).ToList()));
                        logger.LogInformation($"{nameof(MetadataContract)} added");
                        break;
                    case nameof(WorkContract):
                        collection.Add(new WorkContract(args => works.Find(work => work.Name.Equals(args["name"])).Work));
                        logger.LogInformation($"{nameof(WorkContract)} added");
                        break;
                    default:
                        logger.LogWarning($"Can't add contract: {contractName}");
                        break;
                }
            }
            foreach (var contractName in contracts.POST)
            {
                switch (contractName)
                {
                    case nameof(ResultContract):
                        collection.Add(new ResultContract((result, args) => works.Find(work => work.Name.Equals(args["name"])).Server.SetResult(result)));
                        logger.LogInformation($"{nameof(ResultContract)} added");
                        break;
                    default:
                        logger.LogWarning($"Can't add contract: {contractName}");
                        break;
                }
            }
            contractsCollection = collection;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogError("StopAsync() called");
            throw new NotImplementedException();
        }
    }
}
