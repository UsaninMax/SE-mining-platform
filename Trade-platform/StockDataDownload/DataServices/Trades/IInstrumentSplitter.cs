using System.Collections.Generic;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Trades
{
    public interface IInstrumentSplitter
    {
        IEnumerable<Instrument> Split(Instrument instrument);
    }
}
