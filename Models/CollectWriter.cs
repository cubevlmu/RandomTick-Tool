using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Name3.Models
{
    class CollectWriter : IDisposable
    {
        private Stream FileStream { get; }

        private Collect Collect { get; }
        public CollectWriter(Collect collect)
        {
            this.Collect = collect;
            FileStream = new MemoryStream();
        }

        private void WriteBoolean(bool value)
        {
            FileStream.WriteByte((byte)(value ? 0x01 : 0x00));
        }

        private void WriteInt(int value)
        {
            var span = new byte[4];
            BinaryPrimitives.WriteInt32BigEndian(span, value);
            FileStream.Write(span, 0, 4);
        }

        private void WriteString(string value)
        {
            var buffer = Encoding.UTF8.GetBytes(value);
            WriteInt(buffer.Length);
            FileStream.Write(buffer, 0, buffer.Length);
        }

        public bool WriteTo(string name)
        {
            try
            {
                WriteString(Collect.Name);
                WriteString(Collect.Auther);
                WriteInt(Collect.Elements.Count);
                foreach (var element in Collect.Elements)
                    WriteString(element);
                File.WriteAllBytes($"{name}.collect", ParseToBytes());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private byte[] ParseToBytes()
        {
            FileStream.Position = 0;
            var buffer = new byte[FileStream.Length];
            for (var totalBytesCopied = 0; totalBytesCopied < FileStream.Length;)
                totalBytesCopied += FileStream.Read(buffer, totalBytesCopied, Convert.ToInt32(FileStream.Length) - totalBytesCopied);
            return buffer;
        }

        public void Dispose()
        {
            FileStream.Close();
            FileStream.Dispose();
            GC.SuppressFinalize(FileStream);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        ~CollectWriter()
        {
            Dispose();
        }
    }
}
