using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ClsOom.ClassOOM.model;
using Newtonsoft.Json.Linq;

namespace RandomTick.RandomTick.seat;

public class CheatEncoder
{
    private readonly string _file;
    private readonly Dictionary<string, string> _whiteList = new();
    private readonly Dictionary<string, string> _blacklist = new();
    private string? _creator;

    public CheatEncoder(string file)
    {
        if (!File.Exists(file))
            throw new Exception("file is not exist");
        _file = file;
    }

    public CheatEncoder(IEnumerable<string> w, IEnumerable<string> b)
    {
        _file = "";
        foreach (var s in w)
        {
            if(!s.Contains('-')) continue;
            _whiteList.Add(s.Split('-')[0], s.Split('-')[1]);
        }
        
        foreach (var s in b)
        {
            if(!s.Contains('-')) continue;
            _blacklist.Add(s.Split('-')[0], s.Split('-')[1]);
        }
    }

    public async Task Begin()
    {
        var rr = await File.ReadAllBytesAsync(_file);
        var ss = Encoding.UTF8.GetString(rr);
        var root = JObject.Parse(ss);
        if (!root.TryGetValue("creator", out var vv))
        {
            var rs = vv!.ToString();
            _creator = rs;
        }
        if (root.TryGetValue("whitelist", out var value))
        {
            if(value is not JArray whitelist) return;
            foreach (var tk in whitelist)
            {
                if(tk is not JObject obj) continue;
                if(!obj.ContainsKey("source") || !obj.ContainsKey("beneficiary"))
                    continue;
                _whiteList.Add(obj["source"]!.ToString(), obj["beneficiary"]!.ToString());
            }
        }

        if (!root.ContainsKey("blacklist")) return;
        {
            if(root["blacklist"] is not JArray whitelist) return;
            foreach (var tk in whitelist)
            {
                if(tk is not JObject obj) continue;
                if(!obj.ContainsKey("source") || !obj.ContainsKey("effect"))
                    continue;
                _blacklist.Add(obj["source"]!.ToString(), obj["effect"]!.ToString());
            }
        }
    }

    public async Task End()
    {
        await using PacketStream stm = new();
        stm.WriteString(_creator ?? "[anonymous]");
        
        await stm.WriteVarIntAsync(_whiteList.Count);
        foreach (var vv in _whiteList)
        {
            await stm.WriteStringAsync(vv.Key);
            await stm.WriteStringAsync(vv.Value);
        }
        
        await stm.WriteVarIntAsync(_blacklist.Count);
        foreach (var vv in _blacklist)
        {
            await stm.WriteStringAsync(vv.Key);
            await stm.WriteStringAsync(vv.Value);
        }

        var pkt = new PacketPackage(stm);
        var rst = pkt.EncodePacket(19);

        App.RttApp.Server.GetDataSubDir("profiles", out var dir);
        await File.WriteAllBytesAsync($"{dir}/set_profile.pf", rst);
    }
}