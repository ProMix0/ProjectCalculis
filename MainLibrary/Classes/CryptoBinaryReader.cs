using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MainLibrary.Classes
{
    internal class CryptoBinaryReader : BinaryReader
    {
        private ICryptoTransform transform;

        public CryptoBinaryReader(Stream input, ICryptoTransform transform):base(input,Encoding.UTF8,true)
        {
            this.transform = transform;
        }

        public override int ReadInt32()
        {
            using CryptoStream cryptoStream = new(BaseStream, transform, CryptoStreamMode.Read,true);
            using BinaryReader reader = new(cryptoStream, Encoding.UTF8, true);
            return reader.ReadInt32();
        }

        public override string ReadString()
        {
            using CryptoStream cryptoStream = new(BaseStream, transform, CryptoStreamMode.Read, true);
            using BinaryReader reader = new(cryptoStream, Encoding.UTF8, true);
            return reader.ReadString();
        }

        public override byte[] ReadBytes(int count)
        {
            using CryptoStream cryptoStream = new(BaseStream, transform, CryptoStreamMode.Read, true);
            using BinaryReader reader = new(cryptoStream, Encoding.UTF8, true);
            return reader.ReadBytes(count);
        }

        //public override byte ReadByte()
        //{
        //    using CryptoStream cryptoStream = new(BaseStream, transform, CryptoStreamMode.Read, true);
        //    using BinaryReader reader = new(cryptoStream, Encoding.UTF8, true);
        //    return reader.ReadByte();
        //}
    }
}