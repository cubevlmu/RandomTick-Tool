namespace ClsOom.ClassOOM.config.bml;

[Obsolete]
internal readonly struct BmlArrayValue
{
    public readonly BmlType Type;
    public readonly object Value;

    public BmlArrayValue(BmlType type, object value) : this()
    {
        Type = type;
        Value = value;
    }
    public int ToInt()
        => int.TryParse(Value.ToString(), out var v) ? v : -1;
    public byte[]? ToBytes()
        => Value as byte[];
    public override string? ToString()
        => Value.ToString();
    public BmlObject ToObject()
        => (Value as BmlObject)!;
}