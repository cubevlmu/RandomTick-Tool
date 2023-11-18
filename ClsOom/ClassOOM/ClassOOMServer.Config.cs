using ClsOom.ClassOOM.config;

namespace ClsOom.ClassOOM;

public partial class ClassOomServer
{
    public ClassOomServer BeginConfig(Config cfg)
    {
        _configList.Clear();
        _ = GetDataDir(out var dir);
        _configName = $"{dir}//{_app.AppName}{cfg.Backend.ExtensionType}";
        _cfg = cfg;
        cfg.TryLoad(_app);
        
        return this;
    }

    public ClassOomServer BeginWrite()
    {
        _configList.Clear();
        return this;
    }
    
    public ClassOomServer WriteToQueue<T>(OptionItem<T> item)
    {
        _configList.Add(item.GetKey(), item.SerializeTo());
        return this;
    }

    public ClassOomServer EndWrite()
    {
        if (_configName == null || _cfg == null) return this;
        _cfg.Backend.SaveAll(_configName, _configList);
        _configList.Clear();
        return this;
    }

    public ClassOomServer BeginRead()
    {
        if (_configName == null || _cfg == null) return this;
        _ = IsExist(_configName, out var result);
        if (!result) return this;
        _configList.Clear();
        foreach (var keyValuePair in _cfg.Backend.ReadAll(_configName))
            _configList.Add(keyValuePair.Key, keyValuePair.Value);

        return this;
    }

    public ClassOomServer ReadOption<T>(OptionItem<T> item)
    {
        var r = _configList.ContainsKey(item.GetKey());
        if (!r) return this;
        item.SerializeFrom(_configList[item.GetKey()]);
        return this;
    }

    public ClassOomServer EndRead()
    {
        _configList.Clear();
        return this;
    }

    public ClassOomServer SaveConfig()
    {
        if (_cfg == null) return this;
        _cfg.TrySave(_app);
        return this;
    }

    public ClassOomServer EndConfig()
    {
        _ = SaveConfig();
        
        _configName = null;
        _cfg = null;
        _configList.Clear();
        return this;
    }

    private string? _configName;
    private Config? _cfg;
    private readonly Dictionary<string, string> _configList = new();
}