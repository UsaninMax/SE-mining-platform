using System;

namespace SE_mining_base.Charts.Data.Predicates.Basis
{
    public abstract class DateChartPredicate : ChartPredicate
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
