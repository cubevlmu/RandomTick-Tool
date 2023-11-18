using System.Text;
using ClsOom.ClassOOM.model.Tools;

namespace ClsOom.ClassOOM.model;

public partial class PacketPackage
{
    private int PacketId { get; set; }
    public void ReadPacket()
    {
        if (BufferRead == null) return;
        Stream = new PacketStream(BufferRead);
        GC.Collect();
    }

    public void ReadPacket(Stream stream)
    {
        Stream = new PacketStream(stream);
    }

    public int GetPacketId() => PacketId;

    public PacketStream? DecodeStream()
    {
        if (Stream == null) return null;
        PacketId = Stream.ReadInt32();
        var length = Stream.ReadInt32();
        var content = Gzip.Decompress(Stream.ReadBytes(length)) ?? throw new ArgumentNullException($"Decompress Stream.ReadBytes {nameof(length)}");

        var passLength = Stream.ReadInt32();
        var passBuffer = Stream.ReadBytes(passLength);

        Helper.encryptKey = Encoding.UTF8.GetString(passBuffer);
        var buffer = Helper.DecryptDll(content);

        return new PacketStream(buffer);
    }
}