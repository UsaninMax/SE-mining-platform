using System;
using System.Collections.Generic;
using System.Windows.Threading;
using SEMining.Charts.Data.Predicates.Basis;
using SEMining.Charts.Vizualization.Configurations;
using SEMining.Charts.Vizualization.ViewModels;
using SEMining.Sandbox.Models;
using SEMining.Sandbox.Transactios.Models;

namespace SEMining.Charts.Vizualization.Dispatching
{
    public class ChartProxy : IChartProxy
    {
        private readonly Dispatcher _dispatcher;

        public ChartProxy()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void ShowCharts(IEnumerable<PanelViewPredicate> configuration, IChartsBuilder builder)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                builder.Build(configuration);
            }));
        }

        public void Push(IChartViewModel chartViewModel, IEnumerable<Candle> list, ChartPredicate predicate)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.Push(list, predicate);
            }));
        }

        public void Push(IChartViewModel chartViewModel, IEnumerable<double> list, ChartPredicate predicate)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.Push(list, predicate);
            }));
        }

        public void Push(IChartViewModel chartViewModel, IEnumerable<Indicator> list, ChartPredicate predicate)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.Push(list, predicate);
            }));
        }

        public void Push(IChartViewModel chartViewModel, IEnumerable<Transaction> list)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.Push(list);
            }));
        }

        public void Clear(IChartViewModel chartViewModel)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.ClearAll();
            }));
        }
    }
}
