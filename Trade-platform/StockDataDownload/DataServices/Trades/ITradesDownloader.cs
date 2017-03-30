using System.Collections.Generic;
using TradePlatform.StockDataDownload.model;
using TradePlatform.StockDataDownload.Models;

namespace TradePlatform.StockDataDownload.Services
{
    interface ITradesDownloader
    {
        IList<Trade> Download(Instrument instrument);
    }
}
