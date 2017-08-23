using System.Collections.Generic;
using SEMining.Charts.Vizualization.ViewModels;
using SE_mining_base.Charts.Data.Predicates.Basis;
using SE_mining_base.Charts.Vizualization.Configurations;
using SE_mining_base.Sandbox.Models;
using SE_mining_base.Transactios.Models;

namespace SEMining.Charts.Vizualization.Dispatching
{
    public interface IChartProxy
    {
        void ShowCharts(IEnumerable<PanelViewPredicate> configuration, IChartsBuilder builder);
        void Push(IChartViewModel chartViewModel, IEnumerable<Candle> list, ChartPredicate predicate);
        void Push(IChartViewModel chartViewModel, IEnumerable<double> list, ChartPredicate predicate);
        void Push(IChartViewModel chartViewModel, IEnumerable<Indicator> list, ChartPredicate predicate);
        void Push(IChartViewModel chartViewModel, IEnumerable<Transaction> list);
        void Clear(IChartViewModel chartViewModel);
    }
}
