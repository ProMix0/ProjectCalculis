using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientShell
{
    class RelayCommandFactory
    {
        private readonly ILoggerFactory loggerFactory;

        public RelayCommandFactory(IServiceProvider provider)
        {
            loggerFactory = provider.GetRequiredService<ILoggerFactory>();
        }

        public RelayCommand New(Action<object> execute, string category = null, Func<object, bool> canExecute = null)
        {
            return new(execute, loggerFactory.CreateLogger(category is not null ? category : nameof(RelayCommand)), canExecute);
        }
    }
}
