using ClsOom.ClassOOM.model.Tools;

namespace ClsOom.ClassOOM.model;

public sealed partial class PacketStream : Stream, IDisposable
{
    public override bool CanRead => _basicStream.CanRead;
    public override bool CanSeek => _basicStream.CanSeek;
    public override bool CanWrite => _basicStream.CanWrite;
    public override long Length => _basicStream.Length;
    public override void Flush() => _basicStream.Flush();
    public override int Read(byte[] buffer, int offset, int count)
        => _basicStream.Read(buffer, offset, count);
    
    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        try
        {
            var read = await _basicStream.ReadAsync(buffer.AsMemory(offset, count), cancellationToken);

            return read;
        }
        catch (Exception)
        {
            return 0;
        }//TODO better handling of this
    }

    public async Task<int> ReadAsync(byte[] buffer, CancellationToken cancellationToken = default)
    {
        try
        {
            var read = await _basicStream.ReadAsync(buffer, cancellationToken);

            return read;
        }
        catch (Exception)
        {
            return 0;
        }//TODO better handling of this
    }
    
    public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        await _basicStream.WriteAsync(buffer.AsMemory(offset, count), cancellationToken);
    }

    public async Task WriteAsync(byte[] buffer, CancellationToken cancellationToken = default)
    {
        await _basicStream.WriteAsync(buffer, cancellationToken);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _basicStream.Write(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin) => _basicStream.Seek(offset, origin);

    public override void SetLength(long value) => _basicStream.SetLength(value);

    public override long Position {
        get => _basicStream.Position;
        set => _basicStream.Position = value;
    }
    
    public PacketStream()
    {
        _basicStream = new MemoryStream();
    }

    public PacketStream(byte[] bytes)
    {
        _basicStream = new MemoryStream(bytes);
    }

    public PacketStream(Stream stream)
    {
        _basicStream = stream;
    }

    public byte[] GetBytes()
    {
        _basicStream.Position = 0;
        var buffer = new byte[_basicStream.Length];
        for (var totalBytesCopied = 0; totalBytesCopied < _basicStream.Length;)
            totalBytesCopied += _basicStream.Read(buffer, totalBytesCopied, Convert.ToInt32(_basicStream.Length) - totalBytesCopied);
        return _isCompressionEnable ? Gzip.CompressBytes(buffer) : buffer;
    }

    public new void Dispose()
    {
        _isDead = true;
        _basicStream.Close();
        _basicStream.Dispose();
    }
    
    ~PacketStream()
    {
        if(!_isDead) Dispose();
        GC.Collect();
    }


    private bool _isDead;
    public void SetCompression(bool mode) => _isCompressionEnable = mode;
    private bool _isCompressionEnable;
    private readonly Stream _basicStream;
}