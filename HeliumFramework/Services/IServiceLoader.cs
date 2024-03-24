namespace HeliumFramework.Services;

public interface IServiceLoader<out T> where T : IService
{ 
    public T CreateInstance(params object[] args);

    public ServiceIdentify GetId() => new(nameof(T), false, false);
}