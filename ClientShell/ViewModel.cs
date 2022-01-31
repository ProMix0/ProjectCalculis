using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using Client;
using System.Windows.Input;
using Microsoft.Extensions.Logging;

namespace ClientShell
{
    public class ViewModel : IViewModel
    {

        public ViewModel(IClientModel model, ILogger<ViewModel> logger, IRelayCommandFactory commandFactory)
        {
            this.model = model;
            this.logger = logger;
            ExecuteCommand = commandFactory.New(async (parameter, logger) =>
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
                                 },
                                 parameter => parameter != null && CanExecuteWork,
                                "Work executing button");
        }

        private IClientModel model;
        private readonly ILogger<ViewModel> logger;

        public IReadOnlyCollection<IWorkMetadata> Metadatas => model.WorksMetas;

        public RelayCommand ExecuteCommand { get; }

        public bool CanExecuteWork { get; set; } = true;
    }
}
