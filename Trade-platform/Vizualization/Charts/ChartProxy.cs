using System.Windows.Threading;
using TradePlatform.Vizualization.Holders;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using TradePlatform.Vizualization.Builders.Predicates;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.Vizualization.Views;
using TradePlatform.Vizualization.ViewModels;
using System.Linq;
using System;
using LiveCharts.Wpf;
using LiveCharts;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Vizualization.Charts
{
    public class ChartProxy
    {
        private readonly Dispatcher _dispatcher;
        private IChartsHolder _chartHolder;

        public ChartProxy()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _chartHolder = ContainerBuilder.Container.Resolve<IChartsHolder>();
        }

        public void ShowCharts(IEnumerable<Panel> configuration)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                configuration.ForEach(x =>
                {
                ContainerBuilder.Container.Resolve<ChartPanelView>(
                    new DependencyOverride<IEnumerable<IChartViewModel>>(
                        x.Charts.Where(s => s.Ids.Count() != 0)
                        .Select(y =>
                             {
                                 return _chartHolder.Get(y.Ids.First());
                             }).ToList())).Show();
                });
            }));
        }

        public void Push(string chartId, ChartValues<double> values)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                _chartHolder.Get(chartId).Push(new LineSeries()
                {
                    Values = values
                });
            }));
        }

        public void Push(string chartId, ChartValues<Candle> values)
        {
            _dispatcher.BeginInvoke((Action)(() =>
            {
                _chartHolder.Get(chartId).Push(new OhlcSeries()
                {
                    Values = values
                });
            }));
        }
    }
}
