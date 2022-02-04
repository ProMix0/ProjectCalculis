using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MainLibrary.Abstractions
{
    public abstract class GetContract<T> : TransferContract, IGetContract
        where T : class
    {
        private readonly Func<Dictionary<string, string>, T> onSend;

        protected GetContract(string requestTemplate, Regex requestRegex, Func<Dictionary<string, string>, T> onSend) : base(requestTemplate, requestRegex)
        {
            this.onSend = onSend;
        }

        public async Task<T> RequestData(Stream stream, Dictionary<string, string> args)
        {
            if (ConnectionSide != ConnectionSideEnum.Client)
            {
                logger?.LogError($"Can't call {nameof(RequestData)} due to it isn't client side");
                throw new InvalidOperationException();
            }
            args ??= new();
            Args = args;

            string request = requestTemplate;
            foreach (var arg in args)
                request = request.Replace(arg.Key, arg.Value);
            await stream.WriteAsync(Encoding.Default.GetBytes( $"GET {request}"));
            return await ReceiveDataInner(stream);
        }

        protected abstract Task<T> ReceiveDataInner(Stream stream);

        public Task SendData(Stream stream)
        {
            if (ConnectionSide != ConnectionSideEnum.Server)
            {
                logger?.LogError($"Can't call {nameof(SendData)} due to it isn't server side");
                throw new InvalidOperationException();
            }

            return SendDataInner(stream, onSend?.Invoke(Args));
        }

        protected abstract Task SendDataInner(Stream stream, T data);
    }
}
