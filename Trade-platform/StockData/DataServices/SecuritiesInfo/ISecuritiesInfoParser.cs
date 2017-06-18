using System.Collections.Generic;
using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.DataServices.SecuritiesInfo
{
    public interface ISecuritiesInfoParser
    {
        IList<Security> Parse(string message);
    }
}
