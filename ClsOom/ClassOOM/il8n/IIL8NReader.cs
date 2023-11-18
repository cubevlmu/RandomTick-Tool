namespace ClsOom.ClassOOM.il8n;

public enum Il8NType : byte
{
    ZhCn, ZhTw, ZhHk, EnUs, JaJp
}

public struct Il8NInfo
{
    public Il8NType Type { get; init; }
    public string? Name { get; init; }
    public string? Author { get; init; }
}

public abstract class Il8NReader
{
    public abstract Dictionary<string, string> ReadAll(byte[] data);
    public abstract bool SaveAll(Dictionary<string, string> map, string fileName);

    public Il8NInfo? Info;
}