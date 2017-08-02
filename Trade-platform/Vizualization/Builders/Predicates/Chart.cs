using System;
using System.Collections.Generic;

namespace TradePlatform.Vizualization.Builders.Predicates
{
    public class Chart
    {
        public IEnumerable<string> Ids { get; set; }
        public long xAxis { get; set; } = TimeSpan.FromSeconds(1).Ticks;
    }
}
