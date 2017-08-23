using System.Collections.Generic;
using SE_mining_base.Charts.Data.Predicates.Basis;
using SE_mining_base.Sandbox.Models;
using SE_mining_base.Transactios.Models;

namespace SEMining.Charts.Vizualization.ViewModels
{
    public interface IChartViewModel
    {
        void Push(IEnumerable<Indicator> values, ChartPredicate predicate);
        void Push(IEnumerable<Candle> values, ChartPredicate predicate);
        void Push(IEnumerable<double> values, ChartPredicate predicate);
        void Push(IEnumerable<Transaction> values);
        void ClearAll();
    }
}
