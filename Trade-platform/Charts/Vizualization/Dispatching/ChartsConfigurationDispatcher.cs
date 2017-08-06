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
                .ToDictionary(t => t.Item1, t =>
                {
                    if (t.Item2 is DateChartViewPredicate)
                    {
                        var predicate = t.Item2 as DateChartViewPredicate;
                        return ContainerBuilder.Container.Resolve<IChartViewModel>("DateChartViewModel",
                            new DependencyOverride<TimeSpan>(predicate.XAxis));
                    }
                    return ContainerBuilder.Container.Resolve<IChartViewModel>("IndexChartViewModel");
                });
        }
    }
}
