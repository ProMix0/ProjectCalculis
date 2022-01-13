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
            ExecuteCommand = new(async parameter =>
                                {
                                    CanExecuteWork = false;
                                    await model.Execute(parameter as IWorkMetadata);
                                    CanExecuteWork = true;
                                },
                                 parameter => parameter != null && CanExecuteWork);
        }

        private IClientModel model;

        public IReadOnlyCollection<IWorkMetadata> Metadatas => model.WorksMetas;

        public RelayCommand ExecuteCommand { get; }

        public bool CanExecuteWork { get; set; } = true;
    }
}
