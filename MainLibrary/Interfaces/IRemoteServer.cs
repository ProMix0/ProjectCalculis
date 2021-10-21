using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface IRemoteServer
    {
        void ConnectTo(IPEndPoint endPoint);

        Task<IWork> GetWorkAsync(string workName);
    }
}
