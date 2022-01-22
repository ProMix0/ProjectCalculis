using MainLibrary.Classes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureAppConfiguration(configHost =>
                {
                    configHost
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("settings.json", optional: false)
                    .AddCommandLine(args);
                })
                .ConfigureServices((context, services) =>
                {
                    services

                    .Configure<ContractsOptions>(context.Configuration.GetSection(ContractsOptions.Path))
                    .Configure<PathOptions>(context.Configuration.GetSection(PathOptions.Path))
                    .Configure<Options>(context.Configuration)

                    .AddTransient<IRemoteClientFactory, RemoteClientFactory>()
                    .AddHostedService<Worker>();
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging

                    .AddConfiguration(context.Configuration.GetSection("Logging"))
                    .AddConsole();
                })
                .RunConsoleAsync();
        }
    }
}
