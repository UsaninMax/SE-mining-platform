using System;

namespace TradePlatform.Vizualization.Populating.Predicates
{
    public abstract class ExistDataPredicate : ChartPredicate
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string InstrumentId { get; set; }
    }
}
