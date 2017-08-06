using System;
using System.Collections.Generic;
using System.Windows.Threading;
using TradePlatform.Charts.Data.Predicates;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.ViewModels;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Charts.Vizualization.Dispatching
{
    public class ChartProxy
    {
        private readonly Dispatcher _dispatcher;

        public ChartProxy()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        internal void ShowCharts(IEnumerable<PanelViewPredicate> configuration, IChartsBuilder builder)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                builder.Build(configuration);
            }));
        }

        internal void Push(IChartViewModel chartViewModel, IList<Candle> list, ChartPredicate predicate)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.Push(list, predicate);
            }));
        }

        internal void Push(IChartViewModel chartViewModel, IList<double> list, ChartPredicate predicate)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.Push(list, predicate);
            }));
        }

        internal void Push(IChartViewModel chartViewModel, IList<Indicator> list, ChartPredicate predicate)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.Push(list, predicate);
            }));
        }

        internal void Push(IChartViewModel chartViewModel, IList<Transaction> list)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.Push(list);
            }));
        }

        internal void Clear(IChartViewModel chartViewModel)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.ClearAll();
            }));
        }
    }
}
