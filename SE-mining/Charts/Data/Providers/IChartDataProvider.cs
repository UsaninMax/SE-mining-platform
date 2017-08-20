using System.Collections.Generic;
namespace SEMining.Charts.Data.Providers
{
    public interface IChartDataProvider
    {
        IEnumerable<T> GetExistStorageData<T>(string instrumentId);
        IEnumerable<T> GetCustomStorageData<T>(string instrumentId);
    }
}
