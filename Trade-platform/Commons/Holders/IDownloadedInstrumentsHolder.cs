using System.Collections.Generic;
using TradePlatform.Commons.Trades;

namespace TradePlatform.Commons.Holders
{
    interface IDownloadedInstrumentsHolder
    {
        void Put(Instrument instrument);
        void Remove(Instrument instrument);
        HashSet<Instrument> GetAll();
        void RestoreFromSettings();
    }
}
