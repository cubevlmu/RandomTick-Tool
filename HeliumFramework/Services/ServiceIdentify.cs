namespace HeliumFramework.Services;

public struct ServiceIdentify
{
    public ServiceIdentify(string name, bool supportUi, bool multiThread)
    {
        Name = name;
        SupportUi = supportUi;
        MultiThread = multiThread;
    }

    public string Name { get; }
    public bool SupportUi { get; }
    public bool MultiThread { get; }
    
    //public IServiceLoader<> Loader { get; internal set; }
    public IService ServiceInstance { get; private set; }
    
    public static implicit operator string(ServiceIdentify v)
            => v.Name;
    
}