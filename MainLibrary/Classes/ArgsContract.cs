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
        public ArgsContract() : base("ARGS name", new(@"ARGS (?<name>\w+)"),  null)
        {
            AsClient();
        }
        public ArgsContract(Func<Dictionary<string, string>, byte[]> onSend) : base("ARGS name", new(@"ARGS (?<name>\w+)"),  onSend)
        {
            AsServer();
        }


        protected override Task<byte[]> ReceiveData(BinaryReader reader)
        {
            return Task.FromResult(reader.ReadBytes(reader.ReadInt32()));
        }

        protected override Task SendData(BinaryWriter writer, byte[] data)
        {
            writer.Write(data.Length);
            writer.Write(data);
            return Task.CompletedTask;
        }
    }
}
