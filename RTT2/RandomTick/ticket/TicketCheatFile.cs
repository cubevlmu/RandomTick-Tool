using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClsOom.ClassOOM.model;

namespace RandomTick.RandomTick.ticket;

public class TicketCheatFile
{
    private string? _creator = "";
    private readonly string _file;
    private readonly List<string> _whiteList = new();
    
    public TicketCheatFile()
    {
        App.RttApp.Server.GetDataSubDir("profiles", out var dir);
        if (!File.Exists($"{dir}/ticket_profile.pf"))
            throw new Exception("failed to read profile!");
        _file = $"{dir}/ticket_profile.pf";
    }

    public string[] GetResult()
        => _whiteList.ToArray();

    public bool IsInCheat(string vv)
        => _whiteList.Contains(vv);
    
    public async Task Setup()
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
            _whiteList.Add(kk);
        }
    }
}