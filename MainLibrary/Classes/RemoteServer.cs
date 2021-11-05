using MainLibrary.Interfaces;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    public class RemoteServer : IRemoteServer
    {
        private Stream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        public void ConnectTo(IPEndPoint endPoint)
        {
            TcpClient client = new();
            client.Connect(endPoint);
            stream = client.GetStream();
            reader = new(stream);
            writer = new(stream);
        }

        public async Task<IWork> GetWorkAsync(string workName)
        {
            writer.Write(workName);

            DirectoryInfo temp = new(Path.GetTempPath() + workName);
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string name = reader.ReadString();

                int size = reader.ReadInt32();

                FileInfo file = new(temp.FullName + name);
                file.Directory.Create();
                byte[] data = reader.ReadBytes(size);
                using Stream fileStream = file.Create();
                await fileStream.WriteAsync(data);
            }

            return new Work(workName, temp);
        }
    }
}
