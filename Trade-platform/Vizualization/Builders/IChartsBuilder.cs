using System.Collections.Generic;
using TradePlatform.Vizualization.Builders.Predicates;

namespace TradePlatform.Vizualization.Builders
{
    public interface IChartsBuilder
    {
        void Build(IEnumerable<PanelViewPredicate> configuration);
    }
}
