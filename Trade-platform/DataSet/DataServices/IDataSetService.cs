using System.Threading;
using System.Threading.Tasks;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.DataServices
{
    public interface IDataSetService
    {
        void BuildSet(DataSetItem item, CancellationToken cancellationToken);
        void Delete(DataSetItem item, Task build, CancellationTokenSource cancellationTokenSource);
        bool CheckFiles(DataSetItem item);
    }
}
