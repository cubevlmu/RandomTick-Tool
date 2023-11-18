using ClsOom.ClassOOM.service;
using RandomTick.ClassOOM.service;

namespace ClsOom.ClassOOM;

public partial class ClassOomServer
{
    public ClassOomServer BeginServices()
    {
        _services.Clear();
        return this;
    }

    public ClassOomServer GetService<T>(string name, out T? serviceInstance)
    {
        serviceInstance = default;
        if (!_services.TryGetValue(name, out var value)) return this;
        serviceInstance = (T?)value;
        return this;
    }
    
    public ClassOomServer RegisterService(IService s)
    {
        var name = s.Name;
        _services.Add(name, s);
        s.OnLoad(this);
        return this;
    }

    public ClassOomServer RemoveService(string name)
    {
        if (!_services.TryGetValue(name, out var value)) return this;
        value.OnKill(this);
        _services.Remove(name);
        return this;
    }

    public ClassOomServer GetServiceFunc<T,TR>(string name, string funcName, out DelegatedFunction<T, TR>? func)
    {
        func = null;
        if (!_services.TryGetValue(name, out var value)) return this;
        func = (DelegatedFunction<T, TR>?)value.GetFunction(funcName);
        return this;
    }
    
    public ClassOomServer GetServiceField<T>(string name, string funcName, out DelegatedField<T>? field)
    {
        field = null;
        if (!_services.TryGetValue(name, out var value)) return this;
        field = (DelegatedField<T>?)value.GetField(funcName);
        return this;
    }

    public ClassOomServer EndServices()
    {
        foreach (var (_, value) in _services)
            value.OnKill(this);
        _services.Clear();
        return this;
    }

    private readonly Dictionary<string, IService> _services = new();
}