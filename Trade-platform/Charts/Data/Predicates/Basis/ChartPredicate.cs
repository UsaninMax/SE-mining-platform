using System;
using System.Windows.Media;

namespace TradePlatform.Charts.Data.Predicates.Basis
{
    public abstract class ChartPredicate
    {
        public string ChartId { get;  set; }
        public string InstrumentId { get;  set; }
        public Brush Color { get; set; }
        public Type CasType { get; set; }

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
