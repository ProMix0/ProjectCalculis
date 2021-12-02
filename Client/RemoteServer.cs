using MainLibrary.Classes;
using MainLibrary.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Client
{
    public class RemoteServer : IRemoteServer
    {
        private Stream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private TcpClient client;
        private DirectoryInfo worksDirectory;

        private MetadataListContract metadataContract;
        private WorkContract workContract;
        private ResultContract resultContract;
        private ArgsContract argsContract;

        public RemoteServer(IOptions<PathOptions> options)
        {
            worksDirectory = new(options.Value.WorksDirectory);

            workContract = new(worksDirectory);
            metadataContract = new();
            resultContract = new();
            argsContract = new();
        }

        public void ConnectTo(IPEndPoint endPoint)
        {
            client = new();
            client.Connect(endPoint);
            stream = client.GetStream();

            //SslStream ssl = new(stream);
            //ssl.AuthenticateAsClient(new SslClientAuthenticationOptions());
            //stream = ssl;

            //reader = new(stream);
            //writer = new(stream);

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

        public Task<IWork> DownloadWork(IWorkMetadata workMetadata)
        {
            return workContract.RequestData(stream,new string[] { workMetadata.Name });
        }

        public Task<byte[]> GetArgs(IWorkMetadata metadata)
        {
            return argsContract.RequestData(stream,new string[] { metadata.Name });
        }

        public Task<List<IWorkMetadata>> GetWorksList()
        {
            return metadataContract.RequestData(stream, null);
        }

        public Task SendWorkResult(IWorkMetadata metadata, byte[] result)
        {
            return resultContract.SendData(stream, result, new string[] { metadata.Name });
        }
    }
}
