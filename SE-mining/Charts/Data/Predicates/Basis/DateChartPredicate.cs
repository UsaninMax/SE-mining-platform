using System;

namespace SEMining.Charts.Data.Predicates.Basis
{
    public abstract class DateChartPredicate : ChartPredicate
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
