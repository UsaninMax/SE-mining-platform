using System;

namespace TradePlatform.Vizualization.Populating.Predicates
{
    public abstract class ExistDataPredicate : ChartPredicate
    {
        public DateTime To { get; set; } = DateTime.Now;
        public int GetCount { get; set; } = 100;
        public string InstrumentId { get; set; }
    }
}
