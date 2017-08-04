using System.Collections.Generic;
using TradePlatform.Vizualization.Populating.Predicates;

namespace TradePlatform.Vizualization.Populating.Providers
{
    public interface IChartDataProvider
    {
        IList<T> GetExistStorageData<T>(ChartPredicate predicate);
        IList<T> GetCustomStorageData<T>(ChartPredicate predicate);
    }
}
