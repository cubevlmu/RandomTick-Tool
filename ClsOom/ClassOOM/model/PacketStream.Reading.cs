using System.Buffers.Binary;
using System.Text;
using ClsOom.ClassOOM.model.Tools;
using ClsOom.ClassOOM.utils.collection;

namespace ClsOom.ClassOOM.model;

public sealed partial class PacketStream
{
    public async Task<long> ReadVarLongAsync()
    {
        int numRead = 0;
        long result = 0;
        byte read;
        do
        {
            read = await ReadUnsignedByteAsync();
            int value = (read & 0b01111111);
            result |= (long)value << (7 * numRead);

            numRead++;
            if (numRead > 10)
            {
                throw new InvalidOperationException("VarLong is too big");
            }
        } while ((read & 0b10000000) != 0);

        return result;
    }
    
    public async Task<byte[]> ReadUInt8ArrayAsync(int length = 0)
    {
        if (length == 0)
            length = await ReadVarIntAsync();

        var result = new byte[length];
        if (length == 0)
            return result;

        int n = length;
        while (true)
        {
            n -= await ReadAsync(result, length - n, n);
            if (n == 0)
                break;
        }
        return result;
    }

    public async Task<byte> ReadUInt8Async()
    {
        int value = await ReadByteAsync();
        if (value == -1)
            throw new EndOfStreamException();
        return (byte)value;
    }
    
    public async Task<string> ReadStringAsync(int maxLength = 32767)
    {
        var length = await ReadVarIntAsync();
        using var buffer = new RentedArray<byte>(length);
        if (BitConverter.IsLittleEndian)
        {
            buffer.Span.Reverse();
        }
        await ReadExactlyAsync(buffer);

        var value = Encoding.UTF8.GetString(buffer);
        if (maxLength > 0 && value.Length > maxLength)
        {
            throw new ArgumentException($"string ({value.Length}) exceeded maximum length ({maxLength})", nameof(maxLength));
        }
        return value;
    }
    
    public async Task<double> ReadDoubleAsync()
    {
        using var buffer = new RentedArray<byte>(sizeof(double));
        await ReadExactlyAsync(buffer);
        return BinaryPrimitives.ReadDoubleBigEndian(buffer);
    }

    public async Task<float> ReadFloatAsync()
    {
        using var buffer = new RentedArray<byte>(sizeof(float));
        await ReadExactlyAsync(buffer);
        return BinaryPrimitives.ReadSingleBigEndian(buffer);
    }
    
    public async Task<ulong> ReadUnsignedLongAsync()
    {
        using var buffer = new RentedArray<byte>(sizeof(ulong));
        await ReadExactlyAsync(buffer);
        return BinaryPrimitives.ReadUInt64BigEndian(buffer);
    }
    
    public async Task<long> ReadLongAsync()
    {
        using var buffer = new RentedArray<byte>(sizeof(long));
        await ReadExactlyAsync(buffer);
        return BinaryPrimitives.ReadInt64BigEndian(buffer);
    }
    
    public async Task<int> ReadIntAsync()
    {
        using var buffer = new RentedArray<byte>(sizeof(int));
        await ReadExactlyAsync(buffer);
        return BinaryPrimitives.ReadInt32BigEndian(buffer);
    }
    
    public async Task<short> ReadShortAsync()
    {
        using var buffer = new RentedArray<byte>(sizeof(short));
        await ReadExactlyAsync(buffer);
        return BinaryPrimitives.ReadInt16BigEndian(buffer);
    }
    
    public async Task<bool> ReadBooleanAsync()
    {
        var value = (int)await ReadByteAsync();
        return value switch
        {
            0x00 => false,
            0x01 => true,
            _ => throw new ArgumentOutOfRangeException("Byte returned by stream is out of range (0x00 or 0x01)",
                nameof(_basicStream))
        };
    }
    
    public async Task<byte> ReadUnsignedByteAsync()
    {
        var buffer = new byte[1];
        await ReadAsync(buffer);
        return buffer[0];
    }
    
    public async Task<sbyte> ReadByteAsync() => (sbyte)await ReadUnsignedByteAsync();
    
    public async Task<int> ReadVarIntAsync()
    {
        var numRead = 0;
        var result = 0;
        byte read;
        do
        {
            read = await ReadUnsignedByteAsync();
            var value = read & 0b01111111;
            result |= value << (7 * numRead);

            numRead++;
            if (numRead > 5)
            {
                throw new InvalidOperationException("VarInt is too big");
            }
        } while ((read & 0b10000000) != 0);

        return result;
    }
    
    public new byte ReadByte()
    {
        var buffer = new byte[1];
        _ = _basicStream.Read(buffer, 0, buffer.Length);
        return buffer[0];
    }
    public void ReadByte(ref byte data)
    {
        var buffer = new byte[1];
        _ = _basicStream.Read(buffer, 0, 1);
        data = buffer[0];
    }

    public byte[] ReadBytes(int length)
    {
        byte[] buffer = new byte[length];
        _ = _basicStream.Read(buffer, 0, buffer.Length);
        return buffer.ToArray();
    }
    public void ReadBytes(ref byte[] data)
        => _basicStream.Read(data, 0, data.Length);

    public bool ReadBoolean()
    {
        byte[] buffer = new byte[1];
        _ = _basicStream.Read(buffer, 0, buffer.Length);
        return buffer[0] == 1;
    }
    public void ReadBoolean(ref bool data)
    {
        byte[] buffer = new byte[1];
        _ = _basicStream.Read(buffer, 0, buffer.Length);
        data = buffer[0] == 1;
    }

    public int ReadInt32()
    {
        byte[] buffer = new byte[5];
        _ = _basicStream.Read(buffer, 0, buffer.Length);
        return SignInt.Bit5ToInt32(buffer.ToArray());
    }
    public void ReadInt32(ref int data)
    {
        byte[] buffer = new byte[5];
        _basicStream.Read(buffer, 0, buffer.Length);
        data = SignInt.Bit5ToInt32(buffer.ToArray());
    }

    public long ReadInt64()
    {
        byte[] buffer = new byte[10];
        _ = _basicStream.Read(buffer, 0, buffer.Length);
        return SignInt.Bit10ToInt64(buffer.ToArray());
    }
    public void ReadInt64(ref long data)
    {
        byte[] buffer = new byte[10];
        _ = _basicStream.Read(buffer, 0, buffer.Length);
        data = SignInt.Bit10ToInt64(buffer.ToArray());
    }

    public string ReadString()
    {
        var length = ReadInt32();
        var buffer = new byte[length];
        _ = _basicStream.Read(buffer, 0, length);

        var value = Encoding.UTF8.GetString(buffer);

        return value;
    }

    public string ReadLString()
    {
        int length = ReadVerInt();
        byte[] buffer = new byte[length];
        _ = _basicStream.Read(buffer, 0, length);

        string value = Encoding.UTF8.GetString(buffer);

        return value;
    }

    public void ReadString(ref string data)
    {
        int length = ReadInt32();
        byte[] buffer = new byte[length];
        _ = _basicStream.Read(buffer, 0, length);
        data = Encoding.UTF8.GetString(buffer);
    }

    public void ReadLString(ref string data)
    {
        int length = ReadVerInt();
        byte[] buffer = new byte[length];
        _ = _basicStream.Read(buffer, 0, length);
        data = Encoding.UTF8.GetString(buffer);
    }

    public int ReadVerInt()
        => VarNumberUtil.ReadVarInt2Int32(this);
    public long ReadVerLong()
        => VarNumberUtil.ReadVarLong2Int64(this);
    public void ReadVerInt(ref int value)
        => value = VarNumberUtil.ReadVarInt2Int32(this);
    public void ReadVerLong(ref long value)
        => value = VarNumberUtil.ReadVarLong2Int64(this);

    public int ReadSInt()
        => SignSIntUtil.ReadSInt2Int32(this);
    public long ReadSLong()
        => SignSIntUtil.ReadSLong2Int64(this);

    public void ReadSInt(ref int data)
        => data = SignSIntUtil.ReadSInt2Int32(this);

    public void ReadSLong(ref long data)
        => data = SignSIntUtil.ReadSLong2Int64(this);

    public void ReadComponent(ref IStreamComponent comp)
        => comp.OnDeSerialize(this);

    public IStreamComponent ReadComponent(IStreamComponent component)
    {
        component.OnDeSerialize(this);
        return component;
    }

    public T ReadComponent<T>(T ins) where T : IStreamComponent
    {
        ins.OnDeSerialize(this);
        return ins;
    }
}