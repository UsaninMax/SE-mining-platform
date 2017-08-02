using System.Windows.Threading;
using System.Collections.Generic;
using TradePlatform.Vizualization.Builders.Predicates;
using System;
using TradePlatform.Sandbox.Models;
using TradePlatform.Vizualization.Builders;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Charts
{
    public class ChartProxy
    {
        private readonly Dispatcher _dispatcher;

        public ChartProxy()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void ShowCharts(IEnumerable<Panel> configuration, IChartsBuilder builder)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                builder.Build(configuration);
            }));
        }

        internal void Push(IChartViewModel chartViewModel, IList<Candle> list)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.Push(list);
            }));
        }

        internal void Push(IChartViewModel chartViewModel, IList<Indicator> list)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                chartViewModel.Push(list);
            }));
        }
    }
}
