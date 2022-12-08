using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Name3.Models
{
    class CollectReader : IDisposable
    {
        private Stream FileStream { get; }

        public CollectReader(string name)
        {
            this.FileStream = new MemoryStream(File.ReadAllBytes($"{name}.collect"));
            GC.Collect();
        }

        private byte ReadUnsignedByte()
        {
            var buffer = new byte[1];
            FileStream.Read(buffer, 0, 1);
            return buffer[0];
        }

        private bool ReadBoolean()
        {
            return ReadUnsignedByte() == 0x01;
        }

        private int ReadInt()
        {
            byte[] buffer = new byte[4];
            FileStream.Read(buffer, 0, 4);
            return BinaryPrimitives.ReadInt32BigEndian(buffer);
        }

        private string ReadString()
        {
            var length = ReadInt();
            byte[] buffer = new byte[length];
            FileStream.Read(buffer, 0, length);

            return Encoding.UTF8.GetString(buffer);
        }

        public Collect ReadCollect()
        {
            var name = ReadString();
            var auther = ReadString();

            var eleSize = ReadInt();
            List<string> elements = new List<string>();
            for(int i = 0; i <= eleSize; i++)
                elements.Add(ReadString());

            return new Collect()
            {
                Auther = auther,
                Name = name,
                Elements = elements
            };
        }

        public void Dispose()
        {
            FileStream.Close();
            FileStream.Dispose();
            GC.SuppressFinalize(FileStream);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        ~CollectReader()
        {
            Dispose();
        }
    }
}
