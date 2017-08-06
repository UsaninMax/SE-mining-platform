using System.Windows.Media;

namespace TradePlatform.Charts.Data.Predicates
{
    public abstract class ChartPredicate
    {
        public string ChartId { get;  set; }
        public string InstrumentId { get;  set; }
        public Brush Color { get; set; }
        public int Index { get; set; } = int.MaxValue;

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
