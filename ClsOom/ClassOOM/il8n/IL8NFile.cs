namespace ClsOom.ClassOOM.il8n;

public class Il8NFile
{
    public Il8NFile(Il8NReader reader, string name, ClassOomServer server)
    {
        server.GetDataSubDir("il8n", out var dir);
        if (!File.Exists($"{dir}//{name}")) 
            throw new Exception("Lang File Can't Find");
        _name = name;

        var data = File.ReadAllBytes($"{dir}//{name}");
        var r = reader.ReadAll(data);
        _items = r;
        Info = reader.Info!.Value;
        reader.Info = null;

        server.GetLogger("ClassOOM", out var logger);
        logger.Info("Loaded Lang File {0} Lang Type {1} Author {2}", name, Info.Type.ToString(), Info.Author ?? "");
    }
    
    public void Save()
    {
        
    }

    public string Get(string key) => _items[key];
    public bool Contains(string key) => _items.ContainsKey(key);
    public string this[string key] => Get(key);

    public readonly Il8NInfo Info;
    private readonly Dictionary<string, string> _items;
    private readonly string _name;
}