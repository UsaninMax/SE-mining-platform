using System.Collections.Generic;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.Populating.Predicates;

namespace TradePlatform.Vizualization.Charts
{
    public interface IChartsPopulator
    {
        void SetUpCharts(IEnumerable<Panel> configuration);
        void Populate(CandlesDataPredicate predicate);
        void Populate(IndicatorDataPredicate predicate);
    }
}
