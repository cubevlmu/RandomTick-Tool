using ClsOom.ClassOOM.model;

namespace ClsOom.ClassOOM.config.bml;

[Obsolete]
public sealed class BmlArray
{
    public BmlArray(PacketStream stream)
    {
        this._stream = stream;
        _arrayLength = stream.ReadByte();
        ReadArray();
    }
    public BmlArray()
        => _stream = new PacketStream();
        
    private void ReadArray()
    {
        for(var i = 0; i <= _arrayLength; i++)
        {
            var type = (BmlType) _stream.ReadByte();
            var obj = Read(type);
            _array.Add(new BmlArrayValue(type, obj));
        }
    }

    private object Read(BmlType type)
    {
        switch (type)
        {
            case BmlType.String: return _stream.ReadString();
            case BmlType.Int: return _stream.ReadInt32();
            case BmlType.Byte: return _stream.ReadByte();
            case BmlType.Bobj:
                var buffer = new byte[_stream.ReadInt32()];
                _stream.ReadBytes(ref buffer);
                return buffer;
            case BmlType.Barr:
                //TODO
                break;
            case BmlType.Bytes:
                var bf1 = new byte[_stream.ReadInt32()];
                _stream.ReadBytes(ref bf1);
                return bf1;
            case BmlType.Bool:
                break;
        }
        return null;
    }

    public void Add(string obj)
        => _array.Add(new BmlArrayValue(BmlType.String, obj));
    public void Add(int obj)
        => _array.Add(new BmlArrayValue(BmlType.Int, obj));
    public void Add(BmlObject obj)
        => _array.Add(new BmlArrayValue(BmlType.Bobj, obj));
    public void Add(BmlArray obj)
        => _array.Add(new BmlArrayValue(BmlType.Barr, obj));

    private void Write()
    {
        _stream.WriteByte((byte) _array.Count);
        foreach(var item in _array)
        {
            _stream.WriteByte((byte) item.Type);
            switch (item.Type)
            {
                case BmlType.String:
                    _stream.WriteString((item.Value as string)!);
                    break;
                case BmlType.Int:
                    _stream.WriteInt32((int) item.Value);
                    break;
                case BmlType.Bobj:
                    BmlObject obj = (BmlObject)item.Value;
                    byte[] content = obj.GetContent();
                    _stream.WriteInt32(content.Length);
                    _stream.WriteBytes(content);
                    break;
                case BmlType.Barr:
                    //TODO
                    break;
                case BmlType.Bytes:
                    byte[] data = (byte[])item.Value;
                    _stream.WriteInt32(data.Length);
                    _stream.WriteBytes(data);
                    break;
                case BmlType.Byte:
                    _stream.WriteByte((byte) item.Value);
                    break;
            }
        }
        GC.Collect();
    }

    public byte[] GetContent()
    {
        Write();
        return _stream.GetBytes();
    }

    ~BmlArray()
    {
        _stream.Dispose();
    }

    private readonly int _arrayLength;
    private readonly List<BmlArrayValue> _array = new();
    private readonly PacketStream _stream;
}