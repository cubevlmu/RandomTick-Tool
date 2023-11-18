using ClsOom.ClassOOM;
using ClsOom.ClassOOM.config;
using ClsOom.ClassOOM.il8n;

namespace RandomTick.RandomTick;

public class RandomTickConfig : Config
{
    public RandomTickConfig(IConfigBackend backend) 
        : base(backend)
    { }

    protected override void OnRead(ClassOomServer server)
    {
        server
            .BeginRead()
            .ReadOption(AppTitle)
            .ReadOption(AppVersion)
            .ReadOption(IsDebugMode)
            .ReadOption(SelectedLanguage)
            .ReadOption(CheatMode)
            .EndRead();
    }

    protected override void OnSave(ClassOomServer server)
    {
        server
            .BeginWrite()
            .WriteToQueue(AppTitle)
            .WriteToQueue(AppVersion)
            .WriteToQueue(IsDebugMode)
            .WriteToQueue(SelectedLanguage)
            .WriteToQueue(CheatMode)
            .EndWrite();
    }

    public readonly BoolOption CheatMode = new("CheatMode", false);
    public readonly Il8NTypeOption SelectedLanguage = new("SelectedLanguage", Il8NType.ZhCn);
    public readonly StringOption AppTitle = new("AppTitle", "RandomTick Tool 2");
    public readonly IntOption AppVersion = new("AppVersion", 2);
    public readonly BoolOption IsDebugMode = new("IsDebugMode", false);
}