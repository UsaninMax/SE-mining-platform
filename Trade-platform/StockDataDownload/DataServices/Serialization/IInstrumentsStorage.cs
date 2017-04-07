
using System.Collections.Generic;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Serialization
{
    interface IInstrumentsStorage
    {
        void Store(IEnumerable<Instrument> instruments);
        IList<Instrument> ReStore();
    }
}
