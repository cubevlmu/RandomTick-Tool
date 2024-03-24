using System;
using System.Threading.Tasks;
using ClsOom.ClassOOM;
using ClsOom.ClassOOM.config.backends;
using ClsOom.ClassOOM.il8n.readers;
using RandomTick.RandomTick.services;
using ClassOomServer = ClsOom.ClassOOM.ClassOomServer;

namespace RandomTick.RandomTick;

public class RandomTickApp : IGuiApp
{
    public static RandomTickApp? Service;
    
    public RandomTickApp()
    {
        Service = this;
        Server = ClassOomServer.CreateServer(this);
    }

    public async Task Init()
        => await Server.Init();
    
    public void OnLoad(ClassOomServer server)
    {
        server
            .BeginConfig(Config)
            .BeginServices()
            .RegisterService(new TickSetService());
    }

    
    public void OnKill(ClassOomServer server)
    {
        server
            .EndConfig();
    }

    
    public ClassOomServer Server { get; }
    public RandomTickConfig Config { get; } = new(new XmlBackend());

    public string AppName => "RandomTick";
    public string AppId => "RTT";
    public Version AppVersion => new(2, 5);
}