
using System.Collections.Generic;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.DataServices.Serialization
{
    public interface IDataTickStorage
    {
        void Store(IEnumerable<DataTick> ticks, string path, string file);
        IEnumerable<DataTick> ReStore(string path);
    }
}
