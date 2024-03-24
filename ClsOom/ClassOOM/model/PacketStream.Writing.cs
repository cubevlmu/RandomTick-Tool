using System.Buffers.Binary;
using System.Text;
using ClsOom.ClassOOM.model.Tools;
using ClsOom.ClassOOM.utils.collection;

namespace ClsOom.ClassOOM.model;

public sealed partial class PacketStream
{
    public async Task WriteVarLongAsync(long value)
    {
        var unsigned = (ulong)value;

        do
        {
            var temp = (byte)(unsigned & 127);

            unsigned >>= 7;

            if (unsigned != 0)
                temp |= 128;


            await WriteUnsignedByteAsync(temp);
        } while (unsigned != 0);
    }

    public async Task WriteLongArrayAsync(long[] values)
    {
        foreach (var value in values)
            await WriteLongAsync(value);
    }

    public async Task WriteLongArrayAsync(ulong[] values)
    {
        foreach (var value in values)
            await WriteLongAsync((long)value);
    }

    public async Task WriteVarIntAsync(Enum value) => await WriteVarIntAsync(Convert.ToInt32(value));

    public async Task WriteVarIntAsync(int value)
    {
        //await Globals.PacketLogger.LogDebugAsync($"Writing VarInt ({value})");

        var unsigned = (uint)value;

        do
        {
            var temp = (byte)(unsigned & 127);

            unsigned >>= 7;

            if (unsigned != 0)
                temp |= 128;

            await WriteUnsignedByteAsync(temp);
        } while (unsigned != 0);
    }

    public async Task WriteStringAsync(string value, int maxLength = short.MaxValue)
    {
        //await Globals.PacketLogger.LogDebugAsync($"Writing String ({value})");

        ArgumentNullException.ThrowIfNull(value);

        if (value.Length > maxLength)
            throw new ArgumentException($"string ({value.Length}) exceeded maximum length ({maxLength})",
                nameof(value));

        using var bytes = new RentedArray<byte>(Encoding.UTF8.GetByteCount(value));
        Encoding.UTF8.GetBytes(value, bytes.Span);
        await WriteVarIntAsync(bytes.Length);
        await WriteAsync(bytes);
    }

    public void WriteNullableString(string? value, int maxLength = short.MaxValue)
    {
        if (value is null)
            return;

        System.Diagnostics.Debug.Assert(value.Length <= maxLength);

        using var bytes = new RentedArray<byte>(Encoding.UTF8.GetByteCount(value));
        Encoding.UTF8.GetBytes(value, bytes.Span);
        WriteVarInt(bytes.Length);
        Write(bytes);
    }

    public async Task WriteDoubleAsync(double value)
    {
#if PACKETLOG
            await Globals.PacketLogger.LogDebugAsync($"Writing Double ({value})");
#endif

        using var write = new RentedArray<byte>(sizeof(double));
        BinaryPrimitives.WriteDoubleBigEndian(write, value);
        await WriteAsync(write);
    }

    public async Task WriteFloatAsync(float value)
    {
#if PACKETLOG
            await Globals.PacketLogger.LogDebugAsync($"Writing Float ({value})");
#endif

        using var write = new RentedArray<byte>(sizeof(float));
        BinaryPrimitives.WriteSingleBigEndian(write, value);
        await WriteAsync(write);
    }


    public async Task WriteLongAsync(long value)
    {
#if PACKETLOG
            await Globals.PacketLogger.LogDebugAsync($"Writing Long ({value})");
#endif

        using var write = new RentedArray<byte>(sizeof(long));
        BinaryPrimitives.WriteInt64BigEndian(write, value);
        await WriteAsync(write);
    }

    public async Task WriteIntAsync(int value)
    {
#if PACKETLOG
            await Globals.PacketLogger.LogDebugAsync($"Writing Int ({value})");
#endif

        using var write = new RentedArray<byte>(sizeof(int));
        BinaryPrimitives.WriteInt32BigEndian(write, value);
        await WriteAsync(write);
    }

    public async Task WriteShortAsync(short value)
    {
#if PACKETLOG
            await Globals.PacketLogger.LogDebugAsync($"Writing Short ({value})");
#endif

        using var write = new RentedArray<byte>(sizeof(short));
        BinaryPrimitives.WriteInt16BigEndian(write, value);
        await WriteAsync(write);
    }

    public async Task WriteUnsignedShortAsync(ushort value)
    {
#if PACKETLOG
            await Globals.PacketLogger.LogDebugAsync($"Writing unsigned Short ({value})");
#endif

        using var write = new RentedArray<byte>(sizeof(ushort));
        BinaryPrimitives.WriteUInt16BigEndian(write, value);
        await WriteAsync(write);
    }

    public async Task WriteBooleanAsync(bool value)
    {
#if PACKETLOG
            await Globals.PacketLogger.LogDebugAsync($"Writing Boolean ({value})");
#endif

        await WriteByteAsync((sbyte)(value ? 0x01 : 0x00));
    }

    public async Task WriteUnsignedByteAsync(byte value)
    {
#if PACKETLOG
            await Globals.PacketLogger.LogDebugAsync($"Writing unsigned Byte (0x{value.ToString("X")})");
#endif

        await WriteAsync(new[] { value });
    }

    public async Task WriteByteAsync(sbyte value)
    {
#if PACKETLOG
            await Globals.PacketLogger.LogDebugAsync($"Writing Byte (0x{value.ToString("X")})");
#endif

        await WriteUnsignedByteAsync((byte)value);
    }

    public new void WriteByte(byte array)
    {
        var buffer = new byte[1];
        buffer[0] = array;
        _basicStream.Write(buffer, 0, buffer.Length);
    }

    public void WriteBoolean(bool boolean)
    {
        byte[] toWrite;
        if (boolean)
        {
            toWrite = new byte[] { 1 };
            WriteBytes(toWrite);
        }
        else
        {
            toWrite = new byte[] { 0 };
            WriteBytes(toWrite);
        }
    }

    public void WriteInt32(int value)
    {
        var encode = SignInt.Int32ToBit5(value);
        _basicStream.Write(encode, 0, encode.Length);
    }

    public void WriteInt64(long value)
    {
        var encode = SignInt.Int64ToBit10(value);
        _basicStream.Write(encode, 0, encode.Length);
    }

    public void WriteBytes(byte[] array)
    {
        _basicStream.Write(array, 0, array.Length);
    }

    public void WriteString(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteInt32(bytes.Length);
        _basicStream.Write(bytes, 0, bytes.Length);
    }

    public void WriteLString(string? value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteVarInt(bytes.Length);
        _basicStream.Write(bytes, 0, bytes.Length);
    }

    public void WriteVarInt(int value)
    {
        var data = VarNumberUtil.ParseInt32ToVarInt(value);
        WriteBytes(data);
    }

    public void WriteVarLong(long value)
    {
        var data = VarNumberUtil.ParseInt64ToVarLong(value);
        WriteBytes(data);
    }

    public void WriteSInt(int value)
    {
        var data = SignSIntUtil.ParseInt32ToSInt(value);
        WriteBytes(data);
    }

    public void WriteSLong(long value)
    {
        var data = SignSIntUtil.ParseInt64ToSLong(value);
        WriteBytes(data);
    }

    public void WriteComponent(IStreamComponent component)
        => component.OnSerialize(this);

    public void WriteComponent<T>(T ins) where T : IStreamComponent
        => ins.OnSerialize(this);
}