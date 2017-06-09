
using System.Collections.Generic;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.DataServices.Serialization
{
    interface IDataTickStorage
    {
        void Store(IList<DataTick> ticks, string path, string file);
        IList<DataTick> ReStore(string path);
    }
}
