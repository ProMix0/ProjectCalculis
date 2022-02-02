using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class Extensions
    {
        public static IServiceCollection AddClientModel(this IServiceCollection collection, HostBuilderContext context) => 
            collection
                     .Configure<PathOptions>(context.Configuration.GetSection(PathOptions.Path))
                     .Configure<Options>(context.Configuration)
                     .AddTransient<IRemoteServer, RemoteServer>()
                     .AddSingleton<IClientModel, ClientModel>();
             
    }
}
