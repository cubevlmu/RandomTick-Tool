using System.Text;
using ClsOom.ClassOOM.model;
using ClsOom.ClassOOM.model.Tools;

namespace ClsOom.ClassOOM.config.bml;

[Obsolete]
public sealed class BmlObject : IDisposable
{
    public BmlObject(PacketStream stream)
    {
        this._stream = stream;
        GC.KeepAlive(stream);
    }
    public BmlObject(byte[] obj)
    {
        this._stream = new PacketStream(Gzip.Decompress(obj));
        GC.KeepAlive(_stream);
    }
    public BmlObject()
    {
        _stream = new PacketStream();
        GC.KeepAlive(_stream);
    }
        
    public void WriteObject(string key, int obj)
        => Write(BmlType.Int, obj, key);
    public void WriteObject(string key, bool obj)
        => Write(BmlType.Bool, obj, key);
    public void WriteObject(string key, string obj)
        => Write(BmlType.String, obj, key);
    public void WriteObject(string key, BmlObject obj)
        => Write(BmlType.Bobj, obj, key);
    public void WriteObject(string key, byte[] obj)
        => Write(BmlType.Bytes, obj, key);
    public void WriteObject(string key, byte obj)
        => Write(BmlType.Byte, obj, key);

    private void Write(BmlType type, object value, string key)
    {
        _stream.WriteByte((byte)type);

        var dd = Encoding.UTF8.GetBytes(key);
        if (dd.Length > 255)
            throw new Exception("BML Tag Too Long~");
        _stream.WriteByte((byte)dd.Length);
        _stream.WriteBytes(dd);

        switch (type)
        {
            case BmlType.String:
                _stream.WriteLString(value as string);
                break;
            case BmlType.Int:
                _stream.WriteInt32((int)value);
                break;
            case BmlType.Bobj:
                byte[] buffer = (value as BmlObject)?.GetContent();
                _stream.WriteInt32(buffer.Length);
                _stream.WriteBytes(buffer);
                break;
            case BmlType.Barr:
                byte[] data = (value as BmlArray).GetContent();
                _stream.WriteInt32(data.Length);
                _stream.WriteBytes(data);
                break;
            case BmlType.Bytes:
                byte[] bytes = value as byte[];
                _stream.WriteInt32(bytes.Length);
                _stream.WriteBytes(bytes);
                break;
            case BmlType.Byte:
                _stream.WriteByte((byte)value);
                break;
            case BmlType.Bool:
                _stream.WriteBoolean((bool)value);
                break;
        }
    }

    public BmlValue ReadNext()
    {
        byte id = _stream.ReadByte();
        BmlType type = (BmlType)id;
            
        byte[] buffer = new byte[_stream.ReadByte()];
        _stream.ReadBytes(ref buffer);

        string key = Encoding.UTF8.GetString(buffer);
        var obj = Read(type);

        return new BmlValue(key, obj);
    }

    private object Read(BmlType type)
    {
        switch (type)
        {
            case BmlType.String: return _stream.ReadLString(); 
            case BmlType.Int:    return _stream.ReadInt32();
            case BmlType.Byte:   return _stream.ReadByte();
            case BmlType.Bobj:
                byte[] buffer = new byte[_stream.ReadInt32()];
                _stream.ReadBytes(ref buffer);
                return buffer;
            case BmlType.Barr:
                byte[] data = new byte[_stream.ReadInt32()];
                _stream.ReadBytes(ref data);
                return data;
            case BmlType.Bytes:
                byte[] bf1 = new byte[_stream.ReadInt32()];
                _stream.ReadBytes(ref bf1);
                return bf1;
            case BmlType.Bool:
                bool res = false;
                _stream.ReadBoolean(ref res);
                return res;
        }
        GC.Collect();
        return null;
    }

    public byte[] GetContent()
        => _stream.GetBytes();

    public byte[] GetData()
        => Gzip.CompressBytes(GetContent());

    public void Dispose()
    {
        _stream.Dispose();
        GC.SuppressFinalize(_stream);
        GC.Collect();
    }

    ~BmlObject() => Dispose();
    private readonly PacketStream _stream;
}

public enum BmlType
{
    String = 0,
    Int    = 1,
    Bobj   = 2,
    Barr   = 3,
    Bytes  = 4,
    Byte   = 5,
    Bool   = 6
}