using System.Collections.Generic;
using SEMining.StockData.Models;

namespace SEMining.StockData.DataServices.SecuritiesInfo
{
    public interface ISecuritiesInfoParser
    {
        IEnumerable<Security> Parse(string message);
    }
}
