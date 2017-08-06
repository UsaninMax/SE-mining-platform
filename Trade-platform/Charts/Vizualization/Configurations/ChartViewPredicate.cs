using System.Collections.Generic;

namespace TradePlatform.Charts.Vizualization.Configurations
{
    public abstract class ChartViewPredicate
    {
        public IEnumerable<string> Ids { get; set; }
        public int YSize { get; set; } = 600;
    }
}
