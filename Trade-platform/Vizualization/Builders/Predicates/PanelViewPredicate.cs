using System.Collections.Generic;

namespace TradePlatform.Vizualization.Builders.Predicates
{
    public class PanelViewPredicate
    {
        public IEnumerable<ChartViewPredicate> Charts { get; set; }
    }
}
