using System.Collections.Generic;
using SEMining.StockData.Models;

namespace SEMining.StockData.DataServices.Serialization
{
    public interface IInstrumentsStorage
    {
        void Store(IEnumerable<Instrument> instruments);
        IEnumerable<Instrument> ReStore();
    }
}
