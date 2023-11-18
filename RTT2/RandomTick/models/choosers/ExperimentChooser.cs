using System;
using System.Collections.Generic;

namespace RandomTick.RandomTick.models.choosers;

/// <summary>
/// Every Ticket In The Set Will Be At Least Chosen For One Time
/// But Buggy
/// </summary>
public class ExperimentChooser : IChooser
{
    private const int GroupCount = 3;

    public ChooserType Type => ChooserType.Experiment;

    public string Choose() => _lists[^1].Get();

    public void ReGen()
    {
        for (var i = 0; i < _lists.Length; i++)
            _lists[i] = null;
        OnCreate(_set!);
    }
    
    public void OnCreate(TicketSet set)
    {
        _set = set;
        
        var r = set.Get();
        var rl = new List<string>(r);

        if (r.Length < GroupCount) 
            throw new Exception("Can't Use This Mode, Because This Set Has Too Few Tickets");
        
        var group = r.Length / GroupCount;
        var left = r.Length % GroupCount;

        TicketList? lastList = null;
        for (var i = GroupCount; i > 0; i--)
        {
            if (i == GroupCount)
            {
                var lst = new TicketList(group + left);
                
                for (var j = 0; j < group + left; j++)
                {
                    var id = _rd.Next(0, rl.Count - 1);
                    var dd = rl[id];
                    rl.Remove(dd);
                    lst.Add(dd);
                }
                
                lastList = lst;
                _lists[GroupCount - i] = lst;
            }
            else
            {
                if (lastList == null) throw new Exception("Create SubList Error");
                var isRoot = i == 1;
                var lt = new TicketList(group, isRoot ? this : null, lastList);
                
                for (var j = 0; j < group; j++)
                {
                    var id = _rd.Next(0, rl.Count - 1);
                    var dd = rl[id];
                    rl.Remove(dd);
                    lt.Add(dd);
                }
                
                lastList = lt;                
                _lists[GroupCount - i] = lt;
            }
        }
    }

    private TicketSet? _set;
    private readonly Random _rd = new();
    private readonly TicketList[] _lists = new TicketList[GroupCount];
}