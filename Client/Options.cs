using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Options
    {
        public PathOptions Path { get; set; }
    }

    public class PathOptions
    {
        public const string Path = "Path";

        public string WorksDirectory { get; set; }
    }
}
