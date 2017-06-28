using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TradePlatform.DataSet.Models;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.DataServices
{
    public interface IDataSetService
    {
        void Store(DataSetItem item, CancellationToken cancellationToken);
        void Delete(DataSetItem item, Task build, CancellationTokenSource cancellationTokenSource);
        bool CheckIfExist(DataSetItem item);
        IList<DataTick> Get(string id);
    }
}
