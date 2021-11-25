using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using Client;
using System.Windows.Input;

namespace ClientShell
{
    public class ViewModel : IViewModel
    {

        public ViewModel(IClientModel model)
        {
            this.model = model;
            ExecuteCommand = new(async parameter => await model.ExecuteAsync(parameter as IWorkMetadata),
                                 parameter => parameter != null);
        }

        private IClientModel model;

        public IReadOnlyCollection<IWorkMetadata> Metadatas => model.WorksMetas;

        public RelayCommand ExecuteCommand { get; }
    }
}
