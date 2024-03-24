using YamlDotNet.RepresentationModel;

namespace ClsOom.ClassOOM.config.backends;

public class YamlBackend : IConfigBackend
{
    public IDictionary<string, string> ReadAll(string path)
    {
        if (!File.Exists(path)) return new Dictionary<string, string>();
        Dictionary<string, string> map = new();

        var yamlText = File.ReadAllText(path);
        var yamlStream = new YamlStream();
        yamlStream.Load(new StringReader(yamlText));

        var rootNode = (YamlMappingNode)yamlStream.Documents[0].RootNode;
        foreach (var entry in rootNode.Children)
        {
            var key = ((YamlScalarNode)entry.Key).Value;
            var value = ((YamlScalarNode)entry.Value).Value;
            
            if(key == null || value == null) continue;
            map.Add(key, value);
        }

        return map;
    }

    public bool SaveAll(string path, IDictionary<string, string> data)
    {
        var rootNode = new YamlMappingNode();
        foreach (var pair in data)
        {
            rootNode.Add(new YamlScalarNode(pair.Key), new YamlScalarNode(pair.Value));
        }

        var yamlStream = new YamlStream(new YamlDocument(rootNode));
        File.WriteAllText(path, yamlStream.ToString());
        return true;
    }

    public string ExtensionType => ".yaml";
}
