using Newtonsoft.Json.Linq;

namespace ClsOom.ClassOOM.config.backends;

public class JsonBackend : IConfigBackend
{
    public IDictionary<string, string> ReadAll(string path)
    {
        if (!File.Exists(path)) return new Dictionary<string, string>();
        Dictionary<string, string> map = new();
        
        var root = JArray.Parse(File.ReadAllText(path));
        foreach (var tk in root)
        {
            var strKey = tk["Key"];
            var strValue = tk["Value"];
            if(strKey == null || strValue == null) continue;
            map.Add(strKey.ToString(), strValue.ToString());
        }

        return map;
    }

    
    public bool SaveAll(string path, IDictionary<string, string> v)
    {
        JArray root = new();
        foreach (var pair in v)
        {
            JObject item = new()
            {
                {"Key", pair.Key},
                {"Value", pair.Value}
            };
            root.Add(item);
        }
        File.WriteAllText(path, root.ToString());
        return true;
    }

    public string ExtensionType => ".json";
}