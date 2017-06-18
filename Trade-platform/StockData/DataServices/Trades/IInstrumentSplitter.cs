using System.Collections.Generic;
using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.DataServices.Trades
{
    public interface IInstrumentSplitter
    {
        IEnumerable<Instrument> Split(Instrument instrument);
    }
}
