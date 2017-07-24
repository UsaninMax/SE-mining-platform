using System;

namespace TradePlatform.Vizualization.Populating.Predicates
{
    public class ExistDataPredicate : ChartPredicate
    {
        DateTime From { get; set; }
        DateTime To { get; set; }
        string InstrumentId { get; set; }
    }
}
