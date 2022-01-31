using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientShell
{
    public class RelayCommand : ICommand
    {
        private Action<object, ILogger> execute;
        private readonly ILogger logger;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        internal RelayCommand(Action<object, ILogger> execute, ILogger logger, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.logger = logger;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            logger.LogDebug("CanExecute() called");
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            logger.LogDebug("Execute() called");
            execute(parameter, logger);
        }
    }
}
