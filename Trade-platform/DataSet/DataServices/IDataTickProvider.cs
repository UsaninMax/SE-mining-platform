using System.Collections.Generic;
using System.Threading;
using TradePlatform.DataSet.Models;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.DataServices
{
    public interface IDataTickProvider
    {
        IEnumerable<DataTick> Get(DataSetItem item, CancellationToken cancellationToken);
    }
}
