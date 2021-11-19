using MainLibrary.Interfaces;
using System.Collections.Generic;

namespace ClientShell
{
    public interface IViewModel
    {
        ViewModel.ExecuteCommandClass ExecuteCommand { get; }
        IReadOnlyCollection<IWorkMetadata> Metadatas { get; }
    }
}