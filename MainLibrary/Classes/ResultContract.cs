using MainLibrary.Abstractions;
using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    public class ResultContract : PostContract<byte[]>
    {
        public ResultContract(Action<byte[], Dictionary<string, string>> onReceive) : base("RESULT name", new(@"POST RESULT (?<name>\w+)"), onReceive)
        {
            AsServer();
        }

        public ResultContract() : base("RESULT name", new(@"POST RESULT (?<name>\w+)"), null)
        {
            AsClient();
        }

        protected async override Task<byte[]> ReceiveData(BinaryReader reader)
        {
            byte[] result = new byte[reader.ReadInt32()];
            await reader.BaseStream.ReadAsync(result);
            return result;
        }

        protected async override Task SendData(BinaryWriter writer, byte[] data)
        {
            writer.Write(data.Length);
            await writer.BaseStream.WriteAsync(data);
        }
    }
}
