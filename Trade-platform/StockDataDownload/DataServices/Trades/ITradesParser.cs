using System.Collections.Generic;
using TradePlatform.StockDataDownload.Models;

namespace TradePlatform.StockDataDownload.DataServices.Trades
{
    interface ITradesParser
    {
        IList<Trade> Parse(string trades);
    }
}
