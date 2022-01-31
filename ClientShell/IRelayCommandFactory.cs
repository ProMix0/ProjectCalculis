using Microsoft.Extensions.Logging;
using System;

namespace ClientShell
{
    public interface IRelayCommandFactory
    {
        RelayCommand New(Action<object, ILogger> execute, Func<object, bool> canExecute = null, string category = null);
    }
}