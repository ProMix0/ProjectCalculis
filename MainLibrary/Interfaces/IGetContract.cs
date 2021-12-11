using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface IGetContract
    {
        Task SendData(Stream stream);
        bool IsRequest(string request);
    }
}
