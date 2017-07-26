using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Builders
{
    public class ChartsBuilder : IChartsBuilder
    {
        public IDictionary<string, IChartViewModel> Build(IEnumerable<Panel> configuration)
        {
            return configuration
                .SelectMany(x => x.Charts)
                .SelectMany(x => x.Ids)
                .ToDictionary(x => x, x => ContainerBuilder.Container.Resolve<IChartViewModel>());
        }
    }
}
