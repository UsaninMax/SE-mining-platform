using System.Collections.Generic;
namespace TradePlatform.Charts.Data.Providers
{
    public interface IChartDataProvider
    {
        IEnumerable<T> GetExistStorageData<T>(string instrumentId);
        IEnumerable<T> GetCustomStorageData<T>(string instrumentId);
    }
}
