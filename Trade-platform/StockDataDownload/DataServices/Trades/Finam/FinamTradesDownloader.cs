
using System.Collections.Generic;
using TradePlatform.StockDataDownload.DataServices.Trades;
using TradePlatform.StockDataDownload.model;
using TradePlatform.StockDataDownload.Models;

namespace TradePlatform.StockDataDownload.Services
{
    class FinamTradesDownloader : ITradesDownloader
    {
        private ITradesParser _tadesParser;

        public FinamTradesDownloader (ITradesParser tadesParser)
        {
            this._tadesParser = tadesParser;
        }

        public IList<Trade> Download(Instrument instrument)
        {

            return new List<Trade>() { new Trade() { Date = new System.DateTime(), Price = 11, Volume = 1 } };
        }
    }
}
