using RandomTick.ClassOOM;

namespace ClsOom.ClassOOM;

public partial class ClassOomServer
{
    private static ClassOomServer? _staticServer;
    public static ClassOomServer CreateServer(IGuiApp app)
        => _staticServer = new ClassOomServer(app);

    public static ClassOomServer GetStaticServer()
        => _staticServer ?? throw new Exception("Static Server Is Null, Please Create One First");
    
    private ClassOomServer(IGuiApp app)
    {
        _app = app;
    }

    public ClassOomServer Init()
    {
        BeginLogger()
            .BeginIo();
        _app.OnLoad(this);
        return this;
    }

    public ClassOomServer Gc()
    {
        System.GC.Collect();
        return this;
    }

    public void EndSession()
    {
        _app.OnKill(this);
    }
    
    ~ClassOomServer()
    {
        _app.OnKill(this);
    }
    
    private readonly IGuiApp _app;
}