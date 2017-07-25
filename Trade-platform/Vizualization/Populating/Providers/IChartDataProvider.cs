using System.Collections.Generic;
using TradePlatform.Sandbox.Models;
using TradePlatform.Vizualization.Populating.Predicates;

namespace TradePlatform.Vizualization.Populating.Providers
{
    public interface IChartDataProvider
    {
        IList<Candle> Get(CandlesDataPredicate predicate);
        IList<Indicator> Get(IndicatorDataPredicate predicate);
    }
}
