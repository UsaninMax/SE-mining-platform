using System.Collections.Generic;
using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.DataServices.Serialization
{
    public interface IInstrumentsStorage
    {
        void Store(IEnumerable<Instrument> instruments);
        IEnumerable<Instrument> ReStore();
    }
}
