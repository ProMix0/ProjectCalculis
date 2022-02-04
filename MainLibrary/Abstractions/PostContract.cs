using MainLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainLibrary.Abstractions
{
    public abstract class PostContract<T> : TransferContract, IPostContract
        where T : class
    {
        private readonly Action<T, Dictionary<string, string>> onReceive;

        protected PostContract(string requestTemplate, Regex requestRegex, Action<T, Dictionary<string, string>> onReceive) : base(requestTemplate, requestRegex)
        {
            this.onReceive = onReceive;
        }

        public async Task ReceiveData(Stream stream)
        {
            if (ConnectionSide != ConnectionSideEnum.Server)
            {
                logger?.LogError($"Can't call {nameof(ReceiveData)} due to it isn't server side");
                throw new InvalidOperationException();
            }

            T data = await ReceiveDataInner(stream);
            onReceive?.Invoke(data, Args);
        }

        protected abstract Task<T> ReceiveDataInner(Stream stream);

        public async Task SendData(Stream stream, T data, Dictionary<string, string> args)
        {
            if (ConnectionSide != ConnectionSideEnum.Client)
            {
                logger?.LogError($"Can't call {nameof(SendData)} due to it isn't server side");
                throw new InvalidOperationException();
            }

            using BinaryReader reader = new(stream, Encoding.UTF8, true);
            using BinaryWriter writer = new(stream, Encoding.UTF8, true);
            string request = requestTemplate;
            foreach (var arg in args)
                request = request.Replace(arg.Key, arg.Value);
            await stream.WriteAsync(Encoding.Default.GetBytes($"POST {request}"));
            await SendDataInner(stream, data);
        }

        protected abstract Task SendDataInner(Stream stream, T data);
    }
}
