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

        public WorkContract(DirectoryInfo worksDirectory) : base("WORK name", new(@"WORK (?<name>\w+)"), null)
        {
            AsClient();
            this.worksDirectory = worksDirectory;
        }

        public WorkContract(Func<Dictionary<string, string>, IWork> onSend) : base("WORK name", new(@"WORK (?<name>\w+)"), onSend)
        {
            AsServer();
        }


        protected async override Task<IWork> ReceiveData(BinaryReader reader)
        {
            string workName = Args["name"];
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

        protected async override Task SendData(BinaryWriter writer, IWork work)
        {
            List<FileInfo> files = work.AssemblyDirectory.EnumerateFiles("*", SearchOption.AllDirectories).ToList();
            writer.Write(files.Count);
            foreach (var file in files)
            {
                writer.Write(file.FullName[work.AssemblyDirectory.FullName.Length..]);

                writer.Write((int)file.Length);

                using Stream fileStream = file.OpenRead();
                await fileStream.CopyToAsync(writer.BaseStream);
            }
            return;
        }
    }
}
