using System.Collections.Generic;
using TradePlatform.StockDataDownload.model;

namespace TradePlatform.StockDataDownload.DataServices
{
    interface IInstrumentSplitter
    {
        IList<Instrument> Split(Instrument instrument);
    }
}
