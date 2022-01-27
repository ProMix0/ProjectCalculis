using Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static System.Environment;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging.File;

namespace ClientShell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost host;

        public App()
        {
            host = new HostBuilder()
                .ConfigureAppConfiguration(configHost =>
                {
                    configHost
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddInMemoryCollection(new Dictionary<string, string>() { { "Path:WorksDirectory", $@"{GetFolderPath(SpecialFolder.ApplicationData)}\ProjectCalculis\Works" } })
                    .AddJsonFile(GetFolderPath(SpecialFolder.ApplicationData) + @"\ProjectCalculis\settings.json", optional: true)
                    /*.AddCommandLine(args)*/;
                })
                .ConfigureServices((context, services) =>
                {
                    services

                    .AddSingleton<MainWindow>()
                    .AddSingleton<IViewModel, ViewModel>()

                    .AddHostedService<Worker>();
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging

                    .AddConfiguration(context.Configuration.GetSection("Logging"))
                    .AddFile("Logs/log-{Date}.txt");
                })
                .AddClientModel()
                .Build();
        }

        private async void ApplicationStartup(object sender, StartupEventArgs e)
        {
            await host.StartAsync();

            MainWindow mainWindow = host.Services.GetService<MainWindow>();
            mainWindow.Show();
        }

        private async void ApplicationExit(object sender, ExitEventArgs e)
        {
            using (host)
                await host.StopAsync();
        }
    }
}
