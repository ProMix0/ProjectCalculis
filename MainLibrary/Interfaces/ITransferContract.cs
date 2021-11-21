using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface ITransferContract<T>
    {
        bool IsRequest(string request, out string[] args);
        Task<T> ReceiveData(Stream stream, string[] args);
        Task SendData(Stream stream, T data);
    }
}
