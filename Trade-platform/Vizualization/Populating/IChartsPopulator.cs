using System.Collections.Generic;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.Populating.Predicates;

namespace TradePlatform.Vizualization.Populating
{
    public interface IChartsPopulator
    {
        void SetUpCharts(IEnumerable<Panel> configuration);
        void Populate(IEnumerable<ChartPredicate> predicates);
    }
}
