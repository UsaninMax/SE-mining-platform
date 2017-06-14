using System.Collections.Generic;
using TradePlatform.Commons.BaseModels;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.DataServices
{
    interface IDataTickProvider
    {
        IList<DataTick> Get(DataSetItem item);
    }
}
