namespace TradePlatform.Vizualization.Populating.Predicates
{
    public abstract class ChartPredicate
    {
        public string ChartId { get; set; }
        public string InstrumentId { get; set; }
        public int GetCount { get; set; } = 500;
        public int Index { get; set; } = int.MaxValue;

        public ChartPredicate()
        {
        }

        public ChartPredicate(ChartPredicate predicate )
        {
            ChartId = predicate.ChartId;
            InstrumentId = predicate.InstrumentId;
            GetCount = predicate.GetCount;
            Index = predicate.Index;
        }


        public override bool Equals(object obj)
        {
            var item = obj as ChartPredicate;

            if (item == null)
            {
                return false;
            }

            return ChartId.Equals(item.ChartId) &&
                InstrumentId.Equals(item.InstrumentId);
        }

        public override int GetHashCode()
        {
            return ChartId.GetHashCode() ^ InstrumentId.GetHashCode();
        }
    }
}
