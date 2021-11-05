using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                })
                .RunConsoleAsync();
        }
    }
}
