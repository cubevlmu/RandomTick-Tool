using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClsOom.ClassOOM.service;
using FluentAvalonia.Core;
using RandomTick.ClassOOM;
using RandomTick.ClassOOM.service;
using RandomTick.RandomTick.models;
using ClassOomServer = ClsOom.ClassOOM.ClassOomServer;

namespace RandomTick.RandomTick.services;

public class TickSetService : IService
{
    public TickSetService()
    {
        var instance = this;
        _dfInstance = new DelegatedField<TickSetService>(ref instance);
    }

    public void OnLoad(ClassOomServer s)
    {
        s.GetDataDir(out var dirPath);
        var dir = new DirectoryInfo($"{dirPath}//sets");
        if(!dir.Exists)
            dir.Create();
        var files = dir.GetFiles();
        foreach (var file in files)
        {
            if (file.Extension != ".set") continue;
            TicketSet set = new(file.FullName);
            _sets.Add(set.Name, set);
        }

        s.GetLogger("RTT", out var logger);
        logger.Info("Loaded {0} Sets", _sets.Count);
    }
    
    public void OnKill(ClassOomServer s)
    {
        s.GetDataDir(out var dirPath);
        var dir = new DirectoryInfo($"{dirPath}//sets");
        if(!dir.Exists)
            dir.Create();
        foreach (var (key, value) in _sets)
        {
            value.Save();
            Console.WriteLine($"{dir.FullName}//sets//{key}.set Saved");
        }
    }

    public TicketSet NewSet(string name)
    {
        TicketSet set = new(name, null);
        _sets.Remove(name);
        _sets.Add(name, set);
        return set;
    }

    public object? GetFunction(string name) => null;
    public object? GetField(string name)
    {
        if (name == "getInstance")
            return _dfInstance;
        return null;
    }

    
    public void RemoveSet(TicketSet ticketSet)
    {
        if(!_sets.ContainsValue(ticketSet))
            return;
        var i = _sets.Values.ToArray().IndexOf(ticketSet);
        var key = _sets.Keys.ToArray()[i];
        _sets.Remove(key);
    }


    public TicketSet? GetSetByName(string itm) 
        => _sets.TryGetValue(itm, out var v) ? v : null;

    public TicketSet? this[string itm]
        => GetSetByName(itm);

    public string[] GetSetsNames => _sets.Keys.ToArray();

    public string Name => nameof(TickSetService);
    private readonly Dictionary<string, TicketSet> _sets = new();

    private readonly DelegatedField<TickSetService> _dfInstance;
}