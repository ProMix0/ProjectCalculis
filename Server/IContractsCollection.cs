using MainLibrary.Abstractions;
using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IContractsCollection
    {
        IReadOnlyList<IGetContract> GetContracts { get; }
        IReadOnlyList<IPostContract> PostContracts { get; }
    }
}
