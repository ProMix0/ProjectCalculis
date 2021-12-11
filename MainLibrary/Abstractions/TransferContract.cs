using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainLibrary.Abstractions
{
    public abstract class TransferContract
    {
        protected readonly string requestTemplate;
        protected readonly Regex requestRegex;


        protected TransferContract(string requestTemplate, Regex requestRegex)
        {
            this.requestTemplate = requestTemplate;
            this.requestRegex = requestRegex;

        }

        protected Dictionary<string, string> Args { get; set; } = new();

        protected ConnectionSideEnum ConnectionSide { get; private set; } = ConnectionSideEnum.Undefined;

        public virtual bool IsRequest(string request)
        {
            if (ConnectionSide != ConnectionSideEnum.Server) throw new InvalidOperationException();

            if (requestRegex.IsMatch(request))
            {
                Match match = requestRegex.Match(request);
                Args = new();
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    Group group = match.Groups[i];
                    Args.Add(group.Name, group.Value);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void AsServer()
        {
            if (ConnectionSide == ConnectionSideEnum.Undefined)
                ConnectionSide = ConnectionSideEnum.Server;
            else
                throw new InvalidOperationException();
        }
        protected void AsClient()
        {
            if (ConnectionSide == ConnectionSideEnum.Undefined)
                ConnectionSide = ConnectionSideEnum.Client;
            else
                throw new InvalidOperationException();
        }

        protected enum ConnectionSideEnum
        {
            Undefined,
            Server,
            Client
        }
    }
}
