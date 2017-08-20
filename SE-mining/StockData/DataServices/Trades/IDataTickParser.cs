using System.Collections.Generic;
using SEMining.StockData.Models;

namespace SEMining.StockData.DataServices.Trades
{
    public interface IDataTickParser
    {
        IEnumerable<DataTick> Parse(Instrument instrument);
    }
}
