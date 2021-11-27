using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface IRemoteClient:IDisposable
    {
        Func<string,IWork> GetWork { set; }
    }
}
