using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    interface IRemoteClient
    {
        WorkRequest OnWorkRequest { set; }

        delegate IWork WorkRequest(string workName);
    }
}
