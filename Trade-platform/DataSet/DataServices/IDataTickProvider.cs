using System.Collections.Generic;
using System.Threading;
using TradePlatform.Commons.BaseModels;
using TradePlatform.DataSet.Models;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.DataServices
{
    public interface IDataTickProvider
    {
        IList<DataTick> Get(DataSetItem item, CancellationToken cancellationToken);
    }
}
