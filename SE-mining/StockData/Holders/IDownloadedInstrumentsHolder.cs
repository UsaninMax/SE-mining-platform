using System.Collections.Generic;
using SEMining.StockData.Models;

namespace SEMining.StockData.Holders
{
    public interface IDownloadedInstrumentsHolder
    {
        void Put(Instrument instrument);
        void Remove(Instrument instrument);
        ISet<Instrument> GetAll();
        void Restore();
        void Store();
    }
}
