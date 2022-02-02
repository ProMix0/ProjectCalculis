using Microsoft.Extensions.Logging;
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

        protected ILogger logger { get; private set; }
        protected TransferContract(string requestTemplate, Regex requestRegex)
        {
            this.requestTemplate = requestTemplate;
            this.requestRegex = requestRegex;
        }

        protected Dictionary<string, string> Args { get; set; } = new();

        protected ConnectionSideEnum ConnectionSide { get; private set; } = ConnectionSideEnum.Undefined;

        public virtual bool IsRequest(string request)
        {
            if (ConnectionSide != ConnectionSideEnum.Server)
            {
                logger?.LogError($"Can't call {nameof(IsRequest)} due to it isn't server side");
                throw new InvalidOperationException();
            }

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
            logger?.LogDebug("Trying set as server");
            if (ConnectionSide == ConnectionSideEnum.Undefined)
                ConnectionSide = ConnectionSideEnum.Server;
            else
            {
                logger?.LogError($"Can't set as server: current state {ConnectionSide}, {(int)ConnectionSide}");
                throw new InvalidOperationException();
            }
            logger?.LogDebug("Setted as server");
        }
        protected void AsClient()
        {
            logger?.LogDebug("Trying set as client");
            if (ConnectionSide == ConnectionSideEnum.Undefined)
                ConnectionSide = ConnectionSideEnum.Client;
            else
            {
                logger?.LogError($"Can't set as client: current state {ConnectionSide}, {(int)ConnectionSide}");
                throw new InvalidOperationException();
            }
            logger?.LogDebug("Setted as client");
        }

        public T AddLogger<T>(ILogger<T> logger)
            where T : TransferContract
        {
            this.logger ??= logger;
            return (T)this;
        }

        protected enum ConnectionSideEnum
        {
            Undefined,
            Server,
            Client
        }
    }
}
