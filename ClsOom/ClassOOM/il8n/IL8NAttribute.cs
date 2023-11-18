namespace ClsOom.ClassOOM.il8n;

[AttributeUsage(AttributeTargets.Field)]
public class Il8NAttribute : Attribute
{
    internal readonly string Key;

    public Il8NAttribute(string key = "")
    {
        Key = key;
    }
}

public static class AttributeExtend
{
    public static T? GetAttribute<T>(this IEnumerable<object> attributes)
    {
        foreach (var item in attributes)
        {
            if (item is T item1)
                return item1;
        }
        return default;
    }
    
    public static T? GetAttribute<T>(this IEnumerable<Attribute> attributes)
    {
        foreach (var item in attributes)
        {
            if (item is T item1)
                return item1;
        }
        return default;
    }
}