using System.Collections.Generic;
using SEMining.Charts.Vizualization.Configurations;
using SEMining.Charts.Vizualization.ViewModels;

namespace SEMining.Charts.Vizualization.Dispatching
{
    public interface IChartsConfigurationDispatcher
    {
        IDictionary<string, IChartViewModel> Dispatch(IEnumerable<PanelViewPredicate> configuration);
    }
}
