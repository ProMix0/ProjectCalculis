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
            Console.WriteLine(BitConverter.ToInt32(Encoding.UTF8.GetBytes("0тd ")));
            foreach (byte b in Encoding.UTF8.GetBytes(@"Ї  "))
                Console.WriteLine(b);
            await new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                })
                .RunConsoleAsync();
        }
    }
}
