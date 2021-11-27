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
    public class WorkContract : GetContract<IWork>
    {
        private DirectoryInfo worksDirectory;

        public WorkContract(DirectoryInfo worksDirectory) : base("name",
                                                                 new(@"GET (\w+)"),
                                                                 new string[] { "name" },
                                                                 null)
        {
            AsClient();
            this.worksDirectory = worksDirectory;
        }

        public WorkContract(Func<string[], IWork> onSend) : base("name",
                                                                 new(@"GET (\w+)"),
                                                                 new string[] { "name" },
                                                                 onSend)
        {
            AsServer();
        }

        //public async Task<IWork> RequestData(Stream stream, string[] args)
        //{
        //    using BinaryWriter writer = new(stream, Encoding.UTF8, true);
        //    using BinaryReader reader = new(stream, Encoding.UTF8, true);

        //    string workName = args[0];
        //    writer.Write(requestTemplate.Replace("name", workName));

        //    DirectoryInfo directory = worksDirectory.CreateSubdirectory(workName);
        //    int count = reader.ReadInt32();
        //    for (int i = 0; i < count; i++)
        //    {
        //        string name = reader.ReadString();

        //        int size = reader.ReadInt32();

        //        FileInfo file = new(directory.FullName + name);
        //        file.Directory.Create();
        //        byte[] data = reader.ReadBytes(size);
        //        using Stream fileStream = file.Create();
        //        await fileStream.WriteAsync(data);
        //    }

        //    return new Work(workName, directory);
        //}


        protected async override Task<IWork> RequestData(BinaryReader reader)
        {
            string workName = Args[0];
            DirectoryInfo directory = worksDirectory.CreateSubdirectory(workName);
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

        //public async Task SendData(Stream stream, IWork work)
        //{
        //    using BinaryWriter writer = new(stream, Encoding.UTF8, true);
        //    List<FileInfo> files = work.AssemblyDirectory.EnumerateFiles("*", SearchOption.AllDirectories).ToList();
        //    writer.Write(files.Count);
        //    foreach (var file in files)
        //    {
        //        writer.Write(file.FullName[work.AssemblyDirectory.FullName.Length..]);

        //        writer.Write((int)file.Length);

        //        //byte[] data = File.ReadAllBytes(file.FullName);
        //        //writer.Write(data);
        //        using Stream fileStream = file.OpenRead();
        //        await fileStream.CopyToAsync(stream);
        //    }
        //    return;
        //}

        protected async override Task SendData(BinaryWriter writer, IWork work)
        {
            List<FileInfo> files = work.AssemblyDirectory.EnumerateFiles("*", SearchOption.AllDirectories).ToList();
            writer.Write(files.Count);
            foreach (var file in files)
            {
                writer.Write(file.FullName[work.AssemblyDirectory.FullName.Length..]);

                writer.Write((int)file.Length);

                //byte[] data = File.ReadAllBytes(file.FullName);
                //writer.Write(data);
                using Stream fileStream = file.OpenRead();
                await fileStream.CopyToAsync(writer.BaseStream);
            }
            return;
        }
    }
}
