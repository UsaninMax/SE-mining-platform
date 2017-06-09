using System;
using System.Threading;
using System.Threading.Tasks;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.DataServices
{
    public class DataSetService : IDataSetService
    {
        public void BuildSet(DataSetItem item, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Delete(DataSetItem item, Task build, CancellationTokenSource cancellationTokenSource)
        {
            throw new NotImplementedException();
        }

        public bool CheckFiles(DataSetItem item)
        {
            throw new NotImplementedException();
        }
    }
}
