using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ContractsCollection : IContractsCollection
    {
        public IReadOnlyList<IGetContract> GetContracts => getContracts.AsReadOnly();

        public IReadOnlyList<IPostContract> PostContracts => postContracts.AsReadOnly();

        private List<IGetContract> getContracts = new();
        private List<IPostContract> postContracts = new();

        public void Add(IGetContract contract)
        {
            getContracts.Add(contract);
        }
        public void Add(IPostContract contract)
        {
            postContracts.Add(contract);
        }
    }
}
