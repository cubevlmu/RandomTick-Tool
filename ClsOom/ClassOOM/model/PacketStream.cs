using ClsOom.ClassOOM.model.Tools;

namespace ClsOom.ClassOOM.model;

public partial class PacketStream : IDisposable
{
    public PacketStream()
    {
        BasicStream = new MemoryStream();
        GC.KeepAlive(BasicStream);
    }

    public PacketStream(byte[] bytes)
    {
        BasicStream = new MemoryStream(bytes);
        GC.KeepAlive(BasicStream);
    }

    public PacketStream(Stream stream)
    {
        BasicStream = stream;
        GC.KeepAlive(BasicStream);
    }

    public byte[] GetBytes()
    {
        BasicStream.Position = 0;
        var buffer = new byte[BasicStream.Length];
        for (var totalBytesCopied = 0; totalBytesCopied < BasicStream.Length;)
            totalBytesCopied += BasicStream.Read(buffer, totalBytesCopied, Convert.ToInt32(BasicStream.Length) - totalBytesCopied);
        return IsCompressiomEnable ? Gzip.CompressBytes(buffer) : buffer;
    }

    public void Dispose()
    {
        IsDead = true;
        BasicStream.Close();
        BasicStream.Dispose();
    }

    public void Clear()
    {

    }

    ~PacketStream()
    {
        if(!IsDead) Dispose();
        GC.Collect();
    }

    private bool IsDead = false;
    public void SetCompression(bool mode) => IsCompressiomEnable = mode;
    private bool IsCompressiomEnable = false;
    private readonly Stream BasicStream;
}