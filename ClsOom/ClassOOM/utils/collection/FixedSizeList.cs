using System.Collections;
using System.Text;

namespace ClsOom.ClassOOM.utils.collection;

public class FixedSizeList<T> : IEnumerable<T>, IEnumerator
{
    public FixedSizeList(int size)
    {
        array = new T[size];
    }

    public virtual int Add(T value)
    {
        if (index == array.Length)
            return -1;
        array[index] = value;
        index++;
        return index - 1;
    }

    public void Set(int index, T value)
    {
        if (index < array.Length)
        {
            array[index] = value;
        }
        GC.Collect();
    }

    public void Remove(int index)
    {
        if (index < array.Length)
        {
            var value = array[index];
            array[index] = default;
        }
        GC.Collect();
    }

    public T? this[int idx] => Get(idx);
    public T? Get(int idx)
    {
        if (idx >= array.Length) return default;
        return array[idx] ?? default;
    }

    public int IndexOf(T value)
    {
        var idx = 0;
        foreach (var item in array)
        {
            if (item!.Equals(value))
                return idx;
            idx++;
        }
        return -1;
    }

    public IEnumerator<T> GetEnumerator()
        => null!;//TODO
    IEnumerator IEnumerable.GetEnumerator()
        => this;
    public int Length() => array.Length;

    public override string ToString()
    {
        var builder = new StringBuilder("FixedList[");
        foreach (var item in array)
            builder.Append('"').Append(item).Append('"');
        return builder.Append(']').ToString();
    }

    public bool MoveNext()
    {
        enumIndex += 1;
        var success = enumIndex != array.Length;
        if (!success)
            enumIndex = 0;
        return success;
    }
    public void Reset()
        => enumIndex = 0;
    public object? Current => Get(enumIndex);

    internal int enumIndex;

    internal int index;
    internal readonly T[] array;
    public int Count() => index;
}