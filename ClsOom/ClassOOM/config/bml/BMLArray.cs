using ClsOom.ClassOOM.model;

namespace ClsOom.ClassOOM.config.bml;

public sealed class BmlArray
{
    public BmlArray(PacketStream stream)
    {
        this.stream = stream;
        ArrayLength = stream.ReadByte();
        ReadArray();
    }
    public BmlArray()
        => stream = new PacketStream();
        
    private void ReadArray()
    {
        for(int i = 0; i <= ArrayLength; i++)
        {
            BMLType type = (BMLType) stream.ReadByte();
            object obj = Read(type);
            Array.Add(new BmlArrayValue(type, obj));
        }
        GC.Collect();
    }

    private object Read(BMLType type)
    {
        switch (type)
        {
            case BMLType.STRING: return stream.ReadString();
            case BMLType.INT: return stream.ReadInt32();
            case BMLType.BYTE: return stream.ReadByte();
            case BMLType.BOBJ:
                var buffer = new byte[stream.ReadInt32()];
                stream.ReadBytes(ref buffer);
                return buffer;
            case BMLType.BARR:
                //TODO
                break;
            case BMLType.BYTES:
                var bf1 = new byte[stream.ReadInt32()];
                stream.ReadBytes(ref bf1);
                return bf1;
            case BMLType.BOOL:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        GC.Collect();
        return null;
    }

    public void Add(string obj)
        => Array.Add(new BmlArrayValue(BMLType.STRING, obj));
    public void Add(int obj)
        => Array.Add(new BmlArrayValue(BMLType.INT, obj));
    public void Add(BmlObject obj)
        => Array.Add(new BmlArrayValue(BMLType.BOBJ, obj));
    public void Add(BmlArray obj)
        => Array.Add(new BmlArrayValue(BMLType.BARR, obj));

    private void Write()
    {
        stream.Clear();
        stream.WriteByte((byte) Array.Count);
        foreach(var item in Array)
        {
            stream.WriteByte((byte) item.Type);
            switch (item.Type)
            {
                case BMLType.STRING:
                    stream.WriteString(item.Value as string);
                    break;
                case BMLType.INT:
                    stream.WriteInt32((int) item.Value);
                    break;
                case BMLType.BOBJ:
                    BmlObject obj = (BmlObject)item.Value;
                    byte[] content = obj.GetContent();
                    stream.WriteInt32(content.Length);
                    stream.WriteBytes(content);
                    break;
                case BMLType.BARR:
                    //TODO
                    break;
                case BMLType.BYTES:
                    byte[] data = (byte[])item.Value;
                    stream.WriteInt32(data.Length);
                    stream.WriteBytes(data);
                    break;
                case BMLType.BYTE:
                    stream.WriteByte((byte) item.Value);
                    break;
            }
        }
        GC.Collect();
    }

    public byte[] GetContent()
    {
        Write();
        return stream.GetBytes();
    }

    ~BmlArray()
    {
        stream.Dispose();
        GC.SuppressFinalize(stream);
        GC.Collect();
    }

    private readonly int ArrayLength;
    private readonly List<BmlArrayValue> Array = new();
    private readonly PacketStream stream;
}