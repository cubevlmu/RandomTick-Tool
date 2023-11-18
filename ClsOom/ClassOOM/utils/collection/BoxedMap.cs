namespace ClsOom.ClassOOM.utils.collection;

public sealed class BoxedMap<TK, TV> : Dictionary<TK, List<TV>> where TK : notnull
{
    public void Add(TK key, TV value)
    {
        if (ContainsKey(key))
            base[key].Add(value);
        else
            Add(key, new List<TV> { value });
        GC.Collect();
    }

    public void Deal(TK key, Action<TV> action)
    {
        if (!ContainsKey(key))
            return;
        var list = new List<TV>(base[key]);
        foreach (var item in list)
            action.Invoke(item);
        foreach (var item in _markedToKill)
            _ = base[item.Key].Remove(item.Value);
    }

        
    public new void Clear()
    {
        foreach (var item in Keys)
            base[item].Clear();
    }

        
    public void Clear(TK key)
    {
        if (!ContainsKey(key))
            return;
        base[key].Clear();
        Remove(key);
    }

    public new void Remove(TK key)
    {
        if (!ContainsKey(key))
            return;
        _ = base.Remove(key);
    }

        
    public void RemoveThis(TK key, TV value)
    {
        if (!ContainsKey(key))
            return;
        _ = base[key].Remove(value);
    }

    public void MarkKv(TK key, TV value)
        => _markedToKill.Add(new KeyValuePair<TK, TV>(key, value));

    private readonly List<KeyValuePair<TK, TV>> _markedToKill = new();
}