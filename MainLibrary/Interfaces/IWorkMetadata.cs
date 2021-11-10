using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Interfaces
{
    public interface IWorkMetadata
    {
        string Name { get; }
        byte[] AssemblyHash { get; }
    }
}
