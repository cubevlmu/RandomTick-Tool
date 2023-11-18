using System;
using RandomTick.RandomTick.services;

namespace RandomTick.ViewModels.Editor;

public class EditorHomePageViewModel : ViewModelBase
{
    private readonly TickSetService _service;
    public string[] Files { get; private set; }
    
    public EditorHomePageViewModel()
    {
        App.RTTApp.Server.GetService("TickSetService", out TickSetService? v);
        _service = v ?? throw new Exception("Error To Load Data");
        var names = v.GetSetsNames;
        Files = names;
    }

    public void RefreshFiles()
    {
        var names = _service.GetSetsNames;
        Files = names;
    }

    public TickSetService GetService() => _service;
}