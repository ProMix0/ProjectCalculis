using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface IServerCode
    {
        byte[] GetArgument();
        void SetResult(byte[] result);
    }
}
