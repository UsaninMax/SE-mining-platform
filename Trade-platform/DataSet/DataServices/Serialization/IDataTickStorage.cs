
using System.Collections.Generic;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.DataServices.Serialization
{
    public interface IDataTickStorage
    {
        void Store(IList<DataTick> ticks, string path, string file);
        IList<DataTick> ReStore(string path);
    }
}
