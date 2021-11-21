using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface IRemoteServer : IDisposable
    {
        void ConnectTo(IPEndPoint endPoint);

        Task<List<IWorkMetadata>> GetWorksListAsync();

        Task<IWork> DownloadWorkAsync(IWorkMetadata workMetadata);
    }
}
