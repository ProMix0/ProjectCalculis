using MainLibrary.Interfaces;
using System.Collections.Generic;

namespace ClientShell
{
    public interface IViewModel
    {
        RelayCommand ExecuteCommand { get; }
        IReadOnlyCollection<IWorkMetadata> Metadatas { get; }
    }
}