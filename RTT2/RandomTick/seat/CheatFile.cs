using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClsOom.ClassOOM.model;

namespace RandomTick.RandomTick.seat;

public class CheatFile
{
    private readonly string _file;
    private string _creator = "anonymous";
    private readonly Dictionary<string, string> _whiteList = new();
    private readonly Dictionary<string, string> _blackList = new();

    public CheatFile()
    {
        App.RttApp.Server.GetDataSubDir("profiles", out var dir);
        if (!File.Exists($"{dir}/set_profile.pf"))
            throw new Exception("failed to read profile!");
        _file = $"{dir}/set_profile.pf";
    }

    public async Task Read()
    {
        var bt = await File.ReadAllBytesAsync(_file);
        var pkg = new PacketPackage(bt);
        pkg.ReadPacket();
        _ = pkg.GetPacketId();
        var decode = pkg.DecodeStream();
        if (decode == null) 
            throw new Exception("File can't decode");

        _creator = decode.ReadString();

        var count = decode.ReadVerInt();
        for (var i = 0; i < count; i++)
        {
            var kk = await decode.ReadStringAsync();
            var vv = await decode.ReadStringAsync();
            _whiteList.Add(kk, vv);
        }
        
        count = decode.ReadVerInt();
        for (var i = 0; i < count; i++)
        {
            var kk = await decode.ReadStringAsync();
            var vv = await decode.ReadStringAsync();
            _blackList.Add(kk, vv);
        }
    }

    public string[] GetWhiteListKeys()
        => _whiteList.Keys.ToArray();

    public string FindWhiteList(string kk)
        => !_whiteList.TryGetValue(kk, out var value) ? "" : value;
    public string FindBlackList(string kk)
        => !_blackList.TryGetValue(kk, out var value) ? "" : value;
    public string GetCreator() => _creator;
    //TODO
    public string[] GetBlackListKeys()
        => _blackList.Keys.ToArray();
}