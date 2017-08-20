using System.Collections.Generic;
using System.Threading;
using SEMining.DataSet.Models;
using SEMining.StockData.Models;

namespace SEMining.DataSet.DataServices
{
    public interface IDataTickProvider
    {
        IEnumerable<DataTick> Get(DataSetItem item, CancellationToken cancellationToken);
    }
}
