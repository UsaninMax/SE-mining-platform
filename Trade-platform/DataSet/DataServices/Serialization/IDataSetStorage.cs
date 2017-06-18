using System.Collections.Generic;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.DataServices.Serialization
{
    public interface IDataSetStorage
    {
        void Store(IEnumerable<DataSetItem> dataSets);
        IList<DataSetItem> ReStore();
    }
}
