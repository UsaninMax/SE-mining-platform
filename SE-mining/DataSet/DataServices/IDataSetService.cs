using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SEMining.DataSet.Models;
using SEMining.StockData.Models;

namespace SEMining.DataSet.DataServices
{
    public interface IDataSetService
    {
        void Store(DataSetItem item, CancellationToken cancellationToken);
        void Delete(DataSetItem item, Task build, CancellationTokenSource cancellationTokenSource);
        bool CheckIfExist(DataSetItem item);
        IEnumerable<DataTick> Get(string id);
    }
}
