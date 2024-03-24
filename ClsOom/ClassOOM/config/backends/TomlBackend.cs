namespace ClsOom.ClassOOM.config.backends;

using Nett;

public class TomlBackend : IConfigBackend
{
    public IDictionary<string, string> ReadAll(string path)
    {
        if (!File.Exists(path)) return new Dictionary<string, string>();

        var tomlConfig = Toml.ReadFile<TomlConfig>(path);

        return tomlConfig.Entries.ToDictionary(entry => entry.Key, entry => entry.Value);
    }

    public bool SaveAll(string path, IDictionary<string, string> data)
    {
        var tomlConfig = new TomlConfig();
        foreach (var pair in data)
        {
            tomlConfig.Entries.Add(pair.Key, pair.Value);
        }

        Toml.WriteFile(tomlConfig, path);
        return true;
    }

    public string ExtensionType => ".toml";

    private class TomlConfig
    {
        public Dictionary<string, string> Entries { get; set; } = new();
    }
}
