using System.Collections.Generic;
using SEMining.StockData.Models;

namespace SEMining.StockData.DataServices.Trades
{
    public interface IInstrumentSplitter
    {
        IEnumerable<Instrument> Split(Instrument instrument);
    }
}
