using System;

namespace RandomTick.RandomTick.models.choosers;

public class TicketList
{
    public static string? TopListElect;
    
    public TicketList(int count, ExperimentChooser? chooser = null, TicketList? nextPriorityGroup = null)
    {
        _list = new string[count];
        _chooser = chooser;
        _nextPriorityGroup = nextPriorityGroup;
    }

    public void Add(string item)
    {
        _list[_prepareIndex] = item;
        if (_prepareIndex == 0) _defaultFirst = item;
        _prepareIndex++;
    }

    private void SetElect(string e)
    {
        _parentElect = e;
        _nextPriorityGroup?.SetElect(e);
    }

    public static implicit operator string(TicketList lst)
        => lst.Get();
    
    public string Get()
    {
        var r = _list[0];
        
        if (_isNotFirstTime)
        {
            if (r == _defaultFirst && _chooser != null)
            {
                _chooser.ReGen();
                return _chooser.Choose();
            }
        }
        else
            _isNotFirstTime = true;
        SetElect(r);
        if (_chooser != null) TopListElect = r;
        
        for (var i = 1; i <= _list.Length - 1; i++)
            _list[i - 1] = _list[i];
        if (_nextPriorityGroup != null)
        {
            var get = _nextPriorityGroup.Get();
            _list[^1] = get;
        }
        else
        {
            _list[^1] = TopListElect ?? throw new Exception("Can't Find TopParent");
            TopListElect = "";
        }

        return r;
    }
    
    private readonly string[] _list;
    private int _prepareIndex;
    private string _parentElect = "";
    private string _defaultFirst = "";
    private readonly ExperimentChooser? _chooser;
    private bool _isNotFirstTime;
    private readonly TicketList? _nextPriorityGroup;
}