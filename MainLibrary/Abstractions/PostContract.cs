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
    public abstract class PostContract<T> : TransferContract
        where T : class
    {
        private readonly Action<T,string[]> onReceive;

        protected PostContract(string requestTemplate, Regex requestRegex, string[] associations, Action<T,string[]> onReceive) : base(requestTemplate, requestRegex, associations)
        {
            this.onReceive = onReceive;
        }

        public async Task ReceiveData(Stream stream)
        {
            if (ConnectionSide != ConnectionSideEnum.Server) throw new InvalidOperationException();

            T data = await ReceiveData(new BinaryReader(stream, Encoding.UTF8, true));
            onReceive?.Invoke(data,Args);
        }

        protected abstract Task<T> ReceiveData(BinaryReader reader);

        public Task SendData(Stream stream,T data, string[] args)
        {
            if (ConnectionSide != ConnectionSideEnum.Client) throw new InvalidOperationException();

            using BinaryReader reader = new(stream, Encoding.UTF8, true);
            using BinaryWriter writer = new(stream, Encoding.UTF8, true);
            string request = requestTemplate;
            for (int i = 0; i < args.Length; i++)
                request = request.Replace(associations[i], args[i]);
            writer.Write($"POST {request}");
            return SendData(writer, data);
        }

        protected abstract Task SendData(BinaryWriter writer, T data);
    }
}
