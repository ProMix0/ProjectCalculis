using MainLibrary.Interfaces;
using System.Collections.Generic;

namespace ClientShell
{
    public interface IViewModel
    {
        RelayCommand ExecuteCommand { get; }
        RelayCommand LoopCommand { get; }
        IReadOnlyCollection<IWorkMetadata> Metadatas { get; }

        bool CanExecuteWork { get; }
    }
}