using System.Collections.Generic;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.Populating.Predicates;

namespace TradePlatform.Vizualization.Populating
{
    public interface IChartsPopulator
    {
        void Populate(CandlesDataPredicate predicate);
        void Populate(IndicatorDataPredicate predicate);
    }
}
