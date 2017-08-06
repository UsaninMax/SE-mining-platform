using System.Collections.Generic;
using TradePlatform.Charts.Data.Predicates;

namespace TradePlatform.Charts.Data.Providers
{
    public interface IChartDataProvider
    {
        IList<T> GetExistStorageData<T>(ChartPredicate predicate);
        IList<T> GetCustomStorageData<T>(ChartPredicate predicate);
    }
}
