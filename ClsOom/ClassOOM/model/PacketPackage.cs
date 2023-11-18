namespace ClsOom.ClassOOM.model;

public partial class PacketPackage : IDisposable
{
    private PacketStream? Stream { get; set; }
    private PacketStream DataStream { get; } = new PacketStream();
    private Stream? BufferRead { get; set; }

    public PacketPackage(PacketStream stream)
    {
        Stream = stream;
        GC.Collect();
    }

    public PacketPackage(byte[] buffer)
    {
        BufferRead = new MemoryStream(buffer);//Gzip.Decompress(buffer);
        GC.Collect();
    }

    public PacketPackage(Stream data)
    {
        BufferRead = data;
        GC.Collect();
    }

    public PacketPackage()
    {
        //BufferRead = buffer;//Gzip.Decompress(buffer);
        GC.Collect();
    }

    public async void Dispose()
    {
        Stream?.Dispose();
        DataStream.Dispose();
        if(BufferRead != null) await BufferRead.DisposeAsync();
        GC.Collect();
        GC.WaitForFullGCComplete();
    }

    ~PacketPackage()
    {
        Dispose();
    }
}