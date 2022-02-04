using MainLibrary.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    public class ArgsContract : GetContract<byte[]>
    {
        public ArgsContract() : base("ARGS name", new(@"ARGS (?<name>\w+)"), null)
        {
            AsClient();
        }
        public ArgsContract(Func<Dictionary<string, string>, byte[]> onSend) : base("ARGS name", new(@"ARGS (?<name>\w+)"), onSend)
        {
            AsServer();
        }


        protected override async Task<byte[]> ReceiveDataInner(Stream stream)
        {
            int readed = 0;
            byte[] input = new byte[4];
            while (readed < input.Length)
                readed += await stream.ReadAsync(input, readed, input.Length - readed);
            int count = BitConverter.ToInt32(input);
            input = new byte[count];
            readed = 0;
            while (readed < input.Length)
                readed += await stream.ReadAsync(input, readed, input.Length - readed);
            return input;
        }

        protected override async Task SendDataInner(Stream stream, byte[] data)
        {
            await stream.WriteAsync(BitConverter.GetBytes(data.Length));
            await stream.WriteAsync(data);
        }
    }
}
