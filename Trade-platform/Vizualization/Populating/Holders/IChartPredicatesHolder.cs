using System.Collections.Generic;
using TradePlatform.Vizualization.Populating.Predicates;

namespace TradePlatform.Vizualization.Populating.Holders
{
    public interface IChartPredicatesHolder
    {
        void Reset();
        void Update(ChartPredicate predicate);
        void Remove(ChartPredicate predicate);
        void Set(ICollection<ChartPredicate> predicates);
        ICollection<ChartPredicate> GetAll();
    }
}
