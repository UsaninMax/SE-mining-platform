using System.Collections.Generic;
using TradePlatform.StockDataDownload.model;
using TradePlatform.StockDataDownload.Models;

namespace TradePlatform.StockDataDownload.DataServices.Trades
{
    interface ITradesParser
    {
        IList<Trade> parse(Instrument instrument);
    }
}
