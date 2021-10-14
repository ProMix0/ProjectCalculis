using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    interface IRemoteServer
    {
        void ConnectTo(string ip, int port);

        IWork GetWork(string workName);
    }
}
