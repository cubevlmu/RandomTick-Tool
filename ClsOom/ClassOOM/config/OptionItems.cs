using System.Text;
using ClsOom.ClassOOM.il8n;

namespace ClsOom.ClassOOM.config;


public class Option<T> : OptionItem<T> where T : ICustomSerializable<T>
{
    public Option(string name, T defaultValue) 
        : base(name, defaultValue)
    {
    }

    public override string SerializeTo() => Get().SerializeTo(Get());

    public override void SerializeFrom(string textConfig) =>
        Set(Get().SerializeFrom(textConfig));
}


public class IntOption : OptionItem<int>
{
    public IntOption(string name, int defaultValue) 
        : base(name, defaultValue)
    {
    }

    public override string SerializeTo() => Get().ToString();

    public override void SerializeFrom(string textConfig)
    {
        var r = int.TryParse(textConfig, out var v);
        if(!r) return;
        Set(v);
    }
}


public class Il8NTypeOption : OptionItem<Il8NType>
{
    public Il8NTypeOption(string name, Il8NType defaultValue) 
        : base(name, defaultValue)
    {
    }

    public override string SerializeTo() => Get().ToString();

    public override void SerializeFrom(string textConfig)
    {
        var r = Il8NType.TryParse(textConfig, out Il8NType v);
        if(!r) return;
        Set(v);
    }
}


public class LongOption : OptionItem<long>
{
    public LongOption(string name, long defaultValue) 
        : base(name, defaultValue)
    {
    }

    public override string SerializeTo() => Get().ToString();

    public override void SerializeFrom(string textConfig)
    {
        var r = long.TryParse(textConfig, out var v);
        if(!r) return;
        Set(v);
    }
}


public class FloatOption : OptionItem<float>
{
    public FloatOption(string name, float defaultValue) 
        : base(name, defaultValue)
    {
    }

    public override string SerializeTo() => Get().ToString();

    public override void SerializeFrom(string textConfig)
    {
        var r = float.TryParse(textConfig, out var v);
        if(!r) return;
        Set(v);
    }
}


public class DoubleOption : OptionItem<double>
{
    public DoubleOption(string name, double defaultValue)
        : base(name, defaultValue)
    {
    }

    public override string SerializeTo() => Get().ToString();

    public override void SerializeFrom(string textConfig)
    {
        var r = double.TryParse(textConfig, out var v);
        if (!r) return;
        Set(v);
    }
}


public class BoolOption : OptionItem<bool>
{
    public BoolOption(string name, bool defaultValue) 
        : base(name, defaultValue)
    {
    }

    public override string SerializeTo() => Get().ToString();

    public override void SerializeFrom(string textConfig)
    {
        var r = bool.TryParse(textConfig, out var v);
        if(!r) return;
        Set(v);
    }
}


public class StringOption : OptionItem<string>
{
    public StringOption(string name, string defaultValue)
        : base(name, defaultValue)
    {
    }

    public override string SerializeTo() => Get();

    public override void SerializeFrom(string textConfig)
        => Set(textConfig);
}


public class StringBuilderOption : OptionItem<StringBuilder>
{
    public StringBuilderOption(string name, StringBuilder defaultValue)
        : base(name, defaultValue)
    {
    }

    public override string SerializeTo() => Get().ToString();

    public override void SerializeFrom(string textConfig)
        => Set(new StringBuilder(textConfig));
}


public class ListOption<T> : OptionItem<List<OptionItem<T>>>
{
    private readonly OptionItem<T> _template;

    public ListOption(string name, List<OptionItem<T>> defaultValue, OptionItem<T> template)
        : base(name, defaultValue)
    {
        _template = template;
    }

    public override string SerializeTo()
    {
        var builder = new StringBuilder();
        var lst = Get();
        lst.ForEach(item =>
        {
            builder.Append($"{item.SerializeTo()}")
                .Append(',');
        });
        var str = builder.ToString();
        return str.Remove(str.Length - 2);
    }

    public override void SerializeFrom(string textConfig)
    {
        var items = textConfig.Split(",");
        var g = Get();
        foreach (var item in items)
        {
            var nT = (OptionItem<T>)_template.Clone();
            nT.SerializeFrom(item);
            g.Add(nT);
        }
    }
}
