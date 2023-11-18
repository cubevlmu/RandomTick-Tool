using System.Text;
using ClsOom.ClassOOM.model.Tools;

namespace ClsOom.ClassOOM.model;

public partial class PacketStream
{
    public byte ReadByte()
    {
        var buffer = new byte[1];
        _ = BasicStream.Read(buffer, 0, buffer.Length);
        return buffer[0];
    }
    public void ReadByte(ref byte data)
    {
        var buffer = new byte[1];
        _ = BasicStream.Read(buffer, 0, 1);
        data = buffer[0];
    }

    public byte[] ReadBytes(int length)
    {
        byte[] buffer = new byte[length];
        _ = BasicStream.Read(buffer, 0, buffer.Length);
        return buffer.ToArray();
    }
    public void ReadBytes(ref byte[] data)
        => BasicStream.Read(data, 0, data.Length);

    public bool ReadBoolean()
    {
        byte[] buffer = new byte[1];
        _ = BasicStream.Read(buffer, 0, buffer.Length);
        return buffer[0] == 1;
    }
    public void ReadBoolean(ref bool data)
    {
        byte[] buffer = new byte[1];
        _ = BasicStream.Read(buffer, 0, buffer.Length);
        data = buffer[0] == 1;
    }

    public int ReadInt32()
    {
        byte[] buffer = new byte[5];
        _ = BasicStream.Read(buffer, 0, buffer.Length);
        return SignInt.Bit5ToInt32(buffer.ToArray());
    }
    public void ReadInt32(ref int data)
    {
        byte[] buffer = new byte[5];
        BasicStream.Read(buffer, 0, buffer.Length);
        data = SignInt.Bit5ToInt32(buffer.ToArray());
    }

    public long ReadInt64()
    {
        byte[] buffer = new byte[10];
        _ = BasicStream.Read(buffer, 0, buffer.Length);
        return SignInt.Bit10ToInt64(buffer.ToArray());
    }
    public void ReadInt64(ref long data)
    {
        byte[] buffer = new byte[10];
        _ = BasicStream.Read(buffer, 0, buffer.Length);
        data = SignInt.Bit10ToInt64(buffer.ToArray());
    }

    public string ReadString()
    {
        int length = ReadInt32();
        byte[] buffer = new byte[length];
        _ = BasicStream.Read(buffer, 0, length);

        string value = Encoding.UTF8.GetString(buffer);

        return value;
    }

    public string ReadLString()
    {
        int length = ReadVerInt();
        byte[] buffer = new byte[length];
        _ = BasicStream.Read(buffer, 0, length);

        string value = Encoding.UTF8.GetString(buffer);

        return value;
    }

    public void ReadString(ref string data)
    {
        int length = ReadInt32();
        byte[] buffer = new byte[length];
        _ = BasicStream.Read(buffer, 0, length);
        data = Encoding.UTF8.GetString(buffer);
    }

    public void ReadLString(ref string data)
    {
        int length = ReadVerInt();
        byte[] buffer = new byte[length];
        _ = BasicStream.Read(buffer, 0, length);
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
}