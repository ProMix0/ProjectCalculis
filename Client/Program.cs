using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureAppConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    //configHost.AddInMemoryCollection(new Dictionary<string, string>() { { } });
                    configHost.AddJsonFile(GetFolderPath(SpecialFolder.ApplicationData)+@"\ProjectCalculis\settings.json", optional: true);
                    configHost.AddCommandLine(args);
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                })
                .RunConsoleAsync();
        }
    }
}
