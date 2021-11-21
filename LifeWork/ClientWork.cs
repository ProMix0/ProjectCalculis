using MainLibrary.Interfaces;
using System;
using System.Threading.Tasks;

namespace LifeWork
{
    public class ClientWork : IClientCode
    {
        public Task<byte[]> Entrypoint(byte[] args)
        {
            return Task.FromResult(new byte[] { 1, 2, 3 });
        }
    }
}
