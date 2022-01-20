using MainLibrary.Interfaces;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server
{


    public class RemoteClient : IRemoteClient
    {
        private Task listenTask;
        private CancellationTokenSource token;
        private readonly Stream stream;
        private BinaryReader reader;
        //private BinaryWriter writer;
        private TcpClient client;
        private readonly Logger<RemoteClient> logger;
        private IContractsCollection contracts;

        public RemoteClient(TcpClient client, Logger<RemoteClient> logger)
        {
            this.client = client;
            this.logger = logger;
            stream = client.GetStream();

            //https://habr.com/ru/post/497160/
            //SslStream ssl = new(stream);
            //ssl.AuthenticateAsServer(new SslServerAuthenticationOptions());
            //stream = ssl;

            reader = new(stream);
            //writer = new(stream);

            //SymmetricAlgorithm cipher = Rijndael.Create();
            //cipher.Mode = CipherMode.CTS;
            //cipher.Key = new byte[] { 23, 165, 58, 170, 51, 13, 69, 79, 6, 198, 166, 113, 183, 72, 235, 83, 82, 185, 45, 226, 243, 251, 169, 120, 49, 149, 31, 42, 152, 77, 245, 120 };
            //cipher.IV = new byte[] { 196, 55, 81, 107, 226, 189, 74, 184, 9, 69, 91, 21, 103, 45, 208, 210 };

            //reader = new (new CryptoStream( stream, cipher.CreateDecryptor(),CryptoStreamMode.Read));
            //writer = new (new CryptoStream(stream, cipher.CreateEncryptor(),CryptoStreamMode.Write));

            //reader = new CryptoBinaryReader(stream, rijndael.CreateDecryptor());
            //writer = new CryptoBinaryWriter(stream, rijndael.CreateEncryptor());
            token = new CancellationTokenSource();
            listenTask = Task.Run(Listen, token.Token);
        }

        private void Listen()
        {
            try
            {
                while (true)
                {
                    string request = reader.ReadString();
                    switch (request)
                    {
                        case string when request.StartsWith("GET"):
                            foreach (var get in contracts.GetContracts)
                                if (get.IsRequest(request))
                                {
                                    get.SendData(stream).Wait();
                                    break;
                                }
                            break;
                        case string when request.StartsWith("POST"):
                            IReadOnlyList<IPostContract> postContracts = contracts.PostContracts;
                            int count = postContracts.Count;
                            foreach (var post in postContracts)
                                if (post.IsRequest(request))
                                {
                                    post.ReceiveData(stream).Wait();
                                    break;
                                }
                                else count--;
                            if (count == 0) logger.LogWarning($"Contracts can't handle request: {request}");
                            break;
                        default:
                            logger.LogWarning($"Undefined request: {request}");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                logger.LogError($"Exception in Listen(): {e.Message}");
            }
        }

        public void Dispose()
        {
            token.Cancel();
            reader.Dispose();
            //writer.Dispose();
            stream.Dispose();
            client.Dispose();
        }

        public void SetContracts(IContractsCollection contracts)
        {
            this.contracts = contracts;
        }
    }
}
