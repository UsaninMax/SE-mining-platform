using System.Collections.Generic;
using SEMining.Charts.Vizualization.Configurations;

namespace SEMining.Charts.Vizualization.Dispatching
{
    public interface IChartsBuilder
    {
        void Build(IEnumerable<PanelViewPredicate> configuration);
    }
}
