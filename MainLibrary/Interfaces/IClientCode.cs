using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface IClientCode
    {
        Task<byte[]> Entrypoint(byte[] args);
    }
}
