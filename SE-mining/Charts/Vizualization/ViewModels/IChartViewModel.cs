using System.Collections.Generic;
using SE_mining_base.Charts.Data.Predicates.Basis;
using SE_mining_base.Sandbox.Models;
using SE_mining_base.Transactios.Models;

namespace SEMining.Charts.Vizualization.ViewModels
{
    public interface IChartViewModel
    {
        void Push(ICollection<Indicator> values, ChartPredicate predicate);
        void Push(ICollection<Candle> values, ChartPredicate predicate);
        void Push(ICollection<double> values, ChartPredicate predicate);
        void Push(ICollection<Transaction> values);
        void ClearAll();
    }
}
