
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ClsOom.ClassOOM.il8n.readers;

public class XmlLang : Il8NReader
{
    public override Dictionary<string, string> ReadAll(byte[] data)
    {
        var xmlString = Encoding.UTF8.GetString(data);
        var doc = XDocument.Parse(xmlString);

        var infoElement = doc.Element("Info");
        if (infoElement == null) goto Error;

        var author = infoElement.Element("Author")?.Value;
        var name = infoElement.Element("Name")?.Value;
        var type = (Il8NType)int.Parse(infoElement.Element("Type")?.Value ?? throw new Exception("Format Error"));

        Info = new Il8NInfo
        {
            Author = author,
            Name = name,
            Type = type
        };

        var contentElement = doc.Element("Content");
        if (contentElement == null) goto Error;

        var dictionary = new Dictionary<string, string>();
        AnalyzeContent(dictionary, contentElement, "");

        return dictionary;

        Error:
        return new Dictionary<string, string>();
    }

    private static void AnalyzeContent(Dictionary<string, string> dictionary, XElement element, string nowRootName)
    {
        foreach (var childElement in element.Elements())
        {
            switch (childElement.HasElements)
            {
                case true:
                    AnalyzeContent(dictionary, childElement, nowRootName == "" ? childElement.Name.LocalName : $"{nowRootName}.{childElement.Name.LocalName}");
                    break;
                case false:
                    dictionary.Add(nowRootName == "" ? childElement.Name.LocalName : $"{nowRootName}.{childElement.Name.LocalName}", childElement.Value);
                    break;
            }
        }
    }

    public override bool SaveAll(Dictionary<string, string> map, string fileName)
    {
        if (Info == null) return false;

        var infoElement = new XElement("Info",
            new XElement("Name", Info.Value.Name),
            new XElement("Author", Info.Value.Author),
            new XElement("Type", (int)Info.Value.Type)
        );

        var contentElement = new XElement("Content");
        foreach (var (key, value) in map)
        {
            var keyParts = key.Split('.');
            XElement currentElement = contentElement;
            foreach (var part in keyParts)
            {
                var existingElement = currentElement.Element(part);
                if (existingElement == null)
                {
                    var newElement = new XElement(part);
                    currentElement.Add(newElement);
                    currentElement = newElement;
                }
                else
                {
                    currentElement = existingElement;
                }
            }
            currentElement.Value = value;
        }

        var root = new XElement("Root",
            infoElement,
            contentElement
        );

        root.Save(fileName);
        return File.Exists(fileName);
    }
}