using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ClsOom.ClassOOM.model;
using Newtonsoft.Json.Linq;

namespace RandomTick.RandomTick.ticket;

public class TicketCheatEncoder
{
    private readonly string _file;
    private readonly List<string> _whitelist = new();
    private string? _creator;
    private readonly string[]? _lines;

    public TicketCheatEncoder(string file)
    {
        if (!File.Exists(file))
            throw new Exception("file is not exist");
        _file = file;
    }

    public TicketCheatEncoder(string[] lines)
    {
        _lines = lines;
        _file = "";
    }

    private Task PBegin()
    {
        if(_lines == null)
            return Task.CompletedTask;
        _creator = "[anonymous]";
        _whitelist.AddRange(_lines);
        return Task.CompletedTask;
    }
    
    public async Task Begin()
    {
        if (_file == "")
        {
            await PBegin();
            return;
        }
        var rr = await File.ReadAllBytesAsync(_file);
        
        var ss = Encoding.UTF8.GetString(rr);
        var root = JObject.Parse(ss);
        if (!root.TryGetValue("creator", out var vv))
        {
            var rs = vv!.ToString();
            _creator = rs;
        }

        if (!root.TryGetValue("whitelist", out var value)) return;
        if(value is not JArray whitelist) return;
        foreach (var tk in whitelist)
            _whitelist.Add(tk.ToString());
    }

    public async Task End()
    {
        await using PacketStream stm = new();
        stm.WriteString(_creator ?? "[anonymous]");
        
        await stm.WriteVarIntAsync(_whitelist.Count);
        foreach (var vv in _whitelist)
            await stm.WriteStringAsync(vv);

        var pkt = new PacketPackage(stm);
        var rst = pkt.EncodePacket(19);

        App.RttApp.Server.GetDataSubDir("profiles", out var dir);
        await File.WriteAllBytesAsync($"{dir}/ticket_profile.pf", rst);
    }
}