using System;
using System.Collections.Generic;

namespace TradePlatform.Vizualization.Builders.Predicates
{
    public class ChartViewPredicate
    {
        public IEnumerable<string> Ids { get; set; }
        public int YSize { get; set; } = 600;
        public long XAxis { get; set; } = TimeSpan.FromSeconds(1).Ticks;
    }
}
