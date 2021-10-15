using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface IRemoteServer
    {
        void ConnectTo(string ip, int port);

        Task<IWork> GetWorkAsync(string workName);
    }
}
