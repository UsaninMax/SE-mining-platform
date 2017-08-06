using System.Collections.Generic;
namespace TradePlatform.Charts.Data.Providers
{
    public interface IChartDataProvider
    {
        IList<T> GetExistStorageData<T>(string instrumentId);
        IList<T> GetCustomStorageData<T>(string instrumentId);
    }
}
