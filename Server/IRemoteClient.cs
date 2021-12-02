using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface IRemoteClient:IDisposable
    {
        Func<string, byte[]> GetArgs { set; }
        Func<string, IWork> GetWork { set; }
        Func<List<IWorkMetadata>> GetWorksList { set; }
        Action<byte[], string> ReceiveResult { set; }
    }
}
