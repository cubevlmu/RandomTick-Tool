using System;
using System.Collections.Generic;

namespace RandomTick.RandomTick.models.choosers;

public class DefaultChooser : IChooser
{
    public ChooserType Type => ChooserType.Traditional;
    
    public string Choose()
    {
        var idx = _r.Next(0, _list.Count);
        var item = _set![idx];
        _buffered.Add(item);
        _list.Remove(item);

        if (_buffered.Count <= 0) return item;
        
        var it = _buffered[0];
        _list.Add(it);
        _buffered.RemoveAt(0);
        return item;
    }

    public void OnCreate(TicketSet set)
    {
        _set = set;
        var r = set.Get();
        _list.AddRange(r);
    }

    private TicketSet? _set;
    private readonly List<string> _list = new();
    private readonly List<string> _buffered = new();
    private readonly Random _r = new();
}