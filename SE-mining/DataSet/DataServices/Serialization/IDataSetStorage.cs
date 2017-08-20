using System.Collections.Generic;
using SEMining.DataSet.Models;

namespace SEMining.DataSet.DataServices.Serialization
{
    public interface IDataSetStorage
    {
        void Store(IEnumerable<DataSetItem> dataSets);
        IEnumerable<DataSetItem> ReStore();
    }
}
