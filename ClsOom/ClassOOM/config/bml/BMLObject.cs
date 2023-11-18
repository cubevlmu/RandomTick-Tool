using System.Text;
using ClsOom.ClassOOM.model;
using ClsOom.ClassOOM.model.Tools;

namespace ClsOom.ClassOOM.config.bml;

public sealed class BmlObject : IDisposable
{
    public BmlObject(PacketStream stream)
    {
        this.stream = stream;
        GC.KeepAlive(stream);
    }
    public BmlObject(byte[] obj)
    {
        this.stream = new PacketStream(Gzip.Decompress(obj));
        GC.KeepAlive(stream);
    }
    public BmlObject()
    {
        stream = new PacketStream();
        GC.KeepAlive(stream);
    }
        
    public void WriteObject(string key, int obj)
        => Write(BMLType.INT, obj, key);
    public void WriteObject(string key, bool obj)
        => Write(BMLType.BOOL, obj, key);
    public void WriteObject(string key, string obj)
        => Write(BMLType.STRING, obj, key);
    public void WriteObject(string key, BmlObject obj)
        => Write(BMLType.BOBJ, obj, key);
    public void WriteObject(string key, byte[] obj)
        => Write(BMLType.BYTES, obj, key);
    public void WriteObject(string key, byte obj)
        => Write(BMLType.BYTE, obj, key);

    private void Write(BMLType type, object value, string key)
    {
        stream.WriteByte((byte)type);

        byte[] sdata = Encoding.UTF8.GetBytes(key);
        if (sdata.Length > 255)
            throw new Exception("BML Tag Too Long~");
        stream.WriteByte((byte)sdata.Length);
        stream.WriteBytes(sdata);

        switch (type)
        {
            case BMLType.STRING:
                stream.WriteLString(value as string);
                break;
            case BMLType.INT:
                stream.WriteInt32((int)value);
                break;
            case BMLType.BOBJ:
                byte[] buffer = (value as BmlObject).GetContent();
                stream.WriteInt32(buffer.Length);
                stream.WriteBytes(buffer);
                break;
            case BMLType.BARR:
                byte[] data = (value as BmlArray).GetContent();
                stream.WriteInt32(data.Length);
                stream.WriteBytes(data);
                break;
            case BMLType.BYTES:
                byte[] bytes = value as byte[];
                stream.WriteInt32(bytes.Length);
                stream.WriteBytes(bytes);
                break;
            case BMLType.BYTE:
                stream.WriteByte((byte)value);
                break;
            case BMLType.BOOL:
                stream.WriteBoolean((bool)value);
                break;
        }
    }

    public BmlValue ReadNext()
    {
        byte id = stream.ReadByte();
        BMLType type = (BMLType)id;
            
        byte[] buffer = new byte[stream.ReadByte()];
        stream.ReadBytes(ref buffer);

        string key = Encoding.UTF8.GetString(buffer);
        var obj = Read(type);

        return new BmlValue(key, obj);
    }

    private object Read(BMLType type)
    {
        switch (type)
        {
            case BMLType.STRING: return stream.ReadLString(); 
            case BMLType.INT:    return stream.ReadInt32();
            case BMLType.BYTE:   return stream.ReadByte();
            case BMLType.BOBJ:
                byte[] buffer = new byte[stream.ReadInt32()];
                stream.ReadBytes(ref buffer);
                return buffer;
            case BMLType.BARR:
                byte[] data = new byte[stream.ReadInt32()];
                stream.ReadBytes(ref data);
                return data;
            case BMLType.BYTES:
                byte[] bf1 = new byte[stream.ReadInt32()];
                stream.ReadBytes(ref bf1);
                return bf1;
            case BMLType.BOOL:
                bool res = false;
                stream.ReadBoolean(ref res);
                return res;
        }
        GC.Collect();
        return null;
    }

    public byte[] GetContent()
        => stream.GetBytes();

    public byte[] GetData()
        => Gzip.CompressBytes(GetContent());

    public void Dispose()
    {
        stream.Dispose();
        GC.SuppressFinalize(stream);
        GC.Collect();
    }

    ~BmlObject() => Dispose();
    private readonly PacketStream stream;
}

public enum BMLType
{
    STRING = 0,
    INT    = 1,
    BOBJ   = 2,
    BARR   = 3,
    BYTES  = 4,
    BYTE   = 5,
    BOOL   = 6
}