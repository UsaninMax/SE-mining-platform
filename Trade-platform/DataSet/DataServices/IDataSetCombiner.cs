using System.Collections.Generic;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.DataServices
{
    interface IDataSetCombiner
    {
        IList<DataTick> Combine(DataSetItem item);
    }
}
