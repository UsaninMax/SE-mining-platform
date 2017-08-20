using System.Collections.Generic;
using SEMining.Charts.Data.Predicates.Basis;
using SEMining.Sandbox.Models;
using SEMining.Sandbox.Transactios.Models;

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
