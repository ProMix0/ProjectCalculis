﻿using MainLibrary.Interfaces;
using MessagePack;
using MessagePack.Resolvers;
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

namespace MainLibrary.Classes
{
    public class RemoteClient : IRemoteClient
    {
        public Func<string, IWork> GetWork { set; private get; }
        public Func<List<IWorkMetadata>> GetWorksList { set; private get; }
        public Action<byte[],string> ReceiveResult { set; private get; }
        private Task listenTask;
        private CancellationTokenSource token;
        private readonly Stream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private TcpClient client;

        public RemoteClient(TcpClient client)
        {
            this.client = client;
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

        private MetadataListContract metadataContract;
        private WorkContract workContract;
        private ResultContract resultContract;

        private void Listen()
        {
            metadataContract = new(array => GetWorksList());
            workContract = new(array => GetWork(array[0]));
            resultContract = new((result, args) => ReceiveResult(result,args[0]));
            try
            {
                while (true)
                {
                    //while (true)
                    //    foreach(var @byte in reader.ReadBytes(16))
                    //    Console.WriteLine(@byte);

                    string request = reader.ReadString();
                    switch (request)
                    {
                        case string _ when request.StartsWith("GET"):
                            if (metadataContract.IsRequest(request))
                            {
                                metadataContract.SendData(stream).Wait();
                                break;
                            }
                            if (workContract.IsRequest(request))
                            {
                                workContract.SendData(stream).Wait();
                                break;
                            }
                            break;
                        case string _ when request.StartsWith("POST"):
                            if(resultContract.IsRequest(request))
                            {
                                resultContract.ReceiveData(stream).Wait();
                                break;
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            token.Cancel();
            reader.Dispose();
            writer.Dispose();
            stream.Dispose();
            client.Dispose();
        }
    }
}
