using System.Collections.Generic;
using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.Holders
{
    public interface IDownloadedInstrumentsHolder
    {
        void Put(Instrument instrument);
        void Remove(Instrument instrument);
        HashSet<Instrument> GetAll();
        void RestoreFromSettings();
    }
}
