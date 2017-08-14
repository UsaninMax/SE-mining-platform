using System.Collections.Generic;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.ViewModels;

namespace TradePlatform.Charts.Vizualization.Dispatching
{
    public interface IChartsConfigurationDispatcher
    {
        IDictionary<string, IChartViewModel> Dispatch(IEnumerable<PanelViewPredicate> configuration);
    }
}
