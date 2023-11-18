using RandomTick.ClassOOM;

namespace ClsOom.ClassOOM.config;

public abstract class Config
{
    protected Config(IConfigBackend backend) => Backend = backend;
    
    protected abstract void OnRead(ClassOomServer server);
    protected abstract void OnSave(ClassOomServer server);

    public void TrySave(IGuiApp app) => OnSave(app.Server);
    public void TryLoad(IGuiApp app) => OnRead(app.Server);
    
    internal IConfigBackend Backend { get; }
}