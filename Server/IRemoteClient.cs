using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface IRemoteClient:IDisposable
    {
        WorkRequest GetWork { set; }

        delegate IWork WorkRequest(string workName);
        delegate List<IWorkMetadata> WorksListRequest();
    }
}
