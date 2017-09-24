using System.Collections.Generic;
using SEMining.Charts.Vizualization.ViewModels;
using SE_mining_base.Charts.Vizualization.Configurations;

namespace SEMining.Charts.Vizualization.Dispatching
{
    public interface IChartsConfigurationDispatcher
    {
        IDictionary<string, IChartViewModel> Dispatch(IEnumerable<PanelViewPredicate> configuration);
    }
}
