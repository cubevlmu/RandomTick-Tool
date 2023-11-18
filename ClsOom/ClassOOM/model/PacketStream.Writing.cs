using System.Text;
using ClsOom.ClassOOM.model.Tools;

namespace ClsOom.ClassOOM.model;

public partial class PacketStream
{
    public void WriteByte(byte array)
    {
        var buffer = new byte[1];
        buffer[0] = array;
        BasicStream.Write(buffer, 0, buffer.Length);
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
        BasicStream.Write(encode, 0, encode.Length);
    }

    public void WriteInt64(long value)
    {
        var encode = SignInt.Int64ToBit10(value);
        BasicStream.Write(encode, 0, encode.Length);
    }

    public void WriteBytes(byte[] array)
    {
        BasicStream.Write(array, 0, array.Length);
    }

    public void WriteString(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteInt32(bytes.Length);
        BasicStream.Write(bytes, 0, bytes.Length);
    }

    public void WriteLString(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteVarInt(bytes.Length);
        BasicStream.Write(bytes, 0, bytes.Length);
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
}