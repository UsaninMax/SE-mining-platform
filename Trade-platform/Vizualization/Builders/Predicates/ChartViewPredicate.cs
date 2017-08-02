using System;
using System.Collections.Generic;

namespace TradePlatform.Vizualization.Builders.Predicates
{
    public class ChartViewPredicate
    {
        public IEnumerable<string> Ids { get; set; }
        public int ySize { get; set; } = 600;
        public long xAxis { get; set; } = TimeSpan.FromSeconds(1).Ticks;
    }
}
