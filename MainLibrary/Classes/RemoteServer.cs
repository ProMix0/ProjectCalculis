using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    public class RemoteServer : IRemoteServer
    {
        private Stream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private TcpClient client;

        public void ConnectTo(IPEndPoint endPoint)
        {
            client = new();
            client.Connect(endPoint);
            stream = client.GetStream();

            //SslStream ssl = new(stream);
            //ssl.AuthenticateAsClient(new SslClientAuthenticationOptions());
            //stream = ssl;

            reader = new(stream);
            writer = new(stream);

            //SymmetricAlgorithm cipher = Rijndael.Create();
            //cipher.Mode = CipherMode.CTS;
            //cipher.Key = new byte[] { 23, 165, 58, 170, 51, 13, 69, 79, 6, 198, 166, 113, 183, 72, 235, 83, 82, 185, 45, 226, 243, 251, 169, 120, 49, 149, 31, 42, 152, 77, 245, 120 };
            //cipher.IV = new byte[] { 196, 55, 81, 107, 226, 189, 74, 184, 9, 69, 91, 21, 103, 45, 208, 210 };

            //reader = new(new CryptoStream(stream, cipher.CreateDecryptor(), CryptoStreamMode.Read));
            //writer = new(new CryptoStream(stream, cipher.CreateEncryptor(), CryptoStreamMode.Write));

            //reader = new CryptoBinaryReader(stream, rijndael.CreateDecryptor());
            //writer = new CryptoBinaryWriter(stream, rijndael.CreateEncryptor());
        }

        public void Dispose()
        {
            reader.Dispose();
            writer.Dispose();
            stream.Dispose();
            client.Dispose();
        }

        public async Task<IWork> DownloadWorkAsync(IWorkMetadata workMetadata, DirectoryInfo worksDirectory)
        {
            string workName = workMetadata.Name;
            writer.Write(workName);

            DirectoryInfo directory = new(worksDirectory.FullName + workName);
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string name = reader.ReadString();

                int size = reader.ReadInt32();

                FileInfo file = new(directory.FullName + name);
                file.Directory.Create();
                byte[] data = reader.ReadBytes(size);
                using Stream fileStream = file.Create();
                await fileStream.WriteAsync(data);
            }

            return new Work(workName, directory);
        }

        public Task<List<IWorkMetadata>> GetWorksListAsync()
        {
            writer.Write("GET works");

            int count = reader.ReadInt32();
            List<IWorkMetadata> result = new();
            for (int i = 0; i < count; i++)
            {
                string name = reader.ReadString();
                byte[] hash = reader.ReadBytes(reader.ReadInt32());
                result.Add(new WorkMetadata(name, hash));
            }
            return Task.FromResult(result);
        }
    }
}
