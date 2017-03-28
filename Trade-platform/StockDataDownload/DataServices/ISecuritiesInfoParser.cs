using System.Collections.Generic;
using TradePlatform.Commons.Securities;

namespace TradePlatform.StockDataDownload.DataParsers
{
    interface ISecuritiesInfoParser
    {
        IList<Security> Parse(string message);
    }
}
