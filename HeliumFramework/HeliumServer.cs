using HeliumFramework.Services;

namespace HeliumFramework;

public class HeliumServer
{
    private readonly Dictionary<string, ServiceIdentify> _services = new();

    public HeliumServer Init<T>(List<IServiceLoader<T>> loaders) where T : IService
    {
        foreach (var serviceLoader in loaders)
        {
            var id = serviceLoader.GetId();
            _services.Add(id.Name, id);
        }

        return this;
    }

    protected IService GetService(string name)
    {
        return null;
    }
}