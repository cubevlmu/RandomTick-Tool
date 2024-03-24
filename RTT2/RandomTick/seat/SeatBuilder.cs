using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClsOom.ClassOOM.loggers;
using FluentAvalonia.Core;
using RandomTick.RandomTick.models;
using RandomTick.RandomTick.services;

namespace RandomTick.RandomTick.seat;

public class SeatBuilder
{
    public const string Placeholder = "${PLACEHOLDER}";
    private const int MaxTimes = 10;
    
    private readonly TickSetService _service;
    private static readonly CheatFile File = new();
    private readonly string[,] _map;
    private readonly int _w;
    private readonly int _h;
    private readonly Random _r = new();
    private readonly ILogger _logger;

    private TicketSet? _current;
    private readonly List<string> _setContent = new();
    private readonly List<string> _allContent = new();

    public SeatBuilder(int w, int h)
    {
        App.RttApp.Server
            .GetService("TickSetService", out TickSetService? s)
            .GetLogger("Seat", out _logger);
        _service = s ?? throw new Exception("failed to get service");

        _map = new string[w,h];
        _w = w;
        _h = h;
    }

    public int GetWidth() => _w;
    public int GetHeight() => _h;
    public string[,] GetMap() => _map;

    public static async Task PreInit() 
        => await File.Read();
    
    
    public SeatBuilder Begin(TicketSet set)
    {
        _setContent.Clear();
        _current = set;
        _allContent.AddRange(set.Get());
        _setContent.AddRange(set.Get());
        
        var x = 0;
        var y = 0;
        while (true)
        {
            _map[x, y] = string.Empty;
            y++;
            if (y < _h) continue;
            y = 0;
            x++;
            if (x == _w) break;
        }

        return this;
    }

    public SeatBuilder SetPlaceholder(int x, int y)
    {
        if (_current == null)
        {
            _logger.Error("current set is null, please begin first!");
            return this;
        }
        if (x < _w && y < _h)
        {
            _map[x, y] = Placeholder;
        }
        return this;
    }

    public SeatBuilder PreCheatPlace()
    {
        var keys = File.GetWhiteListKeys();
        foreach (var key in keys)
        {
            if(!_setContent.Contains(key)) continue;
            var rr = File.FindWhiteList(key);
            if(!_setContent.Contains(rr)) continue;

            var genRootTimes = 0;
            var retryTimes = 0;
            genRoot:
            var x = _r.Next(0, _w - 1);
            var y = _r.Next(0, _h - 1);

            genRootTimes++;
            if (App.RttApp.Config.IsDebugMode)
                _logger.Debug($"[Seat] Gen root times : {genRootTimes}");
            if (genRootTimes == MaxTimes) 
                throw new Exception("failed to gen root point");

            if (_map[x, y] != string.Empty) 
                goto genRoot;
            _map[x, y] = App.RttApp.Config.IsDebugMode ? key + "[C]" : key;

            var genPointTime = 0;
            genPoint:
            var xA = _r.Next(x == 0 ? 0 : -1, 1);
            var yA = _r.Next(y == 0 ? (xA == 0 ? 1 : 0) : -1, 1);
            
            genPointTime++;
            if (App.RttApp.Config.IsDebugMode)
                _logger.Debug($"[Seat] Gen point times : {genPointTime}");
            if (genPointTime == MaxTimes)
            {
                retryTimes++;
                if (retryTimes == MaxTimes)
                    throw new Exception("failed to gen point");
                _map[x, y] = string.Empty;
                goto genRoot;
            }

            if (xA == 0 && yA == 0)
                goto genPoint;
            if (_map[x + xA, y + yA] != string.Empty)
                goto genPoint;
            
            _map[x + xA, y + yA] = App.RttApp.Config.IsDebugMode ? rr + "[C]" : rr;

            _setContent.Remove(key);
            _setContent.Remove(rr);
        }
        return this;
    }

    public SeatBuilder SetPreFill(int x, int y, int index)
    {
        if (_current == null)
        {
            _logger.Error("current set is null, please begin first!");
            return this;
        }

        if (index >= _setContent.Count)
        {
            _logger.Error("prefill item is not in this set!!!");
            return this;
        }
        var rr = _setContent[index];

        if (x >= _w || y >= _h) return this;
        
        _map[x, y] = App.RttApp.Config.IsDebugMode ? rr + "[p]" : rr;
        _setContent.RemoveAt(index);

        return this;
    }
    
    public SeatBuilder Gen()
    {
        if (_current == null)
        {
            _logger.Error("current set is null, please begin first!");
            return this;
        }
        var randomList = new List<string>();

        while (_setContent.Count != 0)
        {
            var pickResult = _r.Next(0, _setContent.Count - 1);
            randomList.Add(_setContent[pickResult]);
            _setContent.RemoveAt(pickResult);
        }

        var x = 0;
        var y = 0;
        var index = 0;
        var isEnd = false;
        while (true)
        {
            var fieldV = _map[x, y];
            if (fieldV == string.Empty)
            {
                _map[x, y] = isEnd ? "" : randomList[index];
                index++;
                if (index == randomList.Count) isEnd = true;
            }
            y++;
            if (y < _h) continue;
            y = 0;
            x++;
            if (x == _w) break;
        }
        
        return this;
    }

    public SeatBuilder CheatCheck()
    {
        var ll = File.GetBlackListKeys();
        var dic = new Dictionary<string, string>();
        
        foreach (var kk in ll)
        {
            if(!_allContent.Contains(kk)) continue;
            var vv = File.FindBlackList(kk);
            if(!_allContent.Contains(vv)) continue;
            dic.Add(kk, vv);
        }

        for (var i = 0; i < _w; i++)
        {
            for (var j = 0; j < _h; j++)
            {
                if(!dic.ContainsKey(_map[i,j])) continue;
                var aa = dic[_map[i, j]];
                for (var z = i - 1; z <= i + 1 && z < _w; z++)
                {
                    for (var y = j - 1; y <= j + 1 && y < _h; y++)
                    {
                        if (_map[z, y] != aa) continue;
                        var temp = _map[z, y] + "[a]";
                        _map[z, y] = _map[0, 0] + "[a]";
                        _map[0, 0] = temp;
                    }
                }
            }
        }
        return this;
    }
    
    
    public SeatBuilder UnsetThis(int x, int y)
    {
        if (_current == null)
        {
            _logger.Error("current set is null, please begin first!");
            return this;
        }

        if (x >= _w || y >= _h) return this;
        if (_map[x, y].Length > 0 && _map[x, y] != Placeholder)
        {
            var item = _map[x, y];
            if (item.Contains('['))
                item = item.Remove(item.IndexOf('['));
            _setContent.Add(item);
        }

        _map[x, y] = "";
        return this;
    }

    public void End()
    {
        _current = null;
        _setContent.Clear();
    }
    
    public bool ApplyFilter()
    {
        //TODO
        return false;
    }

    public string[] GetCurrent()
        => _setContent.ToArray();
}