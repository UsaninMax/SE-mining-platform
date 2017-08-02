using System.Collections.Generic;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Charts
{
    public interface IChartsConfigurationDispatcher
    {
        IDictionary<string, IChartViewModel> Dispatch(IEnumerable<Panel> configuration);
    }
}
