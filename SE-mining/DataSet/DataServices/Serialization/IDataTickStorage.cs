
using System.Collections.Generic;
using SEMining.StockData.Models;

namespace SEMining.DataSet.DataServices.Serialization
{
    public interface IDataTickStorage
    {
        void Store(IEnumerable<DataTick> ticks, string path, string file);
        IEnumerable<DataTick> ReStore(string path);
    }
}
