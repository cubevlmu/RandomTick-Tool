using System;
using RandomTick.RandomTick.models.choosers;

namespace RandomTick.RandomTick.models;

public enum ChooserType
{
    Experiment,
    Traditional
}

public interface IChooser
{
    string Choose();
    void OnCreate(TicketSet set);
    ChooserType Type { get; }

    public static IChooser Create(ChooserType type, TicketSet set)
    {
        switch (type)
        {
            case ChooserType.Experiment:
                try
                {
                    var experimentChooser = new ExperimentChooser();
                    experimentChooser.OnCreate(set);
                    return experimentChooser;
                }
                catch (Exception)
                {
                    var cs = new DefaultChooser();
                    cs.OnCreate(set);
                    return cs;
                }
            case ChooserType.Traditional:
                var chooser = new DefaultChooser();
                chooser.OnCreate(set);
                return chooser;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}