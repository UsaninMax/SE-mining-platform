using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.ViewModels;
using System;

namespace TradePlatform.Vizualization.Charts
{
    public class ChartsConfigurationDispatcher : IChartsConfigurationDispatcher
    {
        public IDictionary<string, IChartViewModel> Dispatch(IEnumerable<PanelViewPredicate> configuration)
        {
            return configuration
                .SelectMany(x => x.Charts)
                .SelectMany(charty => charty.Ids.Select(id => new Tuple<string, ChartViewPredicate>(id, charty)))
                .ToDictionary(t => t.Item1, t => ContainerBuilder.Container.Resolve<IChartViewModel>(new DependencyOverride<TimeSpan>(t.Item2.XAxis)));
        }
    }
}
