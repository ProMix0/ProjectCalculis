using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientShell
{
    class RelayCommandFactory : IRelayCommandFactory
    {
        private readonly ILoggerFactory loggerFactory;

        public RelayCommandFactory(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        public RelayCommand New(Action<object, ILogger> execute, Func<object, bool> canExecute = null, string category = null) =>
            new(execute, loggerFactory.CreateLogger($"{nameof(RelayCommand)}{(string.IsNullOrEmpty(category) ? "" : $": {category}")}"), canExecute);
    }
}
