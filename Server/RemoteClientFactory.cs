using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Server
{
    class RemoteClientFactory : IRemoteClientFactory
    {
        private readonly IServiceProvider serviceProvider;

        public RemoteClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IRemoteClient New(TcpClient client)
        {
            return new RemoteClient(client, serviceProvider.GetRequiredService<ILogger<RemoteClient>>());
        }
    }
}
