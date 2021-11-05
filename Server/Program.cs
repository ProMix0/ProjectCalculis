using MainLibrary.Classes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IHostBuilder builder = new HostBuilder();
            await builder
                .ConfigureAppConfiguration(configHost=>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("settings.json", optional: false);
                    configHost.AddCommandLine(args);
                })
                .ConfigureServices((context,services )=>
                {
                    services.Configure<PathOptions>(context.Configuration.GetSection(PathOptions.Path));
                    services.AddHostedService<Worker>();
                })
                .RunConsoleAsync();
        }
    }
}
