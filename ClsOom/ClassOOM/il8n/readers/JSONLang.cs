using System.Text;
using Newtonsoft.Json.Linq;

namespace ClsOom.ClassOOM.il8n.readers;

public class JsonLang : Il8NReader
{
    public override Dictionary<string, string> ReadAll(byte[] data)
    {
        var r = Encoding.UTF8.GetString(data);
        var root = JObject.Parse(r);
        var info = root?["Info"];
        if(info == null) goto Error;
        var author = info["Author"]!.ToString();
        var name = info["Name"]!.ToString();
        var type = (Il8NType)byte.Parse(info["Type"]!.ToString() ?? throw new Exception("Format Error"));
        Info = new Il8NInfo
        {
            Author = author, Name = name, Type = type
        };
        
        var content = (JObject?)root?["Content"];
        if (content == null) goto Error;
        var d = new Dictionary<string, string>();

        AnalyzeContent(d, content, "");

        return d;
        Error:
        return new Dictionary<string, string>();
    }

    private static void AnalyzeContent(Dictionary<string, string> d,JObject content, string nowRootName)
    {
        foreach (var (key, value) in content)
        {
            switch (value)
            {
                case null:
                    continue;
                case JObject obj:
                    AnalyzeContent(d, obj, nowRootName == "" ? key : $"{nowRootName}.{key}");
                    break;
                default:
                    d.Add(nowRootName == "" ? key : $"{nowRootName}.{key}", value!.ToString()!);
                    break;
            }
        }
    }

    
    public override bool SaveAll(Dictionary<string, string> map, string fileName)
    {
        if (Info == null) return false;
        
        var info = new JObject
        {
            {"Name", Info.Value.Name},
            {"Author", Info.Value.Author},
            {"Type", (int)Info.Value.Type}
        };
        var content = new JArray();
        foreach (var (key, value) in map)
            content[key] = value;

        var root = new JObject
        {
            {"Info", info},
            {"Content", content}
        };
        File.WriteAllText(fileName, root.ToString());
        return File.Exists(fileName);
    }
}