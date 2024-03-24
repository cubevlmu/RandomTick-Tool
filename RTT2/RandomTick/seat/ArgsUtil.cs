using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomTick.RandomTick.seat;

public static class ArgHelper
{
    public static ArgsUtil ParseArguments(this IEnumerable<string>? args) 
        => new(args ?? new []{""});
}

public class ArgsUtil
{
    private readonly Dictionary<string, string> _argMap = new();

    public ArgsUtil(IEnumerable<string> args)
    {
        foreach (var arg in args)
        {
            if(!arg.Contains('=')) continue;
            var parts = arg.Split('=');
            if (parts.Length != 2)
            {
                var idx = arg.IndexOf('=');
                parts = new[]
                {
                    arg[(idx + 1)..],
                    arg.Remove(idx)
                };
            }

            _argMap.Add(parts[0], parts[1]);
        }
    }

    public string Get(string key)
    {
        return !_argMap.TryGetValue(key, out var value) ? "" : value;
    }
    
    public string this[string key] => Get(key);

    public static implicit operator string[,](ArgsUtil util)
    {
        var keys = util._argMap.Keys.ToList();
        var values = util._argMap.Values.ToList();
        if (keys.Count != values.Count) throw new Exception("???");
        var rr = new string[keys.Count, 2];
        for (var i = 0; i < keys.Count; i++)
        {
            rr[i, 0] = keys[i];
            rr[i, 1] = keys[i];
        }

        return rr;
    }
}