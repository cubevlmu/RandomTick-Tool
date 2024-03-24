namespace ClsOom.ClassOOM.config.bml;

[Obsolete]
public struct BmlValue
{
    public string Key;
    public readonly object Value;

    public BmlValue(string key, object value)
        : this()
    {
        Key = key;
        Value = value;
        GC.Collect();
    }

    public byte ToByte()
        => byte.Parse(Value.ToString());
    public int ToInt()
        => int.TryParse(Value.ToString(), out var v) ? v : -1;
    public byte[] ToBytes()
        => Value as byte[];
    public override string ToString()
        => Value.ToString();
    public BmlObject ToObject()
        => Value is BmlObject ? (BmlObject)Value : null;
    public bool ToBool()
        => (bool)Value;
}