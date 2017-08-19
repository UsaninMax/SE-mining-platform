using System.Collections.Generic;
using TradePlatform.Charts.Data.Predicates.Basis;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Charts.Vizualization.ViewModels
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
