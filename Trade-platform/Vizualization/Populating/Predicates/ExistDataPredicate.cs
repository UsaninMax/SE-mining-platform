using System;

namespace TradePlatform.Vizualization.Populating.Predicates
{
    public abstract class ExistDataPredicate : ChartPredicate
    {
        public int GetCount { get; set; } = 1000;
        public int FromIndex { get; set; } = 0;
        public string InstrumentId { get; set; }
    }
}
