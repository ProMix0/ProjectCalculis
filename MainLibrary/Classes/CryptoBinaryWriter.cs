using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    class CryptoBinaryWriter : BinaryWriter
    {
        private ICryptoTransform transform;

        public CryptoBinaryWriter(Stream output, ICryptoTransform transform) : base(output, Encoding.UTF8, true)
        {
            this.transform = transform;
        }

        public override void Write(int value)
        {
            using (CryptoStream cryptoStream = new(OutStream, transform, CryptoStreamMode.Write, true))
            using (BinaryWriter writer = new(cryptoStream, Encoding.UTF8, true))
                writer.Write(value);
        }

        public override void Write(string value)
        {
            using (CryptoStream cryptoStream = new(OutStream, transform, CryptoStreamMode.Write, true))
            using (BinaryWriter writer = new(cryptoStream, Encoding.UTF8, true))
                writer.Write(value);
        }

        public override void Write(byte[] value)
        {
            using (CryptoStream cryptoStream = new(OutStream, transform, CryptoStreamMode.Write, true))
            using (BinaryWriter writer = new(cryptoStream, Encoding.UTF8, true))
                writer.Write(value);
        }
    }
}
