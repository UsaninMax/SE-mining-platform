using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.ViewModels;

namespace TradePlatform.Charts.Vizualization.Dispatching
{
    public class ChartsConfigurationDispatcher : IChartsConfigurationDispatcher
    {
        public IDictionary<string, IChartViewModel> Dispatch(IEnumerable<PanelViewPredicate> configuration)
        {
            return configuration
                .SelectMany(x => x.Charts)
                .SelectMany(chart => chart.Ids.Select(id => new Tuple<string, ChartViewPredicate>(id, chart)))
                .ToDictionary(t => t.Item1, t => ContainerBuilder.Container.Resolve<IChartViewModel>(new DependencyOverride<TimeSpan>(t.Item2.XAxis)));
        }
    }
}
