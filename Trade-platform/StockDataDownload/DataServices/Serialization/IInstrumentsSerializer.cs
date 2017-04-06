
using System.Collections.Generic;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Serialization
{
    interface IInstrumentsSerializer
    {
        void Serialize(IEnumerable<Instrument> instruments);
        IList<Instrument> Deserialize();
    }
}
