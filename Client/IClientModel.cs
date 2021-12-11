using MainLibrary.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    public interface IClientModel
    {
        IReadOnlyCollection<IWorkMetadata> WorksMetas { get; }

        Task Execute(IWorkMetadata metadata);
    }
}