using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    public class RemoteServer : IRemoteServer
    {
        private Stream stream;

        public void ConnectTo(string ip, int port)
        {
            TcpClient client = new(ip, port);
            stream = client.GetStream();
        } 

        public async Task<IWork> GetWorkAsync(string workName)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(workName);
            await stream.WriteAsync(BitConverter.GetBytes(buffer.Length));
            await stream.WriteAsync(buffer);

            buffer = new byte[4];
            await stream.ReadAsync(buffer);
            buffer = new byte[BitConverter.ToInt32(buffer)];
            await stream.ReadAsync(buffer);

            return JsonSerializer.Deserialize<IWork>(buffer);
        }
    }
}
