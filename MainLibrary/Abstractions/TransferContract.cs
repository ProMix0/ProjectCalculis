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

        protected readonly string[] associations;

        protected TransferContract(string requestTemplate, Regex requestRegex, string[] associations)
        {
            this.requestTemplate = requestTemplate;
            this.requestRegex = requestRegex;
            this.associations = associations;

        }

        protected string[] Args { get;  set; } 

        protected ConnectionSideEnum ConnectionSide { get; private set; } = ConnectionSideEnum.Undefined;

        public virtual bool IsRequest(string request)
        {
            if (ConnectionSide != ConnectionSideEnum.Server) throw new InvalidOperationException();

            if (requestRegex.IsMatch(request))
            {
                Match match = requestRegex.Match(request);
                List<string> groups = new();
                for (int i = 1; i < match.Groups.Count; i++)
                    groups.Add(match.Groups[i].Value);
                Args = groups.ToArray();
                return true;
            }
            else
            {
                Args = Array.Empty<string>();
                return false;
            }
        }

        public virtual TransferContract AsServer()
        {
            if (ConnectionSide == ConnectionSideEnum.Undefined)
                ConnectionSide = ConnectionSideEnum.Server;
            else
                throw new InvalidOperationException();
            return this;
        }
        public virtual TransferContract AsClient()
        {
            if (ConnectionSide == ConnectionSideEnum.Undefined)
                ConnectionSide = ConnectionSideEnum.Client;
            else
                throw new InvalidOperationException();
            return this;
        }

        protected enum ConnectionSideEnum
        {
            Undefined,
            Server,
            Client
        }
    }
}
