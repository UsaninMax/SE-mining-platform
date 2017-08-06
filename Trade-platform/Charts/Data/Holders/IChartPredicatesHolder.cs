using System.Collections.Generic;
using TradePlatform.Charts.Data.Predicates;
using TradePlatform.Charts.Data.Predicates.Basis;

namespace TradePlatform.Charts.Data.Holders
{
    public interface IChartPredicatesHolder
    {
        void Reset();
        IEnumerable<ChartPredicate> Get(string chartId);
        void Update(ChartPredicate predicate);
        void Remove(ChartPredicate predicate);
        void Set(ICollection<ChartPredicate> predicates);
        IEnumerable<ChartPredicate> GetAll();
    }
}
