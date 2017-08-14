using System.Collections.Generic;
using TradePlatform.Charts.Vizualization.Configurations;

namespace TradePlatform.Charts.Vizualization.Dispatching
{
    public interface IChartsBuilder
    {
        void Build(IEnumerable<PanelViewPredicate> configuration);
    }
}
