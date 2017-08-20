using System.Collections.Generic;
using SEMining.Charts.Data.Predicates.Basis;
using SEMining.Charts.Vizualization.Configurations;
using SEMining.Charts.Vizualization.ViewModels;
using SEMining.Sandbox.Models;
using SEMining.Sandbox.Transactios.Models;

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
