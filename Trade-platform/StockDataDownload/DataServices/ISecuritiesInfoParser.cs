using System.Collections.Generic;
using TradePlatform.Commons.Securities;

namespace TradePlatform.StockDataDownload.DataParsers
{
    interface ISecuritiesInfoParser
    {
        IList<ISecurity> Parse(string message);
    }
}
