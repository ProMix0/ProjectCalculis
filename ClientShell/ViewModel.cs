using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using Client;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ClientShell
{
    public class ViewModel : IViewModel
    {

        public ViewModel(IClientModel model, ILogger<ViewModel> logger, IRelayCommandFactory commandFactory)
        {
            this.model = model;
            this.logger = logger;
            ExecuteCommand = commandFactory.New(async (parameter, logger) => await Execute(parameter, logger),
                                                parameter => parameter != null && CanExecuteWork,
                                                "Work executing button");
            LoopCommand = commandFactory.New(async (parameter, logger) => await LoopExecuting(parameter, logger),
                                             category: "Loop executing button");
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
                    CanExecuteWork = false;
                    logger.LogDebug("Before work executing");
                    await model.Execute(metadata);
                    CanExecuteWork = true;
                }
                else
                    logger.LogWarning("Parameter isn't IWorkMetadata");
            }
            finally
            {
                CanExecuteWork = true;
            }
        }

        private async Task LoopExecuting(object parameter, ILogger logger)
        {
            looping = !looping;
            if (looping && (loopTask == null || !loopTask.IsCompleted))
                await (loopTask = Task.Run(async () =>
                 {
                     while (looping)
                     {
                         await Execute(parameter, logger);
                     }
                 }));
        }

        private bool looping = false;
        private Task loopTask;

        private IClientModel model;
        private readonly ILogger<ViewModel> logger;

        public IReadOnlyCollection<IWorkMetadata> Metadatas => model.WorksMetas;

        public RelayCommand ExecuteCommand { get; }
        public RelayCommand LoopCommand { get; }

        public bool CanExecuteWork { get; set; } = true;
    }
}
