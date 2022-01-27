using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientShell
{
    class Worker : IHostedService
    {
        private readonly MainWindow mainWindow;
        private readonly ILogger<Worker> logger;

        public Worker(MainWindow mainWindow, ILogger<Worker> logger)
        {
            this.mainWindow = mainWindow;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug("In StartAsync()");

            mainWindow.Show();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogError("In StopAsync()");
            throw new NotImplementedException();
        }
    }
}
