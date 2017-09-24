using SE_mining_base.Charts.Data.Predicates.Basis;
using System.Collections.Generic;

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
