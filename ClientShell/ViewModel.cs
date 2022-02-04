using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using Client;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Threading;

namespace ClientShell
{
    public class ViewModel : AbstractViewModel
    {
        public ViewModel(IClientModel model, ILogger<ViewModel> logger, IRelayCommandFactory commandFactory)
        {
            this.model = model;
            this.logger = logger;
            ExecuteCommand = commandFactory.New(async (parameter, logger) => await Execute(parameter, logger),
                                                parameter => parameter != null && CanExecuteWork,
                                                "Work executing button");
            LoopCommand = commandFactory.New(async (parameter, logger) => await LoopExecuting(parameter, logger),
                                             parameter => parameter != null,
                                             "Loop executing button");
        }

        private async Task Execute(object parameter, ILogger logger)
        {
            if (!CanExecuteWork)
            {
                logger.LogWarning("Can't execute work: already executing");
                return;
            }
            try
            {

                if (parameter is IWorkMetadata metadata)
                {
                     lock (canExecuteLock) CanExecuteWork = false;
                    logger.LogDebug("Before work executing");
                    await model.Execute(metadata);
                    lock (canExecuteLock) CanExecuteWork = true;
                }
                else
                    logger.LogWarning("Parameter isn't IWorkMetadata");
            }
            finally
            {
            }
        }

        private async Task LoopExecuting(object parameter, ILogger logger)
        {
            looping = !looping;
            if (looping)
            {
                tokenSource = new();
                await (tempTask=Task.Run(async () =>
                 {
                     CancellationToken token = tokenSource.Token;
                     while (true)
                     {
                         if (token.IsCancellationRequested) return;
                         await Execute(parameter, logger);
                     }
                 }));
            }
            else
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
        }
        Task tempTask;

        private bool looping = false;
        private CancellationTokenSource tokenSource;

        private IClientModel model;
        private readonly ILogger<ViewModel> logger;

        private object canExecuteLock=new();

        public override IReadOnlyCollection<IWorkMetadata> Metadatas => model.WorksMetas;

        
    }
}
