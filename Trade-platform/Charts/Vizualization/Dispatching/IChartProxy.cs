using System.Collections.Generic;
using TradePlatform.Charts.Data.Predicates.Basis;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.ViewModels;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Charts.Vizualization.Dispatching
{
    public interface IChartProxy
    {
        void ShowCharts(IEnumerable<PanelViewPredicate> configuration, IChartsBuilder builder);
        void Push(IChartViewModel chartViewModel, IList<Candle> list, ChartPredicate predicate);
        void Push(IChartViewModel chartViewModel, IList<double> list, ChartPredicate predicate);
        void Push(IChartViewModel chartViewModel, IList<Indicator> list, ChartPredicate predicate);
        void Push(IChartViewModel chartViewModel, IList<Transaction> list);
        void Clear(IChartViewModel chartViewModel);
    }
}
