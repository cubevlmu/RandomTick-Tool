namespace ClsOom.ClassOOM.config;

public abstract class OptionItem<T> : ICloneable
{
    protected OptionItem(string name, T defaultValue)
    {
        _value = defaultValue;
        _name = name;
    }
    
    public object Clone()
    {
        var t = GetType();
        var ins = Activator.CreateInstance(t);
        if (ins == null) throw new Exception("Failed To Clone Base Type");
        return ins;
    }

    public T Get() => _value;
    public void Set(T v) => _value = v;
    public string GetKey() => _name;

    public abstract string SerializeTo();
    public abstract void SerializeFrom(string textConfig);

    public static implicit operator T(OptionItem<T> v)
        => v.Get();

    private string _name;
    private T _value;
}