using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Worker : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IRemoteServer server = new RemoteServer();
            server.ConnectTo(new(IPAddress.Loopback, 8008));
            IWork work = await server.GetWorkAsync("TestWork");
            await work.Execute(new object[] { Array.Empty<string>() });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}
