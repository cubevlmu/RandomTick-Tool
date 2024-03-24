using System;
using System.Collections.Generic;
using System.IO;
using ClsOom.ClassOOM.model;
using RandomTick.RandomTick.services;

namespace RandomTick.RandomTick.models;

public class TicketSet
{
    public TicketSet(string file)
    {
        if (!File.Exists(file))
            throw new Exception("File Not Found");

        _file = file;
        var bt = File.ReadAllBytes(file);
        var pkg = new PacketPackage(bt);
        pkg.ReadPacket();
        _ = pkg.GetPacketId();
        var decode = pkg.DecodeStream();
        if (decode == null) throw new Exception("File Can't Decode");

        _name = decode.ReadString();

        var count = decode.ReadVerInt();
        for (var i = 1; i <= count; i++)
            _tickets.Add(decode.ReadString());

        _chooser = IChooser.Create(ChooserType.Experiment, this);
    }

    
    public TicketSet(string name, List<string>? tickets = null)
    {
        App.RttApp.Server.GetDataSubDir("sets", out var s);
        _file = $"{s}//{name}.set";
        _name = name;
        _tickets = tickets ?? new List<string>();
        _chooser = IChooser.Create(ChooserType.Experiment, this);
    }


    public void Add(string name) => _tickets.Add(name);
    public void AddRange(IEnumerable<string> items) => _tickets.AddRange(items);
    public void Remove(int index) => _tickets.RemoveAt(index);
    public void Remove(string name) => _tickets.Remove(name);
    public void RemoveAll() => _tickets.Clear();
    public void ApplyAll() => _chooser.OnCreate(this);
    
    public string this[int idx] => _tickets[idx];
    public string[] Get() => _tickets.ToArray();

    public string RandomNext() => _chooser.Choose();
    
    public async void Save()
    {
        await using PacketStream stm = new();
        stm.WriteString(_name);
        
        await stm.WriteVarIntAsync(_tickets.Count);
        _tickets.ForEach(item =>
        {
            stm.WriteString(item);
        });

        var pkt = new PacketPackage(stm);
        var rst = pkt.EncodePacket(9);
        await File.WriteAllBytesAsync(_file, rst);
    }

    public void DeleteSelf()
    {
        if(File.Exists(_file))
            File.Delete(_file);
        
        App.RttApp.Server.GetService("TickSetService", out TickSetService? v);
        var vs = v ?? throw new Exception("Error To Load Data");
        vs.RemoveSet(this);
        _tickets.Clear();
    }

    public void Rename(string text) => _name = text;

    public string Name => RandomTickApp.Service!.Config.IsDebugMode ? $"{_name}[{_chooser.Type.ToString()}]" : _name;

    private string _name;
    private readonly IChooser _chooser;
    private readonly List<string> _tickets = new();
    private readonly string _file;
}