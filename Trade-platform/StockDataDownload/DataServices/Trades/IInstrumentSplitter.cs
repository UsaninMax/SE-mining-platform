using System.Collections.Generic;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Trades
{
    internal interface IInstrumentSplitter
    {
        IEnumerable<Instrument> Split(Instrument instrument);
    }
}
