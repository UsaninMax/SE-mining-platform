using System.Collections.Generic;
using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.DataServices.Trades
{
    public interface IDataTickParser
    {
        IList<DataTick> Parse(Instrument instrument);
    }
}
