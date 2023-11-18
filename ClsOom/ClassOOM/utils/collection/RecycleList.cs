namespace ClsOom.ClassOOM.utils.collection;

public sealed class RecycleList<T> : FixedSizeList<T>
{
    public RecycleList(int size) : base(size)
        => GC.Collect();

    public override int Add(T value)
    {
        if (index == array.Length)
            index = 0;
        array[index] = value;
        index++;
        return index - 1;
    }
}