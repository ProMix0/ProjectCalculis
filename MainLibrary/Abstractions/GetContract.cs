using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainLibrary.Abstractions
{
    public abstract class GetContract<T> : TransferContract
        where T:class
    {
        private readonly Func<string[], T> onSend;

        protected GetContract(string requestTemplate, Regex requestRegex, string[] associations, Func<string[], T> onSend) : base(requestTemplate, requestRegex, associations)
        {
            this.onSend = onSend;
        }

        public Task<T> RequestData(Stream stream, string[] args)
        {
            if (ConnectionSide != ConnectionSideEnum.Client) throw new InvalidOperationException();
            args ??= new string[0];
            Args = args;

            BinaryReader reader = new(stream, Encoding.UTF8, true);
            using BinaryWriter writer = new(stream, Encoding.UTF8, true);
            string request = requestTemplate;
            for (int i = 0; i < args.Length; i++)
                request = request.Replace(associations[i], args[i]);
            writer.Write($"GET {request}");
            return RequestData(reader).ContinueWith(task=>
            {
                reader.Dispose();
                return task.Result;
            });
        }

        protected abstract Task<T> RequestData(BinaryReader reader);

        public Task SendData(Stream stream)
        {
            if (ConnectionSide != ConnectionSideEnum.Server) throw new InvalidOperationException();

            using BinaryWriter writer = new(stream, Encoding.UTF8, true);
            return SendData(writer, onSend?.Invoke(Args));
        }

        public override sealed GetContract<T> AsServer()
        {
            return (GetContract<T>)base.AsServer();
        }
        public override sealed GetContract<T> AsClient()
        {
            return (GetContract<T>)base.AsClient();
        }

        protected abstract Task SendData(BinaryWriter writer, T data);
    }
}
