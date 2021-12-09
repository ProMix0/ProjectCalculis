using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Options
    {
        public PathOptions Path { get; set; }
    }

    public class PathOptions
    {
        public const string Path = "Path";

        public string[] WorksDirectories { get; set; }
    }

    public class ContractsOptions
    {
        public const string Contracts = "Contracts";

        public string[] GET { get; set; }
        public string[] POST { get; set; }
    }
}
