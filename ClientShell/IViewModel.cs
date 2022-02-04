using MainLibrary.Interfaces;
using System.Collections.Generic;

namespace ClientShell
{
    public interface IViewModel
    {
        bool CanExecuteWork { get; }
        RelayCommand ExecuteCommand { get; }
        RelayCommand LoopCommand { get; }
        IReadOnlyCollection<IWorkMetadata> Metadatas { get; }
    }
}