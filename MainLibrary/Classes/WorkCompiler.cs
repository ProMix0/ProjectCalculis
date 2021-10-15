using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    class WorkCompiler : IWorkCompiler
    {
        public Assembly GetAssembly(IWork work)
        {
            return Assembly.Load(work.WorkCode);
        }
    }
}
