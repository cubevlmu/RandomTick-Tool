namespace ClsOom.ClassOOM.config.bml;

internal readonly struct BmlArrayValue
{
    public readonly BMLType Type;
    public readonly object Value;

    public BmlArrayValue(BMLType type, object value) : this()
    {
        Type = type;
        Value = value;
        GC.Collect();
    }
    public int ToInt()
        => int.TryParse(Value.ToString(), out int v) ? v : -1;
    public byte[] ToBytes()
        => Value as byte[];
    public override string ToString()
        => Value.ToString();
    public BmlObject ToObject()
        => Value is BmlObject ? (BmlObject)Value : null;
}