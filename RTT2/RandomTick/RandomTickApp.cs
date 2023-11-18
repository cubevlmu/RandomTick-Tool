using System;
using ClsOom.ClassOOM;
using ClsOom.ClassOOM.config.backends;
using ClsOom.ClassOOM.il8n;
using ClsOom.ClassOOM.il8n.readers;
using RandomTick.ClassOOM;
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
        Server.Init();
    }
    
    public void OnLoad(ClassOomServer server)
    {
        server
            .BeginConfig(Config)
            .BeginServices()
            .RegisterService(new TickSetService())
            .BeginIl8N(new JsonLang())
            .LoadAllIl8N()
            .SetSelectedType(Config.SelectedLanguage);

        // server.GetServiceField<TickSetService>("TickSetService", "getInstance", out var field);
        //  if(field == null)
        //      return;
        //  var s = (TickSetService)field;
        //  var set = ss.NewSet("TestSet3");
        //  set.Add("test1");
        //  set.Add("test2");
        //
        //  set.Save("test3.set");
    }

    
    public void OnKill(ClassOomServer server)
    {
        server
            .EndConfig()
            .EndIl8N();
    }

    
    public ClassOomServer Server { get; }
    public RandomTickConfig Config { get; } = new(new XmlBackend());

    public string AppName => "RandomTick";
    public string AppId => "RTT";
    public Version AppVersion => new(2, 1);
}