using System.Collections.Generic;
using TradePlatform.StockDataDownload.model;
using TradePlatform.StockDataDownload.Models;

namespace TradePlatform.StockDataDownload.Services
{
    interface ITradesDownloader
    {
        string Download(Instrument instrument);
    }
}
