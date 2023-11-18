using System;
using System.Threading.Tasks;

namespace RandomTick.ClassOOM.service;

public struct DelegateContext<T, TR>
{
    public T Arg { get; init; }
    public TR? Result { get; set; }
}

public struct DelegatedFunction<T, TR>
{
    public DelegatedFunction(string name, Action<DelegateContext<T, TR>> f)
    {
        _body = f;
        _name = name;
    }

    public TR? Invoke(T arg)
    {
        var d = new DelegateContext<T, TR>
        {
            Arg = arg
        };
        _body.Invoke(d);
        return d.Result;
    }

    public Task<TR?> InvokeAsync(T arg)
    {
        var d = new DelegateContext<T, TR>
        {
            Arg = arg
        };
        _body.Invoke(d);
        return Task.FromResult(d.Result);
    }

    private string _name;
    private readonly Action<DelegateContext<T, TR>> _body;
}

public struct DelegatedField<T>
{
    public DelegatedField(ref T field)
        => _v = field;

    public T Get() => _v;
    public void Set(T v) => _v = v;

    public static implicit operator T(DelegatedField<T> d)
        => d.Get();
    
    private T _v;
}