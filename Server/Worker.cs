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
        private ContractsOptions contracts;
        private List<ServerWork> works = new();
        private IContractsCollection contractsCollection;

        public Worker(IOptions<Options> options)
        {
            path = options.Value.Path;
            contracts = options.Value.Contracts;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var directory in path.WorksDirectories.Select(fullPath => new DirectoryInfo(fullPath)))
            {
                if (directory.Exists)
                    foreach (var work in directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                    {
                        ServerWork tempWork = ServerWork.TryCreate(work);
                        if (tempWork != null) works.Add(tempWork);
                    }
            }
            //works.Sort((x, y) => x.Name.CompareTo(y.Name));

            AddContracts();

            TcpListener listener = new(IPAddress.Any, 8008);

            listener.Start();
            while (true)
            {
                IRemoteClient client = new RemoteClient(await listener.AcceptTcpClientAsync());
                client.SetContracts(contractsCollection);
            };

        }

        private void AddContracts()
        {
            ContractsCollection collection = new();
            foreach(var get in contracts.GET)
            {
                switch (get)
                {
                    case nameof(ArgsContract):
                        collection.Add(new ArgsContract(args => works.Find(work => work.Name.Equals(args["name"])).Server.GetArgument()));
                        break;
                    case nameof(MetadataContract):
                        collection.Add(new MetadataContract(_ => works.Select(work => work.Metadata).ToList()));
                        break;
                    case nameof(WorkContract):
                        collection.Add(new WorkContract(args => works.Find(work => work.Name.Equals(args["name"])).Work));
                        break;
                }
            }
            foreach (var post in contracts.POST)
            {
                switch (post)
                {
                    case nameof(ResultContract):
                        collection.Add(new ResultContract((result, args) => works.Find(work => work.Name.Equals(args["name"])).Server.SetResult(result)));
                        break;
                }
            }
            contractsCollection = collection;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
