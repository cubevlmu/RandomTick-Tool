using System.Xml.Linq;

namespace ClsOom.ClassOOM.config.backends;

public class XmlBackend : IConfigBackend
{
    public IDictionary<string, string> ReadAll(string path)
    {
        if (!File.Exists(path)) return new Dictionary<string, string>();
        var doc = XDocument.Parse(File.ReadAllText(path));
        var map = new Dictionary<string, string>();
        
        var rootEle = doc.Root;
        if (rootEle == null) return new Dictionary<string, string>();
        if (!rootEle.HasElements) return new Dictionary<string, string>();
        foreach (var item in rootEle.Elements())
        {
            var attKey = item.Attribute("Key");
            if(attKey == null) continue;
            map.Add(attKey.Value, item.Value);
        }

        return map;
    }

    
    public bool SaveAll(string path, IDictionary<string, string> v)
    {
        var doc = new XDocument();
        XElement cfgRoot = new("ConfigRoot");
        
        foreach (var (key, value) in v)
        {
            XElement item = new("ConfigItem");
            item.SetAttributeValue("Key", key);
            item.SetValue(value);
            cfgRoot.Add(item);
        }

        doc.Add(cfgRoot);
        var str = doc.ToString(SaveOptions.OmitDuplicateNamespaces);
        File.WriteAllText(path, str);
        return true;
    }

    public string ExtensionType => ".xml";
}