using System.Collections.Generic;
using TradePlatform.Commons.BaseModels;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.DataServices
{
    interface IDataSetProvider
    {
        IList<DataTick> Get(DataSetItem item);
    }
}
