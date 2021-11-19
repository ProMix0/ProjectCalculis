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
        public static IHostBuilder AddClientModel(this IHostBuilder builder)
        {
            return builder.ConfigureServices((context, services) =>
            {
                services
                .Configure<PathOptions>(context.Configuration.GetSection(PathOptions.Path))
                .Configure<Options>(context.Configuration)
                .AddScoped<IRemoteServer, RemoteServer>()
                .AddSingleton<IClientModel, ClientModel>();
            });
        }
    }
}
