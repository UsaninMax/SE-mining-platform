using System.Collections.Generic;
using SEMining.Charts.Data.Predicates.Basis;

namespace SEMining.Charts.Data.Holders
{
    public interface IChartPredicatesHolder
    {
        void Reset();
        IEnumerable<ChartPredicate> GetByChartId(string chartId);
        void Add(ChartPredicate predicate);
        void Remove(ChartPredicate predicate);
        void Add(IEnumerable<ChartPredicate> predicates);
        IEnumerable<ChartPredicate> GetAll();
    }
}
