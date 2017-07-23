using System.Collections.Generic;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Builders
{
    public interface IChartsBuilder
    {
        IDictionary<string, IChartViewModel> Build(IEnumerable<Panel> configuration);
    }
}
