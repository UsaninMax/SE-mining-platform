using System.Collections.Generic;
using TradePlatform.Charts.Data.Predicates;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Charts.Vizualization.ViewModels
{
    public interface IChartViewModel
    {
        void Push(IList<Indicator> values, ChartPredicate predicate);
        void Push(IList<Candle> values, ChartPredicate predicate);
        void Push(IList<double> values, ChartPredicate predicate);
        void Push(IList<Transaction> values);
        void ClearAll();
    }
}
