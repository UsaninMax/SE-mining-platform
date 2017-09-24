using System.Collections.Generic;
using SE_mining_base.Charts.Vizualization.Configurations;

namespace SEMining.Charts.Vizualization.Dispatching
{
    public interface IChartsBuilder
    {
        void Build(IEnumerable<PanelViewPredicate> configuration);
    }
}
