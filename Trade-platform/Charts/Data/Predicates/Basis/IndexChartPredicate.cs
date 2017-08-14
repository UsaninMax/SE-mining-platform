namespace TradePlatform.Charts.Data.Predicates.Basis
{
    public abstract class IndexChartPredicate : ChartPredicate
    {
        public int From { get; set; }
        public int To { get; set; }
    }
}
